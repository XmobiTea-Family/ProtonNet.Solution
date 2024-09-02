using System.IO;
using System.Threading.Tasks;
using XmobiTea.Bean.Attributes;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Types;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute;
using XmobiTea.ProtonNet.Server.WebApi.Models;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNet.Server.WebApi.Types;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Extensions;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers
{
    /// <summary>
    /// Controller for handling Proton Web API requests.
    /// </summary>
    [Route("/proton")]
    class ProtonWebApiController : WebApiController
    {
        /// <summary>
        /// RPC protocol service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IRpcProtocolService rpcProtocolService { get; set; }

        /// <summary>
        /// Request service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IRequestService requestService { get; set; }

        /// <summary>
        /// User peer session service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IUserPeerSessionService userPeerSessionService { get; set; }

        /// <summary>
        /// Session service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private ISessionService sessionService { get; set; }

        /// <summary>
        /// User peer service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IUserPeerService userPeerService { get; set; }

        /// <summary>
        /// User peer auth token service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IUserPeerAuthTokenService userPeerAuthTokenService { get; set; }

        /// <summary>
        /// Channel service bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IChannelService channelService { get; set; }

        /// <summary>
        /// Endpoint for checking the service status.
        /// </summary>
        /// <returns>An HTTP response indicating the service is up.</returns>
        [HttpGet("ping")]
        private HttpResponse Ping() => this.Response().MakeOkResponse();

        /// <summary>
        /// Endpoint for getting the current server timestamp.
        /// </summary>
        /// <returns>An HTTP response with the current timestamp.</returns>
        [HttpGet("getts")]
        private HttpResponse GetTs() => this.Response().MakeGetResponse(System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());

        /// <summary>
        /// Middleware for handling API requests and setting session and user peer information.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="authToken">The optional authorization token from headers.</param>
        /// <param name="sessionId">The optional session ID from headers.</param>
        /// <param name="encryptKey">The optional encryption key from headers.</param>
        /// <param name="middlewareContext">The middleware context.</param>
        /// <param name="next">The delegate to invoke the next middleware.</param>
        /// <returns>An HTTP response based on middleware processing.</returns>
        [HttpMiddleware("api")]
        private HttpResponse Api(IWebApiSession session,
            [FromHeader(HeaderNames.Token, IsOptional = true)] string authToken,
            [FromHeader(HeaderNames.SessionId, IsOptional = true)] string sessionId,
            [FromHeader(HeaderNames.EncryptKey, IsOptional = true)] string encryptKey,
            MiddlewareContext middlewareContext,
            NextDelegate next)
        {
            if (!string.IsNullOrEmpty(session.GetSessionId())) sessionId = session.GetSessionId();
            else
            {
                session.SetSessionId(sessionId);
                if (!string.IsNullOrEmpty(sessionId)) this.sessionService.MapSession(sessionId, session);
            }

            if (string.IsNullOrEmpty(sessionId))
            {
                var operationResponse = new OperationResponse(string.Empty)
                {
                    ReturnCode = ReturnCode.OperationInvalid,
                    DebugMessage = "missing sessionId",
                };

                return this.CreateOperationHttpResponse(operationResponse, new SendParameters() { Encrypted = false }, ProtocolProviderType.MessagePack, null, null);
            }

            if (encryptKey != null) session.SetEncryptKey(System.Text.Encoding.UTF8.GetBytes(encryptKey));

            var userPeer = this.GetUserPeer(session, authToken);

            middlewareContext.SetData("UserPeer", userPeer);

            return next.Invoke();
        }

        /// <summary>
        /// Handles the main API request by processing the request payload and returning the appropriate response.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="userPeer">The user peer obtained from middleware context.</param>
        /// <param name="buffer">The request body as a byte array.</param>
        /// <returns>An HTTP response based on the request processing.</returns>
        [HttpPost("api")]
        private async Task<HttpResponse> Api(IWebApiSession session,
            [FromMiddlewareContext("UserPeer")] IUserPeer userPeer,
            [FromBodyBytes] byte[] buffer)
        {
            OperationResponse operationResponse = null;
            OperationHeader header;
            IOperationModel operationModel = null;

            using (var mStream = new MemoryStream(buffer))
            {
                if (!this.rpcProtocolService.TryRead(mStream, out header, out var payload))
                {
                    operationResponse = new OperationResponse(string.Empty)
                    {
                        ReturnCode = ReturnCode.OperationInvalid,
                        DebugMessage = "can not read data body",
                    };

                    header = new OperationHeader();
                    header.CryptoProviderType = CryptoProviderType.Aes;
                    header.SendParameters = new SendParameters();
                }
                else
                {
                    if (header.SendParameters.Encrypted)
                    {
                        if (!this.rpcProtocolService.TryDeserializeEncryptOperationModel(payload, header.OperationType, header.ProtocolProviderType, header.CryptoProviderType.GetValueOrDefault(), session.GetEncryptKey(), out operationModel))
                        {
                            operationResponse = new OperationResponse(string.Empty)
                            {
                                ReturnCode = ReturnCode.OperationInvalid,
                                DebugMessage = "can not read data body",
                            };
                        }
                    }
                    else
                    {
                        if (!this.rpcProtocolService.TryDeserializeOperationModel(payload, header.OperationType, header.ProtocolProviderType, out operationModel))
                        {
                            operationResponse = new OperationResponse(string.Empty)
                            {
                                ReturnCode = ReturnCode.OperationInvalid,
                                DebugMessage = "can not read data body",
                            };
                        }
                    }
                }
            }

            if (operationResponse == null)
            {
                if (!(operationModel is OperationRequest operationRequest))
                {
                    operationResponse = new OperationResponse(string.Empty)
                    {
                        ReturnCode = ReturnCode.OperationInvalid,
                        DebugMessage = "can not read operationModel",
                    };
                }
                else
                {
                    operationResponse = await this.requestService.Handle(operationRequest, header.SendParameters, userPeer, session);

                    if (operationResponse == null)
                    {
                        operationResponse = new OperationResponse(operationRequest.OperationCode)
                        {
                            ReturnCode = ReturnCode.OperationInvalid,
                            DebugMessage = "can not handle operation request, response null",
                        };
                    }

                    operationResponse.ResponseId = operationRequest.RequestId;
                }
            }

            return this.CreateOperationHttpResponse(operationResponse, header.SendParameters, header.ProtocolProviderType, header.CryptoProviderType, session.GetEncryptKey());
        }

        /// <summary>
        /// Retrieves the user peer based on the session and authorization token.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="authToken">The authorization token.</param>
        /// <returns>The user peer associated with the token or default peer.</returns>
        private IUserPeer GetUserPeer(IWebApiSession session, string authToken)
        {
            IUserPeer answer;

            if (string.IsNullOrEmpty(authToken)) answer = this.CreateDefaultUserPeer(session);
            else
            {
                if (!this.userPeerAuthTokenService.TryVerifyToken(authToken, out var header, out var payload)) answer = this.CreateDefaultUserPeer(session);
                else
                {
                    answer = this.userPeerService.GetUserPeer(payload.UserId);

                    if (answer == null)
                    {
                        var userPeer = new UserPeer();

                        userPeer.SetUserId(payload.UserId);
                        userPeer.SetPeerType((PeerType)payload.PeerType);
                        userPeer.SetAuthenticated(true);
                        userPeer.SetSessionId(payload.SessionId);

                        this.userPeerService.MapUserPeer(payload.UserId, userPeer);
                        this.userPeerSessionService.MapUserPeer(payload.SessionId, userPeer);

                        answer = userPeer;
                    }
                }
            }

            return answer;
        }

        /// <summary>
        /// Creates a default user peer for sessions without authentication.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <returns>A default user peer.</returns>
        private IUserPeer CreateDefaultUserPeer(IWebApiSession session)
        {
            var answer = new UserPeer();

            answer.SetPeerType(PeerType.Unknown);
            answer.SetAuthenticated(false);
            answer.SetSessionId(session.GetSessionId());

            return answer;
        }

        /// <summary>
        /// Creates an HTTP response based on the operation response and protocol parameters.
        /// </summary>
        /// <param name="operationResponse">The operation response to include in the HTTP response.</param>
        /// <param name="sendParameters">The parameters for sending the response.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The optional crypto provider type used for encryption.</param>
        /// <param name="encryptKey">The optional encryption key.</param>
        /// <returns>An HTTP response with the operation response data.</returns>
        private HttpResponse CreateOperationHttpResponse(OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType, byte[] encryptKey)
        {
            byte[] dataResponse;

            using (var mStream = new MemoryStream())
            {
                if (sendParameters.Encrypted) this.rpcProtocolService.WriteEncrypt(mStream, OperationType.OperationResponse, operationResponse, sendParameters, protocolProviderType, cryptoProviderType.GetValueOrDefault(), encryptKey);
                else this.rpcProtocolService.Write(mStream, OperationType.OperationResponse, operationResponse, sendParameters, protocolProviderType);

                dataResponse = mStream.ToArray();
            }

            return this.Response().MakeGetResponse(dataResponse, HeaderValues.RpcProtocol);
        }

        /// <summary>
        /// Called when a new session is connected.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        public override void OnConnected(IWebApiSession session)
        {
            if (!string.IsNullOrEmpty(session.GetSessionId())) this.sessionService.MapSession(session.GetSessionId(), session);
        }

        /// <summary>
        /// Called when a session is disconnected.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        public override void OnDisconnected(IWebApiSession session)
        {
            var sessionId = session.GetSessionId();

            if (string.IsNullOrEmpty(sessionId)) return;

            this.sessionService.RemoveSession(session);

            if (this.sessionService.GetSessionCount(sessionId) == 0)
            {
                this.userPeerSessionService.RemoveUserPeer(sessionId);

                var userPeer = this.userPeerSessionService.GetUserPeer(sessionId);

                if (userPeer != null)
                {
                    var userId = userPeer.GetUserId();

                    this.userPeerService.RemoveUserPeer(userId);
                    this.channelService.LeaveAllChannel(userId);
                }
            }
        }

    }

}

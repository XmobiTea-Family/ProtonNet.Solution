using System.IO;
using XmobiTea.ProtonNet.Client.Models;
using XmobiTea.ProtonNet.Client.WebApi.Types;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNetClient.Options;

namespace XmobiTea.ProtonNet.Client.WebApi
{
    /// <summary>
    /// Represents a Web API client peer that communicates with the server using HTTP.
    /// Inherits from <see cref="AbstractWebApiClientPeer"/> and implements the 
    /// <see cref="IWebApiClientPeer"/> interface.
    /// </summary>
    class HttpClientClientPeer : AbstractWebApiClientPeer, IWebApiClientPeer
    {
        /// <summary>
        /// The default protocol provider type used for serialization.
        /// </summary>
        private static readonly ProtocolProviderType DefaultProtocolProviderType = ProtocolProviderType.MessagePack;

        /// <summary>
        /// The default crypto provider type used for encryption.
        /// </summary>
        private static readonly CryptoProviderType DefaultCryptoProviderType = CryptoProviderType.Aes;

        /// <summary>
        /// Prefix used in logging messages specific to this HTTP client peer.
        /// </summary>
        protected override string logPrefix => "HTTP";

        /// <summary>
        /// The HTTP client used to send requests and receive responses.
        /// </summary>
        private System.Net.Http.HttpClient client { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientClientPeer"/> class.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <param name="initRequest">Initial request containing session and client ID information.</param>
        /// <param name="tcpClientOptions">Options for configuring the TCP client.</param>
        public HttpClientClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions)
            : base(serverAddress, initRequest, tcpClientOptions)
        {
            this.client = new System.Net.Http.HttpClient();

            this.client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.ContentType, HeaderValues.RpcProtocol);
            this.client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.SessionId, this.sessionId);
        }

        /// <summary>
        /// Sends an operation request to the server.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to send.</param>
        protected override void SendOperation(OperationRequestPending operationRequestPending) => this.Execute(operationRequestPending);

        /// <summary>
        /// Executes the operation request by sending it to the server and processing the response.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to execute.</param>
        private async void Execute(OperationRequestPending operationRequestPending)
        {
            var operationRequest = operationRequestPending.GetOperationRequest();
            var sendParameters = operationRequestPending.GetSendParameters();

            var fullUrl = this.serverAddress + "/proton/api";

            using (var requestMessage = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, fullUrl))
            {
                byte[] dataBodyRequest;

                using (var mStream = new MemoryStream())
                {
                    if (sendParameters.Encrypted)
                        this.rpcProtocolService.WriteEncrypt(mStream, OperationType.OperationRequest, operationRequest, sendParameters, DefaultProtocolProviderType, DefaultCryptoProviderType, this.encryptKey);
                    else
                        this.rpcProtocolService.Write(mStream, OperationType.OperationRequest, operationRequest, sendParameters, DefaultProtocolProviderType);

                    dataBodyRequest = mStream.ToArray();
                }

                requestMessage.Content = new System.Net.Http.ByteArrayContent(dataBodyRequest);

                this.networkStatistics.ChangeBytesSent(dataBodyRequest.Length);

                if (!string.IsNullOrEmpty(this.authToken))
                    requestMessage.Headers.TryAddWithoutValidation(HeaderNames.Token, this.authToken);
                if (sendParameters.Encrypted)
                    requestMessage.Headers.TryAddWithoutValidation(HeaderNames.EncryptKey, this.encryptKeyStr);

                try
                {
                    using (var responseMessage = await this.client.SendAsync(requestMessage))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        operationRequestPending.OnRecv();

                        var data = await responseMessage.Content.ReadAsByteArrayAsync();

                        this.networkStatistics.ChangeBytesReceived(data.Length);

                        OperationResponse operationResponse;

                        using (var mStream = new MemoryStream(data))
                        {
                            if (!this.rpcProtocolService.TryRead(mStream, out var header, out var payload))
                            {
                                operationResponse = new OperationResponse(operationRequest.OperationCode)
                                {
                                    ReturnCode = ReturnCode.OperationInvalid,
                                    DebugMessage = "Cannot read data body",
                                };

                                operationRequestPending.SetResponseSendParameters(sendParameters);
                            }
                            else
                            {
                                if (header.SendParameters.Encrypted)
                                {
                                    if (!this.rpcProtocolService.TryDeserializeEncryptOperationModel(payload, header.OperationType, header.ProtocolProviderType, header.CryptoProviderType.GetValueOrDefault(), this.encryptKey, out var operationModel))
                                    {
                                        operationResponse = new OperationResponse(string.Empty)
                                        {
                                            ReturnCode = ReturnCode.OperationInvalid,
                                            DebugMessage = "Cannot read data body",
                                        };
                                    }
                                    else
                                    {
                                        operationResponse = (OperationResponse)operationModel;
                                    }
                                }
                                else
                                {
                                    if (!this.rpcProtocolService.TryDeserializeOperationModel(payload, header.OperationType, header.ProtocolProviderType, out var operationModel))
                                    {
                                        operationResponse = new OperationResponse(string.Empty)
                                        {
                                            ReturnCode = ReturnCode.OperationInvalid,
                                            DebugMessage = "Cannot read data body",
                                        };
                                    }
                                    else
                                    {
                                        operationResponse = (OperationResponse)operationModel;
                                    }
                                }

                                operationRequestPending.SetResponseSendParameters(header.SendParameters);

                            }
                        }

                        operationRequestPending.SetOperationResponse(operationResponse);
                    }
                }
                catch (System.Net.Http.HttpRequestException exception)
                {
                    operationRequestPending.OnRecv();

                    var response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = ReturnCode.OperationInvalid,
                        ResponseId = operationRequest.RequestId,
                        DebugMessage = exception.Message,
                    };

                    operationRequestPending.SetOperationResponse(response);
                    operationRequestPending.SetResponseSendParameters(sendParameters);

                    this.logger.Fatal("Exception", exception);
                }
                catch (System.Exception exception)
                {
                    operationRequestPending.OnRecv();

                    var response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = ReturnCode.OperationInvalid,
                        ResponseId = operationRequest.RequestId,
                        DebugMessage = exception.Message,
                    };

                    operationRequestPending.SetOperationResponse(response);
                    operationRequestPending.SetResponseSendParameters(sendParameters);

                    this.logger.Fatal("Exception", exception);
                }
            }
        }

        /// <summary>
        /// Sends a ping request to the server to check the connection status.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the ping request.</param>
        public override async void Ping(OnPingResponse onResponse, int timeoutInSeconds)
        {
            var fullUrl = this.serverAddress + "/proton/ping";

            using (var requestMessage = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, fullUrl))
            {
                try
                {
                    using (var responseMessage = await this.client.SendAsync(requestMessage))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        onResponse?.Invoke(responseMessage.StatusCode == System.Net.HttpStatusCode.OK);
                    }
                }
                catch (System.Net.Http.HttpRequestException exception)
                {
                    onResponse?.Invoke(false);

                    this.logger.Error("Exception", exception);
                }
                catch (System.Exception exception)
                {
                    onResponse?.Invoke(false);

                    this.logger.Error("Exception", exception);
                }
            }
        }

        /// <summary>
        /// Sends a request to the server to get the current timestamp.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's timestamp response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the request.</param>
        public override async void GetTs(OnGetTsResponse onResponse, int timeoutInSeconds)
        {
            var fullUrl = this.serverAddress + "/proton/getts";

            using (var requestMessage = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, fullUrl))
            {
                try
                {
                    using (var responseMessage = await this.client.SendAsync(requestMessage))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        onResponse?.Invoke(long.Parse(await responseMessage.Content.ReadAsStringAsync()));
                    }
                }
                catch (System.Net.Http.HttpRequestException exception)
                {
                    onResponse?.Invoke(System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                    this.logger.Error("Exception", exception);
                }
                catch (System.Exception exception)
                {
                    onResponse?.Invoke(System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                    this.logger.Error("Exception", exception);
                }
            }
        }

    }

}

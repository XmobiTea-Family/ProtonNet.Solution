using System.Collections.Generic;
using XmobiTea.Bean.Attributes;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Services;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.ProtonNet.Server.Socket.Types;
using XmobiTea.ProtonNet.Server.Types;

namespace XmobiTea.ProtonNet.Server.Socket.Handlers
{
    /// <summary>
    /// Defines the interface for handling operations on socket sessions.
    /// </summary>
    interface ISocketOperationModelHandler
    {
        /// <summary>
        /// Handles an operation model for a given session.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        void Handle(ISocketSession session, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Defines a generic interface for handling operations on socket sessions with a specific operation model type.
    /// </summary>
    /// <typeparam name="TOperationModel">The type of the operation model to handle.</typeparam>
    interface IOperationModelHandler<TOperationModel> where TOperationModel : IOperationModel
    {
        /// <summary>
        /// Handles an operation model for a given session.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        void Handle(ISocketSession session, TOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Provides an abstract base class for handling operations on socket sessions with a specific operation model type.
    /// </summary>
    /// <typeparam name="TOperationModel">The type of the operation model to handle.</typeparam>
    abstract class AbstractOperationModelHandler<TOperationModel> : IOperationModelHandler<TOperationModel>, ISocketOperationModelHandler where TOperationModel : IOperationModel
    {
        /// <summary>
        /// Gets the logger instance for logging information, warnings, and errors.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractOperationModelHandler{TOperationModel}"/> class.
        /// </summary>
        public AbstractOperationModelHandler()
        {
            this.logger = LogManager.GetLogger(this);
        }

        /// <summary>
        /// Handles an operation model for a given session, casting the operation model to the specific type.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        void ISocketOperationModelHandler.Handle(ISocketSession session, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
            => this.Handle(session, (TOperationModel)operationModel, sendParameters, protocolProviderType, cryptoProviderType);

        /// <summary>
        /// Handles an operation model for a given session. This method must be implemented by derived classes.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        public abstract void Handle(ISocketSession session, TOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Handles the OperationRequest by processing it and sending a response back to the client.
    /// </summary>
    class OperationRequestHandler : AbstractOperationModelHandler<OperationRequest>
    {
        [AutoBind]
        private IRequestService requestService { get; set; }

        [AutoBind]
        private IUserPeerSessionService userPeerSessionService { get; set; }

        [AutoBind]
        private ISocketSessionEmitService socketSessionEmitService { get; set; }

        /// <summary>
        /// Handles the OperationRequest, validates the session, and processes the request.
        /// </summary>
        public override async void Handle(ISocketSession session, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var sessionId = session.GetSessionId();

            OperationResponse operationResponse;

            if (string.IsNullOrEmpty(sessionId))
                operationResponse = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = ReturnCode.OperationInvalid,
                    DebugMessage = "need handshake to call operation request",
                };
            else
            {
                var userPeer = this.userPeerSessionService.GetUserPeer(sessionId);
                if (userPeer == null)
                    operationResponse = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = ReturnCode.OperationInvalid,
                        DebugMessage = "need handshake before call operation request",
                    };
                else
                {
                    var sessionTime = session.GetSessionTime();
                    sessionTime.SetTicksHandshaked(System.DateTime.UtcNow.Ticks);

                    operationResponse = await this.requestService.Handle(operationRequest, sendParameters, userPeer, session);

                    if (operationResponse == null)
                        operationResponse = new OperationResponse(operationRequest.OperationCode)
                        {
                            ReturnCode = ReturnCode.OperationInvalid,
                            DebugMessage = "can not handle operation request, response null",
                        };
                }
            }

            operationResponse.ResponseId = operationRequest.RequestId;

            var sendResult = this.socketSessionEmitService.SendOperationResponse(session, operationResponse, sendParameters, protocolProviderType, cryptoProviderType);

            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for operationResponse failed " + sendResult);
        }

    }

    /// <summary>
    /// Handles the OperationResponse by logging a warning since only the client should handle it.
    /// </summary>
    class OperationResponseHandler : AbstractOperationModelHandler<OperationResponse>
    {
        /// <summary>
        /// Logs a warning that OperationResponse should only be handled by the client.
        /// </summary>
        public override void Handle(ISocketSession session, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Only client can handle OperationResponse");
        }

    }

    /// <summary>
    /// Handles the OperationEvent by processing it and passing it to the event service.
    /// </summary>
    class OperationEventHandler : AbstractOperationModelHandler<OperationEvent>
    {
        [AutoBind]
        private IEventService eventService { get; set; }

        [AutoBind]
        private IUserPeerSessionService userPeerSessionService { get; set; }

        /// <summary>
        /// Handles the OperationEvent, validates the session, and processes the event.
        /// </summary>
        public override void Handle(ISocketSession session, OperationEvent operationEvent, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var sessionId = session.GetSessionId();

            if (string.IsNullOrEmpty(sessionId)) return;

            var sessionTime = session.GetSessionTime();
            sessionTime.SetTicksHandshaked(System.DateTime.UtcNow.Ticks);

            var userPeer = this.userPeerSessionService.GetUserPeer(sessionId);

            if (userPeer == null) return;

            this.eventService.Handle(operationEvent, sendParameters, userPeer, session);
        }

    }

    /// <summary>
    /// Handles the OperationPing by responding with an OperationPong.
    /// </summary>
    class OperationPingHandler : AbstractOperationModelHandler<OperationPing>
    {
        [AutoBind]
        private ISocketSessionEmitService socketSessionEmitService { get; set; }

        /// <summary>
        /// Handles the OperationPing by sending an OperationPong back to the client.
        /// </summary>
        public override void Handle(ISocketSession session, OperationPing operationPing, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var operationPong = new OperationPong();

            var sendResult = this.socketSessionEmitService.SendOperationPong(session, operationPong, protocolProviderType);

            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for operationPong failed " + sendResult);
        }

    }

    /// <summary>
    /// Handles the OperationPong, but since it's only for clients, it does nothing on the server side.
    /// </summary>
    class OperationPongHandler : AbstractOperationModelHandler<OperationPong>
    {
        /// <summary>
        /// Does nothing since OperationPong is only handled by the client.
        /// </summary>
        public override void Handle(ISocketSession session, OperationPong operationPong, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
        }

    }

    /// <summary>
    /// Handles the OperationHandshake by verifying and establishing a session, 
    /// and mapping the session to a user peer.
    /// </summary>
    class OperationHandshakeHandler : AbstractOperationModelHandler<OperationHandshake>
    {
        [AutoBind]
        private ISocketSessionEmitService socketSessionEmitService { get; set; }

        [AutoBind]
        private ISessionService sessionService { get; set; }

        [AutoBind]
        private IUserPeerSessionService userPeerSessionService { get; set; }

        [AutoBind]
        private IUserPeerAuthTokenService userPeerAuthTokenService { get; set; }

        [AutoBind]
        private IUserPeerService userPeerService { get; set; }

        private int maxUdpSessionRequestPerUser { get; set; }
        private int maxTcpSessionRequestPerUser { get; set; }
        private int maxWsSessionRequestPerUser { get; set; }

        /// <summary>
        /// Sets the maximum number of UDP session requests per user.
        /// </summary>
        internal void SetMaxUdpSessionRequestPerUser(int maxUdpSessionRequestPerUser) => this.maxUdpSessionRequestPerUser = maxUdpSessionRequestPerUser;

        /// <summary>
        /// Sets the maximum number of TCP session requests per user.
        /// </summary>
        internal void SetMaxTcpSessionRequestPerUser(int maxTcpSessionRequestPerUser) => this.maxTcpSessionRequestPerUser = maxTcpSessionRequestPerUser;

        /// <summary>
        /// Sets the maximum number of WebSocket session requests per user.
        /// </summary>
        internal void SetMaxWsSessionRequestPerUser(int maxWsSessionRequestPerUser) => this.maxWsSessionRequestPerUser = maxWsSessionRequestPerUser;

        /// <summary>
        /// Handles the OperationHandshake by establishing a secure session and mapping it to a user peer.
        /// </summary>
        public override void Handle(ISocketSession session, OperationHandshake operationHandshake, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            if (!string.IsNullOrEmpty(session.GetSessionId())) return;
            if (string.IsNullOrEmpty(operationHandshake.SessionId)) return;

            this.CheckAndDisconnectMaxSession(session, operationHandshake.SessionId, protocolProviderType);

            session.SetEncryptKey(operationHandshake.EncryptKey);
            session.SetSessionId(operationHandshake.SessionId);

            var sessionUserPeer = this.userPeerSessionService.GetUserPeer(operationHandshake.SessionId);
            var tokenUserPeer = this.GetUserPeer(session, operationHandshake.AuthToken);

            if (sessionUserPeer == null) sessionUserPeer = tokenUserPeer;
            else
            {
                if (sessionUserPeer.GetUserId() != tokenUserPeer.GetUserId())
                {
                    var operationDisconnect = new OperationDisconnect()
                    {
                        Reason = (byte)DisconnectReason.InvalidOperationHandshake,
                    };

                    this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect, protocolProviderType);

                    session.Disconnect(500);

                    return;
                }

                sessionUserPeer = tokenUserPeer;
            }

            {
                var utcTicks = System.DateTime.UtcNow.Ticks;

                var sessionTime = session.GetSessionTime();
                sessionTime.SetTicksHandshaked(utcTicks);
                sessionTime.SetTicksLastReceived(utcTicks);

                this.userPeerSessionService.MapUserPeer(operationHandshake.SessionId, sessionUserPeer);
                this.sessionService.MapSession(operationHandshake.SessionId, session);

                var operationHandshakeAck = new OperationHandshakeAck()
                {
                    ConnectionId = session.GetConnectionId(),
                    ServerSessionId = session.GetServerSessionId(),
                };

                this.socketSessionEmitService.SendOperationHandshakeAck(session, operationHandshakeAck, protocolProviderType);
            }
        }

        /// <summary>
        /// Checks if the maximum session limit per user is reached and disconnects the oldest session if necessary.
        /// </summary>
        private void CheckAndDisconnectMaxSession(ISocketSession session, string sessionId, ProtocolProviderType protocolProviderType)
        {
            var transportProtocol = session.GetTransportProtocol();

            if (transportProtocol == TransportProtocol.Tcp || transportProtocol == TransportProtocol.Ssl)
            {
                if (this.maxTcpSessionRequestPerUser <= 0)
                {
                    var operationDisconnect = new OperationDisconnect()
                    {
                        Reason = (byte)DisconnectReason.MaxSessionPerUser,
                    };

                    this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect, protocolProviderType);

                    session.Disconnect(500);
                }
                else
                {
                    var sessions = this.GetSessions(sessionId, transportProtocol);

                    if (sessions != null && sessions.Count >= this.maxTcpSessionRequestPerUser)
                    {
                        var operationDisconnect = new OperationDisconnect()
                        {
                            Reason = (byte)DisconnectReason.MaxSessionPerUser,
                        };

                        this.socketSessionEmitService.SendOperationDisconnect(sessions[0], operationDisconnect, protocolProviderType);

                        sessions[0].Disconnect(500);
                    }
                }
            }
            else if (transportProtocol == TransportProtocol.Ws || transportProtocol == TransportProtocol.Wss)
            {
                if (this.maxWsSessionRequestPerUser <= 0)
                {
                    var operationDisconnect = new OperationDisconnect()
                    {
                        Reason = (byte)DisconnectReason.MaxSessionPerUser,
                    };

                    this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect, protocolProviderType);

                    session.Disconnect(500);
                }
                else
                {
                    var sessions = this.GetSessions(sessionId, transportProtocol);

                    if (sessions != null && sessions.Count >= this.maxWsSessionRequestPerUser)
                    {
                        var operationDisconnect = new OperationDisconnect()
                        {
                            Reason = (byte)DisconnectReason.MaxSessionPerUser,
                        };

                        this.socketSessionEmitService.SendOperationDisconnect(sessions[0], operationDisconnect, protocolProviderType);

                        sessions[0].Disconnect(500);
                    }
                }
            }
            else
            {
                if (this.maxUdpSessionRequestPerUser <= 0)
                {
                    var operationDisconnect = new OperationDisconnect()
                    {
                        Reason = (byte)DisconnectReason.MaxSessionPerUser,
                    };

                    this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect, protocolProviderType);

                    session.Disconnect(500);
                }
                else
                {
                    var sessions = this.GetSessions(sessionId, transportProtocol);

                    if (sessions != null && sessions.Count >= this.maxUdpSessionRequestPerUser)
                    {
                        var operationDisconnect = new OperationDisconnect()
                        {
                            Reason = (byte)DisconnectReason.MaxSessionPerUser,
                        };

                        this.socketSessionEmitService.SendOperationDisconnect(sessions[0], operationDisconnect, protocolProviderType);

                        sessions[0].Disconnect(500);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the sessions associated with the specified session ID and transport protocol.
        /// </summary>
        private IList<ISocketSession> GetSessions(string sessionId, TransportProtocol transportProtocol)
        {
            var sessions = this.sessionService.GetSessions(sessionId);

            if (sessions == null) return null;

            var answer = new List<ISocketSession>();

            if (transportProtocol == TransportProtocol.Tcp || transportProtocol == TransportProtocol.Ssl)
            {
                foreach (ISocketSession session in sessions)
                {
                    if (session.GetTransportProtocol() == TransportProtocol.Tcp || session.GetTransportProtocol() == TransportProtocol.Ssl)
                        answer.Add(session);
                }
            }
            else if (transportProtocol == TransportProtocol.Ws || transportProtocol == TransportProtocol.Wss)
            {
                foreach (ISocketSession session in sessions)
                {
                    if (session.GetTransportProtocol() == TransportProtocol.Tcp || session.GetTransportProtocol() == TransportProtocol.Ssl)
                        answer.Add(session);
                }
            }
            else // if (transportProtocol == TransportProtocol.Udp)
            {
                foreach (ISocketSession session in sessions)
                {
                    if (session.GetTransportProtocol() == TransportProtocol.Udp)
                        answer.Add(session);
                }
            }

            return answer;
        }

        /// <summary>
        /// Retrieves the user peer associated with the session, using an authentication token if provided.
        /// </summary>
        private IUserPeer GetUserPeer(ISocketSession session, string authToken)
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

                        answer = userPeer;
                    }
                }
            }

            return answer;
        }

        /// <summary>
        /// Creates a default user peer when no authentication token is provided.
        /// </summary>
        private IUserPeer CreateDefaultUserPeer(ISocketSession session)
        {
            var answer = new UserPeer();

            answer.SetPeerType(PeerType.Unknown);
            answer.SetAuthenticated(false);
            answer.SetSessionId(session.GetSessionId());

            return answer;
        }

    }

    /// <summary>
    /// Handles the OperationHandshakeAck by logging a warning since only the client should handle it.
    /// </summary>
    class OperationHandshakeAckHandler : AbstractOperationModelHandler<OperationHandshakeAck>
    {
        /// <summary>
        /// Logs a warning that OperationHandshakeAck should only be handled by the client.
        /// </summary>
        public override void Handle(ISocketSession session, OperationHandshakeAck operationHandshakeAck, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Only client can handle OperationHandshakeAck");
        }

    }

    /// <summary>
    /// Handles the OperationDisconnect by logging a warning since only the client should handle it.
    /// </summary>
    class OperationDisconnectHandler : AbstractOperationModelHandler<OperationDisconnect>
    {
        /// <summary>
        /// Logs a warning that OperationDisconnect should only be handled by the client.
        /// </summary>
        public override void Handle(ISocketSession session, OperationDisconnect operationDisconnect, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Only client can handle OperationDisconnect");
        }

    }

}

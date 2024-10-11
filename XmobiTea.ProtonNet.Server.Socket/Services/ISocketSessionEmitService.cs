using System.Collections.Generic;
using System.IO;
using XmobiTea.Bean.Attributes;
using XmobiTea.Linq;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.ProtonNet.Server.Socket.Types;

namespace XmobiTea.ProtonNet.Server.Socket.Services
{
    /// <summary>
    /// Defines the interface for a service responsible for emitting various operations (events, responses, requests, etc.)
    /// to socket sessions.
    /// </summary>
    public interface ISocketSessionEmitService
    {
        /// <summary>
        /// Sends an operation event to multiple sessions based on the specified options.
        /// </summary>
        /// <param name="options">The options for sending the operation event.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        void SendOperationEvent(SendOperationEventOptions options, OperationEvent operationEvent, SendParameters sendParameters);

        /// <summary>
        /// Sends an operation event to a single session.
        /// </summary>
        /// <param name="session">The session to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationEvent(ISocketSession session, OperationEvent operationEvent, SendParameters sendParameters);

        /// <summary>
        /// Sends an operation event to multiple sessions.
        /// </summary>
        /// <param name="sessions">The sessions to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>An enumerable of send results for each session.</returns>
        IEnumerable<SendResult> SendOperationEvent(IEnumerable<ISocketSession> sessions, OperationEvent operationEvent, SendParameters sendParameters);

        /// <summary>
        /// Sends an operation response to a single session.
        /// </summary>
        /// <param name="session">The session to send the response to.</param>
        /// <param name="operationResponse">The operation response to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationResponse(ISocketSession session, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null);

        /// <summary>
        /// Sends an operation request to a single session.
        /// </summary>
        /// <param name="session">The session to send the request to.</param>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationRequest(ISocketSession session, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null);

        /// <summary>
        /// Sends a ping operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the ping to.</param>
        /// <param name="operationPing">The ping operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationPing(ISocketSession session, OperationPing operationPing, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a pong operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the pong to.</param>
        /// <param name="operationPong">The pong operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationPong(ISocketSession session, OperationPong operationPong, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a handshake operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the handshake to.</param>
        /// <param name="operationHandshake">The handshake operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationHandshake(ISocketSession session, OperationHandshake operationHandshake, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a handshake acknowledgment operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the handshake acknowledgment to.</param>
        /// <param name="operationHandshakeAck">The handshake acknowledgment operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationHandshakeAck(ISocketSession session, OperationHandshakeAck operationHandshakeAck, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a disconnect operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the disconnect to.</param>
        /// <param name="operationDisconnect">The disconnect operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        SendResult SendOperationDisconnect(ISocketSession session, OperationDisconnect operationDisconnect, ProtocolProviderType? protocolProviderType = null);

    }

    /// <summary>
    /// Represents options for sending an operation event, including the targeted receivers and protocols.
    /// </summary>
    public class SendOperationEventOptions
    {
        /// <summary>
        /// Defines the options for specifying the receivers of the operation event.
        /// </summary>
        public class ReceiverOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether the event should be sent to all online users.
            /// </summary>
            public bool IsAllOnline { get; set; }

            /// <summary>
            /// Gets or sets the list of user IDs to which the event should be sent.
            /// </summary>
            public IEnumerable<string> UserIds { get; set; }

            /// <summary>
            /// Gets or sets the list of channel IDs to which the event should be sent.
            /// </summary>
            public IEnumerable<string> ChannelIds { get; set; }

            /// <summary>
            /// Gets or sets the list of sessions to which the event should be sent.
            /// </summary>
            public IEnumerable<ISocketSession> Sessions { get; set; }

            /// <summary>
            /// Sets the option to send the event to all online users.
            /// </summary>
            /// <param name="isAllOnline">True to send to all online users, otherwise false.</param>
            /// <returns>The current instance of <see cref="ReceiverOptions"/>.</returns>
            public ReceiverOptions SetIsAllOnline(bool isAllOnline)
            {
                this.IsAllOnline = isAllOnline;
                return this;
            }

            /// <summary>
            /// Sets the list of user IDs to which the event should be sent.
            /// </summary>
            /// <param name="userIds">The list of user IDs.</param>
            /// <returns>The current instance of <see cref="ReceiverOptions"/>.</returns>
            public ReceiverOptions SetUserIds(IEnumerable<string> userIds)
            {
                this.UserIds = userIds;
                return this;
            }

            /// <summary>
            /// Sets the list of channel IDs to which the event should be sent.
            /// </summary>
            /// <param name="channelIds">The list of channel IDs.</param>
            /// <returns>The current instance of <see cref="ReceiverOptions"/>.</returns>
            public ReceiverOptions SetChannelIds(IEnumerable<string> channelIds)
            {
                this.ChannelIds = channelIds;
                return this;
            }

            /// <summary>
            /// Sets the list of sessions to which the event should be sent.
            /// </summary>
            /// <param name="sessions">The list of sessions.</param>
            /// <returns>The current instance of <see cref="ReceiverOptions"/>.</returns>
            public ReceiverOptions SetSessions(IEnumerable<ISocketSession> sessions)
            {
                this.Sessions = sessions;
                return this;
            }
        }

        /// <summary>
        /// Defines the options for specifying the protocols through which the operation event should be sent.
        /// </summary>
        public class ReceiverProtocolOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether the event should be sent through all available protocols.
            /// </summary>
            public bool IsAllProtocol { get; set; }

            /// <summary>
            /// Gets or sets the list of protocols through which the event should be sent.
            /// </summary>
            public IEnumerable<TransportProtocol> Protocols { get; set; }

            /// <summary>
            /// Sets the option to send the event through all available protocols.
            /// </summary>
            /// <param name="isAllProtocol">True to send through all protocols, otherwise false.</param>
            /// <returns>The current instance of <see cref="ReceiverProtocolOptions"/>.</returns>
            public ReceiverProtocolOptions SetIsAllProtocol(bool isAllProtocol)
            {
                this.IsAllProtocol = isAllProtocol;
                return this;
            }

            /// <summary>
            /// Sets the list of protocols through which the event should be sent.
            /// </summary>
            /// <param name="protocols">The list of protocols.</param>
            /// <returns>The current instance of <see cref="ReceiverProtocolOptions"/>.</returns>
            public ReceiverProtocolOptions SetProtocols(IEnumerable<TransportProtocol> protocols)
            {
                this.Protocols = protocols;
                return this;
            }
        }

        /// <summary>
        /// Gets or sets the options for specifying the receivers of the operation event.
        /// </summary>
        public ReceiverOptions Receiver { get; set; }

        /// <summary>
        /// Gets or sets the options for specifying the protocols through which the event should be sent.
        /// </summary>
        public ReceiverProtocolOptions ReceiverProtocol { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendOperationEventOptions"/> class.
        /// </summary>
        public SendOperationEventOptions()
        {
            this.Receiver = new ReceiverOptions();
            this.ReceiverProtocol = new ReceiverProtocolOptions();
        }

        /// <summary>
        /// Sets the options for specifying the receivers of the operation event.
        /// </summary>
        /// <param name="receiver">The receiver options.</param>
        /// <returns>The current instance of <see cref="SendOperationEventOptions"/>.</returns>
        public SendOperationEventOptions SetReceiver(ReceiverOptions receiver)
        {
            this.Receiver = receiver;
            return this;
        }

        /// <summary>
        /// Sets the options for specifying the protocols through which the event should be sent.
        /// </summary>
        /// <param name="receiverProtocol">The receiver protocol options.</param>
        /// <returns>The current instance of <see cref="SendOperationEventOptions"/>.</returns>
        public SendOperationEventOptions SetReceiverProtocol(ReceiverProtocolOptions receiverProtocol)
        {
            this.ReceiverProtocol = receiverProtocol;
            return this;
        }

    }

    /// <summary>
    /// Implements the <see cref="ISocketSessionEmitService"/> interface to handle the emission of various operations
    /// (events, responses, etc.) to socket sessions, including encryption and protocol handling.
    /// </summary>
    class SocketSessionEmitService : ISocketSessionEmitService
    {
        private static readonly ProtocolProviderType DefaultProtocolProviderType = ProtocolProviderType.MessagePack;
        private static readonly CryptoProviderType DefaultCryptoProviderType = CryptoProviderType.Aes;

        private ILogger logger { get; }

        /// <summary>
        /// Automatically binds the RPC protocol service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private IRpcProtocolService rpcProtocolService { get; set; }

        /// <summary>
        /// Automatically binds the session service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private ISessionService sessionService { get; set; }

        /// <summary>
        /// Automatically binds the channel service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private IChannelService channelService { get; set; }

        /// <summary>
        /// Automatically binds the user peer service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private IUserPeerService userPeerService { get; set; }

        private int sendBufferSize = int.MaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSessionEmitService"/> class.
        /// </summary>
        public SocketSessionEmitService() => this.logger = LogManager.GetLogger(this);

        /// <summary>
        /// Sends an operation event to multiple sessions based on the specified options.
        /// </summary>
        /// <param name="options">The options for sending the operation event.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        public void SendOperationEvent(SendOperationEventOptions options, OperationEvent operationEvent, SendParameters sendParameters)
        {
            var sessions = this.GetSessions(options);

            this.SendOperationEvent(sessions, operationEvent, sendParameters);
        }

        /// <summary>
        /// Sends an operation event to a single session.
        /// </summary>
        /// <param name="session">The session to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationEvent(ISocketSession session, OperationEvent operationEvent, SendParameters sendParameters)
        {
            if (sendParameters.Encrypted)
                return this.SendEncryptOperationModel(session, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType, DefaultCryptoProviderType);

            return this.SendUnencryptOperationModel(session, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType);
        }

        /// <summary>
        /// Sends an operation event to multiple sessions.
        /// </summary>
        /// <param name="sessions">The sessions to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>An enumerable of send results for each session.</returns>
        public IEnumerable<SendResult> SendOperationEvent(IEnumerable<ISocketSession> sessions, OperationEvent operationEvent, SendParameters sendParameters)
        {
            var answer = !sendParameters.Encrypted ?
                this.SendUnencryptOperationEvent(sessions, operationEvent, sendParameters) :
                this.SendEncryptOperationEvent(sessions, operationEvent, sendParameters);

            return answer;
        }

        /// <summary>
        /// Sends an operation response to a single session.
        /// </summary>
        /// <param name="session">The session to send the response to.</param>
        /// <param name="operationResponse">The operation response to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationResponse(ISocketSession session, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType? protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            if (sendParameters.Encrypted)
                return this.SendEncryptOperationModel(session, OperationType.OperationResponse, operationResponse, sendParameters, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType), cryptoProviderType.GetValueOrDefault(DefaultCryptoProviderType));

            return this.SendUnencryptOperationModel(session, OperationType.OperationResponse, operationResponse, sendParameters, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));
        }

        /// <summary>
        /// Sends an operation request to a single session.
        /// </summary>
        /// <param name="session">The session to send the request to.</param>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationRequest(ISocketSession session, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType? protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Server can not send OperationRequest to client");

            return SendResult.Failed;
        }

        /// <summary>
        /// Sends a ping operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the ping to.</param>
        /// <param name="operationPing">The ping operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationPing(ISocketSession session, OperationPing operationPing, ProtocolProviderType? protocolProviderType) => this.SendUnencryptOperationModel(session, OperationType.OperationPing, operationPing, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Sends a pong operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the pong to.</param>
        /// <param name="operationPong">The pong operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationPong(ISocketSession session, OperationPong operationPong, ProtocolProviderType? protocolProviderType) => this.SendUnencryptOperationModel(session, OperationType.OperationPong, operationPong, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Sends a handshake operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the handshake to.</param>
        /// <param name="operationHandshake">The handshake operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationHandshake(ISocketSession session, OperationHandshake operationHandshake, ProtocolProviderType? protocolProviderType)
        {
            this.logger.Warn("Server can not send OperationHandshake to client");

            return SendResult.Failed;
        }

        /// <summary>
        /// Sends a handshake acknowledgment operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the handshake acknowledgment to.</param>
        /// <param name="operationHandshakeAck">The handshake acknowledgment operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationHandshakeAck(ISocketSession session, OperationHandshakeAck operationHandshakeAck, ProtocolProviderType? protocolProviderType) => this.SendUnencryptOperationModel(session, OperationType.OperationHandshakeAck, operationHandshakeAck, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Sends a disconnect operation to a single session.
        /// </summary>
        /// <param name="session">The session to send the disconnect to.</param>
        /// <param name="operationDisconnect">The disconnect operation to send.</param>
        /// <param name="protocolProviderType">The protocol provider type, if any.</param>
        /// <returns>The result of the send operation.</returns>
        public SendResult SendOperationDisconnect(ISocketSession session, OperationDisconnect operationDisconnect, ProtocolProviderType? protocolProviderType) => this.SendUnencryptOperationModel(session, OperationType.OperationDisconnect, operationDisconnect, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Retrieves the sessions based on the specified <see cref="SendOperationEventOptions"/>.
        /// </summary>
        /// <param name="options">The options for selecting the sessions.</param>
        /// <returns>An enumerable of the selected sessions.</returns>
        private IEnumerable<ISocketSession> GetSessions(SendOperationEventOptions options)
        {
            if (options.Receiver == null) throw new System.MissingMemberException("Receiver must have data");
            if (options.ReceiverProtocol == null) throw new System.MissingMemberException("ReceiverProtocol must have data");

            var answer = new List<ISocketSession>();

            if (options.Receiver.IsAllOnline)
            {
                if (options.ReceiverProtocol.IsAllProtocol)
                    answer.AddRange(this.sessionService.GetSessions().Cast<ISocketSession>());
                else
                    answer.AddRange(this.sessionService.GetSessions().Cast<ISocketSession>().Where(x => options.ReceiverProtocol.Protocols.Contains(x.GetTransportProtocol())));
            }
            else
            {
                var userIds = new List<string>();

                if (options.Receiver.UserIds != null)
                    foreach (var userId in options.Receiver.UserIds)
                        if (!userIds.Contains(userId))
                            userIds.Add(userId);

                if (options.Receiver.ChannelIds != null)
                {
                    foreach (var channelId in options.Receiver.ChannelIds)
                    {
                        var channelUserIds = this.channelService.GetUserIdsInChannel(channelId);
                        foreach (var userId in channelUserIds)
                            if (!userIds.Contains(userId))
                                userIds.Add(userId);
                    }
                }

                if (options.Receiver.Sessions != null)
                {
                    foreach (var session in options.Receiver.Sessions)
                    {
                        if (!answer.Contains(session))
                            answer.Add(session);
                    }
                }

                if (options.ReceiverProtocol.IsAllProtocol)
                {
                    foreach (var userId in userIds)
                    {
                        var userPeer = this.userPeerService.GetUserPeer(userId);

                        if (userPeer == null) continue;

                        var sessions = this.sessionService.GetSessions(userPeer.GetSessionId());

                        if (sessions != null)
                        {
                            foreach (ISocketSession session in sessions)
                            {
                                if (!answer.Contains(session))
                                    answer.Add(session);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var userId in userIds)
                    {
                        var userPeer = this.userPeerService.GetUserPeer(userId);

                        if (userPeer == null) continue;

                        var sessions = this.sessionService.GetSessions(userPeer.GetSessionId());

                        if (sessions != null)
                        {
                            var sessionsNeedAdd = sessions.Cast<ISocketSession>().Where(x => options.ReceiverProtocol.Protocols.Contains(x.GetTransportProtocol()));

                            foreach (ISocketSession session in sessionsNeedAdd)
                            {
                                if (!answer.Contains(session))
                                    answer.Add(session);
                            }
                        }
                    }
                }
            }

            return answer;
        }

        /// <summary>
        /// Sends an encrypted operation event to multiple sessions.
        /// </summary>
        /// <param name="sessions">The sessions to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>An enumerable of send results for each session.</returns>
        private IEnumerable<SendResult> SendEncryptOperationEvent(IEnumerable<ISocketSession> sessions, OperationEvent operationEvent, SendParameters sendParameters)
        {
            var answer = new List<SendResult>();

            foreach (var session in sessions)
            {
                answer.Add(this.SendEncryptOperationModel(session, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType, DefaultCryptoProviderType));
            }

            return answer;
        }

        /// <summary>
        /// Sends an unencrypted operation event to multiple sessions.
        /// </summary>
        /// <param name="sessions">The sessions to send the event to.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <returns>An enumerable of send results for each session.</returns>
        private IEnumerable<SendResult> SendUnencryptOperationEvent(IEnumerable<ISocketSession> sessions, OperationEvent operationEvent, SendParameters sendParameters)
        {
            var answer = new List<SendResult>();

            byte[] dataResponse;

            using (var mStream = new MemoryStream())
            {
                this.rpcProtocolService.Write(mStream, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType);

                dataResponse = mStream.ToArray();
            }

            foreach (var session in sessions)
            {
                if (session == null)
                    answer.Add(SendResult.SessionNull);
                else if (!session.IsConnected())
                    answer.Add(SendResult.Disconnected);
                else if (dataResponse.Length > this.sendBufferSize)
                    answer.Add(SendResult.MessageTooBig);
                else if (!session.SendAsync(dataResponse))
                    answer.Add(SendResult.SendBufferFull);
                else
                    answer.Add(SendResult.Ok);
            }

            return answer;
        }

        /// <summary>
        /// Sends an encrypted operation model to a session.
        /// </summary>
        /// <param name="session">The session to send the operation model to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use.</param>
        /// <param name="cryptoProviderType">The crypto provider type to use.</param>
        /// <returns>The result of the send operation.</returns>
        private SendResult SendEncryptOperationModel(ISocketSession session, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType)
        {
            if (session == null) return SendResult.SessionNull;

            if (!session.IsConnected()) return SendResult.Disconnected;

            if (session.GetEncryptKey() == null) return SendResult.EncryptionNotSupported;

            byte[] buffer;

            using (var mStream = new MemoryStream())
            {
                this.rpcProtocolService.WriteEncrypt(mStream, operationType, operationModel, sendParameters, protocolProviderType, cryptoProviderType, session.GetEncryptKey());

                buffer = mStream.ToArray();
            }

            if (buffer.Length > this.sendBufferSize) return SendResult.MessageTooBig;
            if (sendParameters.Sync) session.Send(buffer);
            else if (!session.SendAsync(buffer)) return SendResult.SendBufferFull;

            return SendResult.Ok;
        }

        /// <summary>
        /// Sends an unencrypted operation model to a session.
        /// </summary>
        /// <param name="session">The session to send the operation model to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to send.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use.</param>
        /// <returns>The result of the send operation.</returns>
        private SendResult SendUnencryptOperationModel(ISocketSession session, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType)
        {
            if (session == null) return SendResult.SessionNull;

            if (!session.IsConnected()) return SendResult.Disconnected;

            if (sendParameters == null) sendParameters = new SendParameters();

            byte[] buffer;

            using (var mStream = new MemoryStream())
            {
                this.rpcProtocolService.Write(mStream, operationType, operationModel, sendParameters, protocolProviderType);

                buffer = mStream.ToArray();
            }

            if (buffer.Length > this.sendBufferSize) return SendResult.MessageTooBig;
            if (sendParameters.Sync) session.Send(buffer);
            else if (!session.SendAsync(buffer)) return SendResult.SendBufferFull;

            return SendResult.Ok;
        }

    }

}

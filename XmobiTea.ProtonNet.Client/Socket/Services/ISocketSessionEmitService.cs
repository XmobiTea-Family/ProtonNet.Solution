using System.IO;
using XmobiTea.Bean.Attributes;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Client.Services;
using XmobiTea.ProtonNet.Client.Socket.Clients;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.Client.Socket.Services
{
    /// <summary>
    /// Interface for a service that handles emitting various types of socket operations, 
    /// such as events, requests, responses, and protocol-specific operations like ping, pong, and handshakes.
    /// </summary>
    public interface ISocketSessionEmitService
    {
        /// <summary>
        /// Sends an operation event to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the event.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">Parameters controlling how the event is sent.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationEvent(ISocketClient client, OperationEvent operationEvent, SendParameters sendParameters);

        /// <summary>
        /// Sends an operation response to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the response.</param>
        /// <param name="operationResponse">The operation response to send.</param>
        /// <param name="sendParameters">Parameters controlling how the response is sent.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <param name="cryptoProviderType">Optional crypto provider type for encryption.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationResponse(ISocketClient client, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null);

        /// <summary>
        /// Sends an operation request to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the request.</param>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">Parameters controlling how the request is sent.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <param name="cryptoProviderType">Optional crypto provider type for encryption.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationRequest(ISocketClient client, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null);

        /// <summary>
        /// Sends a ping operation to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the ping.</param>
        /// <param name="operationPing">The ping operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationPing(ISocketClient client, OperationPing operationPing, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a pong operation to the server in response to a ping.
        /// </summary>
        /// <param name="client">The socket client used to send the pong.</param>
        /// <param name="operationPong">The pong operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationPong(ISocketClient client, OperationPong operationPong, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a handshake operation to the server to initiate a session.
        /// </summary>
        /// <param name="client">The socket client used to send the handshake.</param>
        /// <param name="operationHandshake">The handshake operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationHandshake(ISocketClient client, OperationHandshake operationHandshake, ProtocolProviderType? protocolProviderType = null);

        /// <summary>
        /// Sends a handshake acknowledgment operation to the server in response to a handshake.
        /// </summary>
        /// <param name="client">The socket client used to send the handshake acknowledgment.</param>
        /// <param name="operationHandshakeAck">The handshake acknowledgment operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        SendResult SendOperationHandshakeAck(ISocketClient client, OperationHandshakeAck operationHandshakeAck, ProtocolProviderType? protocolProviderType = null);

    }

    /// <summary>
    /// Implements the <see cref="ISocketSessionEmitService"/> interface to handle the emission of various socket operations, 
    /// including events, requests, pings, pongs, and handshakes.
    /// </summary>
    class SocketSessionEmitService : ISocketSessionEmitService
    {
        /// <summary>
        /// Default protocol provider type used for serialization.
        /// </summary>
        private static readonly ProtocolProviderType DefaultProtocolProviderType = ProtocolProviderType.MessagePack;

        /// <summary>
        /// Default crypto provider type used for encryption.
        /// </summary>
        private static readonly CryptoProviderType DefaultCryptoProviderType = CryptoProviderType.Aes;

        /// <summary>
        /// Logger instance for logging messages.
        /// </summary>
        private ILogger logger { get; }

        /// <summary>
        /// Service responsible for handling the RPC protocol.
        /// </summary>
        [AutoBind]
        private IRpcProtocolService rpcProtocolService { get; set; }

        /// <summary>
        /// Maximum buffer size for sending messages.
        /// </summary>
        private int sendBufferSize = int.MaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSessionEmitService"/> class.
        /// </summary>
        public SocketSessionEmitService() => this.logger = LogManager.GetLogger(this);

        /// <summary>
        /// Sends an operation event to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the event.</param>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">Parameters controlling how the event is sent.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        public SendResult SendOperationEvent(ISocketClient client, OperationEvent operationEvent, SendParameters sendParameters)
        {
            if (sendParameters.Encrypted)
                return this.SendEncryptOperationModel(client, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType, DefaultCryptoProviderType);

            return this.SendUnencryptOperationModel(client, OperationType.OperationEvent, operationEvent, sendParameters, DefaultProtocolProviderType);
        }

        /// <summary>
        /// Logs a warning since clients should not send operation responses to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the response.</param>
        /// <param name="operationResponse">The operation response to send.</param>
        /// <param name="sendParameters">Parameters controlling how the response is sent.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <param name="cryptoProviderType">Optional crypto provider type for encryption.</param>
        /// <returns>A <see cref="SendResult"/> indicating the operation failed.</returns>
        public SendResult SendOperationResponse(ISocketClient client, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null)
        {
            this.logger.Warn("Client cannot send OperationResponse to server");
            return SendResult.Failed;
        }

        /// <summary>
        /// Sends an operation request to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the request.</param>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">Parameters controlling how the request is sent.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <param name="cryptoProviderType">Optional crypto provider type for encryption.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        public SendResult SendOperationRequest(ISocketClient client, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType? protocolProviderType = null, CryptoProviderType? cryptoProviderType = null)
        {
            if (sendParameters.Encrypted)
                return this.SendEncryptOperationModel(client, OperationType.OperationRequest, operationRequest, sendParameters, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType), cryptoProviderType.GetValueOrDefault(DefaultCryptoProviderType));

            return this.SendUnencryptOperationModel(client, OperationType.OperationRequest, operationRequest, sendParameters, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));
        }

        /// <summary>
        /// Sends a ping operation to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the ping.</param>
        /// <param name="operationPing">The ping operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        public SendResult SendOperationPing(ISocketClient client, OperationPing operationPing, ProtocolProviderType? protocolProviderType = null) => this.SendUnencryptOperationModel(client, OperationType.OperationPing, operationPing, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Sends a pong operation to the server in response to a ping.
        /// </summary>
        /// <param name="client">The socket client used to send the pong.</param>
        /// <param name="operationPong">The pong operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        public SendResult SendOperationPong(ISocketClient client, OperationPong operationPong, ProtocolProviderType? protocolProviderType = null) => this.SendUnencryptOperationModel(client, OperationType.OperationPong, operationPong, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Sends a handshake operation to the server to initiate a session.
        /// </summary>
        /// <param name="client">The socket client used to send the handshake.</param>
        /// <param name="operationHandshake">The handshake operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        public SendResult SendOperationHandshake(ISocketClient client, OperationHandshake operationHandshake, ProtocolProviderType? protocolProviderType = null) => this.SendUnencryptOperationModel(client, OperationType.OperationHandshake, operationHandshake, default, protocolProviderType.GetValueOrDefault(DefaultProtocolProviderType));

        /// <summary>
        /// Logs a warning since clients should not send handshake acknowledgment operations to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the handshake acknowledgment.</param>
        /// <param name="operationHandshakeAck">The handshake acknowledgment operation to send.</param>
        /// <param name="protocolProviderType">Optional protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the operation failed.</returns>
        public SendResult SendOperationHandshakeAck(ISocketClient client, OperationHandshakeAck operationHandshakeAck, ProtocolProviderType? protocolProviderType = null)
        {
            this.logger.Warn("Client cannot send OperationHandshakeAck to server");
            return SendResult.Failed;
        }

        /// <summary>
        /// Sends an encrypted operation model to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the operation.</param>
        /// <param name="operationType">The type of operation being sent.</param>
        /// <param name="operationModel">The operation model to send.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type for encryption.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        private SendResult SendEncryptOperationModel(ISocketClient client, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType)
        {
            if (client == null) return SendResult.SessionNull;
            if (!client.IsConnected()) return SendResult.Disconnected;
            if (client.GetEncryptKey() == null) return SendResult.EncryptionNotSupported;

            byte[] buffer;

            using (var mStream = new MemoryStream())
            {
                this.rpcProtocolService.WriteEncrypt(mStream, operationType, operationModel, sendParameters, protocolProviderType, cryptoProviderType, client.GetEncryptKey());
                buffer = mStream.ToArray();
            }

            if (buffer.Length > this.sendBufferSize) return SendResult.MessageTooBig;
            if (sendParameters.Sync) client.Send(buffer);
            else if (!client.SendAsync(buffer)) return SendResult.SendBufferFull;

            return SendResult.Ok;
        }

        /// <summary>
        /// Sends an unencrypted operation model to the server.
        /// </summary>
        /// <param name="client">The socket client used to send the operation.</param>
        /// <param name="operationType">The type of operation being sent.</param>
        /// <param name="operationModel">The operation model to send.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type for serialization.</param>
        /// <returns>A <see cref="SendResult"/> indicating the result of the send operation.</returns>
        private SendResult SendUnencryptOperationModel(ISocketClient client, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType)
        {
            if (client == null) return SendResult.SessionNull;
            if (!client.IsConnected()) return SendResult.Disconnected;

            byte[] buffer;

            using (var mStream = new MemoryStream())
            {
                this.rpcProtocolService.Write(mStream, operationType, operationModel, sendParameters, protocolProviderType);
                buffer = mStream.ToArray();
            }

            if (buffer.Length > this.sendBufferSize) return SendResult.MessageTooBig;
            if (sendParameters.Sync) client.Send(buffer);
            else if (!client.SendAsync(buffer)) return SendResult.SendBufferFull;

            return SendResult.Ok;
        }

    }

}

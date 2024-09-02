using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.Client.Socket.Handlers
{
    /// <summary>
    /// Interface for handling socket operation models of any type.
    /// </summary>
    interface ISocketOperationModelHandler
    {
        /// <summary>
        /// Handles a socket operation model.
        /// </summary>
        /// <param name="socketClientPeer">The client peer handling the operation.</param>
        /// <param name="operationModel">The operation model to be handled.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is handled.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        void Handle(ISocketClientPeer socketClientPeer, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Generic interface for handling specific types of socket operation models.
    /// </summary>
    /// <typeparam name="TOperationModel">The type of the operation model to be handled.</typeparam>
    interface ISocketOperationModelHandler<TOperationModel> where TOperationModel : IOperationModel
    {
        /// <summary>
        /// Handles a specific type of socket operation model.
        /// </summary>
        /// <param name="socketClientPeer">The client peer handling the operation.</param>
        /// <param name="operationModel">The specific operation model to be handled.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is handled.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        void Handle(ISocketClientPeer socketClientPeer, TOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Abstract base class for handling specific types of socket operation models.
    /// </summary>
    /// <typeparam name="TOperationModel">The type of the operation model to be handled.</typeparam>
    abstract class AbstractOperationModelHandler<TOperationModel> : ISocketOperationModelHandler<TOperationModel>, ISocketOperationModelHandler where TOperationModel : IOperationModel
    {
        /// <summary>
        /// Logger instance for logging messages.
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
        /// Handles a socket operation model by casting it to the specific type <typeparamref name="TOperationModel"/>.
        /// </summary>
        /// <param name="socketClientPeer">The client peer handling the operation.</param>
        /// <param name="operationModel">The operation model to be handled.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is handled.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        void ISocketOperationModelHandler.Handle(ISocketClientPeer socketClientPeer, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
            => this.Handle(socketClientPeer, (TOperationModel)operationModel, sendParameters, protocolProviderType, cryptoProviderType);

        /// <summary>
        /// Abstract method for handling a specific type of socket operation model.
        /// </summary>
        /// <param name="socketClientPeer">The client peer handling the operation.</param>
        /// <param name="operationModel">The specific operation model to be handled.</param>
        /// <param name="sendParameters">Parameters controlling how the operation is handled.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        public abstract void Handle(ISocketClientPeer socketClientPeer, TOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Handles <see cref="OperationRequest"/> instances, typically issued by clients.
    /// </summary>
    class OperationRequestHandler : AbstractOperationModelHandler<OperationRequest>
    {
        /// <summary>
        /// Logs a warning as only the server should handle <see cref="OperationRequest"/>.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationRequest operationRequest, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Only server can handle OperationRequest");
        }

    }

    /// <summary>
    /// Handles <see cref="OperationResponse"/> instances, typically issued by servers.
    /// </summary>
    class OperationResponseHandler : AbstractOperationModelHandler<OperationResponse>
    {
        /// <summary>
        /// Handles an <see cref="OperationResponse"/> by invoking the corresponding internal method on the client peer.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationResponse operationResponse, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationResponseInternal(operationResponse);
        }

    }

    /// <summary>
    /// Handles <see cref="OperationEvent"/> instances, typically issued by servers to notify clients of events.
    /// </summary>
    class OperationEventHandler : AbstractOperationModelHandler<OperationEvent>
    {
        /// <summary>
        /// Handles an <see cref="OperationEvent"/> by invoking the corresponding internal method on the client peer.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationEvent operationEvent, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationEventInternal(operationEvent);
        }

    }

    /// <summary>
    /// Handles <see cref="OperationPing"/> instances, typically issued by servers to check the client's connection status.
    /// </summary>
    class OperationPingHandler : AbstractOperationModelHandler<OperationPing>
    {
        /// <summary>
        /// Handles an <see cref="OperationPing"/> by responding with a pong operation.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationPing operationPing, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationPingInternal(operationPing);
        }

    }

    /// <summary>
    /// Handles <see cref="OperationPong"/> instances, typically issued by clients in response to a ping operation from the server.
    /// </summary>
    class OperationPongHandler : AbstractOperationModelHandler<OperationPong>
    {
        /// <summary>
        /// Handles an <see cref="OperationPong"/> by invoking the corresponding internal method on the client peer.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationPong operationPong, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationPongInternal(operationPong);
        }

    }

    /// <summary>
    /// Handles <see cref="OperationHandshake"/> instances, typically issued by clients to initiate a connection with the server.
    /// </summary>
    class OperationHandshakeHandler : AbstractOperationModelHandler<OperationHandshake>
    {
        /// <summary>
        /// Logs a warning as only the server should handle <see cref="OperationHandshake"/>.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationHandshake operationHandshake, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn("Only server can handle OperationHandshake");
        }

    }

    /// <summary>
    /// Handles <see cref="OperationHandshakeAck"/> instances, typically issued by servers to acknowledge a client's handshake request.
    /// </summary>
    class OperationHandshakeAckHandler : AbstractOperationModelHandler<OperationHandshakeAck>
    {
        /// <summary>
        /// Handles an <see cref="OperationHandshakeAck"/> by invoking the corresponding internal method on the client peer.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationHandshakeAck operationHandshakeAck, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationHandshakeAckInternal(operationHandshakeAck);
        }

    }

    /// <summary>
    /// Handles <see cref="OperationDisconnect"/> instances, typically issued by servers to notify clients of disconnection events.
    /// </summary>
    class OperationDisconnectHandler : AbstractOperationModelHandler<OperationDisconnect>
    {
        /// <summary>
        /// Handles an <see cref="OperationDisconnect"/> by logging a warning and invoking the corresponding internal method on the client peer.
        /// </summary>
        public override void Handle(ISocketClientPeer socketClientPeer, OperationDisconnect operationDisconnect, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            this.logger.Warn($"Server send disconnect to client with disconnect code: {operationDisconnect.Reason}, message: {operationDisconnect.Message}");
            ((SocketClientPeer)socketClientPeer).OnReceiveOperationDisconnectInternal(operationDisconnect);
        }

    }

}

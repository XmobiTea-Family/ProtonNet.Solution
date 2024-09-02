using XmobiTea.ProtonNet.Client.Socket.Handlers;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.Client.Socket.Services
{
    /// <summary>
    /// Interface for a service that handles socket operation models based on their operation type.
    /// </summary>
    public interface ISocketOperationModelService
    {
        /// <summary>
        /// Handles a socket operation model by dispatching it to the appropriate handler based on the operation type.
        /// </summary>
        /// <param name="socketClientPeer">The client peer that received the operation.</param>
        /// <param name="operationType">The type of operation to handle.</param>
        /// <param name="operationModel">The operation model to be handled.</param>
        /// <param name="sendParameters">The parameters used for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        void Handle(ISocketClientPeer socketClientPeer, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);
    }

    /// <summary>
    /// Implementation of <see cref="ISocketOperationModelService"/> that manages operation model handlers and processes incoming operations.
    /// </summary>
    class SocketOperationModelService : ISocketOperationModelService
    {
        /// <summary>
        /// Dictionary that maps operation types to their corresponding operation model handlers.
        /// </summary>
        private System.Collections.Generic.IDictionary<OperationType, ISocketOperationModelHandler> operationModelHandlerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketOperationModelService"/> class.
        /// </summary>
        public SocketOperationModelService()
        {
            this.operationModelHandlerDict = new System.Collections.Generic.Dictionary<OperationType, ISocketOperationModelHandler>();
        }

        /// <summary>
        /// Adds a handler for a specific operation type.
        /// </summary>
        /// <param name="operationType">The operation type for which the handler is added.</param>
        /// <param name="handler">The handler that processes the specified operation type.</param>
        public void AddHandler(OperationType operationType, ISocketOperationModelHandler handler) => this.operationModelHandlerDict.Add(operationType, handler);

        /// <summary>
        /// Retrieves the handler associated with the specified operation type.
        /// </summary>
        /// <param name="operationType">The type of operation to retrieve the handler for.</param>
        /// <returns>An instance of <see cref="ISocketOperationModelHandler"/> if a handler is found, otherwise null.</returns>
        private ISocketOperationModelHandler GetHandler(OperationType operationType)
        {
            this.operationModelHandlerDict.TryGetValue(operationType, out var handler);
            return handler;
        }

        /// <summary>
        /// Handles a socket operation model by dispatching it to the appropriate handler based on the operation type.
        /// </summary>
        /// <param name="socketClientPeer">The client peer that received the operation.</param>
        /// <param name="operationType">The type of operation to handle.</param>
        /// <param name="operationModel">The operation model to be handled.</param>
        /// <param name="sendParameters">The parameters used for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption, if applicable.</param>
        public void Handle(ISocketClientPeer socketClientPeer, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var handler = this.GetHandler(operationType);
            if (handler != null) handler.Handle(socketClientPeer, operationModel, sendParameters, protocolProviderType, cryptoProviderType);
        }

    }

}

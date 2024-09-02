using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNet.Server.Socket.Sessions;

namespace XmobiTea.ProtonNet.Server.Socket.Services
{
    /// <summary>
    /// Defines the interface for handling operations on socket sessions, allowing for the management
    /// and processing of different operation models based on their type.
    /// </summary>
    public interface ISocketOperationModelService
    {
        /// <summary>
        /// Handles an operation model for a given session.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationType">The type of operation being handled.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        void Handle(ISocketSession session, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType);

    }

    /// <summary>
    /// Implements the <see cref="ISocketOperationModelService"/> interface to manage and dispatch operations
    /// to the appropriate handlers based on the operation type.
    /// </summary>
    class SocketOperationModelService : ISocketOperationModelService
    {
        /// <summary>
        /// Gets the dictionary that maps operation types to their corresponding handlers.
        /// </summary>
        private System.Collections.Generic.IDictionary<OperationType, Handlers.ISocketOperationModelHandler> operationModelHandlerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketOperationModelService"/> class.
        /// </summary>
        public SocketOperationModelService() => this.operationModelHandlerDict = new System.Collections.Generic.Dictionary<OperationType, Handlers.ISocketOperationModelHandler>();

        /// <summary>
        /// Adds a handler for a specific operation type.
        /// </summary>
        /// <param name="operationType">The type of operation to handle.</param>
        /// <param name="handler">The handler responsible for processing the operation.</param>
        public void AddHandler(OperationType operationType, Handlers.ISocketOperationModelHandler handler) => this.operationModelHandlerDict.Add(operationType, handler);

        /// <summary>
        /// Retrieves the handler for a specific operation type.
        /// </summary>
        /// <param name="operationType">The type of operation.</param>
        /// <returns>The handler for the operation type, or null if no handler is registered.</returns>
        private Handlers.ISocketOperationModelHandler GetHandler(OperationType operationType)
        {
            this.operationModelHandlerDict.TryGetValue(operationType, out var handler);
            return handler;
        }

        /// <summary>
        /// Handles an operation model for a given session by dispatching it to the appropriate handler based on the operation type.
        /// </summary>
        /// <param name="session">The session that received the operation.</param>
        /// <param name="operationType">The type of operation being handled.</param>
        /// <param name="operationModel">The operation model containing the operation data.</param>
        /// <param name="sendParameters">The parameters controlling how the data is sent.</param>
        /// <param name="protocolProviderType">The protocol provider type to use for this operation.</param>
        /// <param name="cryptoProviderType">The crypto provider type, if any, to use for this operation.</param>
        public void Handle(ISocketSession session, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var handler = this.GetHandler(operationType);

            if (handler != null)
                handler.Handle(session, operationModel, sendParameters, protocolProviderType, cryptoProviderType);
        }

    }

}

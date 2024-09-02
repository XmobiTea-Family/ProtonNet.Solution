using XmobiTea.ProtonNet.Client.Socket.Handlers;
using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Socket.Services
{
    /// <summary>
    /// Interface for an event service that manages event handlers and processes incoming operation events.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Retrieves the event handler associated with the specified event code.
        /// </summary>
        /// <param name="code">The code of the event to handle.</param>
        /// <returns>An instance of <see cref="IEventHandler"/> that handles the specified event code, or null if no handler is found.</returns>
        IEventHandler GetHandler(string code);

        /// <summary>
        /// Processes an incoming operation event by passing it to the appropriate event handler.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">The parameters used for sending the event.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        void Handle(OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer);

    }

    /// <summary>
    /// Implementation of <see cref="IEventService"/> that manages event handlers and processes incoming operation events.
    /// </summary>
    class EventService : IEventService
    {
        /// <summary>
        /// Dictionary that maps event codes to their corresponding event handlers.
        /// </summary>
        private System.Collections.Generic.IDictionary<string, IEventHandler> handlerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        public EventService()
        {
            this.handlerDict = new System.Collections.Generic.Dictionary<string, IEventHandler>();
        }

        /// <summary>
        /// Adds an event handler to the service.
        /// </summary>
        /// <param name="eventHandler">The event handler to add.</param>
        public void AddHandler(IEventHandler eventHandler)
        {
            var code = eventHandler.GetCode();
            this.handlerDict[code] = eventHandler;
        }

        /// <summary>
        /// Removes the event handler associated with the specified event code.
        /// </summary>
        /// <param name="code">The code of the event handler to remove.</param>
        /// <returns>True if the handler was successfully removed, otherwise false.</returns>
        public bool RemoveHandler(string code)
        {
            return this.handlerDict.Remove(code);
        }

        /// <summary>
        /// Retrieves the event handler associated with the specified event code.
        /// </summary>
        /// <param name="code">The code of the event to handle.</param>
        /// <returns>An instance of <see cref="IEventHandler"/> that handles the specified event code, or null if no handler is found.</returns>
        public IEventHandler GetHandler(string code)
        {
            this.handlerDict.TryGetValue(code, out var answer);
            return answer;
        }

        /// <summary>
        /// Processes an incoming operation event by passing it to the appropriate event handler.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">The parameters used for sending the event.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        public void Handle(OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer)
        {
            var code = operationEvent.EventCode;

            var handler = this.GetHandler(code);
            if (handler != null) handler.Handle(operationEvent, sendParameters, clientPeer);
        }

    }

}

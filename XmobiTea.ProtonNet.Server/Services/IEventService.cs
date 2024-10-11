using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Handlers;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Types;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines methods for managing and handling events.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Gets the event handler for a specific event code.
        /// </summary>
        /// <param name="eventCode">The code of the event.</param>
        /// <returns>The event handler associated with the specified event code.</returns>
        IEventHandler GetHandler(string eventCode);

        /// <summary>
        /// Handles an operation event with the provided parameters.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer involved in the event.</param>
        /// <param name="session">The session in which the event occurred.</param>
        void Handle(OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session);

    }

    /// <summary>
    /// Implements <see cref="IEventService"/> to manage and handle events.
    /// </summary>
    public class EventService : IEventService
    {
        private ILogger logger { get; }

        private System.Collections.Generic.IDictionary<string, IEventHandler> handlerDict { get; }
        private System.Collections.Generic.IList<string> eventAllowAnonymousCodeLst { get; }
        private System.Collections.Generic.IList<string> eventOnlyServerCodeLst { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        public EventService()
        {
            this.logger = LogManager.GetLogger(this);

            this.handlerDict = new System.Collections.Generic.Dictionary<string, IEventHandler>();
            this.eventAllowAnonymousCodeLst = new System.Collections.Generic.List<string>();
            this.eventOnlyServerCodeLst = new System.Collections.Generic.List<string>();
        }

        /// <summary>
        /// Adds an event handler to the service.
        /// </summary>
        /// <param name="eventHandler">The event handler to add.</param>
        /// <param name="allowAnonymous">Indicates whether anonymous is allowed for this event.</param>
        /// <param name="onlyServer">Indicates whether the event is only for server-side handling.</param>
        public void AddHandler(IEventHandler eventHandler, bool allowAnonymous = false, bool onlyServer = false)
        {
            var eventCode = eventHandler.GetEventCode();

            if (this.handlerDict.ContainsKey(eventCode))
                this.logger.Warn("Change exists event handler: " + this.handlerDict[eventCode] + " to new event handler " + eventHandler);

            this.handlerDict[eventCode] = eventHandler;

            if (allowAnonymous) this.eventAllowAnonymousCodeLst.Add(eventCode);
            if (onlyServer) this.eventOnlyServerCodeLst.Add(eventCode);
        }

        /// <summary>
        /// Removes an event handler from the service.
        /// </summary>
        /// <param name="eventCode">The code of the event handler to remove.</param>
        /// <returns><c>true</c> if the handler was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveHandler(string eventCode)
        {
            return this.handlerDict.Remove(eventCode);
        }

        /// <summary>
        /// Gets the event handler for a specific event code.
        /// </summary>
        /// <param name="eventCode">The code of the event.</param>
        /// <returns>The event handler associated with the specified event code.</returns>
        public IEventHandler GetHandler(string eventCode)
        {
            this.handlerDict.TryGetValue(eventCode, out var answer);
            return answer;
        }

        /// <summary>
        /// Handles an operation event with the provided parameters.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer involved in the event.</param>
        /// <param name="session">The session in which the event occurred.</param>
        public void Handle(OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session)
        {
            var eventCode = operationEvent.EventCode;

            if (!this.eventAllowAnonymousCodeLst.Contains(eventCode) && !userPeer.IsAuthenticated())
                return;

            if ((userPeer.GetPeerType() == PeerType.SocketClient || userPeer.GetPeerType() == PeerType.WebApiClient) && this.eventOnlyServerCodeLst.Contains(eventCode))
                return;

            var handler = this.GetHandler(eventCode);
            if (handler != null) handler.Handle(operationEvent, sendParameters, userPeer, session);
        }

    }

}

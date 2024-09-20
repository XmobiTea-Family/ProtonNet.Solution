using System;
using XmobiTea.Bean.Attributes;
using XmobiTea.Data.Converter;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Handlers
{
    /// <summary>
    /// Defines the interface for event handlers.
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Gets the event code.
        /// </summary>
        /// <returns>The event code as a string.</returns>
        string GetEventCode();

        /// <summary>
        /// Handles the event with specified parameters.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the event.</param>
        /// <param name="session">The session associated with the event.</param>
        void Handle(OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session);

    }

    /// <summary>
    /// Provides a base implementation for event handlers.
    /// </summary>
    public abstract class EventHandler : IEventHandler
    {
        /// <summary>
        /// Logger instance for logging events.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandler"/> class.
        /// </summary>
        public EventHandler() => this.logger = LogManager.GetLogger(this);

        /// <summary>
        /// Gets the event code.
        /// </summary>
        /// <returns>The event code as a string.</returns>
        public abstract string GetEventCode();

        /// <summary>
        /// Handles the event with specified parameters.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the event.</param>
        /// <param name="session">The session associated with the event.</param>
        public abstract void Handle(OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session);

    }

    /// <summary>
    /// Provides a generic implementation for event handlers with a specific event model.
    /// </summary>
    /// <typeparam name="TEventModel">The type of the event model.</typeparam>
    public abstract class EventHandler<TEventModel> : EventHandler, IEventHandler
    {
        /// <summary>
        /// Data converter for converting data.
        /// </summary>
        [AutoBind]
        private IDataConverter dataConverter { get; set; }

        /// <summary>
        /// Handles the event with specified parameters and converts it to the event model.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the event.</param>
        /// <param name="session">The session associated with the event.</param>
        public override void Handle(OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session)
        {
            var eventModel = this.ConvertToRequestModel(operationEvent);

            if (eventModel == null)
                return;

            this.Handle(eventModel, operationEvent, sendParameters, userPeer, session);
        }

        /// <summary>
        /// Converts the operation event to the specific event model.
        /// </summary>
        /// <param name="operationEvent">The operation event to convert.</param>
        /// <returns>The event model, or default value if conversion fails.</returns>
        private TEventModel ConvertToRequestModel(OperationEvent operationEvent)
        {
            try
            {
                return this.dataConverter.DeserializeObject<TEventModel>(operationEvent.Parameters);
            }
            catch (Exception ex)
            {
                this.logger.Fatal(ex);
            }

            return default;
        }

        /// <summary>
        /// Handles the event with the specified event model.
        /// </summary>
        /// <param name="eventModel">The event model to handle.</param>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the event.</param>
        /// <param name="session">The session associated with the event.</param>
        public abstract void Handle(TEventModel eventModel, OperationEvent operationEvent, SendParameters sendParameters, IUserPeer userPeer, ISession session);

    }

}

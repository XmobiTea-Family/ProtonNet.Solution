using XmobiTea.Bean.Attributes;
using XmobiTea.Data.Converter;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Socket.Handlers
{
    /// <summary>
    /// Interface for handling events that occur during socket communication.
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Gets the code associated with this event handler, used to identify the event type.
        /// </summary>
        /// <returns>A string representing the event code.</returns>
        string GetCode();

        /// <summary>
        /// Handles the specified operation event.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters controlling how the event is handled.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        void Handle(OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer);

    }

    /// <summary>
    /// Abstract base class for handling events during socket communication.
    /// </summary>
    public abstract class EventHandler : IEventHandler
    {
        /// <summary>
        /// Logger instance for logging messages.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandler"/> class.
        /// </summary>
        public EventHandler()
        {
            this.logger = LogManager.GetLogger(this);
        }

        /// <summary>
        /// Gets the code associated with this event handler, used to identify the event type.
        /// </summary>
        /// <returns>A string representing the event code.</returns>
        public abstract string GetCode();

        /// <summary>
        /// Handles the specified operation event.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters controlling how the event is handled.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        public abstract void Handle(OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer);

    }

    /// <summary>
    /// Abstract generic class for handling specific types of events during socket communication.
    /// </summary>
    /// <typeparam name="TEventModel">The type of the event model to be handled.</typeparam>
    public abstract class EventHandler<TEventModel> : EventHandler, IEventHandler
    {
        /// <summary>
        /// Data converter for deserializing event parameters.
        /// </summary>
        [AutoBind]
        private IDataConverter dataConverter { get; set; }

        /// <summary>
        /// Handles the specified operation event by converting its parameters to the specific event model.
        /// </summary>
        /// <param name="operationEvent">The operation event to handle.</param>
        /// <param name="sendParameters">Parameters controlling how the event is handled.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        public override void Handle(OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer)
        {
            var eventModel = this.ConvertToRequestModel(operationEvent);

            if (eventModel == null)
                return;

            this.Handle(eventModel, operationEvent, sendParameters, clientPeer);
        }

        /// <summary>
        /// Converts the operation event's parameters to the specific event model.
        /// </summary>
        /// <param name="operationEvent">The operation event containing the parameters to convert.</param>
        /// <returns>An instance of <typeparamref name="TEventModel"/> representing the event model, or default if conversion fails.</returns>
        private TEventModel ConvertToRequestModel(OperationEvent operationEvent)
        {
            try
            {
                return this.dataConverter.DeserializeObject<TEventModel>(operationEvent.Parameters);
            }
            catch (System.Exception ex)
            {
                this.logger.Fatal(ex);
            }

            return default;
        }

        /// <summary>
        /// Abstract method for handling the specific event model after conversion.
        /// </summary>
        /// <param name="eventModel">The converted event model to handle.</param>
        /// <param name="operationEvent">The original operation event.</param>
        /// <param name="sendParameters">Parameters controlling how the event is handled.</param>
        /// <param name="clientPeer">The client peer that received the event.</param>
        public abstract void Handle(TEventModel eventModel, OperationEvent operationEvent, SendParameters sendParameters, ISocketClientPeer clientPeer);

    }

}

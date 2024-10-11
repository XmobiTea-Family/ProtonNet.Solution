using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Models
{
    /// <summary>
    /// Represents a pending operation event that is queued for sending to the server.
    /// Stores the event data and the parameters used for sending it.
    /// </summary>
    public class OperationEventPending
    {
        /// <summary>
        /// The operation event that is pending to be sent.
        /// </summary>
        private OperationEvent operationEvent { get; }

        /// <summary>
        /// The parameters used for sending the operation event.
        /// </summary>
        private SendParameters sendParameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEventPending"/> class.
        /// </summary>
        /// <param name="operationEvent">The operation event to be sent.</param>
        /// <param name="sendParameters">The parameters used for sending the event.</param>
        public OperationEventPending(OperationEvent operationEvent, SendParameters sendParameters)
        {
            this.operationEvent = operationEvent;
            this.sendParameters = sendParameters;
        }

        /// <summary>
        /// Gets the operation event that is pending to be sent.
        /// </summary>
        /// <returns>The <see cref="OperationEvent"/> that is pending.</returns>
        public OperationEvent GetOperationEvent() => this.operationEvent;

        /// <summary>
        /// Gets the parameters used for sending the operation event.
        /// </summary>
        /// <returns>The <see cref="SendParameters"/> used for sending the event.</returns>
        public SendParameters GetSendParameters() => this.sendParameters;

    }

}

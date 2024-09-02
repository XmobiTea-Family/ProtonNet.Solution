using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Models
{
    /// <summary>
    /// Represents a pending operation request that is queued for sending to the server.
    /// Stores the request data, parameters, and the callback for handling the server's response.
    /// </summary>
    public class OperationRequestPending
    {
        /// <summary>
        /// Constant for converting milliseconds to ticks.
        /// </summary>
        private static readonly int MillisecondsToTick = 10000;

        /// <summary>
        /// Constant for converting seconds to ticks.
        /// </summary>
        private static readonly int SecondsToTick = 1000 * MillisecondsToTick;

        /// <summary>
        /// The operation request that is pending to be sent.
        /// </summary>
        private OperationRequest operationRequest { get; }

        /// <summary>
        /// The parameters used for sending the operation request.
        /// </summary>
        private SendParameters sendParameters { get; }

        /// <summary>
        /// The callback action to be invoked when the server responds to the request.
        /// </summary>
        private System.Action<OperationResponse> onOperationResponse { get; }

        /// <summary>
        /// The timeout period for the operation request in seconds.
        /// </summary>
        private int timeoutInSeconds { get; }

        /// <summary>
        /// The response received from the server for this operation request.
        /// </summary>
        private OperationResponse operationResponse { get; set; }

        /// <summary>
        /// The tick count at which the request will timeout.
        /// </summary>
        private long tickEndTimeout { get; set; }

        /// <summary>
        /// The tick count when the request was sent.
        /// </summary>
        private long tickSend { get; set; }

        /// <summary>
        /// The tick count when the response was received.
        /// </summary>
        private long tickRecv { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationRequestPending"/> class.
        /// </summary>
        /// <param name="operationRequest">The operation request to be sent.</param>
        /// <param name="onOperationResponse">The callback to be invoked upon receiving the response.</param>
        /// <param name="sendParameters">The parameters used for sending the request.</param>
        /// <param name="timeoutInSeconds">The timeout period for the request in seconds.</param>
        public OperationRequestPending(OperationRequest operationRequest, System.Action<OperationResponse> onOperationResponse, SendParameters sendParameters, int timeoutInSeconds)
        {
            this.operationResponse = null;
            this.operationRequest = operationRequest;
            this.onOperationResponse = onOperationResponse;
            this.sendParameters = sendParameters;

            this.timeoutInSeconds = timeoutInSeconds;
            this.tickEndTimeout = System.DateTime.UtcNow.Ticks + this.timeoutInSeconds * SecondsToTick;
            this.tickSend = 0;
            this.tickRecv = 0;
        }

        /// <summary>
        /// Marks the request as sent and updates the send and timeout ticks.
        /// </summary>
        public void OnSend()
        {
            this.tickSend = System.DateTime.UtcNow.Ticks;
            this.tickEndTimeout = this.tickSend + this.timeoutInSeconds * SecondsToTick;
        }

        /// <summary>
        /// Marks the request as received by updating the receive tick count.
        /// </summary>
        public void OnRecv() => this.tickRecv = System.DateTime.UtcNow.Ticks;

        /// <summary>
        /// Sets the response received from the server for this operation request.
        /// </summary>
        /// <param name="operationResponse">The response from the server.</param>
        public void SetOperationResponse(OperationResponse operationResponse) => this.operationResponse = operationResponse;

        /// <summary>
        /// Gets the execution time in milliseconds for the operation request, 
        /// calculated from the time it was sent until the response was received.
        /// </summary>
        /// <returns>The execution time in milliseconds.</returns>
        public int GetExecuteTimerInMs() => (int)((this.tickRecv - this.tickSend) / MillisecondsToTick);

        /// <summary>
        /// Checks if the operation request has timed out.
        /// </summary>
        /// <returns>True if the request has timed out, otherwise false.</returns>
        public bool IsTimeout() => this.tickEndTimeout < System.DateTime.UtcNow.Ticks;

        /// <summary>
        /// Gets the operation request that is pending to be sent.
        /// </summary>
        /// <returns>The <see cref="OperationRequest"/> that is pending.</returns>
        public OperationRequest GetOperationRequest() => this.operationRequest;

        /// <summary>
        /// Gets the response received from the server for this operation request.
        /// </summary>
        /// <returns>The <see cref="OperationResponse"/> received from the server.</returns>
        public OperationResponse GetOperationResponse() => this.operationResponse;

        /// <summary>
        /// Gets the parameters used for sending the operation request.
        /// </summary>
        /// <returns>The <see cref="SendParameters"/> used for sending the request.</returns>
        public SendParameters GetSendParameters() => this.sendParameters;

        /// <summary>
        /// Gets the callback action to be invoked when the server responds to the request.
        /// </summary>
        /// <returns>The callback action for handling the response.</returns>
        public System.Action<OperationResponse> GetCallback() => this.onOperationResponse;

    }

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

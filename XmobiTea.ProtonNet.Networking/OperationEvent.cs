using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents an event operation in the networking layer.
    /// </summary>
    public class OperationEvent : IOperationModel
    {
        /// <summary>
        /// Gets or sets the event code that identifies the event.
        /// </summary>
        public string EventCode { get; set; }

        /// <summary>
        /// Gets or sets the parameters associated with the event.
        /// </summary>
        public GNHashtable Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class.
        /// </summary>
        public OperationEvent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class with a specific event code.
        /// </summary>
        /// <param name="eventCode">The event code that identifies the event.</param>
        public OperationEvent(string eventCode) => this.EventCode = eventCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEvent"/> class with a specific event code and parameters.
        /// </summary>
        /// <param name="eventCode">The event code that identifies the event.</param>
        /// <param name="parameters">The parameters associated with the event.</param>
        public OperationEvent(string eventCode, GNHashtable parameters) : this(eventCode) => this.Parameters = parameters;

        /// <summary>
        /// Gets or sets the parameter value associated with the specified key.
        /// </summary>
        /// <param name="parameterKey">The key of the parameter.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object this[string parameterKey]
        {
            get => this.Parameters.GetObject(parameterKey);
            set => this.Parameters.Add(parameterKey, value);
        }

    }

}

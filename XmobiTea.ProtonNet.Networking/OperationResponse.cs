using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents the response to an operation in the networking layer.
    /// </summary>
    public class OperationResponse : IOperationModel
    {
        /// <summary>
        /// Gets or sets the operation code that identifies the operation.
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Gets or sets the unique response ID for the operation.
        /// </summary>
        public ushort ResponseId { get; set; }

        /// <summary>
        /// Gets or sets the return code indicating the result of the operation.
        /// </summary>
        public byte ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the parameters associated with the operation response.
        /// </summary>
        public GNHashtable Parameters { get; set; }

        /// <summary>
        /// Gets or sets a debug message providing additional information about the operation result.
        /// </summary>
        public string DebugMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResponse"/> class.
        /// </summary>
        public OperationResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResponse"/> class with a specific operation code.
        /// </summary>
        /// <param name="operationCode">The operation code that identifies the operation.</param>
        public OperationResponse(string operationCode) => this.OperationCode = operationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResponse"/> class with a specific operation code and parameters.
        /// </summary>
        /// <param name="operationCode">The operation code that identifies the operation.</param>
        /// <param name="parameters">The parameters associated with the operation response.</param>
        public OperationResponse(string operationCode, GNHashtable parameters) : this(operationCode) => this.Parameters = parameters;

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

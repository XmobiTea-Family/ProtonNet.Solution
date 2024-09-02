using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents a request for an operation in the networking layer.
    /// </summary>
    public class OperationRequest : IOperationModel
    {
        /// <summary>
        /// Gets or sets the operation code that identifies the operation.
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Gets or sets the unique request ID for the operation.
        /// </summary>
        public ushort RequestId { get; set; }

        /// <summary>
        /// Gets or sets the parameters associated with the operation request.
        /// </summary>
        public GNHashtable Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationRequest"/> class.
        /// </summary>
        public OperationRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationRequest"/> class with a specific operation code.
        /// </summary>
        /// <param name="operationCode">The operation code that identifies the operation.</param>
        public OperationRequest(string operationCode) => this.OperationCode = operationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationRequest"/> class with a specific operation code and parameters.
        /// </summary>
        /// <param name="operationCode">The operation code that identifies the operation.</param>
        /// <param name="parameters">The parameters associated with the operation request.</param>
        public OperationRequest(string operationCode, GNHashtable parameters) : this(operationCode) => this.Parameters = parameters;

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

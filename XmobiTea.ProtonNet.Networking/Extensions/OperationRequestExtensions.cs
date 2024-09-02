using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="OperationRequest"/> class.
    /// </summary>
    public static class OperationRequestExtensions
    {
        /// <summary>
        /// Sets the operation code of the <see cref="OperationRequest"/>.
        /// </summary>
        /// <param name="operationRequest">The <see cref="OperationRequest"/> to modify.</param>
        /// <param name="operationCode">The operation code to set.</param>
        /// <returns>The modified <see cref="OperationRequest"/>.</returns>
        public static OperationRequest SetOperationCode(this OperationRequest operationRequest, string operationCode)
        {
            operationRequest.OperationCode = operationCode;
            return operationRequest;
        }

        /// <summary>
        /// Sets the parameters of the <see cref="OperationRequest"/>.
        /// </summary>
        /// <param name="operationRequest">The <see cref="OperationRequest"/> to modify.</param>
        /// <param name="parameters">The parameters to set.</param>
        /// <returns>The modified <see cref="OperationRequest"/>.</returns>
        public static OperationRequest SetParameters(this OperationRequest operationRequest, GNHashtable parameters)
        {
            operationRequest.Parameters = parameters;
            return operationRequest;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="OperationRequest"/>.
        /// </summary>
        /// <param name="operationRequest">The <see cref="OperationRequest"/> to modify.</param>
        /// <param name="key">The key of the parameter to add.</param>
        /// <param name="value">The value of the parameter to add.</param>
        /// <returns>The modified <see cref="OperationRequest"/>.</returns>
        public static OperationRequest AddParameter(this OperationRequest operationRequest, string key, object value)
        {
            if (operationRequest.Parameters == null) operationRequest.Parameters = new GNHashtable();
            operationRequest.Parameters.Add(key, value);
            return operationRequest;
        }

        /// <summary>
        /// Removes a parameter from the <see cref="OperationRequest"/>.
        /// </summary>
        /// <param name="operationRequest">The <see cref="OperationRequest"/> to modify.</param>
        /// <param name="key">The key of the parameter to remove.</param>
        /// <returns>The modified <see cref="OperationRequest"/>.</returns>
        public static OperationRequest RemoveParameter(this OperationRequest operationRequest, string key)
        {
            if (operationRequest.Parameters != null) operationRequest.Parameters.Remove(key);
            return operationRequest;
        }

    }

}

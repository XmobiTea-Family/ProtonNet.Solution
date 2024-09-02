using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="OperationResponse"/> class.
    /// </summary>
    public static class OperationResponseExtensions
    {
        /// <summary>
        /// Sets the operation code of the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="operationCode">The operation code to set.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse SetOperationCode(this OperationResponse operationResponse, string operationCode)
        {
            operationResponse.OperationCode = operationCode;
            return operationResponse;
        }

        /// <summary>
        /// Sets the parameters of the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="parameters">The parameters to set.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse SetParameters(this OperationResponse operationResponse, GNHashtable parameters)
        {
            operationResponse.Parameters = parameters;
            return operationResponse;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="key">The key of the parameter to add.</param>
        /// <param name="value">The value of the parameter to add.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse AddParameter(this OperationResponse operationResponse, string key, object value)
        {
            if (operationResponse.Parameters == null) operationResponse.Parameters = new GNHashtable();
            operationResponse.Parameters.Add(key, value);
            return operationResponse;
        }

        /// <summary>
        /// Removes a parameter from the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="key">The key of the parameter to remove.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse RemoveParameter(this OperationResponse operationResponse, string key)
        {
            if (operationResponse.Parameters != null) operationResponse.Parameters.Remove(key);
            return operationResponse;
        }

        /// <summary>
        /// Sets the return code of the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="returnCode">The return code to set.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse SetReturnCode(this OperationResponse operationResponse, byte returnCode)
        {
            operationResponse.ReturnCode = returnCode;
            return operationResponse;
        }

        /// <summary>
        /// Sets the debug message of the <see cref="OperationResponse"/>.
        /// </summary>
        /// <param name="operationResponse">The <see cref="OperationResponse"/> to modify.</param>
        /// <param name="debugMessage">The debug message to set.</param>
        /// <returns>The modified <see cref="OperationResponse"/>.</returns>
        public static OperationResponse SetDebugMessage(this OperationResponse operationResponse, string debugMessage)
        {
            operationResponse.DebugMessage = debugMessage;
            return operationResponse;
        }

    }

}

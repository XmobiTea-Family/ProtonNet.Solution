using XmobiTea.Data;
using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Server.Helper
{
    /// <summary>
    /// Provides helper methods for handling operation responses.
    /// </summary>
    public static class OperationHelper
    {
        /// <summary>
        /// Handles an unknown operation code.
        /// </summary>
        /// <param name="operationRequest">The operation request associated with the unknown code.</param>
        /// <param name="debugMessage">An optional debug message for additional information.</param>
        /// <returns>An <see cref="OperationResponse"/> indicating the operation code is unknown.</returns>
        public static OperationResponse HandleOperationUnknownCode(OperationRequest operationRequest, string debugMessage = null)
        {
            // Debug.LogFormat("Unknown operation code: OpCode={0}", operationRequest.OperationCode);

            return new OperationResponse(operationRequest.OperationCode)
            {
                ReturnCode = ReturnCode.OperationInvalid,
                DebugMessage = debugMessage,
            };
        }

        /// <summary>
        /// Handles an invalid operation.
        /// </summary>
        /// <param name="operationRequest">The operation request associated with the invalid operation.</param>
        /// <param name="debugMessage">An optional debug message for additional information.</param>
        /// <returns>An <see cref="OperationResponse"/> indicating the operation is invalid.</returns>
        public static OperationResponse HandleOperationInvalid(OperationRequest operationRequest, string debugMessage = null)
        {
            return new OperationResponse(operationRequest.OperationCode)
            {
                ReturnCode = ReturnCode.OperationInvalid,
                DebugMessage = debugMessage,
            };
        }

        /// <summary>
        /// Handles an invalid operation specific to server-side issues.
        /// </summary>
        /// <param name="operationRequest">The operation request associated with the invalid operation.</param>
        /// <param name="debugMessage">An optional debug message for additional information.</param>
        /// <returns>An <see cref="OperationResponse"/> indicating the operation is invalid in the server context.</returns>
        public static OperationResponse HandleOperationInvalidInServer(OperationRequest operationRequest, string debugMessage = null)
        {
            return new OperationResponse(operationRequest.OperationCode)
            {
                ReturnCode = ReturnCode.OperationInvalidInServer,
                DebugMessage = debugMessage,
            };
        }

        /// <summary>
        /// Handles an unauthorized operation request.
        /// </summary>
        /// <param name="operationRequest">The operation request associated with the unauthorized access.</param>
        /// <param name="debugMessage">An optional debug message for additional information.</param>
        /// <returns>An <see cref="OperationResponse"/> indicating the operation is not authorized.</returns>
        public static OperationResponse HandleOperationNotAuthorized(OperationRequest operationRequest, string debugMessage = null)
        {
            return new OperationResponse(operationRequest.OperationCode)
            {
                ReturnCode = ReturnCode.OperationNotAuthorized,
                DebugMessage = debugMessage,
            };
        }

        /// <summary>
        /// Handles a successful operation request.
        /// </summary>
        /// <param name="operationRequest">The operation request that was successful.</param>
        /// <param name="parameters">Parameters to include in the response.</param>
        /// <param name="debugMessage">An optional debug message for additional information.</param>
        /// <returns>An <see cref="OperationResponse"/> indicating the operation was successful.</returns>
        public static OperationResponse HandleOperationOk(OperationRequest operationRequest, GNHashtable parameters, string debugMessage = null)
        {
            return new OperationResponse(operationRequest.OperationCode, parameters)
            {
                ReturnCode = ReturnCode.Ok,
                DebugMessage = debugMessage,
            };
        }

    }

}

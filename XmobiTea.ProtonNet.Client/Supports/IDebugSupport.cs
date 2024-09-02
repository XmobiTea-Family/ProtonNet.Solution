using System.Collections.Generic;
using XmobiTea.Data;
using XmobiTea.ProtonNet.Client.Models;
using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Supports
{
    /// <summary>
    /// Interface that provides methods for generating debug information 
    /// related to the lifecycle of operation requests and events.
    /// </summary>
    public interface IDebugSupport
    {
        /// <summary>
        /// Generates a string representation for when an operation request is enqueued.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was enqueued.</param>
        /// <returns>A string containing debug information about the enqueued operation request.</returns>
        string ToStringOnEnqueue(OperationRequestPending operationRequestPending);

        /// <summary>
        /// Generates a string representation for when an operation request is sent.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was sent.</param>
        /// <returns>A string containing debug information about the sent operation request.</returns>
        string ToStringOnSend(OperationRequestPending operationRequestPending);

        /// <summary>
        /// Generates a string representation for when an operation request is received.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was received.</param>
        /// <returns>A string containing debug information about the received operation request.</returns>
        string ToStringOnRecv(OperationRequestPending operationRequestPending);

        /// <summary>
        /// Generates a string representation for when an operation event is enqueued.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was enqueued.</param>
        /// <returns>A string containing debug information about the enqueued operation event.</returns>
        string ToStringOnEnqueue(OperationEventPending operationEventPending);

        /// <summary>
        /// Generates a string representation for when an operation event is sent.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was sent.</param>
        /// <returns>A string containing debug information about the sent operation event.</returns>
        string ToStringOnSend(OperationEventPending operationEventPending);

        /// <summary>
        /// Generates a string representation for when an operation event is received.
        /// </summary>
        /// <param name="operationEvent">The operation event that was received.</param>
        /// <returns>A string containing debug information about the received operation event.</returns>
        string ToStringOnEvent(OperationEvent operationEvent);

    }

    /// <summary>
    /// Default implementation of the <see cref="IDebugSupport"/> interface, 
    /// providing methods for generating debug information related to the 
    /// lifecycle of operation requests and events.
    /// </summary>
    class DefaultDebugOperationService : IDebugSupport
    {
        /// <summary>
        /// Default string used when an unknown return code is encountered.
        /// </summary>
        private static readonly string UnknownReturnCode = "UnknownReturnCode";

        /// <summary>
        /// Default string used when an object is null.
        /// </summary>
        private static readonly string Null = "null";

        /// <summary>
        /// Dictionary that maps return codes to their string representations.
        /// </summary>
        private IDictionary<byte, string> returnCodeToStringDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDebugOperationService"/> class.
        /// </summary>
        public DefaultDebugOperationService()
        {
            this.returnCodeToStringDict = new Dictionary<byte, string>();

            this.AddReturnCodeToStrings();
        }

        /// <summary>
        /// Adds predefined return code strings to the dictionary.
        /// </summary>
        private void AddReturnCodeToStrings()
        {
            this.returnCodeToStringDict[ReturnCode.Ok] = "Ok";
            this.returnCodeToStringDict[ReturnCode.OperationTimeout] = "OperationTimeout";
            this.returnCodeToStringDict[ReturnCode.OperationInvalid] = "OperationInvalid";
            this.returnCodeToStringDict[ReturnCode.OperationInvalidInServer] = "OperationInvalidInServer";
            this.returnCodeToStringDict[ReturnCode.OperationNotAuthorized] = "OperationNotAuthorized";
        }

        /// <summary>
        /// Retrieves the string representation of a return code.
        /// </summary>
        /// <param name="returnCode">The return code to convert to a string.</param>
        /// <returns>A string representing the return code.</returns>
        private string GetReturnCodeToString(byte returnCode)
        {
            if (this.returnCodeToStringDict.TryGetValue(returnCode, out var answer))
                return answer;

            return UnknownReturnCode;
        }

        /// <summary>
        /// Generates a debug string when an operation request is enqueued.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was enqueued.</param>
        /// <returns>A string containing debug information about the enqueued operation request.</returns>
        public string ToStringOnEnqueue(OperationRequestPending operationRequestPending)
        {
            var request = operationRequestPending.GetOperationRequest();
            return $"Code: {this.ToStringOperationCode(request.OperationCode)}, Id: {request.RequestId}";
        }

        /// <summary>
        /// Generates a debug string when an operation request is sent.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was sent.</param>
        /// <returns>A string containing debug information about the sent operation request.</returns>
        public string ToStringOnSend(OperationRequestPending operationRequestPending)
        {
            var request = operationRequestPending.GetOperationRequest();
            return $"Code: {this.ToStringOperationCode(request.OperationCode)}, Parameters: {this.ToStringParameters(request.Parameters)}";
        }

        /// <summary>
        /// Generates a debug string when an operation request is received.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request that was received.</param>
        /// <returns>A string containing debug information about the received operation request.</returns>
        public string ToStringOnRecv(OperationRequestPending operationRequestPending)
        {
            var response = operationRequestPending.GetOperationResponse();
            return $"Code: {this.ToStringOperationCode(response.OperationCode)}, ReturnCode: {this.ToStringReturnCode(response.ReturnCode)}, ExecuteTimer: {operationRequestPending.GetExecuteTimerInMs()} ms, Parameters: {this.ToStringParameters(response.Parameters)}, DebugMessage: {response.DebugMessage}";
        }

        /// <summary>
        /// Generates a debug string when an operation event is received.
        /// </summary>
        /// <param name="operationEvent">The operation event that was received.</param>
        /// <returns>A string containing debug information about the received operation event.</returns>
        public string ToStringOnEvent(OperationEvent operationEvent)
        {
            return $"Code: {this.ToStringEventCode(operationEvent.EventCode)}, Parameters: {this.ToStringParameters(operationEvent.Parameters)}";
        }

        /// <summary>
        /// Converts the operation code to a string.
        /// </summary>
        /// <param name="operationCode">The operation code to convert.</param>
        /// <returns>A string representation of the operation code.</returns>
        private string ToStringOperationCode(string operationCode)
        {
            return operationCode.ToString();
        }

        /// <summary>
        /// Converts the return code to a string.
        /// </summary>
        /// <param name="returnCode">The return code to convert.</param>
        /// <returns>A string representation of the return code.</returns>
        private string ToStringReturnCode(byte returnCode)
        {
            return this.GetReturnCodeToString(returnCode);
        }

        /// <summary>
        /// Converts the parameters to a string representation.
        /// </summary>
        /// <param name="parameters">The parameters to convert.</param>
        /// <returns>A string representation of the parameters.</returns>
        private string ToStringParameters(GNHashtable parameters)
        {
            return parameters == null ? Null : parameters.ToString();
        }

        /// <summary>
        /// Converts the event code to a string.
        /// </summary>
        /// <param name="eventCode">The event code to convert.</param>
        /// <returns>A string representation of the event code.</returns>
        private string ToStringEventCode(string eventCode)
        {
            return eventCode.ToString();
        }

        /// <summary>
        /// Generates a debug string when an operation event is enqueued.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was enqueued.</param>
        /// <returns>A string containing debug information about the enqueued operation event.</returns>
        public string ToStringOnEnqueue(OperationEventPending operationEventPending)
        {
            var operationEvent = operationEventPending.GetOperationEvent();
            return $"Code: {this.ToStringEventCode(operationEvent.EventCode)}";
        }

        /// <summary>
        /// Generates a debug string when an operation event is sent.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was sent.</param>
        /// <returns>A string containing debug information about the sent operation event.</returns>
        public string ToStringOnSend(OperationEventPending operationEventPending)
        {
            var operationEvent = operationEventPending.GetOperationEvent();
            return $"Code: {this.ToStringEventCode(operationEvent.EventCode)}, Parameters: {this.ToStringParameters(operationEvent.Parameters)}";
        }

    }

}

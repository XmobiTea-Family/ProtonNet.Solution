using XmobiTea.Data;

namespace XmobiTea.ProtonNet.Networking.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="OperationEvent"/> class.
    /// </summary>
    public static class OperationEventExtensions
    {
        /// <summary>
        /// Sets the event code of the <see cref="OperationEvent"/>.
        /// </summary>
        /// <param name="operationEvent">The <see cref="OperationEvent"/> to modify.</param>
        /// <param name="eventCode">The event code to set.</param>
        /// <returns>The modified <see cref="OperationEvent"/>.</returns>
        public static OperationEvent SetEventCode(this OperationEvent operationEvent, string eventCode)
        {
            operationEvent.EventCode = eventCode;
            return operationEvent;
        }

        /// <summary>
        /// Sets the parameters of the <see cref="OperationEvent"/>.
        /// </summary>
        /// <param name="operationEvent">The <see cref="OperationEvent"/> to modify.</param>
        /// <param name="parameters">The parameters to set.</param>
        /// <returns>The modified <see cref="OperationEvent"/>.</returns>
        public static OperationEvent SetParameters(this OperationEvent operationEvent, GNHashtable parameters)
        {
            operationEvent.Parameters = parameters;
            return operationEvent;
        }

        /// <summary>
        /// Adds a parameter to the <see cref="OperationEvent"/>.
        /// </summary>
        /// <param name="operationEvent">The <see cref="OperationEvent"/> to modify.</param>
        /// <param name="key">The key of the parameter to add.</param>
        /// <param name="value">The value of the parameter to add.</param>
        /// <returns>The modified <see cref="OperationEvent"/>.</returns>
        public static OperationEvent AddParameter(this OperationEvent operationEvent, string key, object value)
        {
            if (operationEvent.Parameters == null) operationEvent.Parameters = new GNHashtable();
            operationEvent.Parameters.Add(key, value);
            return operationEvent;
        }

        /// <summary>
        /// Removes a parameter from the <see cref="OperationEvent"/>.
        /// </summary>
        /// <param name="operationEvent">The <see cref="OperationEvent"/> to modify.</param>
        /// <param name="key">The key of the parameter to remove.</param>
        /// <returns>The modified <see cref="OperationEvent"/>.</returns>
        public static OperationEvent RemoveParameter(this OperationEvent operationEvent, string key)
        {
            if (operationEvent.Parameters != null) operationEvent.Parameters.Remove(key);
            return operationEvent;
        }

    }

}

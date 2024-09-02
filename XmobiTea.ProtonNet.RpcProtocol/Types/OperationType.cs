namespace XmobiTea.ProtonNet.RpcProtocol.Types
{
    /// <summary>
    /// Enum representing the various types of operations 
    /// supported in the XmobiTea.ProtonNet library.
    /// </summary>
    public enum OperationType : byte
    {
        /// <summary>
        /// Operation representing a request.
        /// </summary>
        OperationRequest = 0,

        /// <summary>
        /// Operation representing a response.
        /// </summary>
        OperationResponse = 1,

        /// <summary>
        /// Operation representing an event.
        /// </summary>
        OperationEvent = 2,

        /// <summary>
        /// Operation representing a ping.
        /// </summary>
        OperationPing = 3,

        /// <summary>
        /// Operation representing a pong.
        /// </summary>
        OperationPong = 4,

        /// <summary>
        /// Operation representing a handshake.
        /// </summary>
        OperationHandshake = 5,

        /// <summary>
        /// Operation representing a handshake acknowledgment.
        /// </summary>
        OperationHandshakeAck = 6,

        /// <summary>
        /// Operation representing a disconnect.
        /// </summary>
        OperationDisconnect = 7,

    }

}

namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents a handshake acknowledgment operation in the networking layer.
    /// </summary>
    public class OperationHandshakeAck : IOperationModel
    {
        /// <summary>
        /// Gets or sets the connection ID associated with the handshake.
        /// </summary>
        public int ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the server session ID received during the handshake.
        /// </summary>
        public string ServerSessionId { get; set; }

    }

}

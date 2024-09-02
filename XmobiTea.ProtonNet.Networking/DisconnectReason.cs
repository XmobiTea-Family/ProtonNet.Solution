namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Enumeration representing the reasons for disconnection in the networking layer.
    /// </summary>
    public enum DisconnectReason : byte
    {
        /// <summary>
        /// The disconnection reason is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The client was unable to connect to the server.
        /// </summary>
        CantConnectServer = 1,

        /// <summary>
        /// The maximum number of sessions has been reached.
        /// </summary>
        MaxSession = 2,

        /// <summary>
        /// The operation handshake was invalid.
        /// </summary>
        InvalidOperationHandshake = 3,

        /// <summary>
        /// The maximum number of sessions per user has been reached.
        /// </summary>
        MaxSessionPerUser = 4,

        /// <summary>
        /// The handshake operation timed out.
        /// </summary>
        HandshakeTimeout = 5,

        /// <summary>
        /// The connection was idle for too long and timed out.
        /// </summary>
        IdleTimeout = 6,

        /// <summary>
        /// The client disconnected intentionally.
        /// </summary>
        DisconnectByClient = 7,

    }

}

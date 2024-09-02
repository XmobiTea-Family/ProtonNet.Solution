namespace XmobiTea.ProtonNet.Server.Types
{
    /// <summary>
    /// Represents the type of peer in the system.
    /// </summary>
    public enum PeerType
    {
        /// <summary>
        /// Indicates an unknown peer type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents a client that uses Web API.
        /// </summary>
        WebApiClient = 1,

        /// <summary>
        /// Represents a client that uses sockets.
        /// </summary>
        SocketClient = 2,

        /// <summary>
        /// Represents a server that uses Web API.
        /// </summary>
        WebApiServer = 3,

        /// <summary>
        /// Represents a server that uses sockets.
        /// </summary>
        SocketServer = 4,

    }

}

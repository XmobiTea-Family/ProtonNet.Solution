namespace XmobiTea.ProtonNet.Server.Socket.Types
{
    /// <summary>
    /// Defines the various transport protocols supported by the socket server.
    /// </summary>
    public enum TransportProtocol
    {
        /// <summary>
        /// Represents the UDP (User Datagram Protocol) transport protocol.
        /// </summary>
        Udp = 0,

        /// <summary>
        /// Represents the TCP (Transmission Control Protocol) transport protocol.
        /// </summary>
        Tcp = 1,

        /// <summary>
        /// Represents the SSL (Secure Sockets Layer) transport protocol over TCP.
        /// </summary>
        Ssl = 2,

        /// <summary>
        /// Represents the WebSocket (WS) transport protocol.
        /// </summary>
        Ws = 4,

        /// <summary>
        /// Represents the WebSocket Secure (WSS) transport protocol over SSL.
        /// </summary>
        Wss = 5,

    }

}

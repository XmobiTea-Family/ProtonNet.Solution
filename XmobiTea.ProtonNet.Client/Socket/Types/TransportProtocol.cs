namespace XmobiTea.ProtonNet.Client.Socket.Types
{
    /// <summary>
    /// Enum representing the different transport protocols that can be used for socket communication.
    /// </summary>
    public enum TransportProtocol
    {
        /// <summary>
        /// User Datagram Protocol (UDP) transport.
        /// </summary>
        Udp = 0,

        /// <summary>
        /// Transmission Control Protocol (TCP) transport.
        /// </summary>
        Tcp = 1,

        /// <summary>
        /// WebSocket (WS) transport.
        /// </summary>
        Ws = 4,

    }

    /// <summary>
    /// Enum representing the different SSL (Secure Sockets Layer) transport protocols that can be used for secure socket communication.
    /// </summary>
    public enum SslTransportProtocol
    {
        /// <summary>
        /// Secure Sockets Layer (SSL) transport.
        /// </summary>
        Ssl = 2,

        /// <summary>
        /// Secure WebSocket (WSS) transport.
        /// </summary>
        Wss = 5,

    }

}

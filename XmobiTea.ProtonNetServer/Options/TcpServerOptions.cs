namespace XmobiTea.ProtonNetServer.Options
{
    /// <summary>
    /// Represents the configuration options for the TCP server.
    /// </summary>
    public class TcpServerOptions
    {
        /// <summary>
        /// Gets or sets the maximum length of the pending connections queue.
        /// </summary>
        /// <value>The default value is 1024.</value>
        public int AcceptorBacklog { get; set; } = 1024;

        /// <summary>
        /// Gets or sets a value indicating whether to use dual-mode sockets.
        /// </summary>
        /// <value>True if dual-mode sockets are used; otherwise, false.</value>
        public bool DualMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use keep-alive packets.
        /// </summary>
        /// <value>True if keep-alive packets are enabled; otherwise, false.</value>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the time, in seconds, between keep-alive packets.
        /// Only available in .NET Core.
        /// </summary>
        /// <value>The default value is -1, which means keep-alive packets are disabled.</value>
        public int TcpKeepAliveTime { get; set; } = -1;

        /// <summary>
        /// Gets or sets the interval, in seconds, between keep-alive packet retries.
        /// Only available in .NET Core.
        /// </summary>
        /// <value>The default value is -1, which means keep-alive packets are disabled.</value>
        public int TcpKeepAliveInterval { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum number of keep-alive packet retries.
        /// Only available in .NET Core.
        /// </summary>
        /// <value>The default value is -1, which means keep-alive packets are disabled.</value>
        public int TcpKeepAliveRetryCount { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether to use the no-delay option.
        /// </summary>
        /// <value>True if no-delay option is used; otherwise, false.</value>
        public bool NoDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow address reuse.
        /// </summary>
        /// <value>True if address reuse is allowed; otherwise, false.</value>
        public bool ReuseAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use exclusive address binding.
        /// </summary>
        /// <value>True if exclusive address binding is used; otherwise, false.</value>
        public bool ExclusiveAddressUse { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of bytes that can be received in the buffer.
        /// </summary>
        public int ReceiveBufferLimit { get; set; }

        /// <summary>
        /// Gets or sets the capacity of the receive buffer.
        /// </summary>
        /// <value>The default value is 8192 bytes.</value>
        public int ReceiveBufferCapacity { get; set; } = 8192;

        /// <summary>
        /// Gets or sets the maximum number of bytes that can be sent in the buffer.
        /// </summary>
        public int SendBufferLimit { get; set; }

        /// <summary>
        /// Gets or sets the capacity of the send buffer.
        /// </summary>
        /// <value>The default value is 8192 bytes.</value>
        public int SendBufferCapacity { get; set; } = 8192;

    }

}

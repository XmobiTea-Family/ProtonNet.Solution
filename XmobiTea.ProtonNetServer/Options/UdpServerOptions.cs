namespace XmobiTea.ProtonNetServer.Options
{
    /// <summary>
    /// Represents the configuration options for the UDP server.
    /// </summary>
    public class UdpServerOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use dual-mode sockets.
        /// </summary>
        /// <value>True if dual-mode sockets are used; otherwise, false.</value>
        public bool DualMode { get; set; }

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
        /// <value>The default value is 0, which means no limit is applied.</value>
        public int ReceiveBufferLimit { get; set; } = 0;

        /// <summary>
        /// Gets or sets the capacity of the receive buffer.
        /// </summary>
        /// <value>The default value is 8192 bytes.</value>
        public int ReceiveBufferCapacity { get; set; } = 8192;

        /// <summary>
        /// Gets or sets the maximum number of bytes that can be sent in the buffer.
        /// </summary>
        /// <value>The default value is 0, which means no limit is applied.</value>
        public int SendBufferLimit { get; set; } = 0;

    }

}

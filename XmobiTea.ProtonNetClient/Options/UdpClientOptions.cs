namespace XmobiTea.ProtonNetClient.Options
{
    /// <summary>
    /// Represents the configuration options for a UDP client.
    /// Provides properties to configure various UDP settings 
    /// such as dual mode, address reuse, multicast, buffer sizes, and more.
    /// </summary>
    public class UdpClientOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the UDP client 
        /// operates in dual mode (IPv4/IPv6).
        /// </summary>
        public bool DualMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the UDP socket 
        /// can be bound to an address that is already in use.
        /// </summary>
        public bool ReuseAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the UDP socket 
        /// should use an exclusive address.
        /// </summary>
        public bool ExclusiveAddressUse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multicast is 
        /// enabled for the UDP client.
        /// </summary>
        public bool Multicast { get; set; }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the receive 
        /// buffer. A value of 0 indicates no limit.
        /// </summary>
        public int ReceiveBufferLimit { get; set; } = 0;

        /// <summary>
        /// Gets or sets the initial capacity, in bytes, of the receive 
        /// buffer. Default value is 8192 bytes.
        /// </summary>
        public int ReceiveBufferCapacity { get; set; } = 8192;

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the send 
        /// buffer. A value of 0 indicates no limit.
        /// </summary>
        public int SendBufferLimit { get; set; } = 0;

        /// <summary>
        /// Gets or sets the initial capacity, in bytes, of the send 
        /// buffer. Default value is 8192 bytes.
        /// </summary>
        public int SendBufferCapacity { get; set; } = 8192;

    }

}

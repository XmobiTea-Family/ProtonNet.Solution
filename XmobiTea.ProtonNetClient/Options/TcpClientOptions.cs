namespace XmobiTea.ProtonNetClient.Options
{
    /// <summary>
    /// Represents the configuration options for a TCP client.
    /// Provides properties to configure various TCP settings 
    /// such as dual mode, keep-alive, buffer sizes, and more.
    /// </summary>
    public class TcpClientOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the TCP client 
        /// operates in dual mode (IPv4/IPv6).
        /// </summary>
        public bool DualMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TCP keep-alive 
        /// is enabled for the connection.
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the duration, in seconds, before the first 
        /// keep-alive packet is sent. 
        /// This property is only applicable in .NET Core.
        /// A value of -1 indicates the default system setting.
        /// </summary>
        public int TcpKeepAliveTime { get; set; } = -1;

        /// <summary>
        /// Gets or sets the interval, in seconds, between subsequent 
        /// keep-alive packets if no acknowledgment is received. 
        /// This property is only applicable in .NET Core.
        /// A value of -1 indicates the default system setting.
        /// </summary>
        public int TcpKeepAliveInterval { get; set; } = -1;

        /// <summary>
        /// Gets or sets the number of keep-alive probes that will 
        /// be sent before dropping the connection. 
        /// This property is only applicable in .NET Core.
        /// A value of -1 indicates the default system setting.
        /// </summary>
        public int TcpKeepAliveRetryCount { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether the TCP NoDelay 
        /// option is enabled, which disables the Nagle algorithm.
        /// </summary>
        public bool NoDelay { get; set; }

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

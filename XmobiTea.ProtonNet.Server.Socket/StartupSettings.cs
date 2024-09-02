namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Represents the settings for thread pool size, specifically for different types of fibers.
    /// </summary>
    public class ThreadPoolSizeSettings
    {
        /// <summary>
        /// Gets the size of the other fiber in the thread pool.
        /// </summary>
        public byte OtherFiber { get; }

        /// <summary>
        /// Gets the size of the received fiber in the thread pool.
        /// </summary>
        public byte ReceivedFiber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPoolSizeSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the thread pool size settings.</param>
        private ThreadPoolSizeSettings(Builder builder)
        {
            this.OtherFiber = builder.OtherFiber;
            this.ReceivedFiber = builder.ReceivedFiber;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="ThreadPoolSizeSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="ThreadPoolSizeSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the size of the other fiber in the thread pool.
            /// </summary>
            public byte OtherFiber { get; set; }

            /// <summary>
            /// Gets or sets the size of the received fiber in the thread pool.
            /// </summary>
            public byte ReceivedFiber { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the size of the other fiber in the thread pool.
            /// </summary>
            /// <param name="otherFiber">The size of the other fiber.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetOtherFiber(byte otherFiber)
            {
                this.OtherFiber = otherFiber;
                return this;
            }

            /// <summary>
            /// Sets the size of the received fiber in the thread pool.
            /// </summary>
            /// <param name="receivedFiber">The size of the received fiber.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReceivedFiber(byte receivedFiber)
            {
                this.ReceivedFiber = receivedFiber;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="ThreadPoolSizeSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="ThreadPoolSizeSettings"/>.</returns>
            public ThreadPoolSizeSettings Build() => new ThreadPoolSizeSettings(this);

        }

    }

    /// <summary>
    /// Represents the SSL configuration settings, including enabling SSL, port number, 
    /// certificate file path, and certificate password.
    /// </summary>
    public class SslConfigSettings
    {
        /// <summary>
        /// Gets a value indicating whether SSL is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the port number used for SSL.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the file path to the SSL certificate.
        /// </summary>
        public string CerFilePath { get; }

        /// <summary>
        /// Gets the password for the SSL certificate.
        /// </summary>
        public string CerPassword { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SslConfigSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the SSL settings.</param>
        private SslConfigSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Port = builder.Port;
            this.CerFilePath = builder.CerFilePath;
            this.CerPassword = builder.CerPassword;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="SslConfigSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="SslConfigSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether SSL is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the port number used for SSL.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the file path to the SSL certificate.
            /// </summary>
            public string CerFilePath { get; set; }

            /// <summary>
            /// Gets or sets the password for the SSL certificate.
            /// </summary>
            public string CerPassword { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether SSL is enabled.
            /// </summary>
            /// <param name="enable">A boolean value indicating whether SSL is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the port number used for SSL.
            /// </summary>
            /// <param name="port">The port number.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the file path to the SSL certificate.
            /// </summary>
            /// <param name="cerFilePath">The file path to the SSL certificate.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetCerFilePath(string cerFilePath)
            {
                this.CerFilePath = cerFilePath;
                return this;
            }

            /// <summary>
            /// Sets the password for the SSL certificate.
            /// </summary>
            /// <param name="password">The password for the SSL certificate.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetCerPassword(string password)
            {
                this.CerPassword = password;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="SslConfigSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="SslConfigSettings"/>.</returns>
            public SslConfigSettings Build() => new SslConfigSettings(this);

        }

    }

    /// <summary>
    /// Represents the TCP server configuration settings, including enabling the server,
    /// address, port, session configuration, and SSL configuration.
    /// </summary>
    public class TcpServerSettings
    {
        /// <summary>
        /// Gets a value indicating whether the TCP server is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the address where the TCP server will listen.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port number on which the TCP server will listen.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the session configuration settings for the TCP server.
        /// </summary>
        public SessionConfigSettings SessionConfig { get; }

        /// <summary>
        /// Gets the SSL configuration settings for the TCP server.
        /// </summary>
        public SslConfigSettings SslConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServerSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the TCP server settings.</param>
        private TcpServerSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Port = builder.Port;
            this.Address = builder.Address;
            this.SessionConfig = builder.SessionConfig;
            this.SslConfig = builder.SslConfig;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="TcpServerSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="TcpServerSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether the TCP server is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the address where the TCP server will listen.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Gets or sets the port number on which the TCP server will listen.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the session configuration settings for the TCP server.
            /// </summary>
            public SessionConfigSettings SessionConfig { get; set; }

            /// <summary>
            /// Gets or sets the SSL configuration settings for the TCP server.
            /// </summary>
            public SslConfigSettings SslConfig { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether the TCP server is enabled.
            /// </summary>
            /// <param name="enable">A boolean value indicating whether the TCP server is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the address where the TCP server will listen.
            /// </summary>
            /// <param name="address">The address where the TCP server will listen.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetAddress(string address)
            {
                this.Address = address;
                return this;
            }

            /// <summary>
            /// Sets the port number on which the TCP server will listen.
            /// </summary>
            /// <param name="port">The port number.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the session configuration settings for the TCP server.
            /// </summary>
            /// <param name="sessionConfig">The session configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSessionConfig(SessionConfigSettings sessionConfig)
            {
                this.SessionConfig = sessionConfig;
                return this;
            }

            /// <summary>
            /// Sets the SSL configuration settings for the TCP server.
            /// </summary>
            /// <param name="sslConfig">The SSL configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSslConfig(SslConfigSettings sslConfig)
            {
                this.SslConfig = sslConfig;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="TcpServerSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="TcpServerSettings"/>.</returns>
            public TcpServerSettings Build() => new TcpServerSettings(this);

        }

    }

    /// <summary>
    /// Represents the UDP server configuration settings, including enabling the server,
    /// address, port, and session configuration.
    /// </summary>
    public class UdpServerSettings
    {
        /// <summary>
        /// Gets a value indicating whether the UDP server is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the address where the UDP server will listen.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port number on which the UDP server will listen.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the session configuration settings for the UDP server.
        /// </summary>
        public UdpSessionConfigSettings SessionConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpServerSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the UDP server settings.</param>
        private UdpServerSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Address = builder.Address;
            this.Port = builder.Port;
            this.SessionConfig = builder.SessionConfig;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="UdpServerSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="UdpServerSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether the UDP server is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the address where the UDP server will listen.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Gets or sets the port number on which the UDP server will listen.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the session configuration settings for the UDP server.
            /// </summary>
            public UdpSessionConfigSettings SessionConfig { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether the UDP server is enabled.
            /// </summary>
            /// <param name="enable">A boolean value indicating whether the UDP server is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the address where the UDP server will listen.
            /// </summary>
            /// <param name="address">The address where the UDP server will listen.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetAddress(string address)
            {
                this.Address = address;
                return this;
            }

            /// <summary>
            /// Sets the port number on which the UDP server will listen.
            /// </summary>
            /// <param name="port">The port number.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the session configuration settings for the UDP server.
            /// </summary>
            /// <param name="sessionConfig">The session configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSessionConfig(UdpSessionConfigSettings sessionConfig)
            {
                this.SessionConfig = sessionConfig;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="UdpServerSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="UdpServerSettings"/>.</returns>
            public UdpServerSettings Build() => new UdpServerSettings(this);

        }

    }

    /// <summary>
    /// Represents the WebSocket server configuration settings, including enabling the server,
    /// address, port, maximum frame size, session configuration, and SSL configuration.
    /// </summary>
    public class WebSocketServerSettings
    {
        /// <summary>
        /// Gets a value indicating whether the WebSocket server is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the address where the WebSocket server will listen.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port number on which the WebSocket server will listen.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the maximum frame size for the WebSocket server.
        /// </summary>
        public int MaxFrameSize { get; }

        /// <summary>
        /// Gets the session configuration settings for the WebSocket server.
        /// </summary>
        public SessionConfigSettings SessionConfig { get; }

        /// <summary>
        /// Gets the SSL configuration settings for the WebSocket server.
        /// </summary>
        public SslConfigSettings SslConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketServerSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the WebSocket server settings.</param>
        private WebSocketServerSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Port = builder.Port;
            this.Address = builder.Address;
            this.MaxFrameSize = builder.MaxFrameSize;
            this.SessionConfig = builder.SessionConfig;
            this.SslConfig = builder.SslConfig;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="WebSocketServerSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="WebSocketServerSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether the WebSocket server is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the address where the WebSocket server will listen.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Gets or sets the port number on which the WebSocket server will listen.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the maximum frame size for the WebSocket server.
            /// </summary>
            public int MaxFrameSize { get; set; }

            /// <summary>
            /// Gets or sets the session configuration settings for the WebSocket server.
            /// </summary>
            public SessionConfigSettings SessionConfig { get; set; }

            /// <summary>
            /// Gets or sets the SSL configuration settings for the WebSocket server.
            /// </summary>
            public SslConfigSettings SslConfig { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether the WebSocket server is enabled.
            /// </summary>
            /// <param name="enable">A boolean value indicating whether the WebSocket server is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the address where the WebSocket server will listen.
            /// </summary>
            /// <param name="address">The address where the WebSocket server will listen.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetAddress(string address)
            {
                this.Address = address;
                return this;
            }

            /// <summary>
            /// Sets the port number on which the WebSocket server will listen.
            /// </summary>
            /// <param name="port">The port number.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the maximum frame size for the WebSocket server.
            /// </summary>
            /// <param name="maxFrameSize">The maximum frame size.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxFrameSize(int maxFrameSize)
            {
                this.MaxFrameSize = maxFrameSize;
                return this;
            }

            /// <summary>
            /// Sets the session configuration settings for the WebSocket server.
            /// </summary>
            /// <param name="sessionConfig">The session configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSessionConfig(SessionConfigSettings sessionConfig)
            {
                this.SessionConfig = sessionConfig;
                return this;
            }

            /// <summary>
            /// Sets the SSL configuration settings for the WebSocket server.
            /// </summary>
            /// <param name="sslConfig">The SSL configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSslConfig(SslConfigSettings sslConfig)
            {
                this.SslConfig = sslConfig;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="WebSocketServerSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="WebSocketServerSettings"/>.</returns>
            public WebSocketServerSettings Build() => new WebSocketServerSettings(this);
        }

    }

    /// <summary>
    /// Represents the session configuration settings, including various TCP and buffer settings
    /// for optimizing network communication.
    /// </summary>
    public class SessionConfigSettings
    {
        /// <summary>
        /// Gets the backlog limit for accepting connections.
        /// </summary>
        public int AcceptorBacklog { get; }

        /// <summary>
        /// Gets a value indicating whether dual-mode sockets are enabled.
        /// </summary>
        public bool DualMode { get; }

        /// <summary>
        /// Gets a value indicating whether the keep-alive option is enabled.
        /// </summary>
        public bool KeepAlive { get; }

        /// <summary>
        /// Gets the keep-alive time for TCP connections in milliseconds.
        /// </summary>
        public int TcpKeepAliveTime { get; }

        /// <summary>
        /// Gets the interval between keep-alive probes for TCP connections in milliseconds.
        /// </summary>
        public int TcpKeepAliveInterval { get; }

        /// <summary>
        /// Gets the number of keep-alive probes before timing out the TCP connection.
        /// </summary>
        public int TcpKeepAliveRetryCount { get; }

        /// <summary>
        /// Gets a value indicating whether the Nagle's algorithm is disabled (NoDelay).
        /// </summary>
        public bool NoDelay { get; }

        /// <summary>
        /// Gets a value indicating whether the address can be reused.
        /// </summary>
        public bool ReuseAddress { get; }

        /// <summary>
        /// Gets a value indicating whether the socket will have exclusive access to the address.
        /// </summary>
        public bool ExclusiveAddressUse { get; }

        /// <summary>
        /// Gets the limit for the receive buffer size in bytes.
        /// </summary>
        public int ReceiveBufferLimit { get; }

        /// <summary>
        /// Gets the capacity of the receive buffer in bytes.
        /// </summary>
        public int ReceiveBufferCapacity { get; }

        /// <summary>
        /// Gets the limit for the send buffer size in bytes.
        /// </summary>
        public int SendBufferLimit { get; }

        /// <summary>
        /// Gets the capacity of the send buffer in bytes.
        /// </summary>
        public int SendBufferCapacity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionConfigSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the session settings.</param>
        private SessionConfigSettings(Builder builder)
        {
            this.AcceptorBacklog = builder.AcceptorBacklog;
            this.DualMode = builder.DualMode;
            this.KeepAlive = builder.KeepAlive;
            this.TcpKeepAliveTime = builder.TcpKeepAliveTime;
            this.TcpKeepAliveInterval = builder.TcpKeepAliveInterval;
            this.TcpKeepAliveRetryCount = builder.TcpKeepAliveRetryCount;
            this.NoDelay = builder.NoDelay;
            this.ReuseAddress = builder.ReuseAddress;
            this.ExclusiveAddressUse = builder.ExclusiveAddressUse;
            this.ReceiveBufferLimit = builder.ReceiveBufferLimit;
            this.ReceiveBufferCapacity = builder.ReceiveBufferCapacity;
            this.SendBufferLimit = builder.SendBufferLimit;
            this.SendBufferCapacity = builder.SendBufferCapacity;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="SessionConfigSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="SessionConfigSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the backlog limit for accepting connections.
            /// </summary>
            public int AcceptorBacklog { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            public bool DualMode { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the keep-alive option is enabled.
            /// </summary>
            public bool KeepAlive { get; set; }

            /// <summary>
            /// Gets or sets the keep-alive time for TCP connections in milliseconds.
            /// </summary>
            public int TcpKeepAliveTime { get; set; }

            /// <summary>
            /// Gets or sets the interval between keep-alive probes for TCP connections in milliseconds.
            /// </summary>
            public int TcpKeepAliveInterval { get; set; }

            /// <summary>
            /// Gets or sets the number of keep-alive probes before timing out the TCP connection.
            /// </summary>
            public int TcpKeepAliveRetryCount { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the Nagle's algorithm is disabled (NoDelay).
            /// </summary>
            public bool NoDelay { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the address can be reused.
            /// </summary>
            public bool ReuseAddress { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the socket will have exclusive access to the address.
            /// </summary>
            public bool ExclusiveAddressUse { get; set; }

            /// <summary>
            /// Gets or sets the limit for the receive buffer size in bytes.
            /// </summary>
            public int ReceiveBufferLimit { get; set; }

            /// <summary>
            /// Gets or sets the capacity of the receive buffer in bytes.
            /// </summary>
            public int ReceiveBufferCapacity { get; set; }

            /// <summary>
            /// Gets or sets the limit for the send buffer size in bytes.
            /// </summary>
            public int SendBufferLimit { get; set; }

            /// <summary>
            /// Gets or sets the capacity of the send buffer in bytes.
            /// </summary>
            public int SendBufferCapacity { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the backlog limit for accepting connections.
            /// </summary>
            /// <param name="acceptorBacklog">The backlog limit.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetAcceptorBacklog(int acceptorBacklog)
            {
                this.AcceptorBacklog = acceptorBacklog;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            /// <param name="dualMode">A boolean value indicating whether dual-mode sockets are enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetDualMode(bool dualMode)
            {
                this.DualMode = dualMode;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the keep-alive option is enabled.
            /// </summary>
            /// <param name="keepAlive">A boolean value indicating whether the keep-alive option is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetKeepAlive(bool keepAlive)
            {
                this.KeepAlive = keepAlive;
                return this;
            }

            /// <summary>
            /// Sets the keep-alive time for TCP connections in milliseconds.
            /// </summary>
            /// <param name="tcpKeepAliveTime">The keep-alive time in milliseconds.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetTcpKeepAliveTime(int tcpKeepAliveTime)
            {
                this.TcpKeepAliveTime = tcpKeepAliveTime;
                return this;
            }

            /// <summary>
            /// Sets the interval between keep-alive probes for TCP connections in milliseconds.
            /// </summary>
            /// <param name="tcpKeepAliveInterval">The keep-alive interval in milliseconds.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetTcpKeepAliveInterval(int tcpKeepAliveInterval)
            {
                this.TcpKeepAliveInterval = tcpKeepAliveInterval;
                return this;
            }

            /// <summary>
            /// Sets the number of keep-alive probes before timing out the TCP connection.
            /// </summary>
            /// <param name="tcpKeepAliveRetryCount">The number of keep-alive probes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetTcpKeepAliveRetryCount(int tcpKeepAliveRetryCount)
            {
                this.TcpKeepAliveRetryCount = tcpKeepAliveRetryCount;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the Nagle's algorithm is disabled (NoDelay).
            /// </summary>
            /// <param name="noDelay">A boolean value indicating whether NoDelay is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetNoDelay(bool noDelay)
            {
                this.NoDelay = noDelay;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the address can be reused.
            /// </summary>
            /// <param name="reuseAddress">A boolean value indicating whether the address can be reused.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReuseAddress(bool reuseAddress)
            {
                this.ReuseAddress = reuseAddress;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the socket will have exclusive access to the address.
            /// </summary>
            /// <param name="exclusiveAddressUse">A boolean value indicating whether exclusive access is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetExclusiveAddressUse(bool exclusiveAddressUse)
            {
                this.ExclusiveAddressUse = exclusiveAddressUse;
                return this;
            }

            /// <summary>
            /// Sets the limit for the receive buffer size in bytes.
            /// </summary>
            /// <param name="receiveBufferLimit">The receive buffer size limit in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReceiveBufferLimit(int receiveBufferLimit)
            {
                this.ReceiveBufferLimit = receiveBufferLimit;
                return this;
            }

            /// <summary>
            /// Sets the capacity of the receive buffer in bytes.
            /// </summary>
            /// <param name="receiveBufferCapacity">The receive buffer capacity in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReceiveBufferCapacity(int receiveBufferCapacity)
            {
                this.ReceiveBufferCapacity = receiveBufferCapacity;
                return this;
            }

            /// <summary>
            /// Sets the limit for the send buffer size in bytes.
            /// </summary>
            /// <param name="sendBufferLimit">The send buffer size limit in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSendBufferLimit(int sendBufferLimit)
            {
                this.SendBufferLimit = sendBufferLimit;
                return this;
            }

            /// <summary>
            /// Sets the capacity of the send buffer in bytes.
            /// </summary>
            /// <param name="sendBufferCapacity">The send buffer capacity in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSendBufferCapacity(int sendBufferCapacity)
            {
                this.SendBufferCapacity = sendBufferCapacity;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="SessionConfigSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="SessionConfigSettings"/>.</returns>
            public SessionConfigSettings Build() => new SessionConfigSettings(this);

        }

    }

    /// <summary>
    /// Represents the UDP session configuration settings, including dual-mode,
    /// address reuse, exclusive address use, and buffer limits and capacities.
    /// </summary>
    public class UdpSessionConfigSettings
    {
        /// <summary>
        /// Gets a value indicating whether dual-mode sockets are enabled.
        /// </summary>
        public bool DualMode { get; }

        /// <summary>
        /// Gets a value indicating whether the address can be reused.
        /// </summary>
        public bool ReuseAddress { get; }

        /// <summary>
        /// Gets a value indicating whether the socket will have exclusive access to the address.
        /// </summary>
        public bool ExclusiveAddressUse { get; }

        /// <summary>
        /// Gets the limit for the receive buffer size in bytes.
        /// </summary>
        public int ReceiveBufferLimit { get; }

        /// <summary>
        /// Gets the capacity of the receive buffer in bytes.
        /// </summary>
        public int ReceiveBufferCapacity { get; }

        /// <summary>
        /// Gets the limit for the send buffer size in bytes.
        /// </summary>
        public int SendBufferLimit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpSessionConfigSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the UDP session settings.</param>
        private UdpSessionConfigSettings(Builder builder)
        {
            this.DualMode = builder.DualMode;
            this.ReuseAddress = builder.ReuseAddress;
            this.ExclusiveAddressUse = builder.ExclusiveAddressUse;
            this.ReceiveBufferLimit = builder.ReceiveBufferLimit;
            this.ReceiveBufferCapacity = builder.ReceiveBufferCapacity;
            this.SendBufferLimit = builder.SendBufferLimit;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="UdpSessionConfigSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="UdpSessionConfigSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            public bool DualMode { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the address can be reused.
            /// </summary>
            public bool ReuseAddress { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the socket will have exclusive access to the address.
            /// </summary>
            public bool ExclusiveAddressUse { get; set; }

            /// <summary>
            /// Gets or sets the limit for the receive buffer size in bytes.
            /// </summary>
            public int ReceiveBufferLimit { get; set; }

            /// <summary>
            /// Gets or sets the capacity of the receive buffer in bytes.
            /// </summary>
            public int ReceiveBufferCapacity { get; set; }

            /// <summary>
            /// Gets or sets the limit for the send buffer size in bytes.
            /// </summary>
            public int SendBufferLimit { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            /// <param name="dualMode">A boolean value indicating whether dual-mode sockets are enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetDualMode(bool dualMode)
            {
                this.DualMode = dualMode;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the address can be reused.
            /// </summary>
            /// <param name="reuseAddress">A boolean value indicating whether the address can be reused.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReuseAddress(bool reuseAddress)
            {
                this.ReuseAddress = reuseAddress;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the socket will have exclusive access to the address.
            /// </summary>
            /// <param name="exclusiveAddressUse">A boolean value indicating whether exclusive access is enabled.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetExclusiveAddressUse(bool exclusiveAddressUse)
            {
                this.ExclusiveAddressUse = exclusiveAddressUse;
                return this;
            }

            /// <summary>
            /// Sets the limit for the receive buffer size in bytes.
            /// </summary>
            /// <param name="receiveBufferLimit">The receive buffer size limit in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReceiveBufferLimit(int receiveBufferLimit)
            {
                this.ReceiveBufferLimit = receiveBufferLimit;
                return this;
            }

            /// <summary>
            /// Sets the capacity of the receive buffer in bytes.
            /// </summary>
            /// <param name="receiveBufferCapacity">The receive buffer capacity in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetReceiveBufferCapacity(int receiveBufferCapacity)
            {
                this.ReceiveBufferCapacity = receiveBufferCapacity;
                return this;
            }

            /// <summary>
            /// Sets the limit for the send buffer size in bytes.
            /// </summary>
            /// <param name="sendBufferLimit">The send buffer size limit in bytes.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetSendBufferLimit(int sendBufferLimit)
            {
                this.SendBufferLimit = sendBufferLimit;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="UdpSessionConfigSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="UdpSessionConfigSettings"/>.</returns>
            public UdpSessionConfigSettings Build() => new UdpSessionConfigSettings(this);

        }

    }

    /// <summary>
    /// Represents the startup configuration settings, including session limits,
    /// timeout settings, and server configurations for TCP, UDP, and WebSocket.
    /// </summary>
    public class StartupSettings
    {
        /// <summary>
        /// Gets the name of the startup configuration.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the maximum number of sessions allowed.
        /// </summary>
        public int MaxSession { get; }

        /// <summary>
        /// Gets the maximum number of pending requests allowed.
        /// </summary>
        public int MaxPendingRequest { get; }

        /// <summary>
        /// Gets the maximum number of pending requests per session.
        /// </summary>
        public int MaxSessionPendingRequest { get; }

        /// <summary>
        /// Gets the maximum number of requests per session per second.
        /// </summary>
        public int MaxSessionRequestPerSecond { get; }

        /// <summary>
        /// Gets the maximum number of UDP session requests per user.
        /// </summary>
        public int MaxUdpSessionRequestPerUser { get; }

        /// <summary>
        /// Gets the maximum number of TCP session requests per user.
        /// </summary>
        public int MaxTcpSessionRequestPerUser { get; }

        /// <summary>
        /// Gets the maximum number of WebSocket session requests per user.
        /// </summary>
        public int MaxWsSessionRequestPerUser { get; }

        /// <summary>
        /// Gets the handshake timeout in seconds.
        /// </summary>
        public int HandshakeTimeoutInSeconds { get; }

        /// <summary>
        /// Gets the idle timeout in seconds.
        /// </summary>
        public int IdleTimeoutInSeconds { get; }

        /// <summary>
        /// Gets the TCP server configuration settings.
        /// </summary>
        public TcpServerSettings TcpServer { get; }

        /// <summary>
        /// Gets the UDP server configuration settings.
        /// </summary>
        public UdpServerSettings UdpServer { get; }

        /// <summary>
        /// Gets the WebSocket server configuration settings.
        /// </summary>
        public WebSocketServerSettings WebSocketServer { get; }

        /// <summary>
        /// Gets the thread pool size configuration settings.
        /// </summary>
        public ThreadPoolSizeSettings ThreadPoolSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupSettings"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the startup settings.</param>
        private StartupSettings(Builder builder)
        {
            this.Name = builder.Name;
            this.MaxSession = builder.MaxSession;
            this.MaxPendingRequest = builder.MaxPendingRequest;
            this.MaxSessionPendingRequest = builder.MaxSessionPendingRequest;
            this.MaxSessionRequestPerSecond = builder.MaxSessionRequestPerSecond;
            this.MaxUdpSessionRequestPerUser = builder.MaxUdpSessionRequestPerUser;
            this.MaxTcpSessionRequestPerUser = builder.MaxTcpSessionRequestPerUser;
            this.MaxWsSessionRequestPerUser = builder.MaxWsSessionRequestPerUser;
            this.HandshakeTimeoutInSeconds = builder.HandshakeTimeoutInSeconds;
            this.IdleTimeoutInSeconds = builder.IdleTimeoutInSeconds;
            this.TcpServer = builder.TcpServer;
            this.UdpServer = builder.UdpServer;
            this.WebSocketServer = builder.WebSocketServer;
            this.ThreadPoolSize = builder.ThreadPoolSize;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="StartupSettings"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="StartupSettings"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the name of the startup configuration.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of sessions allowed.
            /// </summary>
            public int MaxSession { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of pending requests allowed.
            /// </summary>
            public int MaxPendingRequest { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of pending requests per session.
            /// </summary>
            public int MaxSessionPendingRequest { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of requests per session per second.
            /// </summary>
            public int MaxSessionRequestPerSecond { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of UDP session requests per user.
            /// </summary>
            public int MaxUdpSessionRequestPerUser { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of TCP session requests per user.
            /// </summary>
            public int MaxTcpSessionRequestPerUser { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of WebSocket session requests per user.
            /// </summary>
            public int MaxWsSessionRequestPerUser { get; set; }

            /// <summary>
            /// Gets or sets the handshake timeout in seconds.
            /// </summary>
            public int HandshakeTimeoutInSeconds { get; set; }

            /// <summary>
            /// Gets or sets the idle timeout in seconds.
            /// </summary>
            public int IdleTimeoutInSeconds { get; set; }

            /// <summary>
            /// Gets or sets the TCP server configuration settings.
            /// </summary>
            public TcpServerSettings TcpServer { get; set; }

            /// <summary>
            /// Gets or sets the UDP server configuration settings.
            /// </summary>
            public UdpServerSettings UdpServer { get; set; }

            /// <summary>
            /// Gets or sets the WebSocket server configuration settings.
            /// </summary>
            public WebSocketServerSettings WebSocketServer { get; set; }

            /// <summary>
            /// Gets or sets the thread pool size configuration settings.
            /// </summary>
            public ThreadPoolSizeSettings ThreadPoolSize { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the name of the startup configuration.
            /// </summary>
            /// <param name="name">The name of the startup configuration.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetName(string name)
            {
                this.Name = name;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of sessions allowed.
            /// </summary>
            /// <param name="maxSession">The maximum number of sessions.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxSession(int maxSession)
            {
                this.MaxSession = maxSession;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of pending requests allowed.
            /// </summary>
            /// <param name="maxPendingRequest">The maximum number of pending requests.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxPendingRequest(int maxPendingRequest)
            {
                this.MaxPendingRequest = maxPendingRequest;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of pending requests per session.
            /// </summary>
            /// <param name="maxSessionPendingRequest">The maximum number of pending requests per session.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxSessionPendingRequest(int maxSessionPendingRequest)
            {
                this.MaxSessionPendingRequest = maxSessionPendingRequest;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of requests per session per second.
            /// </summary>
            /// <param name="maxSessionRequestPerSecond">The maximum number of requests per session per second.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxSessionRequestPerSecond(int maxSessionRequestPerSecond)
            {
                this.MaxSessionRequestPerSecond = maxSessionRequestPerSecond;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of UDP session requests per user.
            /// </summary>
            /// <param name="maxUdpSessionRequestPerUser">The maximum number of UDP session requests per user.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxUdpSessionRequestPerUser(int maxUdpSessionRequestPerUser)
            {
                this.MaxUdpSessionRequestPerUser = maxUdpSessionRequestPerUser;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of TCP session requests per user.
            /// </summary>
            /// <param name="maxTcpSessionRequestPerUser">The maximum number of TCP session requests per user.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxTcpSessionRequestPerUser(int maxTcpSessionRequestPerUser)
            {
                this.MaxTcpSessionRequestPerUser = maxTcpSessionRequestPerUser;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of WebSocket session requests per user.
            /// </summary>
            /// <param name="maxWsSessionRequestPerUser">The maximum number of WebSocket session requests per user.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetMaxWsSessionRequestPerUser(int maxWsSessionRequestPerUser)
            {
                this.MaxWsSessionRequestPerUser = maxWsSessionRequestPerUser;
                return this;
            }

            /// <summary>
            /// Sets the handshake timeout in seconds.
            /// </summary>
            /// <param name="handshakeTimeoutInSeconds">The handshake timeout in seconds.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetHandshakeTimeoutInSeconds(int handshakeTimeoutInSeconds)
            {
                this.HandshakeTimeoutInSeconds = handshakeTimeoutInSeconds;
                return this;
            }

            /// <summary>
            /// Sets the idle timeout in seconds.
            /// </summary>
            /// <param name="idleTimeoutInSeconds">The idle timeout in seconds.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetIdleTimeoutInSeconds(int idleTimeoutInSeconds)
            {
                this.IdleTimeoutInSeconds = idleTimeoutInSeconds;
                return this;
            }

            /// <summary>
            /// Sets the TCP server configuration settings.
            /// </summary>
            /// <param name="tcpServer">The TCP server configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetTcpServer(TcpServerSettings tcpServer)
            {
                this.TcpServer = tcpServer;
                return this;
            }

            /// <summary>
            /// Sets the UDP server configuration settings.
            /// </summary>
            /// <param name="udpServer">The UDP server configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetUdpServer(UdpServerSettings udpServer)
            {
                this.UdpServer = udpServer;
                return this;
            }

            /// <summary>
            /// Sets the WebSocket server configuration settings.
            /// </summary>
            /// <param name="webSocketServer">The WebSocket server configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetWebSocketServer(WebSocketServerSettings webSocketServer)
            {
                this.WebSocketServer = webSocketServer;
                return this;
            }

            /// <summary>
            /// Sets the thread pool size configuration settings.
            /// </summary>
            /// <param name="threadPoolSize">The thread pool size configuration settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetThreadPoolSize(ThreadPoolSizeSettings threadPoolSize)
            {
                this.ThreadPoolSize = threadPoolSize;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="StartupSettings"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="StartupSettings"/>.</returns>
            public StartupSettings Build() => new StartupSettings(this);

        }

    }

}

namespace XmobiTea.ProtonNet.Server.WebApi
{
    /// <summary>
    /// Settings auth token service
    /// </summary>
    public class AuthTokenSettings
    {
        /// <summary>
        /// Gets the password of UserPeerAuthTokenService
        /// </summary>
        public string Password { get; }

        private AuthTokenSettings(Builder builder)
        {
            this.Password = builder.Password;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for ThreadPoolSizeSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the password of UserPeerAuthTokenService.
            /// </summary>
            public string Password { get; set; }

            internal Builder() { }

            /// <summary>
            /// Sets the password of UserPeerAuthTokenService.
            /// </summary>
            /// <param name="password">The password of UserPeerAuthTokenService.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetPassword(string password)
            {
                this.Password = password;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="AuthTokenSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="AuthTokenSettings"/> instance.</returns>
            public AuthTokenSettings Build() => new AuthTokenSettings(this);

        }

    }

    /// <summary>
    /// Settings thread pool size
    /// </summary>
    public class ThreadPoolSizeSettings
    {
        /// <summary>
        /// Gets the number of fibers for other tasks.
        /// </summary>
        public byte OtherFiber { get; }

        /// <summary>
        /// Gets the number of fibers for received tasks.
        /// </summary>
        public byte ReceivedFiber { get; }

        private ThreadPoolSizeSettings(Builder builder)
        {
            this.OtherFiber = builder.OtherFiber;
            this.ReceivedFiber = builder.ReceivedFiber;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for ThreadPoolSizeSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the number of fibers for other tasks.
            /// </summary>
            public byte OtherFiber { get; set; }

            /// <summary>
            /// Gets or sets the number of fibers for received tasks.
            /// </summary>
            public byte ReceivedFiber { get; set; }

            internal Builder() { }

            /// <summary>
            /// Sets the number of fibers for other tasks.
            /// </summary>
            /// <param name="otherFiber">The number of fibers for other tasks.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetOtherFiber(byte otherFiber)
            {
                this.OtherFiber = otherFiber;
                return this;
            }

            /// <summary>
            /// Sets the number of fibers for received tasks.
            /// </summary>
            /// <param name="receivedFiber">The number of fibers for received tasks.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetReceivedFiber(byte receivedFiber)
            {
                this.ReceivedFiber = receivedFiber;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="ThreadPoolSizeSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="ThreadPoolSizeSettings"/> instance.</returns>
            public ThreadPoolSizeSettings Build() => new ThreadPoolSizeSettings(this);

        }

    }

    /// <summary>
    /// Settings ssl config
    /// </summary>
    public class SslConfigSettings
    {
        /// <summary>
        /// Gets a value indicating whether SSL is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the port number for SSL.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the file path to the certificate.
        /// </summary>
        public string CertFilePath { get; }

        /// <summary>
        /// Gets the password for the certificate.
        /// </summary>
        public string CertPassword { get; }

        private SslConfigSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Port = builder.Port;
            this.CertFilePath = builder.CertFilePath;
            this.CertPassword = builder.CertPassword;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for SslConfigSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether SSL is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the port number for SSL.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the file path to the certificate.
            /// </summary>
            public string CertFilePath { get; set; }

            /// <summary>
            /// Gets or sets the password for the certificate.
            /// </summary>
            public string CertPassword { get; set; }

            internal Builder() { }

            /// <summary>
            /// Sets whether SSL is enabled.
            /// </summary>
            /// <param name="enable">A value indicating whether SSL is enabled.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the port number for SSL.
            /// </summary>
            /// <param name="port">The port number for SSL.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the file path to the certificate.
            /// </summary>
            /// <param name="certFilePath">The file path to the certificate.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetCertFilePath(string certFilePath)
            {
                this.CertFilePath = certFilePath;
                return this;
            }

            /// <summary>
            /// Sets the password for the certificate.
            /// </summary>
            /// <param name="certPassword">The password for the certificate.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetCertPassword(string certPassword)
            {
                this.CertPassword = certPassword;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="SslConfigSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="SslConfigSettings"/> instance.</returns>
            public SslConfigSettings Build() => new SslConfigSettings(this);

        }

    }

    /// <summary>
    /// Settings HttpServer
    /// </summary>
    public class HttpServerSettings
    {
        /// <summary>
        /// Gets a value indicating whether the HTTP server is enabled.
        /// </summary>
        public bool Enable { get; }

        /// <summary>
        /// Gets the address on which the HTTP server will listen.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port on which the HTTP server will listen.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the session configuration settings for the HTTP server.
        /// </summary>
        public SessionConfigSettings SessionConfig { get; }

        /// <summary>
        /// Gets the SSL configuration settings for the HTTP server.
        /// </summary>
        public SslConfigSettings SslConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerSettings"/> class.
        /// </summary>
        /// <param name="builder">The builder instance used to construct this instance.</param>
        private HttpServerSettings(Builder builder)
        {
            this.Enable = builder.Enable;
            this.Port = builder.Port;
            this.Address = builder.Address;

            this.SessionConfig = builder.SessionConfig;
            this.SslConfig = builder.SslConfig;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for HttpServerSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets a value indicating whether the HTTP server is enabled.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// Gets or sets the address on which the HTTP server will listen.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Gets or sets the port on which the HTTP server will listen.
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the session configuration settings for the HTTP server.
            /// </summary>
            public SessionConfigSettings SessionConfig { get; set; }

            /// <summary>
            /// Gets or sets the SSL configuration settings for the HTTP server.
            /// </summary>
            public SslConfigSettings SslConfig { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets a value indicating whether the HTTP server is enabled.
            /// </summary>
            /// <param name="enable">A value indicating whether the HTTP server is enabled.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetEnable(bool enable)
            {
                this.Enable = enable;
                return this;
            }

            /// <summary>
            /// Sets the address on which the HTTP server will listen.
            /// </summary>
            /// <param name="address">The address on which the HTTP server will listen.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetAddress(string address)
            {
                this.Address = address;
                return this;
            }

            /// <summary>
            /// Sets the port on which the HTTP server will listen.
            /// </summary>
            /// <param name="port">The port on which the HTTP server will listen.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetPort(int port)
            {
                this.Port = port;
                return this;
            }

            /// <summary>
            /// Sets the session configuration settings for the HTTP server.
            /// </summary>
            /// <param name="sessionConfig">The session configuration settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetSessionConfig(SessionConfigSettings sessionConfig)
            {
                this.SessionConfig = sessionConfig;
                return this;
            }

            /// <summary>
            /// Sets the SSL configuration settings for the HTTP server.
            /// </summary>
            /// <param name="sslConfig">The SSL configuration settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetSslConfig(SslConfigSettings sslConfig)
            {
                this.SslConfig = sslConfig;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="HttpServerSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="HttpServerSettings"/> instance.</returns>
            public HttpServerSettings Build() => new HttpServerSettings(this);

        }

    }

    /// <summary>
    /// Settings SessionConfig
    /// </summary>
    public class SessionConfigSettings
    {
        /// <summary>
        /// Gets the backlog size for the acceptor.
        /// </summary>
        public int AcceptorBacklog { get; }

        /// <summary>
        /// Gets a value indicating whether dual-mode sockets are enabled.
        /// </summary>
        public bool DualMode { get; }

        /// <summary>
        /// Gets a value indicating whether keep-alive is enabled.
        /// </summary>
        public bool KeepAlive { get; }

        /// <summary>
        /// Gets the TCP keep-alive time in seconds.
        /// Only available on .NET Core
        /// </summary>
        public int TcpKeepAliveTime { get; }

        /// <summary>
        /// Gets the TCP keep-alive interval in seconds.
        /// Only available on .NET Core
        /// </summary>
        public int TcpKeepAliveInterval { get; }

        /// <summary>
        /// Gets the TCP keep-alive retry count.
        /// Only available on .NET Core
        /// </summary>
        public int TcpKeepAliveRetryCount { get; }

        /// <summary>
        /// Gets a value indicating whether the Nagle algorithm is disabled (no delay).
        /// </summary>
        public bool NoDelay { get; }

        /// <summary>
        /// Gets a value indicating whether address reuse is allowed.
        /// </summary>
        public bool ReuseAddress { get; }

        /// <summary>
        /// Gets a value indicating whether exclusive address usage is enforced.
        /// </summary>
        public bool ExclusiveAddressUse { get; }

        /// <summary>
        /// Gets the limit of the receive buffer size.
        /// </summary>
        public int ReceiveBufferLimit { get; }

        /// <summary>
        /// Gets the capacity of the receive buffer size.
        /// </summary>
        public int ReceiveBufferCapacity { get; }

        /// <summary>
        /// Gets the limit of the send buffer size.
        /// </summary>
        public int SendBufferLimit { get; }

        /// <summary>
        /// Gets the capacity of the send buffer size.
        /// </summary>
        public int SendBufferCapacity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionConfigSettings"/> class.
        /// </summary>
        /// <param name="builder">The builder instance used to construct this instance.</param>
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
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for SessionConfigSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the backlog size for the acceptor.
            /// </summary>
            public int AcceptorBacklog { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            public bool DualMode { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether keep-alive is enabled.
            /// </summary>
            public bool KeepAlive { get; set; }

            /// <summary>
            /// Gets or sets the TCP keep-alive time.
            /// Only available on .NET Core
            /// </summary>
            public int TcpKeepAliveTime { get; set; }

            /// <summary>
            /// Gets or sets the TCP keep-alive interval.
            /// Only available on .NET Core
            /// </summary>
            public int TcpKeepAliveInterval { get; set; }

            /// <summary>
            /// Gets or sets the TCP keep-alive retry count.
            /// Only available on .NET Core
            /// </summary>
            public int TcpKeepAliveRetryCount { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the Nagle algorithm is disabled (no delay).
            /// </summary>
            public bool NoDelay { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether address reuse is allowed.
            /// </summary>
            public bool ReuseAddress { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether exclusive address usage is enforced.
            /// </summary>
            public bool ExclusiveAddressUse { get; set; }

            /// <summary>
            /// Gets or sets the limit of the receive buffer size.
            /// </summary>
            public int ReceiveBufferLimit { get; set; }

            /// <summary>
            /// Gets or sets the capacity of the receive buffer size.
            /// </summary>
            public int ReceiveBufferCapacity { get; set; }

            /// <summary>
            /// Gets or sets the limit of the send buffer size.
            /// </summary>
            public int SendBufferLimit { get; set; }

            /// <summary>
            /// Gets or sets the capacity of the send buffer size.
            /// </summary>
            public int SendBufferCapacity { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the backlog size for the acceptor.
            /// </summary>
            /// <param name="acceptorBacklog">The backlog size for the acceptor.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetAcceptorBacklog(int acceptorBacklog)
            {
                this.AcceptorBacklog = acceptorBacklog;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether dual-mode sockets are enabled.
            /// </summary>
            /// <param name="dualMode">A value indicating whether dual-mode sockets are enabled.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetDualMode(bool dualMode)
            {
                this.DualMode = dualMode;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether keep-alive is enabled.
            /// </summary>
            /// <param name="keepAlive">A value indicating whether keep-alive is enabled.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetKeepAlive(bool keepAlive)
            {
                this.KeepAlive = keepAlive;
                return this;
            }

            /// <summary>
            /// Sets the TCP keep-alive time in seconds.
            /// Only available on .NET Core
            /// </summary>
            /// <param name="tcpKeepAliveTime">The TCP keep-alive time in seconds.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetTcpKeepAliveTime(int tcpKeepAliveTime)
            {
                this.TcpKeepAliveTime = tcpKeepAliveTime;
                return this;
            }

            /// <summary>
            /// Sets the TCP keep-alive interval in seconds.
            /// Only available on .NET Core
            /// </summary>
            /// <param name="tcpKeepAliveInterval">The TCP keep-alive interval in seconds.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetTcpKeepAliveInterval(int tcpKeepAliveInterval)
            {
                this.TcpKeepAliveInterval = tcpKeepAliveInterval;
                return this;
            }

            /// <summary>
            /// Sets the TCP keep-alive retry count.
            /// Only available on .NET Core
            /// </summary>
            /// <param name="tcpKeepAliveRetryCount">The TCP keep-alive retry count.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetTcpKeepAliveRetryCount(int tcpKeepAliveRetryCount)
            {
                this.TcpKeepAliveRetryCount = tcpKeepAliveRetryCount;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether the Nagle algorithm is disabled (no delay).
            /// </summary>
            /// <param name="noDelay">A value indicating whether the Nagle algorithm is disabled (no delay).</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetNoDelay(bool noDelay)
            {
                this.NoDelay = noDelay;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether address reuse is allowed.
            /// </summary>
            /// <param name="reuseAddress">A value indicating whether address reuse is allowed.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetReuseAddress(bool reuseAddress)
            {
                this.ReuseAddress = reuseAddress;
                return this;
            }

            /// <summary>
            /// Sets a value indicating whether exclusive address usage is enforced.
            /// </summary>
            /// <param name="exclusiveAddressUse">A value indicating whether exclusive address usage is enforced.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetExclusiveAddressUse(bool exclusiveAddressUse)
            {
                this.ExclusiveAddressUse = exclusiveAddressUse;
                return this;
            }

            /// <summary>
            /// Sets the limit of the receive buffer size.
            /// </summary>
            /// <param name="receiveBufferLimit">The limit of the receive buffer size.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetReceiveBufferLimit(int receiveBufferLimit)
            {
                this.ReceiveBufferLimit = receiveBufferLimit;
                return this;
            }

            /// <summary>
            /// Sets the capacity of the receive buffer size.
            /// </summary>
            /// <param name="receiveBufferCapacity">The capacity of the receive buffer size.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetReceiveBufferCapacity(int receiveBufferCapacity)
            {
                this.ReceiveBufferCapacity = receiveBufferCapacity;
                return this;
            }

            /// <summary>
            /// Sets the limit of the send buffer size.
            /// </summary>
            /// <param name="sendBufferLimit">The limit of the send buffer size.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetSendBufferLimit(int sendBufferLimit)
            {
                this.SendBufferLimit = sendBufferLimit;
                return this;
            }

            /// <summary>
            /// Sets the capacity of the send buffer size.
            /// </summary>
            /// <param name="sendBufferCapacity">The capacity of the send buffer size.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetSendBufferCapacity(int sendBufferCapacity)
            {
                this.SendBufferCapacity = sendBufferCapacity;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="SessionConfigSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="SessionConfigSettings"/> instance.</returns>
            public SessionConfigSettings Build() => new SessionConfigSettings(this);

        }

    }

    /// <summary>
    /// Startup Settings
    /// </summary>
    public class StartupSettings
    {
        /// <summary>
        /// Gets the name of the startup settings.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the maximum number of pending requests allowed.
        /// </summary>
        public int MaxPendingRequest { get; }

        /// <summary>
        /// Gets the maximum number of session pending requests allowed.
        /// </summary>
        public int MaxSessionPendingRequest { get; }

        /// <summary>
        /// Gets the maximum number of session requests allowed per second.
        /// </summary>
        public int MaxSessionRequestPerSecond { get; }

        /// <summary>
        /// Gets the HTTP server settings.
        /// </summary>
        public HttpServerSettings HttpServer { get; }

        /// <summary>
        /// Gets the thread pool size settings.
        /// </summary>
        public ThreadPoolSizeSettings ThreadPoolSize { get; }

        /// <summary>
        /// Gets the auth token settings.
        /// </summary>
        public AuthTokenSettings AuthToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupSettings"/> class.
        /// </summary>
        /// <param name="builder">The builder instance used to construct this instance.</param>
        private StartupSettings(Builder builder)
        {
            this.Name = builder.Name;

            this.MaxPendingRequest = builder.MaxPendingRequest;
            this.MaxSessionPendingRequest = builder.MaxSessionPendingRequest;
            this.MaxSessionRequestPerSecond = builder.MaxSessionRequestPerSecond;

            this.HttpServer = builder.HttpServer;
            this.ThreadPoolSize = builder.ThreadPoolSize;
            this.AuthToken = builder.AuthToken;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for StartupSettings
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the name of the startup settings.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of pending requests allowed.
            /// </summary>
            public int MaxPendingRequest { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of session pending requests allowed.
            /// </summary>
            public int MaxSessionPendingRequest { get; set; }

            /// <summary>
            /// Gets or sets the maximum number of session requests allowed per second.
            /// </summary>
            public int MaxSessionRequestPerSecond { get; set; }

            /// <summary>
            /// Gets or sets the HTTP server settings.
            /// </summary>
            public HttpServerSettings HttpServer { get; set; }

            /// <summary>
            /// Gets or sets the thread pool size settings.
            /// </summary>
            public ThreadPoolSizeSettings ThreadPoolSize { get; set; }

            /// <summary>
            /// Gets the auth token settings.
            /// </summary>
            public AuthTokenSettings AuthToken { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the name of the startup settings.
            /// </summary>
            /// <param name="name">The name of the startup settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetName(string name)
            {
                this.Name = name;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of pending requests allowed.
            /// </summary>
            /// <param name="maxPendingRequest">The maximum number of pending requests allowed.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetMaxPendingRequest(int maxPendingRequest)
            {
                this.MaxPendingRequest = maxPendingRequest;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of session pending requests allowed.
            /// </summary>
            /// <param name="maxSessionPendingRequest">The maximum number of session pending requests allowed.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetMaxSessionPendingRequest(int maxSessionPendingRequest)
            {
                this.MaxSessionPendingRequest = maxSessionPendingRequest;
                return this;
            }

            /// <summary>
            /// Sets the maximum number of session requests allowed per second.
            /// </summary>
            /// <param name="maxSessionRequestPerSecond">The maximum number of session requests allowed per second.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetMaxSessionRequestPerSecond(int maxSessionRequestPerSecond)
            {
                this.MaxSessionRequestPerSecond = maxSessionRequestPerSecond;
                return this;
            }

            /// <summary>
            /// Sets the HTTP server settings.
            /// </summary>
            /// <param name="httpServer">The HTTP server settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetHttpServer(HttpServerSettings httpServer)
            {
                this.HttpServer = httpServer;
                return this;
            }

            /// <summary>
            /// Sets the thread pool size settings.
            /// </summary>
            /// <param name="threadPoolSize">The thread pool size settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetThreadPoolSize(ThreadPoolSizeSettings threadPoolSize)
            {
                this.ThreadPoolSize = threadPoolSize;
                return this;
            }

            /// <summary>
            /// Sets the auth token settings.
            /// </summary>
            /// <param name="authToken">The auth token settings.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetAuthToken(AuthTokenSettings authToken)
            {
                this.AuthToken = authToken;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="StartupSettings"/> class.
            /// </summary>
            /// <returns>A new <see cref="StartupSettings"/> instance.</returns>
            public StartupSettings Build() => new StartupSettings(this);

        }

    }

}

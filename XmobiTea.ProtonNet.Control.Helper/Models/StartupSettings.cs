﻿using Newtonsoft.Json;

namespace XmobiTea.ProtonNet.Control.Helper.Models
{
    /// <summary>
    /// Settings auth token service
    /// </summary>
    public class AuthTokenSettings
    {
        /// <summary>
        /// Gets the password of UserPeerAuthTokenService
        /// Default is empty.
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; } = string.Empty;

    }

    /// <summary>
    /// Settings for thread pool size configuration.
    /// </summary>
    public class ThreadPoolSizeSettings
    {
        /// <summary>
        /// The number of fibers used for other purposes.
        /// Default is 2.
        /// </summary>
        [JsonProperty("OtherFiber")]
        public byte OtherFiber { get; private set; } = 2;

        /// <summary>
        /// The number of fibers used for handling received data.
        /// Default is 12.
        /// </summary>
        [JsonProperty("ReceivedFiber")]
        public byte ReceivedFiber { get; private set; } = 12;

    }

    /// <summary>
    /// Configuration settings for SSL/TLS.
    /// </summary>
    public class SslConfigSettings
    {
        /// <summary>
        /// Indicates whether SSL/TLS is enabled.
        /// Default is false.
        /// </summary>
        [JsonProperty("Enable")]
        public bool Enable { get; private set; } = false;

        /// <summary>
        /// Port number for SSL/TLS.
        /// </summary>
        [JsonProperty("Port")]
        public int Port { get; private set; }

        /// <summary>
        /// Path to the SSL/TLS certificate file.
        /// </summary>
        [JsonProperty("CertFilePath")]
        public string CertFilePath { get; private set; }

        /// <summary>
        /// Password for the SSL/TLS certificate.
        /// </summary>
        [JsonProperty("CertPassword")]
        public string CertPassword { get; private set; }

    }

    /// <summary>
    /// Configuration settings for session management.
    /// </summary>
    public class SessionConfigSettings
    {
        /// <summary>
        /// Maximum number of queued connections for the server.
        /// Default is 1024.
        /// </summary>
        [JsonProperty("AcceptorBacklog")]
        public int AcceptorBacklog { get; private set; } = 1024;

        /// <summary>
        /// Indicates whether dual mode (IPv4/IPv6) is enabled.
        /// Default is false.
        /// </summary>
        [JsonProperty("DualMode")]
        public bool DualMode { get; private set; } = false;

        /// <summary>
        /// Indicates whether keep-alive is enabled for TCP connections.
        /// Default is true.
        /// </summary>
        [JsonProperty("KeepAlive")]
        public bool KeepAlive { get; private set; } = true;

        /// <summary>
        /// TCP keep-alive time in seconds.
        /// Default is -1 (disabled).
        /// </summary>
        [JsonProperty("TcpKeepAliveTime")]
        public int TcpKeepAliveTime { get; private set; } = -1;

        /// <summary>
        /// TCP keep-alive interval in seconds.
        /// Default is -1 (disabled).
        /// </summary>
        [JsonProperty("TcpKeepAliveInterval")]
        public int TcpKeepAliveInterval { get; private set; } = -1;

        /// <summary>
        /// Number of TCP keep-alive retry attempts.
        /// Default is -1 (disabled).
        /// </summary>
        [JsonProperty("TcpKeepAliveRetryCount")]
        public int TcpKeepAliveRetryCount { get; private set; } = -1;

        /// <summary>
        /// Indicates whether Nagle's algorithm is disabled (no delay).
        /// Default is true.
        /// </summary>
        [JsonProperty("NoDelay")]
        public bool NoDelay { get; private set; } = true;

        /// <summary>
        /// Indicates whether the address can be reused.
        /// Default is false.
        /// </summary>
        [JsonProperty("ReuseAddress")]
        public bool ReuseAddress { get; private set; } = false;

        /// <summary>
        /// Indicates whether exclusive address use is enforced.
        /// Default is false.
        /// </summary>
        [JsonProperty("ExclusiveAddressUse")]
        public bool ExclusiveAddressUse { get; private set; } = false;

        /// <summary>
        /// Limit of the receive buffer size.
        /// Default is 0 (no limit).
        /// </summary>
        [JsonProperty("ReceiveBufferLimit")]
        public int ReceiveBufferLimit { get; private set; } = 0;

        /// <summary>
        /// Capacity of the receive buffer in bytes.
        /// Default is 8096.
        /// </summary>
        [JsonProperty("ReceiveBufferCapacity")]
        public int ReceiveBufferCapacity { get; private set; } = 8096;

        /// <summary>
        /// Limit of the send buffer size.
        /// Default is 0 (no limit).
        /// </summary>
        [JsonProperty("SendBufferLimit")]
        public int SendBufferLimit { get; private set; } = 0;

        /// <summary>
        /// Capacity of the send buffer in bytes.
        /// Default is 8096.
        /// </summary>
        [JsonProperty("SendBufferCapacity")]
        public int SendBufferCapacity { get; private set; } = 8096;

    }

    /// <summary>
    /// Settings for HTTP server configuration.
    /// </summary>
    public class HttpServerSettings
    {
        /// <summary>
        /// Indicates whether the HTTP server is enabled.
        /// Default is true.
        /// </summary>
        [JsonProperty("Enable")]
        public bool Enable { get; private set; } = true;

        /// <summary>
        /// Address to bind the HTTP server to.
        /// Default is "0.0.0.0".
        /// </summary>
        [JsonProperty("Address")]
        public string Address { get; private set; } = "0.0.0.0";

        /// <summary>
        /// Port number for the HTTP server.
        /// Default is 22202.
        /// </summary>
        [JsonProperty("Port")]
        public int Port { get; private set; } = 22202;

        /// <summary>
        /// Configuration settings for sessions.
        /// </summary>
        [JsonProperty("SessionConfig")]
        public SessionConfigSettings SessionConfig { get; private set; }

        /// <summary>
        /// Configuration settings for SSL/TLS.
        /// </summary>
        [JsonProperty("SslConfig")]
        public SslConfigSettings SslConfig { get; private set; }

    }

    /// <summary>
    /// Startup settings for Web API.
    /// </summary>
    public class WebApiStartupSettings
    {
        /// <summary>
        /// Name of the Web API.
        /// Default is an empty string.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; private set; } = "";

        /// <summary>
        /// Maximum number of pending requests for the Web API.
        /// Default is 10000.
        /// </summary>
        [JsonProperty("MaxPendingRequest")]
        public int MaxPendingRequest { get; private set; } = 10000;

        /// <summary>
        /// Maximum number of pending requests per session.
        /// Default is 100.
        /// </summary>
        [JsonProperty("MaxSessionPendingRequest")]
        public int MaxSessionPendingRequest { get; private set; } = 100;

        /// <summary>
        /// Maximum number of requests per second per session.
        /// Default is 50.
        /// </summary>
        [JsonProperty("MaxSessionRequestPerSecond")]
        public int MaxSessionRequestPerSecond { get; private set; } = 50;

        /// <summary>
        /// HTTP server settings.
        /// </summary>
        [JsonProperty("HttpServer")]
        public HttpServerSettings HttpServer { get; private set; }

        /// <summary>
        /// Thread pool size settings.
        /// </summary>
        [JsonProperty("ThreadPoolSize")]
        public ThreadPoolSizeSettings ThreadPoolSize { get; private set; }

        /// <summary>
        /// Thread pool size settings.
        /// </summary>
        [JsonProperty("AuthToken")]
        public AuthTokenSettings AuthToken { get; private set; }

    }

    /// <summary>
    /// Settings for TCP server configuration.
    /// </summary>
    public class TcpServerSettings
    {
        /// <summary>
        /// Indicates whether the TCP server is enabled.
        /// Default is true.
        /// </summary>
        [JsonProperty("Enable")]
        public bool Enable { get; private set; } = true;

        /// <summary>
        /// Address to bind the TCP server to.
        /// Default is "0.0.0.0".
        /// </summary>
        [JsonProperty("Address")]
        public string Address { get; private set; } = "0.0.0.0";

        /// <summary>
        /// Port number for the TCP server.
        /// Default is 32202.
        /// </summary>
        [JsonProperty("Port")]
        public int Port { get; private set; } = 32202;

        /// <summary>
        /// Configuration settings for sessions.
        /// </summary>
        [JsonProperty("SessionConfig")]
        public SessionConfigSettings SessionConfig { get; private set; }

        /// <summary>
        /// Configuration settings for SSL/TLS.
        /// </summary>
        [JsonProperty("SslConfig")]
        public SslConfigSettings SslConfig { get; private set; }

    }

    /// <summary>
    /// Configuration settings for UDP session.
    /// </summary>
    public class UdpSessionConfigSettings
    {
        /// <summary>
        /// Indicates whether dual mode (IPv4/IPv6) is enabled.
        /// Default is false.
        /// </summary>
        [JsonProperty("DualMode")]
        public bool DualMode { get; private set; } = false;

        /// <summary>
        /// Indicates whether the address can be reused.
        /// Default is false.
        /// </summary>
        [JsonProperty("ReuseAddress")]
        public bool ReuseAddress { get; private set; } = false;

        /// <summary>
        /// Indicates whether exclusive address use is enforced.
        /// Default is false.
        /// </summary>
        [JsonProperty("ExclusiveAddressUse")]
        public bool ExclusiveAddressUse { get; private set; } = false;

        /// <summary>
        /// Limit of the receive buffer size.
        /// Default is 0 (no limit).
        /// </summary>
        [JsonProperty("ReceiveBufferLimit")]
        public int ReceiveBufferLimit { get; private set; } = 0;

        /// <summary>
        /// Capacity of the receive buffer in bytes.
        /// Default is 8096.
        /// </summary>
        [JsonProperty("ReceiveBufferCapacity")]
        public int ReceiveBufferCapacity { get; private set; } = 8096;

        /// <summary>
        /// Limit of the send buffer size.
        /// Default is 0 (no limit).
        /// </summary>
        [JsonProperty("SendBufferLimit")]
        public int SendBufferLimit { get; private set; } = 0;

    }

    /// <summary>
    /// Settings for UDP server configuration.
    /// </summary>
    public class UdpServerSettings
    {
        /// <summary>
        /// Indicates whether the UDP server is enabled.
        /// Default is false.
        /// </summary>
        [JsonProperty("Enable")]
        public bool Enable { get; private set; } = false;

        /// <summary>
        /// Address to bind the UDP server to.
        /// Default is "0.0.0.0".
        /// </summary>
        [JsonProperty("Address")]
        public string Address { get; private set; } = "0.0.0.0";

        /// <summary>
        /// Port number for the UDP server.
        /// Default is 42202.
        /// </summary>
        [JsonProperty("Port")]
        public int Port { get; private set; } = 42202;

        /// <summary>
        /// Configuration settings for sessions.
        /// </summary>
        [JsonProperty("SessionConfig")]
        public UdpSessionConfigSettings SessionConfig { get; private set; }

    }

    /// <summary>
    /// Settings for WebSocket server configuration.
    /// </summary>
    public class WebSocketServerSettings
    {
        /// <summary>
        /// Indicates whether the WebSocket server is enabled.
        /// Default is false.
        /// </summary>
        [JsonProperty("Enable")]
        public bool Enable { get; private set; } = false;

        /// <summary>
        /// Address to bind the WebSocket server to.
        /// Default is "0.0.0.0".
        /// </summary>
        [JsonProperty("Address")]
        public string Address { get; private set; } = "0.0.0.0";

        /// <summary>
        /// Port number for the WebSocket server.
        /// Default is 52202.
        /// </summary>
        [JsonProperty("Port")]
        public int Port { get; private set; } = 52202;

        /// <summary>
        /// Maximum frame size for WebSocket messages.
        /// Default is 8096 bytes.
        /// </summary>
        [JsonProperty("MaxFrameSize")]
        public int MaxFrameSize { get; private set; } = 8096;

        /// <summary>
        /// Configuration settings for sessions.
        /// </summary>
        [JsonProperty("SessionConfig")]
        public SessionConfigSettings SessionConfig { get; private set; }

        /// <summary>
        /// Configuration settings for SSL/TLS.
        /// </summary>
        [JsonProperty("SslConfig")]
        public SslConfigSettings SslConfig { get; private set; }

    }

    /// <summary>
    /// Startup settings for the socket server.
    /// </summary>
    public class SocketStartupSettings
    {
        /// <summary>
        /// Name of the socket server.
        /// Default is an empty string.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; private set; } = "";

        /// <summary>
        /// Maximum number of sessions for the socket server.
        /// Default is 5000.
        /// </summary>
        [JsonProperty("MaxSession")]
        public int MaxSession { get; private set; } = 5000;

        /// <summary>
        /// Maximum number of pending requests for the socket server.
        /// Default is 10000.
        /// </summary>
        [JsonProperty("MaxPendingRequest")]
        public int MaxPendingRequest { get; private set; } = 10000;

        /// <summary>
        /// Maximum number of pending requests per session.
        /// Default is 100.
        /// </summary>
        [JsonProperty("MaxSessionPendingRequest")]
        public int MaxSessionPendingRequest { get; private set; } = 100;

        /// <summary>
        /// Maximum number of requests per second per session.
        /// Default is 50.
        /// </summary>
        [JsonProperty("MaxSessionRequestPerSecond")]
        public int MaxSessionRequestPerSecond { get; private set; } = 50;

        /// <summary>
        /// Maximum number of UDP session requests per user.
        /// Default is 1.
        /// </summary>
        [JsonProperty("MaxUdpSessionRequestPerUser")]
        public int MaxUdpSessionRequestPerUser { get; private set; } = 1;

        /// <summary>
        /// Maximum number of TCP session requests per user.
        /// Default is 1.
        /// </summary>
        [JsonProperty("MaxTcpSessionRequestPerUser")]
        public int MaxTcpSessionRequestPerUser { get; private set; } = 1;

        /// <summary>
        /// Maximum number of WebSocket session requests per user.
        /// Default is 1.
        /// </summary>
        [JsonProperty("MaxWsSessionRequestPerUser")]
        public int MaxWsSessionRequestPerUser { get; private set; } = 1;

        /// <summary>
        /// Timeout for handshake in seconds.
        /// Default is 300 seconds.
        /// </summary>
        [JsonProperty("HandshakeTimeoutInSeconds")]
        public int HandshakeTimeoutInSeconds { get; private set; } = 300;

        /// <summary>
        /// Timeout for idle sessions in seconds.
        /// Default is 120 seconds.
        /// </summary>
        [JsonProperty("IdleTimeoutInSeconds")]
        public int IdleTimeoutInSeconds { get; private set; } = 120;

        /// <summary>
        /// TCP server settings.
        /// </summary>
        [JsonProperty("TcpServer")]
        public TcpServerSettings TcpServer { get; private set; }

        /// <summary>
        /// UDP server settings.
        /// </summary>
        [JsonProperty("UdpServer")]
        public UdpServerSettings UdpServer { get; private set; }

        /// <summary>
        /// WebSocket server settings.
        /// </summary>
        [JsonProperty("WebSocketServer")]
        public WebSocketServerSettings WebSocketServer { get; private set; }

        /// <summary>
        /// Thread pool size settings.
        /// </summary>
        [JsonProperty("ThreadPoolSize")]
        public ThreadPoolSizeSettings ThreadPoolSize { get; private set; }

        /// <summary>
        /// Auth token settings.
        /// </summary>
        [JsonProperty("AuthToken")]
        public AuthTokenSettings AuthToken { get; private set; }

    }

}

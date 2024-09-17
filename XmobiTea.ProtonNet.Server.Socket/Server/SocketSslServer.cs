using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.Socket.Server
{
    /// <summary>
    /// Represents a secure TCP server (SSL/TLS) that integrates with the ProtonNet framework,
    /// allowing for secure TCP communication within the socket server context.
    /// </summary>
    class SocketSslServer : SslServer, IServer
    {
        /// <summary>
        /// Gets the context of the socket server, which provides services and configuration for the server.
        /// </summary>
        private ISocketServerContext context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSslServer"/> class with the specified address, port, TCP options, SSL options, and context.
        /// </summary>
        /// <param name="address">The IP address the server will bind to.</param>
        /// <param name="port">The port the server will listen on.</param>
        /// <param name="options">The TCP server options for configuring the server's behavior.</param>
        /// <param name="sslOptions">The SSL options for secure communication.</param>
        /// <param name="context">The context that provides services and configuration for the server.</param>
        public SocketSslServer(string address, int port, TcpServerOptions options, ProtonNetCommon.SslOptions sslOptions, ISocketServerContext context) : base(address, port, options, sslOptions) => this.context = context;

        /// <summary>
        /// Gets the address the server is bound to.
        /// </summary>
        /// <returns>The server's IP address.</returns>
        public string GetAddress() => this.Address;

        /// <summary>
        /// Gets the port the server is listening on.
        /// </summary>
        /// <returns>The server's port number.</returns>
        public int GetPort() => this.Port;

        /// <summary>
        /// Creates a new instance of the <see cref="SocketSslSession"/> to handle a secure TCP session.
        /// </summary>
        /// <returns>A new <see cref="SocketSslSession"/> instance.</returns>
        protected override SslSession CreateSession() => new SocketSslSession(this, this.context);

    }

}

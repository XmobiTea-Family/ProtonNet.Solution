using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.Socket.Server
{
    /// <summary>
    /// Represents a TCP server that integrates with the ProtonNet framework,
    /// allowing for TCP communication within the socket server context.
    /// </summary>
    class SocketTcpServer : TcpServer, IServer
    {
        /// <summary>
        /// Gets the context of the socket server, which provides services and configuration for the server.
        /// </summary>
        private ISocketServerContext context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketTcpServer"/> class with the specified address, port, and options.
        /// </summary>
        /// <param name="address">The IP address the server will bind to.</param>
        /// <param name="port">The port the server will listen on.</param>
        /// <param name="options">The TCP server options for configuring the server's behavior.</param>
        /// <param name="context">The context that provides services and configuration for the server.</param>
        public SocketTcpServer(string address, int port, TcpServerOptions options, ISocketServerContext context) : base(address, port, options) => this.context = context;

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
        /// Creates a new instance of the <see cref="SocketTcpSession"/> to handle a TCP session.
        /// </summary>
        /// <returns>A new <see cref="SocketTcpSession"/> instance.</returns>
        protected override TcpSession CreateSession() => new SocketTcpSession(this, this.context);

    }

}

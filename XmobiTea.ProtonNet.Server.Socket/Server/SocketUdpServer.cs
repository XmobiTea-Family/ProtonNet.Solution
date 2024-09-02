using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.Socket.Server
{
    /// <summary>
    /// Represents a UDP server that integrates with the ProtonNet framework,
    /// allowing for UDP communication within the socket server context.
    /// </summary>
    class SocketUdpServer : UdpServer, IServer
    {
        /// <summary>
        /// Gets the context of the socket server, which provides services and configuration for the server.
        /// </summary>
        private ISocketServerContext context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketUdpServer"/> class with the specified address, port, and options.
        /// </summary>
        /// <param name="address">The IP address the server will bind to.</param>
        /// <param name="port">The port the server will listen on.</param>
        /// <param name="options">The UDP server options for configuring the server's behavior.</param>
        /// <param name="context">The context that provides services and configuration for the server.</param>
        public SocketUdpServer(string address, int port, UdpServerOptions options, ISocketServerContext context) : base(address, port, options) => this.context = context;

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
        /// Creates a new instance of the <see cref="SocketUdpSession"/> to handle a UDP session.
        /// </summary>
        /// <returns>A new <see cref="SocketUdpSession"/> instance.</returns>
        protected override UdpSession CreateSession() => new SocketUdpSession(this, this.context);

    }

}

using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.WebApi.Server
{
    /// <summary>
    /// Represents an HTTPS server for handling Web API requests, inheriting from HttpsServer and implementing ISubServer.
    /// </summary>
    /// <remarks>Handles the server context and provides methods to get server address and port. Creates WebApiHttpsSession for handling individual sessions.</remarks>
    class WebApiHttpsServer : HttpsServer, ISubServer
    {
        private IWebApiServerContext context { get; }

        /// <summary>
        /// Initializes a new instance of the WebApiHttpsServer class.
        /// </summary>
        /// <param name="address">The address to bind the server to.</param>
        /// <param name="port">The port number to listen on.</param>
        /// <param name="options">TCP server options for configuration.</param>
        /// <param name="sslOptions">SSL options for securing connections.</param>
        /// <param name="context">The context for the Web API server.</param>
        /// <remarks>Passes parameters to the base HttpsServer class and sets the server context.</remarks>
        public WebApiHttpsServer(string address, int port, TcpServerOptions options, SslOptions sslOptions, IWebApiServerContext context) : base(address, port, options, sslOptions) => this.context = context;

        /// <summary>
        /// Gets the address the server is bound to.
        /// </summary>
        /// <returns>The address of the server.</returns>
        public string GetAddress() => this.Address;

        /// <summary>
        /// Gets the port number the server is listening on.
        /// </summary>
        /// <returns>The port number of the server.</returns>
        public int GetPort() => this.Port;

        /// <summary>
        /// Creates a new HTTPS session for handling Web API requests.
        /// </summary>
        /// <returns>A new instance of WebApiHttpsSession.</returns>
        protected override SslSession CreateSession() => new WebApiHttpsSession(this, this.context);

    }

}

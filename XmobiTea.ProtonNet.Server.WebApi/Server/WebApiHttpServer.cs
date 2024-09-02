using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.WebApi.Server
{
    /// <summary>
    /// Represents an HTTP server for handling Web API requests, inheriting from HttpServer and implementing ISubServer.
    /// </summary>
    /// <remarks>Handles the server context and provides methods to get server address and port. Creates WebApiHttpSession for handling individual sessions.</remarks>
    class WebApiHttpServer : HttpServer, ISubServer
    {
        private IWebApiServerContext context { get; }

        /// <summary>
        /// Initializes a new instance of the WebApiHttpServer class.
        /// </summary>
        /// <param name="address">The address to bind the server to.</param>
        /// <param name="port">The port number to listen on.</param>
        /// <param name="options">TCP server options for configuration.</param>
        /// <param name="context">The context for the Web API server.</param>
        /// <remarks>Passes parameters to the base HttpServer class and sets the server context.</remarks>
        public WebApiHttpServer(string address, int port, TcpServerOptions options, IWebApiServerContext context) : base(address, port, options)
        {
            this.context = context;
        }

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
        /// Creates a new HTTP session for handling Web API requests.
        /// </summary>
        /// <returns>A new instance of WebApiHttpSession.</returns>
        protected override TcpSession CreateSession() => new WebApiHttpSession(this, this.context);

    }

}

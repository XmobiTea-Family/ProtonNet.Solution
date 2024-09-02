using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents an interface for an HTTP server, inheriting from the IServer interface.
    /// </summary>
    public interface IHttpServer : IServer
    {

    }

    /// <summary>
    /// Represents an HTTP server, inheriting from the TcpServer class.
    /// </summary>
    public class HttpServer : TcpServer, IHttpServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="address">The IP address of the server.</param>
        /// <param name="port">The port number of the server.</param>
        /// <param name="options">The TCP server options.</param>
        public HttpServer(string address, int port, TcpServerOptions options)
            : base(address, port, options) { }

        /// <summary>
        /// Creates a new HTTP session.
        /// </summary>
        /// <returns>A new instance of <see cref="HttpSession"/>.</returns>
        protected override TcpSession CreateSession() => new HttpSession(this);

    }

    /// <summary>
    /// Represents an HTTPS server, inheriting from the SslServer class.
    /// </summary>
    public class HttpsServer : SslServer, IHttpServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpsServer"/> class.
        /// </summary>
        /// <param name="address">The IP address of the server.</param>
        /// <param name="port">The port number of the server.</param>
        /// <param name="options">The TCP server options.</param>
        /// <param name="sslOptions">The Ssl options for secure communication.</param>
        public HttpsServer(string address, int port, TcpServerOptions options, SslOptions sslOptions)
            : base(address, port, options, sslOptions) { }

        /// <summary>
        /// Creates a new HTTPS session.
        /// </summary>
        /// <returns>A new instance of <see cref="HttpsSession"/>.</returns>
        protected override SslSession CreateSession() => new HttpsSession(this);

    }

}

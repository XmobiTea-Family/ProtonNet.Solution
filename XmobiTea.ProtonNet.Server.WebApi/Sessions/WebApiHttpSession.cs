using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNet.Server.WebApi.Extensions;
using XmobiTea.ProtonNet.Server.WebApi.Server;
using XmobiTea.ProtonNet.Server.WebApi.Types;
using XmobiTea.ProtonNetServer;
using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.WebApi.Sessions
{
    /// <summary>
    /// Represents an HTTP session in the Web API server.
    /// </summary>
    class WebApiHttpSession : HttpSession, IWebApiSession
    {
        /// <summary>
        /// Gets the server context associated with the session.
        /// </summary>
        protected IWebApiServerContext context { get; }

        private int connectionId { get; }
        private string serverSessionId { get; }
        private byte[] encryptKey { get; set; }
        private string sessionId { get; set; }
        private IFiber fiber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiHttpSession"/> class.
        /// </summary>
        /// <param name="server">The Web API server associated with this session.</param>
        /// <param name="context">The server context associated with this session.</param>
        public WebApiHttpSession(WebApiHttpServer server, IWebApiServerContext context) : base(server)
        {
            this.context = context;

            var initRequest = this.context.GetInitRequestProviderService().NewSessionInitRequest();

            this.connectionId = initRequest.ConnectionId;
            this.serverSessionId = initRequest.ServerSessionId;
            this.sessionId = initRequest.SessionId;
            this.fiber = initRequest.Fiber;
        }

        /// <summary>
        /// Gets the Hypertext Transfer Protocol used by this session.
        /// </summary>
        /// <returns>The <see cref="HypertextTransferProtocol"/> used by this session.</returns>
        public HypertextTransferProtocol GetHypertextTransferProtocol() => HypertextTransferProtocol.Http;

        /// <summary>
        /// Gets the connection ID of this session.
        /// </summary>
        /// <returns>The connection ID.</returns>
        public int GetConnectionId() => this.connectionId;

        /// <summary>
        /// Gets the server session ID of this session.
        /// </summary>
        /// <returns>The server session ID.</returns>
        public string GetServerSessionId() => this.serverSessionId;

        /// <summary>
        /// Gets the encryption key for this session.
        /// </summary>
        /// <returns>The encryption key as a byte array.</returns>
        public byte[] GetEncryptKey() => this.encryptKey;

        /// <summary>
        /// Gets the session ID of this session.
        /// </summary>
        /// <returns>The session ID.</returns>
        public string GetSessionId() => this.sessionId;

        /// <summary>
        /// Gets the remote IP address of the client.
        /// </summary>
        /// <returns>The remote IP address as a string.</returns>
        public string GetRemoteIP() => this.Socket == null ? string.Empty : (this.Socket.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString();

        /// <summary>
        /// Gets the remote port of the client.
        /// </summary>
        /// <returns>The remote port.</returns>
        public int GetRemotePort() => this.Socket == null ? -1 : (this.Socket.RemoteEndPoint as System.Net.IPEndPoint).Port;

        /// <summary>
        /// Sets the session ID for this session.
        /// </summary>
        /// <param name="sessionId">The new session ID.</param>
        public void SetSessionId(string sessionId) => this.sessionId = sessionId;

        /// <summary>
        /// Sets the encryption key for this session.
        /// </summary>
        /// <param name="encryptKey">The new encryption key as a byte array.</param>
        public void SetEncryptKey(byte[] encryptKey) => this.encryptKey = encryptKey;

        /// <summary>
        /// Gets the fiber associated with this session.
        /// </summary>
        /// <returns>The <see cref="IFiber"/> associated with this session.</returns>
        public IFiber GetFiber() => this.fiber;

        /// <summary>
        /// Handles the event when the session is connected.
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();

            this.context.GetControllerService().OnConnected(this);
        }

        /// <summary>
        /// Handles the event when a request is received.
        /// </summary>
        /// <param name="request">The received <see cref="ProtonNetCommon.HttpRequest"/>.</param>
        protected override void OnReceivedRequest(ProtonNetCommon.HttpRequest request)
        {
            var cloneRequest = request.Clone();

            if (this.fiber == null) this.context.GetControllerService().OnReceived(this, cloneRequest);
            else this.fiber.Enqueue(() => this.context.GetControllerService().OnReceived(this, cloneRequest));
        }

        /// <summary>
        /// Handles the event when a request error is received.
        /// </summary>
        /// <param name="request">The <see cref="ProtonNetCommon.HttpRequest"/> that caused the error.</param>
        /// <param name="error">The error message.</param>
        protected override void OnReceivedRequestError(ProtonNetCommon.HttpRequest request, string error)
        {
            var cloneRequest = request.Clone();

            this.context.GetControllerService().OnReceivedRequestError(this, cloneRequest, error);
        }

        /// <summary>
        /// Handles the event when a socket error occurs.
        /// </summary>
        /// <param name="error">The <see cref="System.Net.Sockets.SocketError"/>.</param>
        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            this.context.GetControllerService().OnError(this, error);
        }

        /// <summary>
        /// Handles the event when the session is disconnected.
        /// </summary>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            this.context.GetControllerService().OnDisconnected(this);

            if (this.fiber is IFiberControl fiberControl) fiberControl.Dispose();
        }

    }

}

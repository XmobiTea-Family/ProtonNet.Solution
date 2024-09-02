using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents an interface for an HTTP session, inheriting from the ISession interface.
    /// </summary>
    public interface IHttpSession : ISession
    {
        /// <summary>
        /// Sends an HTTP response synchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendResponse(HttpResponse response);

        /// <summary>
        /// Sends an HTTP response asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>True if the response was sent successfully; otherwise, false.</returns>
        bool SendResponseAsync(HttpResponse response);
    }

    /// <summary>
    /// Represents an HTTP session, inheriting from the TcpSession class.
    /// </summary>
    public class HttpSession : TcpSession, IHttpSession
    {
        /// <summary>
        /// Gets the current HTTP request being processed.
        /// </summary>
        protected HttpRequest Request { get; }

        /// <summary>
        /// Gets the current HTTP response being processed.
        /// </summary>
        public HttpResponse Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSession"/> class.
        /// </summary>
        /// <param name="server">The HTTP server that manages this session.</param>
        public HttpSession(HttpServer server) : base(server)
        {
            this.Request = new HttpRequest();
            this.Response = new HttpResponse();
        }

        /// <summary>
        /// Sends an HTTP response synchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int SendResponse(HttpResponse response) => this.Send(response.Cache.Buffer, response.Cache.Position, response.Cache.Length);

        /// <summary>
        /// Sends an HTTP response asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>True if the response was sent successfully; otherwise, false.</returns>
        public virtual bool SendResponseAsync(HttpResponse response) => this.SendAsync(response.Cache.Buffer, response.Cache.Position, response.Cache.Length);

        /// <summary>
        /// Handles the data received from the client.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.Request.IsPendingHeader())
            {
                if (this.Request.SetHeaderBuffer(buffer, position, length))
                    this.OnReceivedRequestHeader(this.Request);

                length = 0;
            }

            if (this.Request.IsErrorSet)
            {
                this.OnReceivedRequestError(this.Request, "Invalid HTTP request after setting header.");
                this.Request.Clear();
                this.Disconnect();
                return;
            }

            if (this.Request.SetBodyBuffer(buffer, position, length))
            {
                this.OnReceivedRequestInternal(this.Request);
                this.Request.Clear();
                return;
            }

            if (this.Request.IsErrorSet)
            {
                this.OnReceivedRequestError(this.Request, "Invalid HTTP request after setting body.");
                this.Request.Clear();
                this.Disconnect();
                return;
            }
        }

        /// <summary>
        /// Handles the session disconnection.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.Request.IsPendingBody())
            {
                this.OnReceivedRequestInternal(this.Request);
                this.Request.Clear();
                return;
            }
        }

        /// <summary>
        /// Called when the request header is received.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        protected virtual void OnReceivedRequestHeader(HttpRequest request) { }

        /// <summary>
        /// Called when the full request is received.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        protected virtual void OnReceivedRequest(HttpRequest request) { }

        /// <summary>
        /// Called when an error occurs while processing the request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="error">The error message.</param>
        protected virtual void OnReceivedRequestError(HttpRequest request, string error) { }

        /// <summary>
        /// Internal method to process the received request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        private void OnReceivedRequestInternal(HttpRequest request) => this.OnReceivedRequest(request);
    }

    /// <summary>
    /// Represents an HTTPS session, inheriting from the SslSession class.
    /// </summary>
    public class HttpsSession : SslSession, IHttpSession
    {
        /// <summary>
        /// Gets the current HTTP request being processed.
        /// </summary>
        protected HttpRequest Request { get; }

        /// <summary>
        /// Gets the current HTTP response being processed.
        /// </summary>
        public HttpResponse Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpsSession"/> class.
        /// </summary>
        /// <param name="server">The HTTPS server that manages this session.</param>
        public HttpsSession(HttpsServer server) : base(server)
        {
            this.Request = new HttpRequest();
            this.Response = new HttpResponse();
        }

        /// <summary>
        /// Sends an HTTP response synchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int SendResponse(HttpResponse response) => this.Send(response.Cache.Buffer, response.Cache.Position, response.Cache.Length);

        /// <summary>
        /// Sends an HTTP response asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response to send.</param>
        /// <returns>True if the response was sent successfully; otherwise, false.</returns>
        public virtual bool SendResponseAsync(HttpResponse response) => this.SendAsync(response.Cache.Buffer, response.Cache.Position, response.Cache.Length);

        /// <summary>
        /// Handles the data received from the client.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.Request.IsPendingHeader())
            {
                if (this.Request.SetHeaderBuffer(buffer, position, length))
                    this.OnReceivedRequestHeader(this.Request);

                length = 0;
            }

            if (this.Request.IsErrorSet)
            {
                this.OnReceivedRequestError(this.Request, "Invalid HTTP request after setting header.");
                this.Request.Clear();
                this.Disconnect();
                return;
            }

            if (this.Request.SetBodyBuffer(buffer, position, length))
            {
                this.OnReceivedRequestInternal(this.Request);
                this.Request.Clear();
                return;
            }

            if (this.Request.IsErrorSet)
            {
                this.OnReceivedRequestError(this.Request, "Invalid HTTP request after setting body.");
                this.Request.Clear();
                this.Disconnect();
                return;
            }
        }

        /// <summary>
        /// Handles the session disconnection.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.Request.IsPendingBody())
            {
                this.OnReceivedRequestInternal(this.Request);
                this.Request.Clear();
                return;
            }
        }

        /// <summary>
        /// Called when the request header is received.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        protected virtual void OnReceivedRequestHeader(HttpRequest request) { }

        /// <summary>
        /// Called when the full request is received.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        protected virtual void OnReceivedRequest(HttpRequest request) { }

        /// <summary>
        /// Called when an error occurs while processing the request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="error">The error message.</param>
        protected virtual void OnReceivedRequestError(HttpRequest request, string error) { }

        /// <summary>
        /// Internal method to process the received request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        private void OnReceivedRequestInternal(HttpRequest request) => this.OnReceivedRequest(request);

    }

}

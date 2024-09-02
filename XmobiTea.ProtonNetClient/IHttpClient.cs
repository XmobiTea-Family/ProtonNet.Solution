using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNetClient
{
    /// <summary>
    /// Defines the interface for an HTTP client that can send requests 
    /// and handle responses over a network. Inherits from <see cref="IClient"/>.
    /// </summary>
    public interface IHttpClient : IClient
    {
        /// <summary>
        /// Sends an HTTP request synchronously.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendRequest(HttpRequest request);

        /// <summary>
        /// Sends an HTTP request asynchronously.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>True if the request was sent successfully; otherwise, false.</returns>
        bool SendRequestAsync(HttpRequest request);
    }

    /// <summary>
    /// Represents an HTTP client that handles sending requests and 
    /// receiving responses over a TCP connection.
    /// </summary>
    public class HttpClient : TcpClient, IHttpClient
    {
        /// <summary>
        /// Gets the current HTTP request being processed by the client.
        /// </summary>
        public HttpRequest Request { get; }

        /// <summary>
        /// Gets the current HTTP response received from the server.
        /// </summary>
        protected HttpResponse Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        public HttpClient(string address, int port, TcpClientOptions options)
            : base(address, port, options)
        {
            this.Request = new HttpRequest();
            this.Response = new HttpResponse();
        }

        /// <summary>
        /// Sends an HTTP request synchronously using the provided request data.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendRequest(HttpRequest request) => this.Send(request.Cache.Buffer, request.Cache.Position, request.Cache.Length);

        /// <summary>
        /// Sends an HTTP request asynchronously using the provided request data.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>True if the request was sent successfully; otherwise, false.</returns>
        public bool SendRequestAsync(HttpRequest request) => this.SendAsync(request.Cache.Buffer, request.Cache.Position, request.Cache.Length);

        /// <summary>
        /// Handles data received from the server. Processes HTTP response headers 
        /// and body data accordingly.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.Response.IsPendingHeader())
            {
                if (this.Response.SetHeaderBuffer(buffer, position, length))
                    this.OnReceivedResponseHeader(this.Response);

                length = 0;
            }

            if (this.Response.IsErrorSet)
            {
                this.OnReceivedResponseError(this.Response, "Invalid HttpResponse after set header.");
                this.Response.Clear();
                this.Disconnect();
                return;
            }

            if (this.Response.SetBodyBuffer(buffer, position, length))
            {
                this.OnReceivedResponse(this.Response);
                this.Response.Clear();
                return;
            }

            if (this.Response.IsErrorSet)
            {
                this.OnReceivedResponseError(this.Response, "Invalid HttpResponse after set body.");
                this.Response.Clear();
                this.Disconnect();
                return;
            }
        }

        /// <summary>
        /// Handles the disconnection event and processes any remaining 
        /// HTTP response body data.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.Response.IsPendingBody())
            {
                this.OnReceivedResponse(this.Response);
                this.Response.Clear();
                return;
            }
        }

        /// <summary>
        /// Called when an HTTP response header has been successfully received.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response.</param>
        protected virtual void OnReceivedResponseHeader(HttpResponse response) { }

        /// <summary>
        /// Called when an entire HTTP response has been successfully received.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response.</param>
        protected virtual void OnReceivedResponse(HttpResponse response) { }

        /// <summary>
        /// Called when an error occurs during the processing of an HTTP response.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response that caused the error.</param>
        /// <param name="error">A message describing the error.</param>
        protected virtual void OnReceivedResponseError(HttpResponse response, string error) { }

    }

    /// <summary>
    /// Represents an HTTPS client that handles sending requests and 
    /// receiving responses over an SSL/TLS-encrypted TCP connection.
    /// </summary>
    public class HttpsClient : SslClient, IHttpClient
    {
        /// <summary>
        /// Gets the current HTTP request being processed by the client.
        /// </summary>
        public HttpRequest Request { get; }

        /// <summary>
        /// Gets the current HTTP response received from the server.
        /// </summary>
        protected HttpResponse Response { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpsClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        /// <param name="sslOptions">The SSL/TLS context for secure communication.</param>
        public HttpsClient(string address, int port, TcpClientOptions options, SslOptions sslOptions)
            : base(address, port, options, sslOptions)
        {
            this.Request = new HttpRequest();
            this.Response = new HttpResponse();
        }

        /// <summary>
        /// Sends an HTTP request synchronously using the provided request data.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendRequest(HttpRequest request) => this.Send(request.Cache.Buffer, request.Cache.Position, request.Cache.Length);

        /// <summary>
        /// Sends an HTTP request asynchronously using the provided request data.
        /// </summary>
        /// <param name="request">The HTTP request to be sent.</param>
        /// <returns>True if the request was sent successfully; otherwise, false.</returns>
        public bool SendRequestAsync(HttpRequest request) => this.SendAsync(request.Cache.Buffer, request.Cache.Position, request.Cache.Length);

        /// <summary>
        /// Handles data received from the server. Processes HTTP response headers 
        /// and body data accordingly.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.Response.IsPendingHeader())
            {
                if (this.Response.SetHeaderBuffer(buffer, position, length))
                    this.OnReceivedResponseHeader(this.Response);

                length = 0;
            }

            if (this.Response.IsErrorSet)
            {
                this.OnReceivedResponseError(this.Response, "Invalid HttpResponse after set header.");
                this.Response.Clear();
                this.Disconnect();
                return;
            }

            if (this.Response.SetBodyBuffer(buffer, position, length))
            {
                this.OnReceivedResponse(this.Response);
                this.Response.Clear();
                return;
            }

            if (this.Response.IsErrorSet)
            {
                this.OnReceivedResponseError(this.Response, "Invalid HttpResponse after set body.");
                this.Response.Clear();
                this.Disconnect();
                return;
            }
        }

        /// <summary>
        /// Handles the disconnection event and processes any remaining 
        /// HTTP response body data.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.Response.IsPendingBody())
            {
                this.OnReceivedResponse(this.Response);
                this.Response.Clear();
                return;
            }
        }

        /// <summary>
        /// Called when an HTTP response header has been successfully received.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response.</param>
        protected virtual void OnReceivedResponseHeader(HttpResponse response) { }

        /// <summary>
        /// Called when an entire HTTP response has been successfully received.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response.</param>
        protected virtual void OnReceivedResponse(HttpResponse response) { }

        /// <summary>
        /// Called when an error occurs during the processing of an HTTP response.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="response">The received HTTP response that caused the error.</param>
        /// <param name="error">A message describing the error.</param>
        protected virtual void OnReceivedResponseError(HttpResponse response, string error) { }

    }

}

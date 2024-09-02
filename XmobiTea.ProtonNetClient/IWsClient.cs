using System.Net.Sockets;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetClient
{
    /// <summary>
    /// Defines the interface for a WebSocket client that can send and receive 
    /// various types of data frames over a network.
    /// </summary>
    public interface IWsClient : IClient
    {
        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendText(byte[] buffer);

        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendText(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendTextAsync(byte[] buffer);

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendTextAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendBinary(byte[] buffer);

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendBinary(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendBinaryAsync(byte[] buffer);

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendBinaryAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendClose(int status, byte[] buffer);

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendClose(int status, byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendCloseAsync(int status, byte[] buffer);

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendCloseAsync(int status, byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPing(byte[] buffer);

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPing(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendPingAsync(byte[] buffer);

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendPingAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPong(byte[] buffer);

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPong(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendPongAsync(byte[] buffer);

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        bool SendPongAsync(byte[] buffer, int position, int length);

    }

    /// <summary>
    /// Implements a WebSocket client over an HTTP connection. 
    /// Supports both synchronous and asynchronous operations.
    /// </summary>
    public class WsClient : HttpClient, IWebSocket, IWsClient
    {
        /// <summary>
        /// Gets the WebSocket instance associated with this client.
        /// </summary>
        private WebSocket webSocket { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection is synchronous.
        /// </summary>
        private bool syncConnect { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        public WsClient(string address, int port, TcpClientOptions options) : base(address, port, options) => this.webSocket = new WebSocket(this);

        /// <summary>
        /// Establishes a synchronous connection to the WebSocket server.
        /// </summary>
        /// <returns>True if connected successfully, otherwise false.</returns>
        public override bool Connect()
        {
            this.syncConnect = true;
            return base.Connect();
        }

        /// <summary>
        /// Establishes an asynchronous connection to the WebSocket server.
        /// </summary>
        /// <returns>True if connection initiation is successful, otherwise false.</returns>
        public override bool ConnectAsync()
        {
            this.syncConnect = false;
            return base.ConnectAsync();
        }

        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer) => this.SendText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendTextAsync(byte[] buffer) => this.SendTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendTextAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer) => this.SendBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendBinaryAsync(byte[] buffer) => this.SendBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendBinaryAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer) => this.SendClose(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, true, buffer, position, length, status);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer) => this.SendCloseAsync(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, true, buffer, position, length, status);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer) => this.SendPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPingAsync(byte[] buffer) => this.SendPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPingAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer) => this.SendPong(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPongAsync(byte[] buffer) => this.SendPongAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPongAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when the client has successfully connected to the server.
        /// </summary>
        protected override void OnConnected()
        {
            this.webSocket.ClearWsBuffers();

            this.OnWsConnecting(this.Request);

            if (this.syncConnect)
                this.SendRequest(this.Request);
            else
                this.SendRequestAsync(this.Request);
        }

        /// <summary>
        /// Called when the client is in the process of disconnecting.
        /// </summary>
        protected override void OnDisconnecting()
        {
            if (this.webSocket.handshaked)
                this.OnWsDisconnecting();
        }

        /// <summary>
        /// Called when the client has successfully disconnected from the server.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.webSocket.handshaked)
            {
                this.webSocket.handshaked = false;
                this.OnWsDisconnected();
            }

            this.Request.Clear();
            this.Response.Clear();

            this.webSocket.ClearWsBuffers();

            this.webSocket.InitWsNonce();
        }

        /// <summary>
        /// Processes the data received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.webSocket.handshaked)
            {
                this.webSocket.PrepareReceiveFrame(buffer, position, length);
                return;
            }

            base.OnReceived(buffer, position, length);
        }

        /// <summary>
        /// Processes the response header received from the server.
        /// </summary>
        /// <param name="response">The HTTP response received.</param>
        protected override void OnReceivedResponseHeader(HttpResponse response)
        {
            if (this.webSocket.handshaked)
                return;

            if (!this.webSocket.PerformClientUpgrade(response, this.Id))
            {
                base.OnReceivedResponseHeader(response);
                return;
            }
        }

        /// <summary>
        /// Processes the response body received from the server.
        /// </summary>
        /// <param name="response">The HTTP response received.</param>
        protected override void OnReceivedResponse(HttpResponse response)
        {
            if (this.webSocket.handshaked)
            {
                var data = this.Response.BodyAsBytes;
                this.webSocket.PrepareReceiveFrame(data, 0, data.Length);
                return;
            }

            base.OnReceivedResponse(response);
        }

        /// <summary>
        /// Handles errors in the response received from the server.
        /// </summary>
        /// <param name="response">The HTTP response that caused the error.</param>
        /// <param name="error">The error message.</param>
        protected override void OnReceivedResponseError(HttpResponse response, string error)
        {
            if (this.webSocket.handshaked)
            {
                this.OnError(SocketError.SocketError);
                return;
            }

            base.OnReceivedResponseError(response, error);
        }

        /// <summary>
        /// Prepares the WebSocket connection by setting the necessary headers.
        /// </summary>
        /// <param name="request">The HTTP request being sent to initiate the WebSocket connection.</param>
        public virtual void OnWsConnecting(HttpRequest request)
        {
            request.SetBegin(MethodNames.Get, "/");
            request.SetHeader(HeaderNames.Upgrade, HeaderValues.WebSocket);
            request.SetHeader(HeaderNames.Connection, HeaderValues.Upgrade);
            request.SetHeader(HeaderNames.SecWebSocketKey, System.Convert.ToBase64String(this.webSocket.nonce));
            request.SetHeader(HeaderNames.SecWebSocketVersion, HeaderValues.SecWsVersion);
            request.SetBody();
        }

        /// <summary>
        /// Called when the WebSocket connection is successfully established.
        /// </summary>
        /// <param name="response">The HTTP response that confirms the WebSocket upgrade.</param>
        public virtual void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when the WebSocket connection is successfully established.
        /// </summary>
        /// <param name="request">The HTTP request that initiated the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// OnWsConnecting
        /// </summary>
        /// <param name="request">The HTTP request that initiated the WebSocket connection.</param>
        /// <param name="response">The HTTP response that initiated the WebSocket connection.</param>
        /// <returns></returns>
        public virtual bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when the WebSocket connection is in the process of disconnecting.
        /// </summary>
        public virtual void OnWsDisconnecting() { }

        /// <summary>
        /// Called when the WebSocket connection is successfully disconnected.
        /// </summary>
        public virtual void OnWsDisconnected() { }

        /// <summary>
        /// Processes the WebSocket frames received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public virtual void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Handles a close frame received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close frame.</param>
        /// <param name="status">The close status code.</param>
        public virtual void OnWsClose(byte[] buffer, int position, int length, int status = 1000)
        {
            this.SendClose(status, buffer, position, length);
            this.DisconnectAsync();
        }

        /// <summary>
        /// Handles a ping frame received from the server by sending a pong frame.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping frame.</param>
        public virtual void OnWsPing(byte[] buffer, int position, int length) => this.SendPongAsync(buffer, position, length);

        /// <summary>
        /// Handles a pong frame received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong frame.</param>
        public virtual void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Handles a WebSocket error by reporting it to the client.
        /// </summary>
        /// <param name="error">The error message.</param>
        public virtual void OnWsError(string error) => this.OnError(SocketError.SocketError);

        /// <summary>
        /// Handles a WebSocket error by reporting it to the client.
        /// </summary>
        /// <param name="error">The socket error.</param>
        public virtual void OnWsError(SocketError error) => this.OnError(error);

        /// <summary>
        /// Upgrades the HTTP connection to a WebSocket connection.
        /// </summary>
        /// <param name="response">The HTTP response confirming the upgrade.</param>
        public void SendUpgrade(HttpResponse response) { }
    }

    /// <summary>
    /// Implements a WebSocket Secure (WSS) client over an HTTPS connection. 
    /// Supports both synchronous and asynchronous operations.
    /// </summary>
    public class WssClient : HttpsClient, IWebSocket, IWsClient
    {
        /// <summary>
        /// Gets the WebSocket instance associated with this client.
        /// </summary>
        private WebSocket webSocket { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection is synchronous.
        /// </summary>
        private bool syncConnect { get; set; }

        /// <summary>
        /// Gets the WebSocket nonce used during the handshake.
        /// </summary>
        public byte[] WsNonce => this.webSocket.nonce;

        /// <summary>
        /// Initializes a new instance of the <see cref="WssClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        /// <param name="sslOptions">The Ssl options used for establishing the secure connection.</param>
        public WssClient(string address, int port, TcpClientOptions options, SslOptions sslOptions)
            : base(address, port, options, sslOptions)
        {
            this.webSocket = new WebSocket(this);
        }

        /// <summary>
        /// Establishes a synchronous connection to the WebSocket server.
        /// </summary>
        /// <returns>True if connected successfully, otherwise false.</returns>
        public override bool Connect()
        {
            this.syncConnect = true;
            return this.Connect();
        }

        /// <summary>
        /// Establishes an asynchronous connection to the WebSocket server.
        /// </summary>
        /// <returns>True if connection initiation is successful, otherwise false.</returns>
        public override bool ConnectAsync()
        {
            this.syncConnect = false;
            return this.ConnectAsync();
        }

        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer) => this.SendText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendTextAsync(byte[] buffer) => this.SendTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendTextAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer) => this.SendBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendBinaryAsync(byte[] buffer) => this.SendBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendBinaryAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer) => this.SendClose(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame with a status code synchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, true, buffer, position, length, status);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer) => this.SendCloseAsync(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame with a status code asynchronously to the server.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The byte array containing the close message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, true, buffer, position, length, status);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer) => this.SendPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPingAsync(byte[] buffer) => this.SendPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPingAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer) => this.SendPong(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame synchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, true, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPongAsync(byte[] buffer) => this.SendPongAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The byte array containing the pong message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
        public bool SendPongAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, true, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when the client is in the process of connecting to the server.
        /// </summary>
        protected override void OnConnecting()
        {

        }

        /// <summary>
        /// Called when the client has successfully connected to the server.
        /// </summary>
        protected override void OnConnected()
        {

        }

        /// <summary>
        /// Called when the SSL/TLS handshake is in progress.
        /// </summary>
        protected override void OnHandshaking()
        {

        }

        /// <summary>
        /// Called when the SSL/TLS handshake is successfully completed.
        /// </summary>
        protected override void OnHandshaked()
        {
            this.webSocket.ClearWsBuffers();

            this.OnWsConnecting(this.Request);

            if (this.syncConnect)
                this.SendRequest(this.Request);
            else
                this.SendRequestAsync(this.Request);
        }

        /// <summary>
        /// Called when the client is in the process of disconnecting.
        /// </summary>
        protected override void OnDisconnecting()
        {
            if (this.webSocket.handshaked)
                this.OnWsDisconnecting();
        }

        /// <summary>
        /// Called when the client has successfully disconnected from the server.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.webSocket.handshaked)
            {
                this.webSocket.handshaked = false;
                this.OnWsDisconnected();
            }

            this.Request.Clear();
            this.Response.Clear();

            this.webSocket.ClearWsBuffers();

            this.webSocket.InitWsNonce();
        }

        /// <summary>
        /// Processes the data received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.webSocket.handshaked)
            {
                this.webSocket.PrepareReceiveFrame(buffer, position, length);
                return;
            }

            base.OnReceived(buffer, position, length);
        }

        /// <summary>
        /// Processes the response header received from the server.
        /// </summary>
        /// <param name="response">The HTTP response received.</param>
        protected override void OnReceivedResponseHeader(HttpResponse response)
        {
            if (this.webSocket.handshaked)
                return;

            if (!this.webSocket.PerformClientUpgrade(response, this.Id))
            {
                base.OnReceivedResponseHeader(response);
                return;
            }
        }

        /// <summary>
        /// Processes the response body received from the server.
        /// </summary>
        /// <param name="response">The HTTP response received.</param>
        protected override void OnReceivedResponse(HttpResponse response)
        {
            if (this.webSocket.handshaked)
            {
                var data = this.Response.BodyAsBytes;
                this.webSocket.PrepareReceiveFrame(data, 0, data.Length);
                return;
            }

            base.OnReceivedResponse(response);
        }

        /// <summary>
        /// Handles errors in the response received from the server.
        /// </summary>
        /// <param name="response">The HTTP response that caused the error.</param>
        /// <param name="error">The error message.</param>
        protected override void OnReceivedResponseError(HttpResponse response, string error)
        {
            if (this.webSocket.handshaked)
            {
                this.OnError(SocketError.SocketError);
                return;
            }

            base.OnReceivedResponseError(response, error);
        }

        /// <summary>
        /// Prepares the WebSocket connection by setting the necessary headers.
        /// </summary>
        /// <param name="request">The HTTP request being sent to initiate the WebSocket connection.</param>
        public virtual void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Called when the WebSocket connection is successfully established.
        /// </summary>
        /// <param name="response">The HTTP response that confirms the WebSocket upgrade.</param>
        public virtual void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when the WebSocket connection is successfully established.
        /// </summary>
        /// <param name="request">The HTTP request that initiated the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// OnWsConnecting
        /// </summary>
        /// <param name="request">The HTTP request that initiated the WebSocket connection.</param>
        /// <param name="response">The HTTP response that initiated the WebSocket connection.</param>
        /// <returns></returns>
        public virtual bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when the WebSocket connection is in the process of disconnecting.
        /// </summary>
        public virtual void OnWsDisconnecting() { }

        /// <summary>
        /// Called when the WebSocket connection is successfully disconnected.
        /// </summary>
        public virtual void OnWsDisconnected() { }

        /// <summary>
        /// Processes the WebSocket frames received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public virtual void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Handles a close frame received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close frame.</param>
        /// <param name="status">The close status code.</param>
        public virtual void OnWsClose(byte[] buffer, int position, int length, int status = 1000)
        {
            this.SendClose(status, buffer, position, length);
            this.DisconnectAsync();
        }

        /// <summary>
        /// Handles a ping frame received from the server by sending a pong frame.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping frame.</param>
        public virtual void OnWsPing(byte[] buffer, int position, int length) => this.SendPongAsync(buffer, position, length);

        /// <summary>
        /// Handles a pong frame received from the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong frame.</param>
        public virtual void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Handles a WebSocket error by reporting it to the client.
        /// </summary>
        /// <param name="error">The error message.</param>
        public virtual void OnWsError(string error) => this.OnError(SocketError.SocketError);

        /// <summary>
        /// Handles a WebSocket error by reporting it to the client.
        /// </summary>
        /// <param name="error">The socket error.</param>
        public virtual void OnWsError(SocketError error) => this.OnError(error);

        /// <summary>
        /// Upgrades the HTTP connection to a WebSocket connection.
        /// </summary>
        /// <param name="response">The HTTP response confirming the upgrade.</param>
        public void SendUpgrade(HttpResponse response) { }

    }

}

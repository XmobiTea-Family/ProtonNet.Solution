using System.Net.Sockets;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a WebSocket session, providing methods for sending various WebSocket frames.
    /// </summary>
    public interface IWsSession : ISession
    {
        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendText(byte[] buffer);

        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendText(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool SendTextAsync(byte[] buffer);

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool SendTextAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendBinary(byte[] buffer);

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendBinary(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool SendBinaryAsync(byte[] buffer);

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool SendBinaryAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendClose(int status, byte[] buffer);

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendClose(int status, byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        bool SendCloseAsync(int status, byte[] buffer);

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        bool SendCloseAsync(int status, byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPing(byte[] buffer);

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPing(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        bool SendPingAsync(byte[] buffer);

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        bool SendPingAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPong(byte[] buffer);

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendPong(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        bool SendPongAsync(byte[] buffer);

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        bool SendPongAsync(byte[] buffer, int position, int length);

    }

    /// <summary>
    /// Represents a WebSocket session that inherits from HttpSession and implements WebSocket functionality.
    /// </summary>
    public class WsSession : HttpSession, IWebSocket, IWsSession
    {
        internal WebSocket WebSocket { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsSession"/> class with the specified WebSocket server.
        /// </summary>
        /// <param name="server">The WebSocket server.</param>
        public WsSession(WsServer server) : base(server) => this.WebSocket = new WebSocket(this);

        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer) => this.SendText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.Send(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendTextAsync(byte[] buffer) => this.SendTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendTextAsync(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.SendAsync(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer) => this.SendBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.Send(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendBinaryAsync(byte[] buffer) => this.SendBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendBinaryAsync(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.SendAsync(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer) => this.SendClose(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, position, length, status);
                return this.Send(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer) => this.SendCloseAsync(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, position, length, status);
                return this.SendAsync(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer) => this.SendPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.Send(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        public bool SendPingAsync(byte[] buffer) => this.SendPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        public bool SendPingAsync(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.SendAsync(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer) => this.SendPong(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, false, buffer, position, length);
                return this.Send(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        public bool SendPongAsync(byte[] buffer) => this.SendPongAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        public bool SendPongAsync(byte[] buffer, int position, int length)
        {
            lock (this.WebSocket.sendLock)
            {
                this.WebSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, false, buffer, position, length);
                return this.SendAsync(this.WebSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when the session is disconnecting.
        /// </summary>
        protected override void OnDisconnecting()
        {
            if (this.WebSocket.handshaked)
                this.OnWsDisconnecting();
        }

        /// <summary>
        /// Called when the session has been disconnected.
        /// </summary>
        protected override void OnDisconnected()
        {
            if (this.WebSocket.handshaked)
            {
                this.WebSocket.handshaked = false;
                this.OnWsDisconnected();
            }

            this.Request.Clear();
            this.Response.Clear();

            this.WebSocket.ClearWsBuffers();

            this.WebSocket.InitWsNonce();
        }

        /// <summary>
        /// Called when data is received from the connection.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            if (this.WebSocket.handshaked)
            {
                this.WebSocket.PrepareReceiveFrame(buffer, position, length);
                return;
            }

            base.OnReceived(buffer, position, length);
        }

        /// <summary>
        /// Called when the HTTP request headers are received.
        /// </summary>
        /// <param name="request">The HTTP request containing the headers.</param>
        protected override void OnReceivedRequestHeader(HttpRequest request)
        {
            if (this.WebSocket.handshaked)
                return;

            if (!this.WebSocket.PerformServerUpgrade(request, this.Response))
            {
                base.OnReceivedRequestHeader(request);
                return;
            }
        }

        /// <summary>
        /// Called when the complete HTTP request is received.
        /// </summary>
        /// <param name="request">The received HTTP request.</param>
        protected override void OnReceivedRequest(HttpRequest request)
        {
            if (this.WebSocket.handshaked)
            {
                var data = this.Request.BodyAsBytes;
                this.WebSocket.PrepareReceiveFrame(data, 0, data.Length);
                return;
            }

            base.OnReceivedRequest(request);
        }

        /// <summary>
        /// Called when an error occurs while receiving the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request that caused the error.</param>
        /// <param name="error">The error message.</param>
        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            if (this.WebSocket.handshaked)
            {
                this.OnError(SocketError.SocketError);
                return;
            }

            base.OnReceivedRequestError(request, error);
        }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public virtual void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// </summary>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        /// <returns>True if the connection is allowed; otherwise, false.</returns>
        public virtual bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is being disconnected.
        /// </summary>
        public virtual void OnWsDisconnecting() { }

        /// <summary>
        /// Called when a WebSocket connection is successfully disconnected.
        /// </summary>
        public virtual void OnWsDisconnected() { }

        /// <summary>
        /// Called when data is received from the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public virtual void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket connection is closed.
        /// </summary>
        /// <param name="buffer">The close data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close data.</param>
        /// <param name="status">The close status code.</param>
        public virtual void OnWsClose(byte[] buffer, int position, int length, int status = 1000)
        {
            this.SendCloseAsync(status, buffer, position, length);
            this.Disconnect();
        }

        /// <summary>
        /// Called when a WebSocket ping is received.
        /// </summary>
        /// <param name="buffer">The ping data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping data.</param>
        public virtual void OnWsPing(byte[] buffer, int position, int length) => this.SendPongAsync(buffer, position, length);

        /// <summary>
        /// Called when a WebSocket pong is received.
        /// </summary>
        /// <param name="buffer">The pong data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong data.</param>
        public virtual void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket error occurs with a custom error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        public virtual void OnWsError(string error) => this.OnError(SocketError.SocketError);

        /// <summary>
        /// Called when a WebSocket error occurs with a <see cref="SocketError"/> instance.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        public virtual void OnWsError(SocketError error) => this.OnError(error);

        /// <summary>
        /// Sends a WebSocket upgrade response to the client.
        /// </summary>
        /// <param name="response">The HTTP response containing the WebSocket upgrade information.</param>
        public void SendUpgrade(HttpResponse response) => this.SendResponseAsync(response);

    }

    /// <summary>
    /// Represents a secure WebSocket session that inherits from HttpsSession and implements WebSocket functionality.
    /// </summary>
    public class WssSession : HttpsSession, IWebSocket, IWsSession
    {
        internal WebSocket webSocket { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WssSession"/> class with the specified WebSocket server.
        /// </summary>
        /// <param name="server">The WebSocket server.</param>
        public WssSession(WssServer server) : base(server) => this.webSocket = new WebSocket(this);

        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer) => this.SendText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendText(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendTextAsync(byte[] buffer) => this.SendTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendTextAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer) => this.SendBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendBinary(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendBinaryAsync(byte[] buffer) => this.SendBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        public bool SendBinaryAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer) => this.SendClose(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame synchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendClose(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, position, length, status);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code and buffer.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer) => this.SendCloseAsync(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a close frame asynchronously to the WebSocket connection with the specified status code, buffer, position, and length.
        /// </summary>
        /// <param name="status">The close status code.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the close frame was sent successfully; otherwise, false.</returns>
        public bool SendCloseAsync(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, position, length, status);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer) => this.SendPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPing(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        public bool SendPingAsync(byte[] buffer) => this.SendPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the ping frame was sent successfully; otherwise, false.</returns>
        public bool SendPingAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer) => this.SendPong(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame synchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendPong(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, false, buffer, position, length);
                return this.Send(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        public bool SendPongAsync(byte[] buffer) => this.SendPongAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a pong frame asynchronously to the WebSocket connection with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the pong frame was sent successfully; otherwise, false.</returns>
        public bool SendPongAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PONG, false, buffer, position, length);
                return this.SendAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when the session is disconnecting.
        /// </summary>
        protected override void OnDisconnecting()
        {
            if (this.webSocket.handshaked)
                this.OnWsDisconnecting();
        }

        /// <summary>
        /// Called when the session has been disconnected.
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
        /// Called when data is received from the connection.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
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
        /// Called when the HTTP request headers are received.
        /// </summary>
        /// <param name="request">The HTTP request containing the headers.</param>
        protected override void OnReceivedRequestHeader(HttpRequest request)
        {
            if (this.webSocket.handshaked)
                return;

            if (!this.webSocket.PerformServerUpgrade(request, this.Response))
            {
                base.OnReceivedRequestHeader(request);
                return;
            }
        }

        /// <summary>
        /// Called when the complete HTTP request is received.
        /// </summary>
        /// <param name="request">The received HTTP request.</param>
        protected override void OnReceivedRequest(HttpRequest request)
        {
            if (this.webSocket.handshaked)
            {
                var data = this.Request.BodyAsBytes;
                this.webSocket.PrepareReceiveFrame(data, 0, data.Length);
                return;
            }

            base.OnReceivedRequest(request);
        }

        /// <summary>
        /// Called when an error occurs while receiving the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request that caused the error.</param>
        /// <param name="error">The error message.</param>
        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            if (this.webSocket.handshaked)
            {
                this.OnError(SocketError.SocketError);
                return;
            }

            base.OnReceivedRequestError(request, error);
        }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public virtual void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// </summary>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        /// <returns>True if the connection is allowed; otherwise, false.</returns>
        public virtual bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public virtual void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is being disconnected.
        /// </summary>
        public virtual void OnWsDisconnecting() { }

        /// <summary>
        /// Called when a WebSocket connection is successfully disconnected.
        /// </summary>
        public virtual void OnWsDisconnected() { }

        /// <summary>
        /// Called when data is received from the WebSocket connection.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public virtual void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket connection is closed.
        /// </summary>
        /// <param name="buffer">The close data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close data.</param>
        /// <param name="status">The close status code.</param>
        public virtual void OnWsClose(byte[] buffer, int position, int length, int status = 1000)
        {
            this.SendCloseAsync(status, buffer, position, length);
            this.Disconnect();
        }

        /// <summary>
        /// Called when a WebSocket ping is received.
        /// </summary>
        /// <param name="buffer">The ping data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping data.</param>
        public virtual void OnWsPing(byte[] buffer, int position, int length) => this.SendPongAsync(buffer, position, length);

        /// <summary>
        /// Called when a WebSocket pong is received.
        /// </summary>
        /// <param name="buffer">The pong data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong data.</param>
        public virtual void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket error occurs with a custom error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        public virtual void OnWsError(string error) => this.OnError(SocketError.SocketError);

        /// <summary>
        /// Called when a WebSocket error occurs with a <see cref="SocketError"/> instance.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        public virtual void OnWsError(SocketError error) => this.OnError(error);

        /// <summary>
        /// Sends a WebSocket upgrade response to the client.
        /// </summary>
        /// <param name="response">The HTTP response containing the WebSocket upgrade information.</param>
        public void SendUpgrade(HttpResponse response) => this.SendResponseAsync(response);

    }

}

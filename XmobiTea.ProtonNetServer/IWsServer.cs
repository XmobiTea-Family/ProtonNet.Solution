using System.Net.Sockets;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Types;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a WebSocket server, providing methods for broadcasting and closing WebSocket connections.
    /// </summary>
    public interface IWsServer : IServer
    {
        /// <summary>
        /// Closes all WebSocket connections.
        /// </summary>
        /// <returns>True if the connections were closed successfully; otherwise, false.</returns>
        bool CloseAll();

        /// <summary>
        /// Closes all WebSocket connections with the specified status code.
        /// </summary>
        /// <param name="status">The status code to send with the close frame.</param>
        /// <returns>True if the connections were closed successfully; otherwise, false.</returns>
        bool CloseAll(int status);

        /// <summary>
        /// Closes all WebSocket connections with the specified status code and a buffer.
        /// </summary>
        /// <param name="status">The status code to send with the close frame.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <returns>True if the connections were closed successfully; otherwise, false.</returns>
        bool CloseAll(int status, byte[] buffer);

        /// <summary>
        /// Closes all WebSocket connections with the specified status code and a buffer with a specified position and length.
        /// </summary>
        /// <param name="status">The status code to send with the close frame.</param>
        /// <param name="buffer">The buffer containing additional data to send with the close frame.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the connections were closed successfully; otherwise, false.</returns>
        bool CloseAll(int status, byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a text message to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastText(byte[] buffer);

        /// <summary>
        /// Broadcasts a text message to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastText(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a text message asynchronously to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastTextAsync(byte[] buffer);

        /// <summary>
        /// Broadcasts a text message asynchronously to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastTextAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a binary message to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastBinary(byte[] buffer);

        /// <summary>
        /// Broadcasts a binary message to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastBinary(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a binary message asynchronously to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastBinaryAsync(byte[] buffer);

        /// <summary>
        /// Broadcasts a binary message asynchronously to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the message was sent successfully; otherwise, false.</returns>
        bool MulticastBinaryAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a ping message to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message.</param>
        /// <returns>True if the ping was sent successfully; otherwise, false.</returns>
        bool MulticastPing(byte[] buffer);

        /// <summary>
        /// Broadcasts a ping message to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the ping was sent successfully; otherwise, false.</returns>
        bool MulticastPing(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a ping message asynchronously to all WebSocket connections.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message.</param>
        /// <returns>True if the ping was sent successfully; otherwise, false.</returns>
        bool MulticastPingAsync(byte[] buffer);

        /// <summary>
        /// Broadcasts a ping message asynchronously to all WebSocket connections with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the ping was sent successfully; otherwise, false.</returns>
        bool MulticastPingAsync(byte[] buffer, int position, int length);

    }

    /// <summary>
    /// Represents a WebSocket server that inherits from HttpServer and implements WebSocket functionality.
    /// </summary>
    public class WsServer : HttpServer, IWebSocket, IWsServer
    {
        /// <summary>
        /// A static empty bytes
        /// </summary>
        private static byte[] EmptyBytes { get; }

        static WsServer()
        {
            EmptyBytes = new byte[0];
        }

        /// <summary>
        /// The web socket handle
        /// </summary>
        private WebSocket webSocket { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsServer"/> class with the specified address, port, and options.
        /// </summary>
        /// <param name="address">The server's IP address.</param>
        /// <param name="port">The server's port number.</param>
        /// <param name="options">The TCP server options.</param>
        public WsServer(string address, int port, TcpServerOptions options) : base(address, port, options) => this.webSocket = new WebSocket(this);

        /// <summary>
        /// Creates a new WebSocket session.
        /// </summary>
        /// <returns>A new instance of <see cref="WsSession"/>.</returns>
        protected override TcpSession CreateSession() => new WsSession(this);

        /// <summary>
        /// Closes all WebSocket sessions with a default status code of 0.
        /// </summary>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll() => this.CloseAll(0);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status) => this.CloseAll(status, EmptyBytes);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code and an additional buffer.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <param name="buffer">The buffer containing additional data to send before closing the sessions.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status, byte[] buffer) => this.CloseAll(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code, buffer, and offset/size parameters.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <param name="buffer">The buffer containing additional data to send before closing the sessions.</param>
        /// <param name="offset">The offset in the buffer where the data begins.</param>
        /// <param name="size">The size of the data to send.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status, byte[] buffer, int offset, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, offset, size, status);
                if (!this.BroadcastAll(this.webSocket.sendBuffer.ToArray()))
                    return false;

                return this.DisconnectAll();
            }
        }

        /// <summary>
        /// Broadcasts a message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAll(byte[] buffer) => this.BroadcastAll(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAll(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
            {
                if (session is WsSession wsSession)
                {
                    if (wsSession.WebSocket.handshaked)
                        wsSession.Send(buffer, position, length);
                }
            }

            return true;
        }

        /// <summary>
        /// Asynchronously broadcasts a message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAllAsync(byte[] buffer) => this.BroadcastAllAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously broadcasts a message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAllAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
            {
                if (session is WsSession wsSession)
                {
                    if (wsSession.WebSocket.handshaked)
                        wsSession.SendAsync(buffer, position, length);
                }
            }

            return true;
        }

        /// <summary>
        /// Sends a text message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastText(byte[] buffer) => this.MulticastText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastText(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a text message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastTextAsync(byte[] buffer) => this.MulticastTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a text message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastTextAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, length);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinary(byte[] buffer) => this.MulticastBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinary(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a binary message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinaryAsync(byte[] buffer) => this.MulticastBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a binary message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinaryAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, length);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPing(byte[] buffer) => this.MulticastPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPing(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a ping message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPingAsync(byte[] buffer) => this.MulticastPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a ping message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPingAsync(byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, length);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// Override this method to add custom logic during WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// Override this method to add custom logic after WebSocket connection.
        /// </summary>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        public void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// Override this method to add custom logic during WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        /// <returns>True if the connection is allowed; otherwise, false.</returns>
        public bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// Override this method to add custom logic after WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is being disconnected.
        /// Override this method to add custom logic during WebSocket disconnection.
        /// </summary>
        public void OnWsDisconnecting() { }

        /// <summary>
        /// Called when a WebSocket connection is successfully disconnected.
        /// Override this method to add custom logic after WebSocket disconnection.
        /// </summary>
        public void OnWsDisconnected() { }

        /// <summary>
        /// Called when data is received from the WebSocket connection.
        /// Override this method to process received data.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket connection is closed.
        /// Override this method to handle WebSocket close events.
        /// </summary>
        /// <param name="buffer">The close data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close data.</param>
        /// <param name="status">The close status code.</param>
        public void OnWsClose(byte[] buffer, int position, int length, int status = 1000) { }

        /// <summary>
        /// Called when a WebSocket ping is received.
        /// Override this method to handle WebSocket ping events.
        /// </summary>
        /// <param name="buffer">The ping data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping data.</param>
        public void OnWsPing(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket pong is received.
        /// Override this method to handle WebSocket pong events.
        /// </summary>
        /// <param name="buffer">The pong data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong data.</param>
        public void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket error occurs.
        /// Override this method to handle WebSocket errors with a custom message.
        /// </summary>
        /// <param name="error">The error message.</param>
        public void OnWsError(string error) { }

        /// <summary>
        /// Called when a WebSocket error occurs.
        /// Override this method to handle WebSocket errors with a <see cref="SocketError"/> instance.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        public void OnWsError(SocketError error) { }

        /// <summary>
        /// Sends a WebSocket upgrade response to the client.
        /// </summary>
        /// <param name="response">The HTTP response containing the WebSocket upgrade information.</param>
        public void SendUpgrade(HttpResponse response) { }
    }

    /// <summary>
    /// Represents a secure WebSocket server that inherits from HttpsServer and implements WebSocket functionality.
    /// </summary>
    public class WssServer : HttpsServer, IWebSocket, IWsServer
    {
        /// <summary>
        /// A static empty bytes
        /// </summary>
        private static byte[] EmptyBytes { get; }

        static WssServer()
        {
            EmptyBytes = new byte[0];
        }

        /// <summary>
        /// The web socket handle
        /// </summary>
        private WebSocket webSocket { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WssServer"/> class with the specified address, port, options, and Ssl options.
        /// </summary>
        /// <param name="address">The server's IP address.</param>
        /// <param name="port">The server's port number.</param>
        /// <param name="options">The TCP server options.</param>
        /// <param name="sslOptions">The Ssl options for secure communication.</param>
        public WssServer(string address, int port, TcpServerOptions options, SslOptions sslOptions) : base(address, port, options, sslOptions) => this.webSocket = new WebSocket(this);

        /// <summary>
        /// Creates a new secure WebSocket session.
        /// </summary>
        /// <returns>A new instance of <see cref="WssSession"/>.</returns>
        protected override SslSession CreateSession() => new WssSession(this);

        /// <summary>
        /// Closes all WebSocket sessions with a default status code of 0.
        /// </summary>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll() => this.CloseAll(0);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status) => this.CloseAll(status, EmptyBytes);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code and an additional buffer.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <param name="buffer">The buffer containing additional data to send before closing the sessions.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status, byte[] buffer) => this.CloseAll(status, buffer, 0, buffer.Length);

        /// <summary>
        /// Closes all WebSocket sessions with the specified status code, buffer, and position/length parameters.
        /// </summary>
        /// <param name="status">The status code to use when closing the sessions.</param>
        /// <param name="buffer">The buffer containing additional data to send before closing the sessions.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if all sessions were closed successfully; otherwise, false.</returns>
        public virtual bool CloseAll(int status, byte[] buffer, int position, int length)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.CLOSE, false, buffer, position, length, status);
                if (!this.BroadcastAll(this.webSocket.sendBuffer.ToArray()))
                    return false;

                return this.DisconnectAll();
            }
        }

        /// <summary>
        /// Broadcasts a message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAll(byte[] buffer) => this.BroadcastAll(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAll(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
            {
                if (session is WssSession wsSession)
                {
                    if (wsSession.webSocket.handshaked)
                        wsSession.Send(buffer, position, length);
                }
            }

            return true;
        }

        /// <summary>
        /// Asynchronously broadcasts a message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAllAsync(byte[] buffer) => this.BroadcastAllAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously broadcasts a message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the message to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcast successfully; otherwise, false.</returns>
        public override bool BroadcastAllAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
            {
                if (session is WssSession wsSession)
                {
                    if (wsSession.webSocket.handshaked)
                        wsSession.SendAsync(buffer, position, length);
                }
            }

            return true;
        }

        /// <summary>
        /// Sends a text message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastText(byte[] buffer) => this.MulticastText(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a text message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastText(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, size);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a text message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastTextAsync(byte[] buffer) => this.MulticastTextAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a text message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the text message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastTextAsync(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.TEXT, false, buffer, position, size);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a binary message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinary(byte[] buffer) => this.MulticastBinary(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a binary message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinary(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, size);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a binary message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinaryAsync(byte[] buffer) => this.MulticastBinaryAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a binary message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the binary message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastBinaryAsync(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.BINARY, false, buffer, position, size);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Sends a ping message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPing(byte[] buffer) => this.MulticastPing(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a ping message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPing(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, size);
                return this.BroadcastAll(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Asynchronously sends a ping message to all connected WebSocket sessions.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPingAsync(byte[] buffer) => this.MulticastPingAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends a ping message to all connected WebSocket sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping message to multicast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="size">The length of the data to multicast.</param>
        /// <returns>True if the message was multicast successfully; otherwise, false.</returns>
        public bool MulticastPingAsync(byte[] buffer, int position, int size)
        {
            lock (this.webSocket.sendLock)
            {
                this.webSocket.PrepareSendFrame(WebSocketOpCodes.FIN | WebSocketOpCodes.PING, false, buffer, position, size);
                return this.BroadcastAllAsync(this.webSocket.sendBuffer.ToArray());
            }
        }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// Override this method to add custom logic during WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// Override this method to add custom logic after WebSocket connection.
        /// </summary>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        public void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Called when a WebSocket connection is being established.
        /// Override this method to add custom logic during WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        /// <param name="response">The HTTP response for the WebSocket connection.</param>
        /// <returns>True if the connection is allowed; otherwise, false.</returns>
        public bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Called when a WebSocket connection is successfully established.
        /// Override this method to add custom logic after WebSocket connection.
        /// </summary>
        /// <param name="request">The HTTP request for the WebSocket connection.</param>
        public void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// Called when a WebSocket connection is being disconnected.
        /// Override this method to add custom logic during WebSocket disconnection.
        /// </summary>
        public void OnWsDisconnecting() { }

        /// <summary>
        /// Called when a WebSocket connection is successfully disconnected.
        /// Override this method to add custom logic after WebSocket disconnection.
        /// </summary>
        public void OnWsDisconnected() { }

        /// <summary>
        /// Called when data is received from the WebSocket connection.
        /// Override this method to process received data.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        public void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket connection is closed.
        /// Override this method to handle WebSocket close events.
        /// </summary>
        /// <param name="buffer">The close data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the close data.</param>
        /// <param name="status">The close status code.</param>
        public void OnWsClose(byte[] buffer, int position, int length, int status = 1000) { }

        /// <summary>
        /// Called when a WebSocket ping is received.
        /// Override this method to handle WebSocket ping events.
        /// </summary>
        /// <param name="buffer">The ping data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the ping data.</param>
        public void OnWsPing(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket pong is received.
        /// Override this method to handle WebSocket pong events.
        /// </summary>
        /// <param name="buffer">The pong data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the pong data.</param>
        public void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when a WebSocket error occurs.
        /// Override this method to handle WebSocket errors with a custom message.
        /// </summary>
        /// <param name="error">The error message.</param>
        public void OnWsError(string error) { }

        /// <summary>
        /// Called when a WebSocket error occurs.
        /// Override this method to handle WebSocket errors with a <see cref="SocketError"/> instance.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        public void OnWsError(SocketError error) { }

        /// <summary>
        /// Sends a WebSocket upgrade response to the client.
        /// </summary>
        /// <param name="response">The HTTP response containing the WebSocket upgrade information.</param>
        public void SendUpgrade(HttpResponse response) { }

    }


}

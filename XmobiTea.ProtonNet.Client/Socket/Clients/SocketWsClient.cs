using XmobiTea.ProtonNetClient.Options;

namespace XmobiTea.ProtonNet.Client.Socket.Clients
{
    /// <summary>
    /// Represents a WebSocket client, implementing the <see cref="ISocketClient"/> and <see cref="ISetEncryptKey"/> interfaces.
    /// </summary>
    class SocketWsClient : ProtonNetClient.WsClient, ISocketClient, ISetEncryptKey
    {
        /// <summary>
        /// Event handler for when the client connects to the server.
        /// </summary>
        internal OnSocketClientConnected onConnected;

        /// <summary>
        /// Event handler for when the client disconnects from the server.
        /// </summary>
        internal OnSocketClientDisconnected onDisconnected;

        /// <summary>
        /// Event handler for when the client receives data from the server.
        /// </summary>
        internal OnSocketClientReceived onReceived;

        /// <summary>
        /// Event handler for when a socket error occurs.
        /// </summary>
        internal OnSocketClientError onError;

        /// <summary>
        /// The encryption key used for encrypting and decrypting data.
        /// </summary>
        private byte[] encryptKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketWsClient"/> class with the specified server address, port, and client options.
        /// </summary>
        /// <param name="address">The server address.</param>
        /// <param name="port">The server port.</param>
        /// <param name="options">The TCP client options.</param>
        public SocketWsClient(string address, int port, TcpClientOptions options)
            : base(address, port, options)
        {

        }

        /// <summary>
        /// Sets the encryption key used by the client.
        /// </summary>
        /// <param name="encryptKey">The encryption key as a byte array.</param>
        public void SetEncryptKey(byte[] encryptKey) => this.encryptKey = encryptKey;

        /// <summary>
        /// Gets the encryption key used by the client.
        /// </summary>
        /// <returns>The encryption key as a byte array.</returns>
        public byte[] GetEncryptKey() => this.encryptKey;

        /// <summary>
        /// Initiates a connection to the server asynchronously.
        /// </summary>
        /// <returns>True if the connection was successfully initiated, otherwise false.</returns>
        public new bool Connect() => base.ConnectAsync();

        /// <summary>
        /// Disconnects from the server asynchronously.
        /// </summary>
        /// <returns>True if the disconnection was successfully initiated, otherwise false.</returns>
        public new bool Disconnect() => base.DisconnectAsync();

        /// <summary>
        /// Checks if the client is currently connected to the server.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        public new bool IsConnected() => base.IsConnected;

        /// <summary>
        /// Reconnects to the server asynchronously.
        /// </summary>
        /// <returns>True if the reconnection was successfully initiated, otherwise false.</returns>
        public new bool Reconnect() => base.ReconnectAsync();

        /// <summary>
        /// Called when the WebSocket client successfully connects to the server.
        /// Invokes the <see cref="onConnected"/> event handler.
        /// </summary>
        /// <param name="response">The HTTP response received upon WebSocket connection.</param>
        public override void OnWsConnected(ProtonNetCommon.HttpResponse response)
        {
            base.OnWsConnected(response);
            this.onConnected?.Invoke();
        }

        /// <summary>
        /// Called when the client disconnects from the server.
        /// Invokes the <see cref="onDisconnected"/> event handler.
        /// </summary>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            this.onDisconnected?.Invoke();
        }

        /// <summary>
        /// Called when the WebSocket client receives data from the server.
        /// Invokes the <see cref="onReceived"/> event handler.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data received.</param>
        public override void OnWsReceived(byte[] buffer, int position, int length)
        {
            base.OnWsReceived(buffer, position, length);
            this.onReceived?.Invoke(buffer, position, length);
        }

        /// <summary>
        /// Called when a socket error occurs.
        /// Invokes the <see cref="onError"/> event handler.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            base.OnError(error);
            this.onError?.Invoke(error);
        }

        /// <summary>
        /// Sends data to the server as a binary message.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public new int Send(byte[] buffer) => this.SendBinary(buffer);

        /// <summary>
        /// Sends data asynchronously to the server as a binary message.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>True if the data was successfully queued for sending, otherwise false.</returns>
        public new bool SendAsync(byte[] buffer) => this.SendBinaryAsync(buffer);

    }

}

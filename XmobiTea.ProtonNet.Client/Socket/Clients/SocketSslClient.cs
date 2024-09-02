using XmobiTea.ProtonNetClient.Options;

namespace XmobiTea.ProtonNet.Client.Socket.Clients
{
    /// <summary>
    /// Represents a secure socket client that uses SSL/TLS for communication, implementing the <see cref="ISocketClient"/> and <see cref="ISetEncryptKey"/> interfaces.
    /// </summary>
    class SocketSslClient : ProtonNetClient.SslClient, ISocketClient, ISetEncryptKey
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
        /// Initializes a new instance of the <see cref="SocketSslClient"/> class with the specified server address, port, client options, and Ssl options.
        /// </summary>
        /// <param name="address">The server address.</param>
        /// <param name="port">The server port.</param>
        /// <param name="options">The TCP client options.</param>
        /// <param name="sslOptions">The Ssl options for secure communication.</param>
        public SocketSslClient(string address, int port, TcpClientOptions options, ProtonNetCommon.SslOptions sslOptions)
            : base(address, port, options, sslOptions)
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
        /// Called when the client successfully connects to the server.
        /// Invokes the <see cref="onConnected"/> event handler.
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();
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
        /// Called when the client receives data from the server.
        /// Invokes the <see cref="onReceived"/> event handler.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data received.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            base.OnReceived(buffer, position, length);
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

    }

}

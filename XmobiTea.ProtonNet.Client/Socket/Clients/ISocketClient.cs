using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Client.Socket.Clients
{
    /// <summary>
    /// Delegate for handling socket client connection events.
    /// </summary>
    delegate void OnSocketClientConnected();

    /// <summary>
    /// Delegate for handling socket client disconnection events.
    /// </summary>
    delegate void OnSocketClientDisconnected();

    /// <summary>
    /// Delegate for handling data received by the socket client.
    /// </summary>
    /// <param name="buffer">The buffer containing the received data.</param>
    /// <param name="position">The position in the buffer where the data starts.</param>
    /// <param name="length">The length of the data received.</param>
    delegate void OnSocketClientReceived(byte[] buffer, int position, int length);

    /// <summary>
    /// Delegate for handling socket client errors.
    /// </summary>
    /// <param name="error">The socket error that occurred.</param>
    delegate void OnSocketClientError(System.Net.Sockets.SocketError error);

    /// <summary>
    /// Interface for setting the encryption key used by the socket client.
    /// </summary>
    public interface ISetEncryptKey
    {
        /// <summary>
        /// Sets the encryption key used for encrypting and decrypting data.
        /// </summary>
        /// <param name="encryptKey">The encryption key as a byte array.</param>
        void SetEncryptKey(byte[] encryptKey);

    }

    /// <summary>
    /// Interface representing a socket client, responsible for managing socket connections and data transmission.
    /// </summary>
    public interface ISocketClient
    {
        /// <summary>
        /// Gets the encryption key used by the socket client.
        /// </summary>
        /// <returns>The encryption key as a byte array.</returns>
        byte[] GetEncryptKey();

        /// <summary>
        /// Initiates a connection to the server.
        /// </summary>
        /// <returns>True if the connection is successful, otherwise false.</returns>
        bool Connect();

        /// <summary>
        /// Reconnects to the server.
        /// </summary>
        /// <returns>True if the reconnection is successful, otherwise false.</returns>
        bool Reconnect();

        /// <summary>
        /// Sends data synchronously to the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Sends data asynchronously to the server.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>True if the data was successfully queued for sending, otherwise false.</returns>
        bool SendAsync(byte[] buffer);

        /// <summary>
        /// Disconnects the socket client from the server.
        /// </summary>
        /// <returns>True if the disconnection is successful, otherwise false.</returns>
        bool Disconnect();

        /// <summary>
        /// Checks if the socket client is currently connected to the server.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        bool IsConnected();

        /// <summary>
        /// Retrieves network statistics for the socket client.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> representing the client's network statistics.</returns>
        INetworkStatistics GetNetworkStatistics();

    }

}

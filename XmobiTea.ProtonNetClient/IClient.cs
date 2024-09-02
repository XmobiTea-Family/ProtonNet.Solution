using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNetClient
{
    /// <summary>
    /// Defines the interface for a client that can connect, disconnect, 
    /// and send data over a network. Supports both synchronous and 
    /// asynchronous operations.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Connects the client to the server.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        bool Connect();

        /// <summary>
        /// Reconnects the client to the server.
        /// </summary>
        /// <returns>True if the reconnection is successful; otherwise, false.</returns>
        bool Reconnect();

        /// <summary>
        /// Disconnects the client from the server.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        bool Disconnect();

        /// <summary>
        /// Asynchronously connects the client to the server.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        bool ConnectAsync();

        /// <summary>
        /// Asynchronously reconnects the client to the server.
        /// </summary>
        /// <returns>True if the reconnection is successful; otherwise, false.</returns>
        bool ReconnectAsync();

        /// <summary>
        /// Asynchronously disconnects the client from the server.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        bool DisconnectAsync();

        /// <summary>
        /// Retrieves the network statistics associated with the client.
        /// </summary>
        /// <returns>An object implementing <see cref="INetworkStatistics"/> containing network statistics.</returns>
        INetworkStatistics GetNetworkStatistics();

        /// <summary>
        /// Sends data to the server.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Sends data to the server starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer, int position, int length);

        /// <summary>
        /// Asynchronously sends data to the server.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(byte[] buffer);

        /// <summary>
        /// Asynchronously sends data to the server starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(byte[] buffer, int position, int length);

    }

}

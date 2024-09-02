using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a server, providing methods for starting, stopping,
    /// and managing connections and data broadcasts.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        bool Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        /// <returns>True if the server stopped successfully; otherwise, false.</returns>
        bool Stop();

        /// <summary>
        /// Restarts the server.
        /// </summary>
        /// <returns>True if the server restarted successfully; otherwise, false.</returns>
        bool Restart();

        /// <summary>
        /// Broadcasts a message to all connected clients.
        /// </summary>
        /// <param name="buffer">The message buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        bool BroadcastAll(byte[] buffer);

        /// <summary>
        /// Broadcasts a message to all connected clients with a specified position and length.
        /// </summary>
        /// <param name="buffer">The message buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        bool BroadcastAll(byte[] buffer, int position, int length);

        /// <summary>
        /// Broadcasts a message asynchronously to all connected clients.
        /// </summary>
        /// <param name="buffer">The message buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        bool BroadcastAllAsync(byte[] buffer);

        /// <summary>
        /// Broadcasts a message asynchronously to all connected clients with a specified position and length.
        /// </summary>
        /// <param name="buffer">The message buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        bool BroadcastAllAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Disconnects all connected clients.
        /// </summary>
        /// <returns>True if all clients were disconnected successfully; otherwise, false.</returns>
        bool DisconnectAll();

        /// <summary>
        /// Gets the network statistics for the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing the network statistics.</returns>
        IServerNetworkStatistics GetNetworkStatistics();

    }

}

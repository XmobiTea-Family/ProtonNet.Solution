using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a session, providing methods for managing the connection,
    /// sending data, and retrieving network statistics.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Disconnects the session.
        /// </summary>
        /// <returns>True if the session was disconnected successfully; otherwise, false.</returns>
        bool Disconnect();

        /// <summary>
        /// Sends data synchronously to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Sends data synchronously to the connected client with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer, int position, int length);

        /// <summary>
        /// Sends data asynchronously to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(byte[] buffer);

        /// <summary>
        /// Sends data asynchronously to the connected client with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(byte[] buffer, int position, int length);

        /// <summary>
        /// Gets the network statistics for the session.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> representing the network statistics.</returns>
        INetworkStatistics GetNetworkStatistics();

    }

}

using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.Socket.Server
{
    /// <summary>
    /// Defines the interface for a server, providing methods to manage server operations
    /// such as starting, stopping, and restarting the server, as well as retrieving network statistics.
    /// </summary>
    interface IServer
    {
        /// <summary>
        /// Gets the IP address the server is bound to.
        /// </summary>
        /// <returns>The server's IP address.</returns>
        string GetAddress();

        /// <summary>
        /// Gets the port the server is listening on.
        /// </summary>
        /// <returns>The server's port number.</returns>
        int GetPort();

        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <returns>A boolean value indicating whether the server started successfully.</returns>
        bool Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        /// <returns>A boolean value indicating whether the server stopped successfully.</returns>
        bool Stop();

        /// <summary>
        /// Restarts the server.
        /// </summary>
        /// <returns>A boolean value indicating whether the server restarted successfully.</returns>
        bool Restart();

        /// <summary>
        /// Retrieves the network statistics for the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing the server's network statistics.</returns>
        IServerNetworkStatistics GetNetworkStatistics();

    }

}

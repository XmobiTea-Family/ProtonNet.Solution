using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi.Server
{
    /// <summary>
    /// Defines the contract for a sub-server, including methods for starting, stopping, and restarting the server,
    /// as well as retrieving network statistics.
    /// </summary>
    interface ISubServer
    {
        /// <summary>
        /// Gets the address the server is bound to.
        /// </summary>
        /// <returns>The address of the server.</returns>
        string GetAddress();

        /// <summary>
        /// Gets the port number the server is listening on.
        /// </summary>
        /// <returns>The port number of the server.</returns>
        int GetPort();

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
        /// Retrieves network statistics for the server.
        /// </summary>
        /// <returns>An object containing network statistics for the server.</returns>
        IServerNetworkStatistics GetNetworkStatistics();

    }

}

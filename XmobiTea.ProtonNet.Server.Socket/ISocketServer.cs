using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Defines the interface for a socket server, including methods to start and stop the server,
    /// and to retrieve the server context and network statistics.
    /// </summary>
    public interface ISocketServer
    {
        /// <summary>
        /// Starts the socket server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the socket server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the context of the socket server, which includes various services and configurations.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketServerContext"/> representing the server's context.</returns>
        ISocketServerContext GetContext();

        /// <summary>
        /// Gets the network statistics for the socket server, providing details about network performance and usage.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing the server's network statistics.</returns>
        IServerNetworkStatistics GetNetworkStatistics();

    }

}

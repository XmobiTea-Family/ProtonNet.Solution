using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi
{
    /// <summary>
    /// Interface for web API server operations.
    /// </summary>
    public interface IWebApiServer
    {
        /// <summary>
        /// Starts the web API server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the web API server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the current server context.
        /// </summary>
        /// <returns>An instance of <see cref="IWebApiServerContext"/> representing the server context.</returns>
        IWebApiServerContext GetContext();

        /// <summary>
        /// Gets the network statistics for the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing network statistics.</returns>
        IServerNetworkStatistics GetNetworkStatistics();

    }

}

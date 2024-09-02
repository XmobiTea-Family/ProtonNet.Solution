using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Implements <see cref="IServerNetworkStatistics"/> to aggregate network statistics from multiple sources.
    /// </summary>
    public class ServerNetworkStatistics : IServerNetworkStatistics
    {
        private IServerNetworkStatistics[] networkStatistics { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerNetworkStatistics"/> class.
        /// </summary>
        /// <param name="networkStatistics">An array of <see cref="IServerNetworkStatistics"/> instances to aggregate.</param>
        public ServerNetworkStatistics(params IServerNetworkStatistics[] networkStatistics)
        {
            this.networkStatistics = networkStatistics;
        }

        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        /// <returns>The total number of bytes sent.</returns>
        public long GetBytesSent()
        {
            long answer = 0;

            foreach (var networkStatistic in this.networkStatistics)
                answer += networkStatistic.GetBytesSent();

            return answer;
        }

        /// <summary>
        /// Gets the total number of bytes received.
        /// </summary>
        /// <returns>The total number of bytes received.</returns>
        public long GetBytesReceived()
        {
            long answer = 0;

            foreach (var networkStatistic in this.networkStatistics)
                answer += networkStatistic.GetBytesReceived();

            return answer;
        }

        /// <summary>
        /// Gets the total number of packets sent.
        /// </summary>
        /// <returns>The total number of packets sent.</returns>
        public long GetPacketSent()
        {
            long answer = 0;

            foreach (var networkStatistic in this.networkStatistics)
                answer += networkStatistic.GetPacketSent();

            return answer;
        }

        /// <summary>
        /// Gets the total number of packets received.
        /// </summary>
        /// <returns>The total number of packets received.</returns>
        public long GetPacketReceived()
        {
            long answer = 0;

            foreach (var networkStatistic in this.networkStatistics)
                answer += networkStatistic.GetPacketReceived();

            return answer;
        }

    }

}

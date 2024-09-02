namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Interface for retrieving server network statistics.
    /// </summary>
    public interface IServerNetworkStatistics
    {
        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        long GetBytesSent();

        /// <summary>
        /// Gets the total number of bytes received.
        /// </summary>
        long GetBytesReceived();

        /// <summary>
        /// Gets the total number of packets sent.
        /// </summary>
        long GetPacketSent();

        /// <summary>
        /// Gets the total number of packets received.
        /// </summary>
        long GetPacketReceived();

    }

    /// <summary>
    /// Interface for modifying server network statistics.
    /// </summary>
    public interface IChangeServerNetworkStatistics : IServerNetworkStatistics
    {
        /// <summary>
        /// Increases the number of bytes sent by the specified amount.
        /// </summary>
        void ChangeBytesSent(long amount);

        /// <summary>
        /// Increases the number of bytes received by the specified amount.
        /// </summary>
        void ChangeBytesReceived(long amount);

        /// <summary>
        /// Sets the total number of bytes sent to the specified value.
        /// </summary>
        void SetBytesSent(long newValue);

        /// <summary>
        /// Sets the total number of bytes received to the specified value.
        /// </summary>
        void SetBytesReceived(long newValue);

        /// <summary>
        /// Increments the total number of packets sent by one.
        /// </summary>
        void IncPacketSent();

        /// <summary>
        /// Increments the total number of packets received by one.
        /// </summary>
        void IncPacketReceived();

        /// <summary>
        /// Resets all server network statistics to zero.
        /// </summary>
        void Reset();

    }

    /// <summary>
    /// Class that implements IServerNetworkStatistics, providing basic retrieval of server network statistics.
    /// </summary>
    public class ServerNetworkStatistics : IServerNetworkStatistics
    {
        /// <summary>
        /// Total number of bytes sent.
        /// </summary>
        protected long bytesSent;

        /// <summary>
        /// Total number of bytes received.
        /// </summary>
        protected long bytesReceived;

        /// <summary>
        /// Total number of packets sent.
        /// </summary>
        protected long packetSent;

        /// <summary>
        /// Total number of packets received.
        /// </summary>
        protected long packetReceived;

        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        public long GetBytesSent() => this.bytesSent;

        /// <summary>
        /// Gets the total number of bytes received.
        /// </summary>
        public long GetBytesReceived() => this.bytesReceived;

        /// <summary>
        /// Gets the total number of packets sent.
        /// </summary>
        public long GetPacketSent() => this.packetSent;

        /// <summary>
        /// Gets the total number of packets received.
        /// </summary>
        public long GetPacketReceived() => this.packetReceived;

    }

    /// <summary>
    /// Class that implements IChangeServerNetworkStatistics, providing methods to modify and retrieve server network statistics.
    /// </summary>
    public class ChangeServerNetworkStatistics : ServerNetworkStatistics, IChangeServerNetworkStatistics
    {
        /// <summary>
        /// Increases the number of bytes sent by the specified amount.
        /// </summary>
        public void ChangeBytesSent(long amount) => System.Threading.Interlocked.Add(ref this.bytesSent, amount);

        /// <summary>
        /// Increases the number of bytes received by the specified amount.
        /// </summary>
        public void ChangeBytesReceived(long amount) => System.Threading.Interlocked.Add(ref this.bytesReceived, amount);

        /// <summary>
        /// Sets the total number of bytes sent to the specified value.
        /// </summary>
        public void SetBytesSent(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesSent, newValue);

        /// <summary>
        /// Sets the total number of bytes received to the specified value.
        /// </summary>
        public void SetBytesReceived(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesReceived, newValue);

        /// <summary>
        /// Increments the total number of packets sent by one.
        /// </summary>
        public void IncPacketSent() => System.Threading.Interlocked.Increment(ref this.packetSent);

        /// <summary>
        /// Increments the total number of packets received by one.
        /// </summary>
        public void IncPacketReceived() => System.Threading.Interlocked.Increment(ref this.packetReceived);

        /// <summary>
        /// Resets all server network statistics to zero.
        /// </summary>
        public void Reset()
        {
            System.Threading.Interlocked.Exchange(ref this.bytesSent, 0);
            System.Threading.Interlocked.Exchange(ref this.bytesReceived, 0);

            System.Threading.Interlocked.Exchange(ref this.packetSent, 0);
            System.Threading.Interlocked.Exchange(ref this.packetReceived, 0);
        }

    }

}

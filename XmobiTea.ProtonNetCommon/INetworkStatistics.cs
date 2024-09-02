namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Interface for retrieving network statistics.
    /// </summary>
    public interface INetworkStatistics
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
        /// Gets the total number of bytes pending to be sent.
        /// </summary>
        long GetBytesPending();

        /// <summary>
        /// Gets the total number of bytes currently being sent.
        /// </summary>
        long GetBytesSending();

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
    /// Interface for modifying network statistics.
    /// </summary>
    public interface IChangeNetworkStatistics : INetworkStatistics
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
        /// Increases the number of bytes pending by the specified amount.
        /// </summary>
        void ChangeBytesPending(long amount);

        /// <summary>
        /// Increases the number of bytes being sent by the specified amount.
        /// </summary>
        void ChangeBytesSending(long amount);

        /// <summary>
        /// Sets the total number of bytes sent to the specified value.
        /// </summary>
        void SetBytesSent(long newValue);

        /// <summary>
        /// Sets the total number of bytes received to the specified value.
        /// </summary>
        void SetBytesReceived(long newValue);

        /// <summary>
        /// Sets the total number of bytes pending to the specified value.
        /// </summary>
        void SetBytesPending(long newValue);

        /// <summary>
        /// Sets the total number of bytes being sent to the specified value.
        /// </summary>
        void SetBytesSending(long newValue);

        /// <summary>
        /// Increments the total number of packets sent by one.
        /// </summary>
        void IncPacketSent();

        /// <summary>
        /// Increments the total number of packets received by one.
        /// </summary>
        void IncPacketReceived();

        /// <summary>
        /// Resets all network statistics to zero.
        /// </summary>
        void Reset();

    }

    /// <summary>
    /// Class that implements INetworkStatistics, providing basic retrieval of network statistics.
    /// </summary>
    public class NetworkStatistics : INetworkStatistics
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
        /// Total number of bytes pending to be sent.
        /// </summary>
        protected long bytesPending;

        /// <summary>
        /// Total number of bytes currently being sent.
        /// </summary>
        protected long bytesSending;

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
        /// Gets the total number of bytes pending to be sent.
        /// </summary>
        public long GetBytesPending() => this.bytesPending;

        /// <summary>
        /// Gets the total number of bytes currently being sent.
        /// </summary>
        public long GetBytesSending() => this.bytesSending;

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
    /// Class that implements IChangeNetworkStatistics, providing methods to modify and retrieve network statistics.
    /// </summary>
    public class ChangeNetworkStatistics : NetworkStatistics, IChangeNetworkStatistics
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
        /// Increases the number of bytes pending by the specified amount.
        /// </summary>
        public void ChangeBytesPending(long amount) => System.Threading.Interlocked.Add(ref this.bytesPending, amount);

        /// <summary>
        /// Increases the number of bytes being sent by the specified amount.
        /// </summary>
        public void ChangeBytesSending(long amount) => System.Threading.Interlocked.Add(ref this.bytesSending, amount);

        /// <summary>
        /// Sets the total number of bytes sent to the specified value.
        /// </summary>
        public void SetBytesSent(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesSent, newValue);

        /// <summary>
        /// Sets the total number of bytes received to the specified value.
        /// </summary>
        public void SetBytesReceived(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesReceived, newValue);

        /// <summary>
        /// Sets the total number of bytes pending to the specified value.
        /// </summary>
        public void SetBytesPending(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesPending, newValue);

        /// <summary>
        /// Sets the total number of bytes being sent to the specified value.
        /// </summary>
        public void SetBytesSending(long newValue) => System.Threading.Interlocked.Exchange(ref this.bytesSending, newValue);

        /// <summary>
        /// Increments the total number of packets sent by one.
        /// </summary>
        public void IncPacketSent() => System.Threading.Interlocked.Increment(ref this.packetSent);

        /// <summary>
        /// Increments the total number of packets received by one.
        /// </summary>
        public void IncPacketReceived() => System.Threading.Interlocked.Increment(ref this.packetReceived);

        /// <summary>
        /// Resets all network statistics to zero.
        /// </summary>
        public void Reset()
        {
            System.Threading.Interlocked.Exchange(ref this.bytesSent, 0);
            System.Threading.Interlocked.Exchange(ref this.bytesReceived, 0);
            System.Threading.Interlocked.Exchange(ref this.bytesPending, 0);
            System.Threading.Interlocked.Exchange(ref this.bytesSending, 0);

            System.Threading.Interlocked.Exchange(ref this.packetSent, 0);
            System.Threading.Interlocked.Exchange(ref this.packetReceived, 0);
        }

    }

}

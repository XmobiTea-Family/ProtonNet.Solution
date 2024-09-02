namespace XmobiTea.ProtonNet.Server.Socket.Models
{
    /// <summary>
    /// Defines the interface for tracking and managing time-related information for a socket session,
    /// such as connection time, handshake time, and the last received data time.
    /// </summary>
    public interface ISocketSessionTime
    {
        /// <summary>
        /// Gets the timestamp (in ticks) when the session was connected.
        /// </summary>
        /// <returns>The timestamp of when the session was connected.</returns>
        long GetTicksConnected();

        /// <summary>
        /// Gets the timestamp (in ticks) when the session completed the handshake.
        /// </summary>
        /// <returns>The timestamp of when the session completed the handshake.</returns>
        long GetTicksHandshaked();

        /// <summary>
        /// Sets the timestamp (in ticks) when the session completed the handshake.
        /// </summary>
        /// <param name="ticks">The timestamp of the handshake completion.</param>
        void SetTicksHandshaked(long ticks);

        /// <summary>
        /// Gets the timestamp (in ticks) of the last received data for the session.
        /// </summary>
        /// <returns>The timestamp of the last received data.</returns>
        long GetTicksLastReceived();

        /// <summary>
        /// Sets the timestamp (in ticks) of the last received data for the session.
        /// </summary>
        /// <param name="ticks">The timestamp of the last received data.</param>
        void SetTicksLastReceived(long ticks);

    }

    /// <summary>
    /// Implements the <see cref="ISocketSessionTime"/> interface to track and manage 
    /// time-related information for a socket session, such as connection time, handshake time, 
    /// and the last received data time.
    /// </summary>
    class SocketSessionTime : ISocketSessionTime
    {
        /// <summary>
        /// Gets the timestamp (in ticks) when the session was connected.
        /// </summary>
        private long ticksConnected { get; }

        /// <summary>
        /// Gets or sets the timestamp (in ticks) when the session completed the handshake.
        /// </summary>
        private long ticksHandshaked { get; set; }

        /// <summary>
        /// Gets or sets the timestamp (in ticks) of the last received data for the session.
        /// </summary>
        private long ticksLastReceived { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSessionTime"/> class and sets the connection time to the current time.
        /// </summary>
        public SocketSessionTime() => this.ticksConnected = System.DateTime.UtcNow.Ticks;

        /// <summary>
        /// Gets the timestamp (in ticks) when the session was connected.
        /// </summary>
        /// <returns>The timestamp of when the session was connected.</returns>
        public long GetTicksConnected() => this.ticksConnected;

        /// <summary>
        /// Gets the timestamp (in ticks) when the session completed the handshake.
        /// </summary>
        /// <returns>The timestamp of when the session completed the handshake.</returns>
        public long GetTicksHandshaked() => this.ticksHandshaked;

        /// <summary>
        /// Sets the timestamp (in ticks) when the session completed the handshake.
        /// </summary>
        /// <param name="ticks">The timestamp of the handshake completion.</param>
        public void SetTicksHandshaked(long ticks) => this.ticksHandshaked = ticks;

        /// <summary>
        /// Gets the timestamp (in ticks) of the last received data for the session.
        /// </summary>
        /// <returns>The timestamp of the last received data.</returns>
        public long GetTicksLastReceived() => this.ticksLastReceived;

        /// <summary>
        /// Sets the timestamp (in ticks) of the last received data for the session.
        /// </summary>
        /// <param name="ticks">The timestamp of the last received data.</param>
        public void SetTicksLastReceived(long ticks) => this.ticksLastReceived = ticks;

    }

}

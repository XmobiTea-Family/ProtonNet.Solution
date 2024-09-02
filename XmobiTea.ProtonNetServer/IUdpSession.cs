using System;
using System.Net.Sockets;
using System.Threading;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a UDP session.
    /// </summary>
    public interface IUdpSession : ISession
    {

    }

    /// <summary>
    /// Represents a UDP session that handles communication with a remote endpoint.
    /// </summary>
    public class UdpSession : IUdpSession, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the session.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the server associated with this session.
        /// </summary>
        public UdpServer Server { get; }

        /// <summary>
        /// Gets the remote endpoint for this session.
        /// </summary>
        public System.Net.EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the session is connected.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the session is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private IChangeNetworkStatistics networkStatistics { get; }

        private Timer disconnectHandlerTimer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpSession"/> class with the specified server.
        /// </summary>
        /// <param name="server">The server associated with this session.</param>
        public UdpSession(UdpServer server)
        {
            this.Id = this.CreateRandomId();
            this.Server = server;
            this.networkStatistics = new ChangeNetworkStatistics();
        }

        /// <summary>
        /// Creates a random identifier for the session.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the network statistics for the session.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> representing the network statistics.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        internal void ConnectInternal(System.Net.EndPoint endPoint)
        {
            this.RemoteEndPoint = endPoint;
            this.networkStatistics.Reset();

            this.OnConnecting();
            this.Server.OnConnectingInternal(this);

            this.IsConnected = true;

            this.OnConnected();
            this.Server.OnConnectedInternal(this);
        }

        /// <summary>
        /// Disconnects the session.
        /// </summary>
        /// <returns>True if the session was disconnected successfully; otherwise, false.</returns>
        public virtual bool Disconnect()
        {
            if (!this.IsConnected)
                return false;

            this.OnDisconnecting();
            this.Server.OnDisconnectingInternal(this);

            this.IsConnected = false;

            this.OnDisconnected();
            this.Server.OnDisconnectedInternal(this);

            this.Server.RemoveSession(this.Id);

            return true;
        }

        internal void ReceivedInternal(byte[] buffer, int position, int length)
        {
            // Reset the disconnect timer on receiving data
            if (this.disconnectHandlerTimer != null)
                this.disconnectHandlerTimer.Dispose();
            this.disconnectHandlerTimer = new Timer(x =>
            {
                this.Disconnect();
            }, this, 10000, -1);

            this.networkStatistics.ChangeBytesReceived(length);
            this.networkStatistics.IncPacketReceived();

            this.OnReceived(buffer, position, length);
        }

        /// <summary>
        /// Called when the session is connecting. Override this method to add custom logic during connection.
        /// </summary>
        protected virtual void OnConnecting() { }

        /// <summary>
        /// Called after the session has connected successfully. Override this method to add custom logic after connection.
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Called when the session is disconnecting. Override this method to add custom logic during disconnection.
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Called after the session has disconnected successfully. Override this method to add custom logic after disconnection.
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Called when data is received from the remote endpoint. Override this method to process received data.
        /// </summary>
        /// <param name="buffer">The received data buffer.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called after data has been sent to the remote endpoint. Override this method to add custom logic after sending data.
        /// </summary>
        /// <param name="sent">The number of bytes sent.</param>
        protected virtual void OnSent(int sent) { }

        /// <summary>
        /// Called when a socket error occurs. Override this method to handle errors in a custom way.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        internal void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error))
                return;

            this.OnError(error);
        }

        /// <summary>
        /// Sends data synchronously to the remote endpoint.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data synchronously to the remote endpoint with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public int Send(byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return 0;

            if (length == 0)
                return 0;

            var sent = this.Server.Send(this.RemoteEndPoint, buffer, position, length);

            if (sent > 0)
            {
                this.networkStatistics.ChangeBytesSent(sent);
                this.networkStatistics.IncPacketSent();

                this.OnSent(sent);
            }

            return sent;
        }

        /// <summary>
        /// Sends data asynchronously to the remote endpoint.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public bool SendAsync(byte[] buffer) => this.SendAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data asynchronously to the remote endpoint with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public bool SendAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return false;

            if (length == 0)
                return true;

            var sendResult = this.Server.SendAsync(this.RemoteEndPoint, buffer, position, length);

            if (sendResult)
            {
                this.networkStatistics.ChangeBytesSent(length);
                this.networkStatistics.IncPacketSent();

                this.OnSent(length);
            }

            return sendResult;
        }

        /// <summary>
        /// Disposes the session and its resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the session, optionally releasing managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release managed resources; otherwise, false.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.disconnectHandlerTimer?.Dispose();
                    this.Disconnect();
                }

                this.IsDisposed = true;
            }
        }
    }
}

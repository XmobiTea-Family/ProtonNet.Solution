using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Defines the interface for a UDP server, extending the basic server functionalities.
    /// </summary>
    public interface IUdpServer : IServer
    {
        /// <summary>
        /// Starts the UDP server with the specified multicast address and port.
        /// </summary>
        /// <param name="multicastAddress">The multicast address to join.</param>
        /// <param name="multicastPort">The multicast port to use.</param>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        bool Start(string multicastAddress, int multicastPort);

        /// <summary>
        /// Sends a datagram to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(EndPoint endpoint, byte[] buffer);

        /// <summary>
        /// Sends a datagram to the specified endpoint with the given position and length.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(EndPoint endpoint, byte[] buffer, int position, int length);

        /// <summary>
        /// Asynchronously sends a datagram to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the data was successfully queued for sending; otherwise, false.</returns>
        bool SendAsync(EndPoint endpoint, byte[] buffer);

        /// <summary>
        /// Asynchronously sends a datagram to the specified endpoint with the given position and length.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the data was successfully queued for sending; otherwise, false.</returns>
        bool SendAsync(EndPoint endpoint, byte[] buffer, int position, int length);

    }

    /// <summary>
    /// Represents a UDP server that provides functionalities for managing UDP connections and data transmission.
    /// </summary>
    public class UdpServer : IDisposable, IUdpServer
    {
        /// <summary>
        /// Gets the unique identifier of the server.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the IP address of the server.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port number on which the server is listening.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the server's local endpoint.
        /// </summary>
        public EndPoint EndPoint { get; private set; }

        /// <summary>
        /// Gets the multicast endpoint used by the server.
        /// </summary>
        public EndPoint MulticastEndpoint { get; private set; }

        /// <summary>
        /// Gets the collection of active UDP sessions.
        /// </summary>
        protected ConcurrentDictionary<string, UdpSession> Sessions { get; }

        /// <summary>
        /// Gets the dictionary that maps endpoints to UDP sessions.
        /// </summary>
        protected ConcurrentDictionary<System.Net.EndPoint, UdpSession> EndPointWithSocketSessionDict { get; }

        /// <summary>
        /// Gets the number of connected sessions.
        /// </summary>
        public int ConnectedSessions => this.Sessions.Count;

        /// <summary>
        /// Socket handler for managing the server's network communication.
        /// </summary>
        private Socket handlerSocket { get; set; }

        /// <summary>
        /// Buffer for storing received data before processing.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// Buffer for storing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBuffer { get; }

        /// <summary>
        /// Endpoint for receiving data from clients.
        /// </summary>
        private EndPoint receiveEndPoint { get; }

        /// <summary>
        /// Endpoint for sending data to clients.
        /// </summary>
        private EndPoint sendEndPoint { get; set; }

        /// <summary>
        /// Flag indicating whether the server is currently receiving data.
        /// </summary>
        private bool receiving { get; set; }

        /// <summary>
        /// Event arguments for handling asynchronous receive operations.
        /// </summary>
        private SocketAsyncEventArgs receiveEventArg { get; set; }

        /// <summary>
        /// Flag indicating whether the server is currently sending data.
        /// </summary>
        private bool sending { get; set; }

        /// <summary>
        /// Event arguments for handling asynchronous send operations.
        /// </summary>
        private SocketAsyncEventArgs sendEventArg { get; set; }

        /// <summary>
        /// Internal statistics for tracking the server's network performance.
        /// </summary>
        internal IChangeServerNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Gets the options used by the UDP server.
        /// </summary>
        public UdpServerOptions Options { get; }

        /// <summary>
        /// Gets a value indicating whether the server has been started.
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server's socket has been disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpServer"/> class with the specified address, port, and options.
        /// </summary>
        /// <param name="address">The IP address of the server.</param>
        /// <param name="port">The port number on which the server listens.</param>
        /// <param name="options">The options used by the server.</param>
        public UdpServer(string address, int port, UdpServerOptions options)
        {
            this.Id = this.CreateRandomId();

            this.Address = address;
            this.Port = port;
            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.Options = options;

            this.IsSocketDisposed = true;
            this.Sessions = new ConcurrentDictionary<string, UdpSession>();
            this.EndPointWithSocketSessionDict = new ConcurrentDictionary<EndPoint, UdpSession>();

            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();

            this.receiveEndPoint = new IPEndPoint(this.EndPoint.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);

            this.networkStatistics = new ChangeServerNetworkStatistics();
        }

        /// <summary>
        /// Generates a random identifier string.
        /// </summary>
        /// <returns>A randomly generated string.</returns>
        protected virtual string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Starts the UDP server with the specified multicast address and port.
        /// </summary>
        /// <param name="multicastAddress">The multicast address to join.</param>
        /// <param name="multicastPort">The multicast port to use.</param>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        public virtual bool Start(string multicastAddress, int multicastPort)
        {
            this.MulticastEndpoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);

            return this.Start();
        }

        /// <summary>
        /// Starts the UDP server.
        /// </summary>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        public virtual bool Start()
        {
            if (this.IsStarted)
                return false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();

            this.receiveEventArg = new SocketAsyncEventArgs();
            this.sendEventArg = new SocketAsyncEventArgs();

            this.receiveEventArg.Completed -= this.OnAsyncCompletedInternal;
            this.receiveEventArg.Completed += this.OnAsyncCompletedInternal;

            this.sendEventArg.Completed -= this.OnAsyncCompletedInternal;
            this.sendEventArg.Completed += this.OnAsyncCompletedInternal;

            this.handlerSocket = new Socket(this.EndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            this.IsSocketDisposed = false;

            this.handlerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, this.Options.ReuseAddress);

            this.handlerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, this.Options.ExclusiveAddressUse);

            if (this.handlerSocket.AddressFamily == AddressFamily.InterNetworkV6)
                this.handlerSocket.DualMode = this.Options.DualMode;

            this.handlerSocket.Bind(this.EndPoint);
            this.EndPoint = this.handlerSocket.LocalEndPoint;

            this.OnStarting();

            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);

            this.networkStatistics.Reset();

            this.IsStarted = true;

            this.OnStarted();

            this.TryReceive();

            return true;
        }

        /// <summary>
        /// Stops the UDP server.
        /// </summary>
        /// <returns>True if the server stopped successfully; otherwise, false.</returns>
        public virtual bool Stop()
        {
            if (!this.IsStarted)
                return false;

            this.receiveEventArg.Completed -= this.OnAsyncCompletedInternal;
            this.sendEventArg.Completed -= this.OnAsyncCompletedInternal;

            this.OnStopping();

            try
            {
                this.handlerSocket.Close();
                this.handlerSocket.Dispose();

                this.receiveEventArg.Dispose();
                this.sendEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.IsStarted = false;

            this.receiving = false;
            this.sending = false;

            this.ClearBuffers();

            this.OnStopped();

            return true;
        }

        /// <summary>
        /// Restarts the UDP server.
        /// </summary>
        /// <returns>True if the server restarted successfully; otherwise, false.</returns>
        public virtual bool Restart()
        {
            if (!this.Stop())
                return false;

            return this.Start();
        }

        /// <summary>
        /// Broadcasts a message to all clients using the specified buffer.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the broadcast was successful; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer) => this.BroadcastAll(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message to all clients using the specified buffer, position, and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the broadcast was successful; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer, int position, int length) => this.Send(this.MulticastEndpoint, buffer, position, length) != 0;

        /// <summary>
        /// Broadcasts a message asynchronously to all clients using the specified buffer.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the broadcast was successful; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer) => this.BroadcastAllAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message asynchronously to all clients using the specified buffer, position, and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the broadcast was successful; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer, int position, int length) => this.SendAsync(this.MulticastEndpoint, buffer, 0, buffer.Length);

        /// <summary>
        /// Disconnects all connected sessions.
        /// </summary>
        /// <returns>True if all sessions were disconnected successfully; otherwise, false.</returns>
        public virtual bool DisconnectAll()
        {
            if (!this.IsStarted)
                return false;

            foreach (var session in this.Sessions.Values)
                session.Disconnect();

            return true;
        }

        /// <summary>
        /// Retrieves the server's network statistics.
        /// </summary>
        /// <returns>An object that provides network statistics.</returns>
        public IServerNetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Sends a datagram asynchronously to the specified endpoint using the provided buffer.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the data was successfully queued for sending; otherwise, false.</returns>
        public virtual bool SendAsync(EndPoint endpoint, byte[] buffer) => this.SendAsync(endpoint, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a datagram asynchronously to the specified endpoint using the provided buffer, position, and length.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the data was successfully queued for sending; otherwise, false.</returns>
        public virtual bool SendAsync(EndPoint endpoint, byte[] buffer, int position, int length)
        {
            if (this.sending)
                return false;

            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            if (this.sendBuffer.Length + length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
            {
                var session = this.GetSession(endpoint);
                if (session != null) session.SendErrorInternal(SocketError.NoBufferSpaceAvailable);

                return false;
            }

            this.networkStatistics.ChangeBytesSent(length);
            this.networkStatistics.IncPacketSent();

            this.sendBuffer.Write(buffer, position, length);

            this.sendEndPoint = endpoint;

            this.TrySend();

            return true;
        }

        /// <summary>
        /// Sends a datagram synchronously to the specified endpoint using the provided buffer.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(EndPoint endpoint, byte[] buffer) => this.Send(endpoint, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends a datagram synchronously to the specified endpoint using the provided buffer, position, and length.
        /// </summary>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(EndPoint endpoint, byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return 0;

            if (length == 0)
                return 0;

            try
            {
                var sent = this.handlerSocket.SendTo(buffer, position, length, SocketFlags.None, endpoint);

                if (sent > 0)
                {
                    this.networkStatistics.ChangeBytesSent(sent);
                    this.networkStatistics.IncPacketSent();

                    this.OnSent(endpoint, sent);
                }

                return sent;
            }
            catch (ObjectDisposedException) { return 0; }
            catch (SocketException ex)
            {
                var session = this.GetSession(endpoint);
                if (session != null) session.SendErrorInternal(ex.SocketErrorCode);

                this.SendErrorInternal(ex.SocketErrorCode);
                return 0;
            }
        }

        /// <summary>
        /// Finds a session by its unique identifier.
        /// </summary>
        /// <param name="id">The session identifier.</param>
        /// <returns>The <see cref="UdpSession"/> associated with the specified identifier, or null if not found.</returns>
        public UdpSession GetSession(string id) => this.Sessions.TryGetValue(id, out var session) ? session : null;

        /// <summary>
        /// Finds a session by its remote endpoint.
        /// </summary>
        /// <param name="endpoint">The remote endpoint.</param>
        /// <returns>The <see cref="UdpSession"/> associated with the specified endpoint, or null if not found.</returns>
        public UdpSession GetSession(EndPoint endpoint) => this.EndPointWithSocketSessionDict.TryGetValue(endpoint, out var session) ? session : null;

        /// <summary>
        /// Maps a UDP session with the specified identifier to the current server instance.
        /// </summary>
        /// <param name="id">The identifier of the UDP session to be mapped.</param>
        /// <param name="session">The UDP session to be mapped.</param>
        internal void MapSession(string id, UdpSession session) => this.Sessions.TryAdd(id, session);

        /// <summary>
        /// Unregisters a session from the server.
        /// </summary>
        /// <param name="id">The unique identifier of the session to unregister.</param>
        internal void RemoveSession(string id)
        {
            if (this.Sessions.TryRemove(id, out var session))
                this.EndPointWithSocketSessionDict.TryRemove(session.RemoteEndPoint, out var _);
        }

        /// <summary>
        /// Attempts to receive a datagram.
        /// </summary>
        private void TryReceive()
        {
            if (this.receiving)
                return;

            if (!this.IsStarted)
                return;

            try
            {
                this.receiving = true;
                this.receiveEventArg.RemoteEndPoint = this.receiveEndPoint;
                this.receiveEventArg.SetBuffer(this.receiveBuffer.Buffer, 0, this.receiveBuffer.Capacity);
                if (!this.handlerSocket.ReceiveFromAsync(this.receiveEventArg))
                    this.ProcessReceiveFromInternal(this.receiveEventArg);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Attempts to send a datagram.
        /// </summary>
        private void TrySend()
        {
            if (this.sending)
                return;

            if (!this.IsStarted)
                return;

            try
            {
                this.sending = true;
                this.sendEventArg.RemoteEndPoint = this.sendEndPoint;
                this.sendEventArg.SetBuffer(this.sendBuffer.Buffer, 0, this.sendBuffer.Length);
                if (!this.handlerSocket.SendToAsync(this.sendEventArg))
                    this.ProcessSendToInternal(this.sendEventArg);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Clears the send and receive buffers.
        /// </summary>
        private void ClearBuffers()
        {
            this.sendBuffer.Clear();

            this.networkStatistics.Reset();
        }

        /// <summary>
        /// Processes a completed receive operation.
        /// </summary>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private void ProcessReceiveFromInternal(SocketAsyncEventArgs e)
        {
            this.receiving = false;

            if (!this.IsStarted)
                return;

            if (e.SocketError != SocketError.Success)
            {
                var session = this.GetSession(e.RemoteEndPoint);
                if (session != null) session.SendErrorInternal(e.SocketError);

                this.SendErrorInternal(e.SocketError);

                this.OnReceived(e.RemoteEndPoint, this.receiveBuffer.Buffer, 0, 0);

                return;
            }

            var received = e.BytesTransferred;

            this.OnReceived(e.RemoteEndPoint, this.receiveBuffer.Buffer, 0, received);

            if (this.receiveBuffer.Capacity == received)
            {
                if (received * 2 > this.Options.ReceiveBufferLimit && this.Options.ReceiveBufferLimit > 0)
                {
                    var session = this.GetSession(e.RemoteEndPoint);
                    if (session != null) session.SendErrorInternal(SocketError.NoBufferSpaceAvailable);

                    this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);

                    this.OnReceived(e.RemoteEndPoint, this.receiveBuffer.Buffer, 0, 0);

                    return;
                }

                this.receiveBuffer.Reserve(received * 2);
            }
        }

        /// <summary>
        /// Processes a completed send operation.
        /// </summary>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private void ProcessSendToInternal(SocketAsyncEventArgs e)
        {
            this.sending = false;

            if (!this.IsStarted)
                return;

            if (e.SocketError != SocketError.Success)
            {
                var session = this.GetSession(e.RemoteEndPoint);
                if (session != null) session.SendErrorInternal(e.SocketError);

                this.SendErrorInternal(e.SocketError);

                this.OnSent(this.sendEndPoint, 0);

                return;
            }

            var sent = e.BytesTransferred;

            if (sent > 0)
            {
                this.sendBuffer.Clear();

                this.OnSent(this.sendEndPoint, sent);
            }
        }

        /// <summary>
        /// Handles the completion of asynchronous socket operations.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        private void OnAsyncCompletedInternal(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.ReceiveFrom:
                    this.ProcessReceiveFromInternal(e);
                    break;
                case SocketAsyncOperation.SendTo:
                    this.ProcessSendToInternal(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }

        /// <summary>
        /// Creates a new instance of the <see cref="UdpSession"/> class.
        /// </summary>
        /// <returns>A new <see cref="UdpSession"/> instance.</returns>
        protected virtual UdpSession CreateSession() => new UdpSession(this);

        /// <summary>
        /// Called when the server is starting.
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Called when the server has started.
        /// </summary>
        protected virtual void OnStarted() { }

        /// <summary>
        /// Called when the server is stopping.
        /// </summary>
        protected virtual void OnStopping() { }

        /// <summary>
        /// Called when the server has stopped.
        /// </summary>
        protected virtual void OnStopped() { }

        /// <summary>
        /// Called when a session is connecting.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        protected virtual void OnConnecting(UdpSession session) { }

        /// <summary>
        /// Called when a session has connected.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        protected virtual void OnConnected(UdpSession session) { }

        /// <summary>
        /// Called when a session is disconnecting.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        protected virtual void OnDisconnecting(UdpSession session) { }

        /// <summary>
        /// Called when a session has disconnected.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        protected virtual void OnDisconnected(UdpSession session) { }

        /// <summary>
        /// Called when data is received from a remote endpoint.
        /// </summary>
        /// <param name="endpoint">The remote endpoint.</param>
        /// <param name="buffer">The data buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(EndPoint endpoint, byte[] buffer, int position, int length)
        {
            if (!this.EndPointWithSocketSessionDict.TryGetValue(endpoint, out var session))
            {
                session = this.CreateSession();
                session.ConnectInternal(endpoint);

                this.MapSession(this.Id, session);

                this.EndPointWithSocketSessionDict[endpoint] = session;
            }

            this.networkStatistics.ChangeBytesReceived(length);
            this.networkStatistics.IncPacketReceived();

            session.ReceivedInternal(buffer, position, length);

            this.TryReceive();
        }

        /// <summary>
        /// Called when data has been sent to a remote endpoint.
        /// </summary>
        /// <param name="endpoint">The remote endpoint.</param>
        /// <param name="sent">The number of bytes sent.</param>
        protected virtual void OnSent(EndPoint endpoint, int sent) { }

        /// <summary>
        /// Called when an error occurs during a socket operation.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Notifies the server that a session is connecting.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        internal void OnConnectingInternal(UdpSession session) => this.OnConnecting(session);

        /// <summary>
        /// Notifies the server that a session has connected.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        internal void OnConnectedInternal(UdpSession session) => this.OnConnected(session);

        /// <summary>
        /// Notifies the server that a session is disconnecting.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        internal void OnDisconnectingInternal(UdpSession session) => this.OnDisconnecting(session);

        /// <summary>
        /// Notifies the server that a session has disconnected.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        internal void OnDisconnectedInternal(UdpSession session) => this.OnDisconnected(session);

        /// <summary>
        /// Handles a socket error internally and calls the <see cref="OnError(SocketError)"/> method.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="UdpServer"/> and optionally releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="UdpServer"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.Stop();
                }

                this.IsDisposed = true;
            }
        }

    }

}

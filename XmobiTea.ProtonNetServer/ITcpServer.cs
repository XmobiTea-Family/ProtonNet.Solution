using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents the interface for a TCP server.
    /// </summary>
    public interface ITcpServer : IServer
    {

    }

    /// <summary>
    /// Represents a TCP server that can handle multiple client sessions.
    /// </summary>
    public class TcpServer : ITcpServer, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the server instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the server's IP address.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the server's port number.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the server's endpoint.
        /// </summary>
        public EndPoint EndPoint { get; private set; }

        /// <summary>
        /// Gets the collection of active sessions.
        /// </summary>
        protected ConcurrentDictionary<string, TcpSession> Sessions { get; }

        /// <summary>
        /// Gets the number of connected sessions.
        /// </summary>
        public int ConnectedSessions => this.Sessions.Count;

        private Socket acceptorSocket { get; set; }
        private SocketAsyncEventArgs acceptorEventArg { get; set; }

        internal IChangeServerNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Gets the TCP server options.
        /// </summary>
        public TcpServerOptions Options { get; }

        /// <summary>
        /// Gets a value indicating whether the server is started.
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server is currently accepting connections.
        /// </summary>
        public bool IsAccepting { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server socket is disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServer"/> class with the specified address, port, and options.
        /// </summary>
        /// <param name="address">The server's IP address.</param>
        /// <param name="port">The server's port number.</param>
        /// <param name="options">The TCP server options.</param>
        public TcpServer(string address, int port, TcpServerOptions options)
        {
            this.Id = this.CreateRandomId();
            this.Address = address;
            this.Port = port;
            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.Options = options;

            this.IsSocketDisposed = true;
            this.Sessions = new ConcurrentDictionary<string, TcpSession>();

            this.networkStatistics = new ChangeServerNetworkStatistics();
        }

        /// <summary>
        /// Creates a random identifier for the server.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        protected virtual string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Starts the TCP server.
        /// </summary>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        public virtual bool Start()
        {
            if (this.IsStarted)
                return false;

            this.acceptorEventArg = new SocketAsyncEventArgs();

            this.acceptorEventArg.Completed -= this.OnAsyncCompletedInternal;
            this.acceptorEventArg.Completed += this.OnAsyncCompletedInternal;

            this.acceptorSocket = new Socket(this.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.IsSocketDisposed = false;

            this.acceptorSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, this.Options.ReuseAddress);

            this.acceptorSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, this.Options.ExclusiveAddressUse);

            if (this.acceptorSocket.AddressFamily == AddressFamily.InterNetworkV6)
                this.acceptorSocket.DualMode = this.Options.DualMode;

            this.acceptorSocket.Bind(this.EndPoint);
            this.EndPoint = this.acceptorSocket.LocalEndPoint;

            this.OnStarting();

            this.acceptorSocket.Listen(this.Options.AcceptorBacklog);

            this.networkStatistics.Reset();

            this.IsStarted = true;

            this.OnStarted();

            this.IsAccepting = true;
            this.StartAcceptInternal(this.acceptorEventArg);

            return true;
        }

        /// <summary>
        /// Stops the TCP server.
        /// </summary>
        /// <returns>True if the server stopped successfully; otherwise, false.</returns>
        public virtual bool Stop()
        {
            if (!this.IsStarted)
                return false;

            this.IsAccepting = false;

            this.acceptorEventArg.Completed -= this.OnAsyncCompletedInternal;

            this.OnStopping();

            try
            {
                this.acceptorSocket.Close();
                this.acceptorSocket.Dispose();

                this.acceptorEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.DisconnectAll();

            this.IsStarted = false;

            this.OnStopped();

            return true;
        }

        /// <summary>
        /// Restarts the TCP server.
        /// </summary>
        /// <returns>True if the server restarted successfully; otherwise, false.</returns>
        public virtual bool Restart()
        {
            if (!this.Stop())
                return false;

            while (this.IsStarted)
                Thread.Yield();

            return this.Start();
        }

        /// <summary>
        /// Broadcasts a message to all connected sessions.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer) => this.BroadcastAll(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message to all connected sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
                session.Send(buffer, position, length);

            return true;
        }

        /// <summary>
        /// Broadcasts a message asynchronously to all connected sessions.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer) => this.BroadcastAllAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message asynchronously to all connected sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
                session.SendAsync(buffer, position, length);

            return true;
        }

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
        /// Gets the network statistics for the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing the network statistics.</returns>
        public IServerNetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Finds a session by its identifier.
        /// </summary>
        /// <param name="id">The session identifier.</param>
        /// <returns>The <see cref="TcpSession"/> if found; otherwise, null.</returns>
        public TcpSession GetSession(string id) => this.Sessions.TryGetValue(id, out var session) ? session : null;

        /// <summary>
        /// Maps a session with the specified identifier to the current TCP server instance.
        /// </summary>
        /// <param name="id">The identifier of the session to be mapped.</param>
        /// <param name="session">The TCP session to be mapped.</param>
        internal void MapSession(string id, TcpSession session) => this.Sessions.TryAdd(id, session);

        /// <summary>
        /// Removes a session with the specified identifier from the current TCP server instance.
        /// </summary>
        /// <param name="id">The identifier of the session to be removed.</param>
        internal void RemoveSession(string id) => this.Sessions.TryRemove(id, out TcpSession _);

        /// <summary>
        /// Starts an asynchronous accept operation for incoming connections.
        /// </summary>
        /// <param name="e">The SocketAsyncEventArgs object representing the asynchronous operation.</param>
        private void StartAcceptInternal(SocketAsyncEventArgs e)
        {
            e.AcceptSocket = null;

            if (!this.acceptorSocket.AcceptAsync(e))
                this.ProcessAcceptInternal(e);
        }

        /// <summary>
        /// Processes the result of an asynchronous accept operation.
        /// </summary>
        /// <param name="e">The SocketAsyncEventArgs object representing the completed operation.</param>
        private void ProcessAcceptInternal(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                var session = this.CreateSession();

                this.MapSession(this.Id, session);

                session.ConnectInternal(e.AcceptSocket);
            }
            else
                this.SendErrorInternal(e.SocketError);

            if (this.IsAccepting)
                this.StartAcceptInternal(e);
        }

        /// <summary>
        /// Handles the completion of an asynchronous socket operation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The SocketAsyncEventArgs object representing the completed operation.</param>
        private void OnAsyncCompletedInternal(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            this.ProcessAcceptInternal(e);
        }

        /// <summary>
        /// Creates a new session. Override this method to provide custom session behavior.
        /// </summary>
        /// <returns>A new instance of <see cref="TcpSession"/>.</returns>
        protected virtual TcpSession CreateSession() => new TcpSession(this);

        /// <summary>
        /// Called when the server is starting. Override this method to add custom logic during server startup.
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Called after the server has started successfully. Override this method to add custom logic after server startup.
        /// </summary>
        protected virtual void OnStarted() { }

        /// <summary>
        /// Called when the server is stopping. Override this method to add custom logic during server shutdown.
        /// </summary>
        protected virtual void OnStopping() { }

        /// <summary>
        /// Called after the server has stopped successfully. Override this method to add custom logic after server shutdown.
        /// </summary>
        protected virtual void OnStopped() { }

        /// <summary>
        /// Called when a session is connecting. Override this method to handle session-specific logic during the connection process.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        protected virtual void OnConnecting(TcpSession session) { }

        /// <summary>
        /// Called after a session has connected successfully. Override this method to handle session-specific logic after the connection process.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        protected virtual void OnConnected(TcpSession session) { }

        /// <summary>
        /// Called when a session is disconnecting. Override this method to handle session-specific logic during the disconnection process.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        protected virtual void OnDisconnecting(TcpSession session) { }

        /// <summary>
        /// Called after a session has disconnected successfully. Override this method to handle session-specific logic after the disconnection process.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        protected virtual void OnDisconnected(TcpSession session) { }

        /// <summary>
        /// Called when a socket error occurs. Override this method to handle errors in a custom way.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Triggers the OnConnecting method for the specified session.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        internal void OnConnectingInternal(TcpSession session) => this.OnConnecting(session);

        /// <summary>
        /// Triggers the OnConnected method for the specified session.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        internal void OnConnectedInternal(TcpSession session) => this.OnConnected(session);

        /// <summary>
        /// Triggers the OnDisconnecting method for the specified session.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        internal void OnDisconnectingInternal(TcpSession session) => this.OnDisconnecting(session);

        /// <summary>
        /// Triggers the OnDisconnected method for the specified session.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        internal void OnDisconnectedInternal(TcpSession session) => this.OnDisconnected(session);

        /// <summary>
        /// Handles socket errors by triggering the OnError method.
        /// Ignores specific disconnect errors based on utility logic.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Disposes the server and its resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the server, optionally releasing managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release managed resources; otherwise, false.</param>
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

    /// <summary>
    /// Represents an SSL server that can handle multiple secure client sessions.
    /// </summary>
    public class SslServer : ITcpServer, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the server instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the Ssl options for secure communication.
        /// </summary>
        public SslOptions SslOptions { get; }

        /// <summary>
        /// Gets the server's IP address.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the server's port number.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the server's endpoint.
        /// </summary>
        public EndPoint EndPoint { get; private set; }

        /// <summary>
        /// Gets the collection of active secure sessions.
        /// </summary>
        protected ConcurrentDictionary<string, SslSession> Sessions { get; }

        /// <summary>
        /// Gets the number of connected secure sessions.
        /// </summary>
        public int ConnectedSessions => this.Sessions.Count;

        private Socket acceptorSocket { get; set; }
        private SocketAsyncEventArgs acceptorEventArg { get; set; }

        internal IChangeServerNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Gets the TCP server options.
        /// </summary>
        public TcpServerOptions Options { get; }

        /// <summary>
        /// Gets a value indicating whether the server is started.
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server is currently accepting connections.
        /// </summary>
        public bool IsAccepting { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server socket is disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SslServer"/> class with the specified address, port, options, and Ssl options.
        /// </summary>
        /// <param name="address">The server's IP address.</param>
        /// <param name="port">The server's port number.</param>
        /// <param name="options">The TCP server options.</param>
        /// <param name="sslOptions">The Ssl options for secure communication.</param>
        public SslServer(string address, int port, TcpServerOptions options, SslOptions sslOptions)
        {
            this.Id = this.CreateRandomId();

            this.SslOptions = sslOptions;
            this.Address = address;
            this.Port = port;

            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.Options = options;

            this.IsSocketDisposed = true;
            this.Sessions = new ConcurrentDictionary<string, SslSession>();

            this.networkStatistics = new ChangeServerNetworkStatistics();
        }

        /// <summary>
        /// Creates a random identifier for the server.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        protected virtual string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Starts the SSL server.
        /// </summary>
        /// <returns>True if the server started successfully; otherwise, false.</returns>
        public virtual bool Start()
        {
            if (this.IsStarted)
                return false;

            this.acceptorEventArg = new SocketAsyncEventArgs();

            this.acceptorEventArg.Completed -= this.OnAsyncCompletedInternal;
            this.acceptorEventArg.Completed += this.OnAsyncCompletedInternal;

            this.acceptorSocket = new Socket(this.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.IsSocketDisposed = false;

            this.acceptorSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, this.Options.ReuseAddress);

            this.acceptorSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, this.Options.ExclusiveAddressUse);

            if (this.acceptorSocket.AddressFamily == AddressFamily.InterNetworkV6)
                this.acceptorSocket.DualMode = this.Options.DualMode;

            this.acceptorSocket.Bind(this.EndPoint);
            this.EndPoint = this.acceptorSocket.LocalEndPoint;

            this.OnStarting();

            this.acceptorSocket.Listen(this.Options.AcceptorBacklog);

            this.networkStatistics.Reset();

            this.IsStarted = true;

            this.OnStarted();

            this.IsAccepting = true;
            this.StartAcceptInternal(this.acceptorEventArg);

            return true;
        }

        /// <summary>
        /// Stops the SSL server.
        /// </summary>
        /// <returns>True if the server stopped successfully; otherwise, false.</returns>
        public virtual bool Stop()
        {
            if (!this.IsStarted)
                return false;

            this.IsAccepting = false;

            this.acceptorEventArg.Completed -= this.OnAsyncCompletedInternal;

            this.OnStopping();

            try
            {
                this.acceptorSocket.Close();

                this.acceptorSocket.Dispose();

                this.acceptorEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.DisconnectAll();

            this.IsStarted = false;

            this.OnStopped();

            return true;
        }

        /// <summary>
        /// Restarts the SSL server.
        /// </summary>
        /// <returns>True if the server restarted successfully; otherwise, false.</returns>
        public virtual bool Restart()
        {
            if (!this.Stop())
                return false;

            while (this.IsStarted)
                Thread.Yield();

            return this.Start();
        }

        /// <summary>
        /// Broadcasts a message to all connected secure sessions.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer) => this.BroadcastAll(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message to all connected secure sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAll(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
                session.Send(buffer, position, length);

            return true;
        }

        /// <summary>
        /// Broadcasts a message asynchronously to all connected secure sessions.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer) => this.BroadcastAllAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Broadcasts a message asynchronously to all connected secure sessions with a specified position and length.
        /// </summary>
        /// <param name="buffer">The data buffer to broadcast.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to broadcast.</param>
        /// <returns>True if the message was broadcasted successfully; otherwise, false.</returns>
        public virtual bool BroadcastAllAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsStarted)
                return false;

            if (length == 0)
                return true;

            foreach (var session in this.Sessions.Values)
                session.SendAsync(buffer, position, length);

            return true;
        }

        /// <summary>
        /// Disconnects all connected secure sessions.
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
        /// Gets the network statistics for the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/> representing the network statistics.</returns>
        public IServerNetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Finds a secure session by its identifier.
        /// </summary>
        /// <param name="id">The session identifier.</param>
        /// <returns>The <see cref="SslSession"/> if found; otherwise, null.</returns>
        public SslSession GetSession(string id) => this.Sessions.TryGetValue(id, out var session) ? session : null;

        /// <summary>
        /// Maps an SSL session with the specified identifier to the current server instance.
        /// </summary>
        /// <param name="id">The identifier of the SSL session to be mapped.</param>
        /// <param name="session">The SSL session to be mapped.</param>
        internal void MapSession(string id, SslSession session) => this.Sessions.TryAdd(id, session);

        /// <summary>
        /// Removes a session from the current SSL server instance.
        /// </summary>
        /// <param name="id">The identifier of the session to be removed.</param>
        internal void RemoveSession(string id) => this.Sessions.TryRemove(id, out SslSession _);

        /// <summary>
        /// Starts an asynchronous accept operation for incoming connections.
        /// </summary>
        /// <param name="e">The SocketAsyncEventArgs object representing the asynchronous operation.</param>
        private void StartAcceptInternal(SocketAsyncEventArgs e)
        {
            e.AcceptSocket = null;

            if (!this.acceptorSocket.AcceptAsync(e))
                this.ProcessAcceptInternal(e);
        }

        /// <summary>
        /// Processes the result of an asynchronous accept operation.
        /// </summary>
        /// <param name="e">The SocketAsyncEventArgs object representing the completed operation.</param>
        private void ProcessAcceptInternal(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                var session = this.CreateSession();

                this.MapSession(this.Id, session);

                session.ConnectInternal(e.AcceptSocket);
            }
            else
                this.SendErrorInternal(e.SocketError);

            if (this.IsAccepting)
                this.StartAcceptInternal(e);
        }

        /// <summary>
        /// Handles the completion of an asynchronous socket operation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The SocketAsyncEventArgs object representing the completed operation.</param>
        private void OnAsyncCompletedInternal(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            this.ProcessAcceptInternal(e);
        }

        /// <summary>
        /// Creates a new secure session. Override this method to provide custom session behavior.
        /// </summary>
        /// <returns>A new instance of <see cref="SslSession"/>.</returns>
        protected virtual SslSession CreateSession() => new SslSession(this);

        /// <summary>
        /// Called when the server is starting. Override this method to add custom logic during server startup.
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Called after the server has started successfully. Override this method to add custom logic after server startup.
        /// </summary>
        protected virtual void OnStarted() { }

        /// <summary>
        /// Called when the server is stopping. Override this method to add custom logic during server shutdown.
        /// </summary>
        protected virtual void OnStopping() { }

        /// <summary>
        /// Called after the server has stopped successfully. Override this method to add custom logic after server shutdown.
        /// </summary>
        protected virtual void OnStopped() { }

        /// <summary>
        /// Called when a session is connecting. Override this method to handle session-specific logic during the connection process.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        protected virtual void OnConnecting(SslSession session) { }

        /// <summary>
        /// Called after a session has connected successfully. Override this method to handle session-specific logic after the connection process.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        protected virtual void OnConnected(SslSession session) { }

        /// <summary>
        /// Called when a session is undergoing the SSL/TLS handshake process. Override this method to handle session-specific logic during the handshake process.
        /// </summary>
        /// <param name="session">The session that is handshaking.</param>
        protected virtual void OnHandshaking(SslSession session) { }

        /// <summary>
        /// Called after a session has successfully completed the SSL/TLS handshake. Override this method to handle session-specific logic after the handshake process.
        /// </summary>
        /// <param name="session">The session that has completed the handshake.</param>
        protected virtual void OnHandshaked(SslSession session) { }

        /// <summary>
        /// Called when a session is disconnecting. Override this method to handle session-specific logic during the disconnection process.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        protected virtual void OnDisconnecting(SslSession session) { }

        /// <summary>
        /// Called after a session has disconnected successfully. Override this method to handle session-specific logic after the disconnection process.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        protected virtual void OnDisconnected(SslSession session) { }

        /// <summary>
        /// Called when a socket error occurs. Override this method to handle errors in a custom way.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Triggers the OnConnecting method for the specified session.
        /// </summary>
        /// <param name="session">The session that is connecting.</param>
        internal void OnConnectingInternal(SslSession session) => this.OnConnecting(session);

        /// <summary>
        /// Triggers the OnConnected method for the specified session.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        internal void OnConnectedInternal(SslSession session) => this.OnConnected(session);

        /// <summary>
        /// Triggers the OnHandshaking method for the specified session.
        /// </summary>
        /// <param name="session">The session that is handshaking.</param>
        internal void OnHandshakingInternal(SslSession session) => this.OnHandshaking(session);

        /// <summary>
        /// Triggers the OnHandshaked method for the specified session.
        /// </summary>
        /// <param name="session">The session that has completed the handshake.</param>
        internal void OnHandshakedInternal(SslSession session) => this.OnHandshaked(session);

        /// <summary>
        /// Triggers the OnDisconnecting method for the specified session.
        /// </summary>
        /// <param name="session">The session that is disconnecting.</param>
        internal void OnDisconnectingInternal(SslSession session) => this.OnDisconnecting(session);

        /// <summary>
        /// Triggers the OnDisconnected method for the specified session.
        /// </summary>
        /// <param name="session">The session that has disconnected.</param>
        internal void OnDisconnectedInternal(SslSession session) => this.OnDisconnected(session);

        /// <summary>
        /// Handles socket errors by triggering the OnError method.
        /// Ignores specific disconnect errors based on utility logic.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }


        /// <summary>
        /// Disposes the server and its resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the server, optionally releasing managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release managed resources; otherwise, false.</param>
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

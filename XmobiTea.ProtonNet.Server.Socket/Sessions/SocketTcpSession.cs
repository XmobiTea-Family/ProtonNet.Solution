using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNet.Server.Socket.Models;
using XmobiTea.ProtonNet.Server.Socket.Server;
using XmobiTea.ProtonNet.Server.Socket.Services;
using XmobiTea.ProtonNet.Server.Socket.Types;
using XmobiTea.ProtonNetServer;
using XmobiTea.Threading;
using XmobiTea.Threading.Models;

namespace XmobiTea.ProtonNet.Server.Socket.Sessions
{
    /// <summary>
    /// Represents a TCP session for handling client connections, sending and receiving data,
    /// and managing session-specific state.
    /// </summary>
    class SocketTcpSession : TcpSession, ISocketSession
    {
        /// <summary>
        /// Gets the controller service responsible for handling various socket events.
        /// </summary>
        protected ISocketControllerService controllerService { get; }

        /// <summary>
        /// Gets the session time management service.
        /// </summary>
        protected ISocketSessionTime sessionTime { get; }

        private int connectionId { get; }
        private string serverSessionId { get; }
        private byte[] encryptKey { get; set; }
        private string sessionId { get; set; }
        private IFiber fiber { get; }

        private IScheduleTask disconnectTask { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketTcpSession"/> class using the specified server and context.
        /// </summary>
        /// <param name="server">The TCP server associated with this session.</param>
        /// <param name="context">The server context providing necessary services.</param>
        public SocketTcpSession(SocketTcpServer server, ISocketServerContext context) : base(server)
        {
            this.controllerService = context.GetControllerService();

            var initRequest = context.GetInitRequestProviderService().NewSessionInitRequest();

            this.connectionId = initRequest.ConnectionId;
            this.serverSessionId = initRequest.ServerSessionId;
            this.sessionId = initRequest.SessionId;
            this.fiber = initRequest.Fiber;

            this.sessionTime = new SocketSessionTime();
        }

        /// <summary>
        /// Gets the transport protocol used by this session.
        /// </summary>
        /// <returns>The transport protocol.</returns>
        public TransportProtocol GetTransportProtocol() => TransportProtocol.Tcp;

        /// <summary>
        /// Determines whether the session is currently connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        public new bool IsConnected() => base.IsConnected;

        /// <summary>
        /// Gets the unique connection ID for this session.
        /// </summary>
        /// <returns>The connection ID.</returns>
        public int GetConnectionId() => this.connectionId;

        /// <summary>
        /// Gets the server-side session ID.
        /// </summary>
        /// <returns>The server session ID.</returns>
        public string GetServerSessionId() => this.serverSessionId;

        /// <summary>
        /// Gets the encryption key used by this session.
        /// </summary>
        /// <returns>The encryption key.</returns>
        public byte[] GetEncryptKey() => this.encryptKey;

        /// <summary>
        /// Gets the client-side session ID.
        /// </summary>
        /// <returns>The session ID.</returns>
        public string GetSessionId() => this.sessionId;

        /// <summary>
        /// Gets the remote IP address of the connected client.
        /// </summary>
        /// <returns>The remote IP address.</returns>
        public string GetRemoteIP() => this.Socket == null ? string.Empty : (this.Socket.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString();

        /// <summary>
        /// Gets the remote port number of the connected client.
        /// </summary>
        /// <returns>The remote port number.</returns>
        public int GetRemotePort() => this.Socket == null ? -1 : (this.Socket.RemoteEndPoint as System.Net.IPEndPoint).Port;

        /// <summary>
        /// Sets the client-side session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        public void SetSessionId(string sessionId) => this.sessionId = sessionId;

        /// <summary>
        /// Sets the encryption key used by this session.
        /// </summary>
        /// <param name="encryptKey">The encryption key.</param>
        public void SetEncryptKey(byte[] encryptKey) => this.encryptKey = encryptKey;

        /// <summary>
        /// Gets the fiber associated with this session for scheduling tasks.
        /// </summary>
        /// <returns>The fiber.</returns>
        public IFiber GetFiber() => this.fiber;

        /// <summary>
        /// Gets the session time management service.
        /// </summary>
        /// <returns>The session time management service.</returns>
        public ISocketSessionTime GetSessionTime() => this.sessionTime;

        /// <summary>
        /// Handles the event when the TCP connection is established.
        /// </summary>
        protected override void OnConnected()
        {
            base.OnConnected();

            this.controllerService.OnConnected(this);
        }

        /// <summary>
        /// Handles the event when data is received over the TCP connection.
        /// </summary>
        /// <param name="buffer">The data buffer.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data.</param>
        protected override void OnReceived(byte[] buffer, int position, int length)
        {
            base.OnReceived(buffer, position, length);

            this.controllerService.OnReceived(this, buffer, position, length);
        }

        /// <summary>
        /// Handles socket errors that occur during the session.
        /// </summary>
        /// <param name="error">The socket error.</param>
        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            base.OnError(error);

            this.controllerService.OnError(this, error);
        }

        /// <summary>
        /// Handles the event when the TCP connection is disconnected.
        /// </summary>
        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            this.controllerService.OnDisconnected(this);
        }

        /// <summary>
        /// Disconnects the session after a specified delay.
        /// </summary>
        /// <param name="afterMilliseconds">The delay in milliseconds before disconnecting.</param>
        /// <returns>True if the session was scheduled to disconnect, otherwise false.</returns>
        public bool Disconnect(int afterMilliseconds)
        {
            if (afterMilliseconds <= 0) return this.Disconnect();
            else
            {
                if (this.disconnectTask == null)
                {
                    this.disconnectTask = this.fiber?.Schedule(() =>
                    {
                        if (this.IsConnected()) this.Disconnect();
                    }, afterMilliseconds);
                }

                return true;
            }
        }

        /// <summary>
        /// Disposes the session, releasing any managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">Indicates whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposingManagedResources)
        {
            base.Dispose(disposingManagedResources);

            if (disposingManagedResources)
            {
                if (this.fiber != null && this.fiber is IFiberControl fiberControl) fiberControl.Dispose();

                this.disconnectTask?.Dispose();
            }
        }

    }

}

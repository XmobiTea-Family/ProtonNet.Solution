using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNetServer
{
    /// <summary>
    /// Represents an interface for TCP session operations.
    /// </summary>
    public interface ITcpSession : ISession
    {

    }

    /// <summary>
    /// Provides the implementation for a TCP session, handling connection, data transmission, and disconnection.
    /// </summary>
    public class TcpSession : ITcpSession, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this session.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the server that owns this session.
        /// </summary>
        public TcpServer Server { get; }

        /// <summary>
        /// Gets the socket associated with this session.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the session is currently connected.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the session has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the socket has been disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        private IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Gets the options for the TCP server.
        /// </summary>
        public TcpServerOptions Options { get; }

        /// <summary>
        /// An object used to synchronize access to the send buffer.
        /// </summary>
        private object sendLock { get; }

        /// <summary>
        /// The buffer used to store received data.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// The buffer used to store data to be sent.
        /// </summary>
        private IMemoryBuffer sendBuffer;

        /// <summary>
        /// The buffer used to temporarily store data during the flushing process.
        /// </summary>
        private IMemoryBuffer sendBufferFlush;

        /// <summary>
        /// The offset position in the send buffer during the flush process.
        /// </summary>
        private int sendBufferFlushOffset { get; set; }

        /// <summary>
        /// Indicates whether data is currently being received.
        /// </summary>
        private bool receiving { get; set; }

        /// <summary>
        /// The event argument used for asynchronous receive operations.
        /// </summary>
        private SocketAsyncEventArgs receiveEventArg { get; set; }

        /// <summary>
        /// Indicates whether data is currently being sent.
        /// </summary>
        private bool sending { get; set; }

        /// <summary>
        /// The event argument used for asynchronous send operations.
        /// </summary>
        private SocketAsyncEventArgs sendEventArg { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpSession"/> class.
        /// </summary>
        /// <param name="server">The server that owns this session.</param>
        public TcpSession(TcpServer server)
        {
            this.Id = this.CreateRandomId();

            this.Server = server;
            this.Options = server.Options;

            this.sendLock = new object();
            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();
            this.sendBufferFlush = new MemoryBuffer();

            this.networkStatistics = new ChangeNetworkStatistics();

            this.IsSocketDisposed = true;
        }

        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the network statistics for this session.
        /// </summary>
        /// <returns>The network statistics.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        private void SetSocketOption()
        {
            if (this.Options.KeepAlive)
                this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
#if NETCOREAPP
            if (this.Options.TcpKeepAliveTime > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, this.Options.TcpKeepAliveTime);
            if (this.Options.TcpKeepAliveInterval > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, this.Options.TcpKeepAliveInterval);
            if (this.Options.TcpKeepAliveRetryCount > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, this.Options.TcpKeepAliveRetryCount);
#endif
            if (this.Options.NoDelay)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        }

        private void ReserveBuffer()
        {
            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);
            this.sendBuffer.Reserve(this.Options.SendBufferCapacity);
            this.sendBufferFlush.Reserve(this.Options.SendBufferCapacity);
        }

        /// <summary>
        /// Internal method to establish a connection using the provided socket.
        /// </summary>
        /// <param name="socket">The socket to connect with.</param>
        internal void ConnectInternal(Socket socket)
        {
            this.Socket = socket;

            this.IsSocketDisposed = false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.receiveEventArg = new SocketAsyncEventArgs();
            this.sendEventArg = new SocketAsyncEventArgs();

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.receiveEventArg.Completed += this.OnAsyncCompleted;

            this.sendEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed += this.OnAsyncCompleted;

            this.SetSocketOption();

            this.ReserveBuffer();

            this.networkStatistics.Reset();

            this.OnConnecting();

            this.Server.OnConnectingInternal(this);

            if (this.IsSocketDisposed)
                return;

            this.IsConnected = true;

            this.OnConnected();

            this.Server.OnConnectedInternal(this);

            this.TryReceive();

            if (this.sendBuffer.IsEmpty())
                this.OnEmpty();
        }

        /// <summary>
        /// Disconnects the session.
        /// </summary>
        /// <returns>True if the session was disconnected successfully; otherwise, false.</returns>
        public virtual bool Disconnect()
        {
            if (!this.IsConnected)
                return false;

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed -= this.OnAsyncCompleted;

            this.OnDisconnecting();

            this.Server.OnDisconnectingInternal(this);

            try
            {
                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException) { }

                this.Socket.Close();
                this.Socket.Dispose();

                this.receiveEventArg.Dispose();
                this.sendEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.IsConnected = false;

            this.receiving = false;
            this.sending = false;

            this.ClearBuffers();

            this.OnDisconnected();

            this.Server.OnDisconnectedInternal(this);

            this.Server.RemoveSession(this.Id);

            return true;
        }

        /// <summary>
        /// Sends data synchronously.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data synchronously with the specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return 0;

            if (length == 0)
                return 0;

            var sent = this.Socket.Send(buffer, position, length, SocketFlags.None, out SocketError ec);

            if (sent > 0)
            {
                this.networkStatistics.ChangeBytesSent(sent);
                this.networkStatistics.IncPacketSent();

                this.Server.networkStatistics.ChangeBytesSent(sent);
                this.Server.networkStatistics.IncPacketSent();

                this.OnSent(sent, (int)(this.networkStatistics.GetBytesPending() + this.networkStatistics.GetBytesSending()));
            }

            if (ec != SocketError.Success)
            {
                this.SendErrorInternal(ec);
                this.Disconnect();
            }

            return sent;
        }

        /// <summary>
        /// Sends data asynchronously.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer) => this.SendAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data asynchronously with the specified position and length.
        /// </summary>
        /// <param name="buffer">The buffer containing the data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return false;

            if (length == 0)
                return true;

            lock (this.sendLock)
            {
                if (this.sendBuffer.Length + length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
                {
                    this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                    return false;
                }

                this.sendBuffer.Write(buffer, position, length);

                if (this.sending)
                    return true;
                else
                    this.sending = true;

                this.TrySend();
            }

            return true;
        }

        /// <summary>
        /// Attempts to receive data asynchronously. 
        /// If already receiving or not connected, the method exits early.
        /// </summary>
        private void TryReceive()
        {
            if (this.receiving)
                return;

            if (!this.IsConnected)
                return;

            var process = true;

            while (process)
            {
                process = false;

                try
                {
                    this.receiving = true;
                    this.receiveEventArg.SetBuffer(this.receiveBuffer.Buffer, 0, this.receiveBuffer.Capacity);
                    if (!this.Socket.ReceiveAsync(this.receiveEventArg))
                        process = this.ProcessReceive(this.receiveEventArg);
                }
                catch (ObjectDisposedException) { }
            }
        }

        /// <summary>
        /// Attempts to send data asynchronously. 
        /// If already sending or not connected, the method exits early.
        /// </summary>
        private void TrySend()
        {
            if (!this.IsConnected)
                return;

            var empty = false;
            var process = true;

            while (process)
            {
                process = false;

                lock (this.sendLock)
                {
                    if (this.sendBufferFlush.IsEmpty())
                    {
                        this.sendBufferFlush = Interlocked.Exchange(ref this.sendBuffer, this.sendBufferFlush);
                        this.sendBufferFlushOffset = 0;

                        this.networkStatistics.SetBytesSending(this.sendBufferFlush.Length);

                        if (this.sendBufferFlush.IsEmpty())
                        {
                            empty = true;

                            this.sending = false;
                        }
                    }
                    else
                        return;
                }

                if (empty)
                {
                    this.OnEmpty();
                    return;
                }

                try
                {
                    this.sendEventArg.SetBuffer(this.sendBufferFlush.Buffer, this.sendBufferFlushOffset, this.sendBufferFlush.Length - this.sendBufferFlushOffset);
                    if (!this.Socket.SendAsync(this.sendEventArg))
                        process = this.ProcessSend(this.sendEventArg);
                }
                catch (ObjectDisposedException) { }
            }
        }

        /// <summary>
        /// Clears the send buffers and resets network statistics related to sending.
        /// </summary>
        private void ClearBuffers()
        {
            lock (this.sendLock)
            {
                this.sendBuffer.Clear();
                this.sendBufferFlush.Clear();
                this.sendBufferFlushOffset = 0;

                this.networkStatistics.SetBytesPending(0);
                this.networkStatistics.SetBytesSending(0);
            }
        }

        /// <summary>
        /// Handles the completion of an asynchronous socket operation, 
        /// determining whether to continue receiving or sending data based on the last operation.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing details of the completed operation.</param>
        private void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    if (this.ProcessReceive(e))
                        this.TryReceive();
                    break;
                case SocketAsyncOperation.Send:
                    if (this.ProcessSend(e))
                        this.TrySend();
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }

        /// <summary>
        /// Processes the data received from a socket operation, updating network statistics 
        /// and checking for errors or buffer limitations.
        /// </summary>
        /// <param name="e">The event arguments containing details of the completed operation.</param>
        /// <returns>Returns true if more data needs to be received, otherwise false.</returns>
        private bool ProcessReceive(SocketAsyncEventArgs e)
        {
            if (!this.IsConnected)
                return false;

            var length = e.BytesTransferred;

            if (length > 0)
            {
                this.networkStatistics.ChangeBytesReceived(length);
                this.networkStatistics.IncPacketReceived();

                this.Server.networkStatistics.ChangeBytesReceived(length);
                this.Server.networkStatistics.IncPacketReceived();

                this.OnReceived(this.receiveBuffer.Buffer, 0, length);

                if (this.receiveBuffer.Capacity == length)
                {
                    if (length * 2 > this.Options.ReceiveBufferLimit && this.Options.ReceiveBufferLimit > 0)
                    {
                        this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                        this.Disconnect();
                        return false;
                    }

                    this.receiveBuffer.Reserve(length * 2);
                }
            }

            this.receiving = false;

            if (e.SocketError == SocketError.Success)
            {
                if (length > 0)
                    return true;
                else
                    this.Disconnect();
            }
            else
            {
                this.SendErrorInternal(e.SocketError);
                this.Disconnect();
            }

            return false;
        }

        /// <summary>
        /// Processes the data sent from a socket operation, updating network statistics 
        /// and checking for errors or if more data needs to be sent.
        /// </summary>
        /// <param name="e">The event arguments containing details of the completed operation.</param>
        /// <returns>Returns true if more data needs to be sent, otherwise false.</returns>
        private bool ProcessSend(SocketAsyncEventArgs e)
        {
            if (!this.IsConnected)
                return false;

            var length = e.BytesTransferred;

            if (length > 0)
            {
                this.networkStatistics.ChangeBytesSending(-length);
                this.networkStatistics.ChangeBytesSent(length);
                this.networkStatistics.IncPacketSent();

                this.Server.networkStatistics.ChangeBytesSent(length);
                this.Server.networkStatistics.IncPacketSent();

                this.sendBufferFlushOffset += length;

                if (this.sendBufferFlushOffset == this.sendBufferFlush.Length)
                {
                    this.sendBufferFlush.Clear();
                    this.sendBufferFlushOffset = 0;
                }

                this.OnSent(length, (int)(this.networkStatistics.GetBytesPending() + this.networkStatistics.GetBytesSending()));
            }

            if (e.SocketError == SocketError.Success)
                return true;
            else
            {
                this.SendErrorInternal(e.SocketError);
                this.Disconnect();
                return false;
            }
        }

        /// <summary>
        /// Invoked when the session is in the process of connecting.
        /// </summary>
        protected virtual void OnConnecting() { }

        /// <summary>
        /// Invoked when the session has successfully connected.
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Invoked when the session is in the process of disconnecting.
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Invoked when the session has successfully disconnected.
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Invoked when data is received from the remote endpoint.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the received data starts.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Invoked when data is successfully sent to the remote endpoint.
        /// </summary>
        /// <param name="sent">The number of bytes sent.</param>
        /// <param name="pending">The number of bytes pending in the send buffer.</param>
        protected virtual void OnSent(int sent, int pending) { }

        /// <summary>
        /// Invoked when the send buffer is empty.
        /// </summary>
        protected virtual void OnEmpty() { }

        /// <summary>
        /// Invoked when an error occurs during the session operation.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Handles socket errors by invoking the error handling method, unless the error is one that should be ignored.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="TcpSession"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="TcpSession"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.Disconnect();
                }

                this.IsDisposed = true;
            }
        }
    }

    /// <summary>
    /// Represents a secure SSL session with the ability to manage connection,
    /// data transmission, and handshake processes.
    /// </summary>
    public class SslSession : ITcpSession, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the session has completed the SSL handshake.
        /// </summary>
        public bool IsHandshaked { get; private set; }

        private SslStream sslStream { get; set; }
        private string sslStreamId { get; set; }

        /// <summary>
        /// Gets the unique identifier for this session.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the associated server instance for this session.
        /// </summary>
        public SslServer Server { get; }

        /// <summary>
        /// Gets the underlying socket for the session.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the session is currently connected.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Indicates whether this session has been disposed.
        /// This flag helps ensure that resources are not accessed after disposal.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Indicates whether the underlying socket has been disposed.
        /// This flag is important for preventing operations on a closed socket.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Manages the statistics related to network performance for this session.
        /// Includes tracking bytes sent/received, packets sent/received, etc.
        /// </summary>
        private IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Contains various configuration options for the TCP server.
        /// This includes settings like buffer sizes, keep-alive time, and more.
        /// </summary>
        public TcpServerOptions Options { get; }

        /// <summary>
        /// Object used for synchronizing access to the send buffer.
        /// Ensures thread safety during send operations.
        /// </summary>
        private object sendLock { get; }

        /// <summary>
        /// Buffer for storing received data before it is processed.
        /// The buffer size is determined by the options provided.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// Buffer for storing data that is to be sent.
        /// This buffer accumulates data until it can be flushed out.
        /// </summary>
        private IMemoryBuffer sendBuffer;

        /// <summary>
        /// Buffer for storing data that is currently being flushed out.
        /// This buffer is swapped with the send buffer during send operations.
        /// </summary>
        private IMemoryBuffer sendBufferFlush;

        /// <summary>
        /// Flag indicating whether a receive operation is currently in progress.
        /// Used to prevent overlapping receive operations.
        /// </summary>
        private bool receiving { get; set; }

        /// <summary>
        /// Flag indicating whether a send operation is currently in progress.
        /// Used to prevent overlapping send operations.
        /// </summary>
        private bool sending { get; set; }

        /// <summary>
        /// Offset for tracking the current position within the send buffer during a flush.
        /// This helps manage partial sends.
        /// </summary>
        private int sendBufferFlushOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SslSession"/> class.
        /// </summary>
        /// <param name="server">The SSL server associated with this session.</param>
        public SslSession(SslServer server)
        {
            this.Id = this.CreateRandomId();

            this.Server = server;
            this.Options = server.Options;

            this.sendLock = new object();
            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();
            this.sendBufferFlush = new MemoryBuffer();

            this.IsSocketDisposed = true;
        }

        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the network statistics for this session.
        /// </summary>
        /// <returns>The network statistics object.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Configures the socket options based on the current server settings.
        /// This includes enabling keep-alive, setting TCP keep-alive parameters, and disabling Nagle's algorithm if specified.
        /// </summary>
        private void SetSocketOption()
        {
            if (this.Options.KeepAlive)
                this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
#if NETCOREAPP
            if (this.Options.TcpKeepAliveTime > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, this.Options.TcpKeepAliveTime);
            if (this.Options.TcpKeepAliveInterval > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, this.Options.TcpKeepAliveInterval);
            if (this.Options.TcpKeepAliveRetryCount > 0)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, this.Options.TcpKeepAliveRetryCount);
#endif
            if (this.Options.NoDelay)
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        }

        /// <summary>
        /// Reserves the necessary buffer sizes for receiving and sending data.
        /// This is based on the configuration options provided by the server.
        /// </summary>
        private void ReserveBuffer()
        {
            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);
            this.sendBuffer.Reserve(this.Options.SendBufferCapacity);
            this.sendBufferFlush.Reserve(this.Options.SendBufferCapacity);
        }

        /// <summary>
        /// Initializes the session with a given socket, setting up the SSL stream and starting the handshake process.
        /// This method also prepares the buffers and resets the network statistics.
        /// </summary>
        /// <param name="socket">The socket to associate with this session.</param>
        internal void ConnectInternal(Socket socket)
        {
            this.Socket = socket;

            this.IsSocketDisposed = false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.SetSocketOption();

            this.ReserveBuffer();

            this.networkStatistics.Reset();

            this.OnConnecting();

            this.Server.OnConnectingInternal(this);

            this.IsConnected = true;

            this.OnConnected();

            this.Server.OnConnectedInternal(this);

            try
            {
                this.sslStreamId = this.CreateRandomId();
                this.sslStream = new SslStream(new NetworkStream(this.Socket, false), false);

                this.OnHandshaking();

                this.Server.OnHandshakingInternal(this);

                this.sslStream.BeginAuthenticateAsServer(
                    this.Server.SslOptions.X509Certificate,
                    false,
                    this.Server.SslOptions.SslProtocols,
                    false,
                    this.ProcessHandshake,
                    this.sslStreamId);
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.NotConnected);
                this.Disconnect();
            }
        }

        /// <summary>
        /// Disconnects the session, releasing all resources and closing the connection.
        /// </summary>
        /// <returns>True if disconnection was successful; otherwise, false.</returns>
        public virtual bool Disconnect()
        {
            if (!this.IsConnected)
                return false;

            this.OnDisconnecting();

            this.Server.OnDisconnectingInternal(this);

            try
            {
#if NETCOREAPP
                try
                {
                    this.sslStream.ShutdownAsync().Wait();
                }
                catch (Exception) { }
#endif

                this.sslStream.Dispose();
                this.sslStreamId = null;

                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException) { }

                this.Socket.Close();
                this.Socket.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.IsHandshaked = false;

            this.IsConnected = false;

            this.receiving = false;
            this.sending = false;

            this.ClearBuffers();

            this.OnDisconnected();

            this.Server.OnDisconnectedInternal(this);

            this.Server.RemoveSession(this.Id);

            return true;
        }

        /// <summary>
        /// Sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The position in the buffer at which to start sending data.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer, int position, int length)
        {
            if (!this.IsHandshaked)
                return 0;

            if (length == 0)
                return 0;

            try
            {
                this.sslStream.Write(buffer, position, length);

                var sent = length;

                this.networkStatistics.ChangeBytesSent(sent);
                this.networkStatistics.IncPacketSent();

                this.Server.networkStatistics.ChangeBytesSent(sent);
                this.Server.networkStatistics.IncPacketSent();

                this.OnSent(sent, (int)(this.networkStatistics.GetBytesPending() + this.networkStatistics.GetBytesSending()));

                return sent;
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.Disconnect();

                return 0;
            }
        }

        /// <summary>
        /// Asynchronously sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the send operation is successfully initiated; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer) => this.SendAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Asynchronously sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <param name="position">The position in the buffer at which to start sending data.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the send operation is successfully initiated; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsHandshaked)
                return false;

            if (length == 0)
                return true;

            lock (this.sendLock)
            {
                if (this.sendBuffer.Length + length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
                {
                    this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                    return false;
                }

                this.sendBuffer.Write(buffer, position, length);

                this.networkStatistics.SetBytesPending(this.sendBuffer.Length);

                if (this.sending)
                    return true;
                else
                    this.sending = true;

                this.TrySend();
            }

            return true;
        }

        /// <summary>
        /// Attempts to receive data asynchronously from the SSL stream.
        /// This method ensures that only one receive operation is active at a time.
        /// </summary>
        private void TryReceive()
        {
            if (this.receiving)
                return;

            if (!this.IsHandshaked)
                return;

            try
            {
                IAsyncResult result;
                do
                {
                    if (!this.IsHandshaked)
                        return;

                    this.receiving = true;
                    result = this.sslStream.BeginRead(this.receiveBuffer.Buffer, 0, this.receiveBuffer.Capacity, this.ProcessReceive, this.sslStreamId);
                } while (result.CompletedSynchronously);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Attempts to send data asynchronously using the SSL stream.
        /// This method ensures that only one send operation is active at a time.
        /// </summary>
        private void TrySend()
        {
            if (!this.IsHandshaked)
                return;

            var empty = false;

            lock (this.sendLock)
            {
                if (this.sendBufferFlush.IsEmpty())
                {
                    this.sendBufferFlush = Interlocked.Exchange(ref this.sendBuffer, this.sendBufferFlush);
                    this.sendBufferFlushOffset = 0;

                    this.networkStatistics.SetBytesSending(this.sendBufferFlush.Length);

                    if (this.sendBufferFlush.IsEmpty())
                    {
                        empty = true;

                        this.sending = false;
                    }
                }
                else
                    return;
            }

            if (empty)
            {
                this.OnEmpty();
                return;
            }

            try
            {
                this.sslStream.BeginWrite(this.sendBufferFlush.Buffer, this.sendBufferFlushOffset, this.sendBufferFlush.Length - this.sendBufferFlushOffset, this.ProcessSend, this.sslStreamId);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Clears the buffers used for sending data, resetting all pending data.
        /// </summary>
        private void ClearBuffers()
        {
            lock (this.sendLock)
            {
                this.sendBuffer.Clear();
                this.sendBufferFlush.Clear();
                this.sendBufferFlushOffset = 0;

                this.networkStatistics.SetBytesPending(0);
                this.networkStatistics.SetBytesSending(0);
            }
        }

        /// <summary>
        /// Processes the result of the SSL handshake operation, finalizing the handshake
        /// and triggering the appropriate events.
        /// </summary>
        /// <param name="result">The asynchronous result of the handshake operation.</param>
        private void ProcessHandshake(IAsyncResult result)
        {
            try
            {
                if (this.IsHandshaked)
                    return;

                var sslStreamId = result.AsyncState as string;
                if (this.sslStreamId != sslStreamId)
                    return;

                this.sslStream.EndAuthenticateAsServer(result);

                this.IsHandshaked = true;

                if (this.IsSocketDisposed)
                    return;

                this.OnHandshaked();

                this.Server.OnHandshakedInternal(this);

                this.TryReceive();

                if (this.sendBuffer.IsEmpty())
                    this.OnEmpty();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.NotConnected);
                this.Disconnect();
            }
        }

        /// <summary>
        /// Processes the result of a data receive operation from the SSL stream.
        /// This method handles the received data and triggers the appropriate events.
        /// </summary>
        /// <param name="result">The asynchronous result of the receive operation.</param>
        private void ProcessReceive(IAsyncResult result)
        {
            try
            {
                if (!this.IsHandshaked)
                    return;

                var sslStreamId = result.AsyncState as string;
                if (this.sslStreamId != sslStreamId)
                    return;

                var length = this.sslStream.EndRead(result);

                if (length > 0)
                {
                    this.networkStatistics.ChangeBytesReceived(length);
                    this.networkStatistics.IncPacketReceived();

                    this.Server.networkStatistics.ChangeBytesReceived(length);
                    this.Server.networkStatistics.IncPacketReceived();

                    this.OnReceived(this.receiveBuffer.Buffer, 0, length);

                    if (this.receiveBuffer.Capacity == length)
                    {
                        if (length * 2 > this.Options.ReceiveBufferLimit && this.Options.ReceiveBufferLimit > 0)
                        {
                            this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                            this.Disconnect();
                            return;
                        }

                        this.receiveBuffer.Reserve(length * 2);
                    }
                }

                this.receiving = false;

                if (length > 0)
                {
                    if (!result.CompletedSynchronously)
                        this.TryReceive();
                }
                else
                    this.Disconnect();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.Disconnect();
            }
        }

        /// <summary>
        /// Processes the result of a data send operation through the SSL stream.
        /// This method handles the completion of the send operation and triggers the appropriate events.
        /// </summary>
        /// <param name="result">The asynchronous result of the send operation.</param>
        private void ProcessSend(IAsyncResult result)
        {
            try
            {
                var sslStreamId = result.AsyncState as string;
                if (this.sslStreamId != sslStreamId)
                    return;

                if (!this.IsHandshaked)
                    return;

                this.sslStream.EndWrite(result);

                var length = this.sendBufferFlush.Length;

                if (length > 0)
                {
                    this.networkStatistics.ChangeBytesSending(-length);
                    this.networkStatistics.ChangeBytesSent(length);
                    this.networkStatistics.IncPacketSent();

                    this.Server.networkStatistics.ChangeBytesSent(length);
                    this.Server.networkStatistics.IncPacketSent();

                    this.sendBufferFlushOffset += length;

                    if (this.sendBufferFlushOffset == this.sendBufferFlush.Length)
                    {
                        this.sendBufferFlush.Clear();
                        this.sendBufferFlushOffset = 0;
                    }

                    this.OnSent(length, (int)(this.networkStatistics.GetBytesPending() + this.networkStatistics.GetBytesSending()));
                }

                this.TrySend();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.Disconnect();
            }
        }

        /// <summary>
        /// Event triggered when the session is connecting.
        /// </summary>
        protected virtual void OnConnecting() { }

        /// <summary>
        /// Event triggered when the session is connected.
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Event triggered when the SSL handshake process begins.
        /// </summary>
        protected virtual void OnHandshaking() { }

        /// <summary>
        /// Event triggered when the SSL handshake process is completed.
        /// </summary>
        protected virtual void OnHandshaked() { }

        /// <summary>
        /// Event triggered when the session is disconnecting.
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Event triggered when the session is disconnected.
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Event triggered when data is received from the client.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The number of bytes received.</param>
        protected virtual void OnReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Event triggered when data is sent to the client.
        /// </summary>
        /// <param name="sent">The number of bytes sent.</param>
        /// <param name="pending">The number of bytes pending to be sent.</param>
        protected virtual void OnSent(int sent, int pending) { }

        /// <summary>
        /// Event triggered when the send buffer is empty.
        /// </summary>
        protected virtual void OnEmpty() { }

        /// <summary>
        /// Event triggered when an error occurs in the session.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Handles socket errors by invoking the error handling method, unless the error is one that should be ignored.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="SslSession"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and, optionally, managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.Disconnect();
                }

                this.IsDisposed = true;
            }
        }

    }

}

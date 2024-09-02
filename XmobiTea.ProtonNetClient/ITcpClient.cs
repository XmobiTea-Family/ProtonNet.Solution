using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;

namespace XmobiTea.ProtonNetClient
{
    /// <summary>
    /// Represents the interface for a TCP client that can connect, 
    /// disconnect, and send data over a network.
    /// </summary>
    public interface ITcpClient : IClient
    {
    }

    /// <summary>
    /// Implements a TCP client for connecting to a server, sending, 
    /// and receiving data. Provides both synchronous and asynchronous operations.
    /// </summary>
    public class TcpClient : ITcpClient, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this TCP client instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the server address to which the client is connected.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port number on the server to which the client is connected.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the endpoint representing the server's address and port.
        /// </summary>
        public EndPoint EndPoint { get; }

        /// <summary>
        /// Gets the underlying socket used for the connection.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets the options used to configure the TCP client.
        /// </summary>
        public TcpClientOptions Options { get; }

        /// <summary>
        /// Gets a value indicating whether the client is currently connecting.
        /// </summary>
        public bool IsConnecting { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the client is connected to the server.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the client has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the socket has been disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Interface for tracking and updating network statistics.
        /// </summary>
        private IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// An object used for synchronizing access to the send buffer.
        /// </summary>
        private object sendLock { get; }

        /// <summary>
        /// The buffer used for storing received data.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// The buffer used for storing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBuffer;

        /// <summary>
        /// The buffer used for flushing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBufferFlush;

        /// <summary>
        /// The offset in the send buffer flush process.
        /// </summary>
        private int sendBufferFlushOffset { get; set; }

        /// <summary>
        /// The event argument used for asynchronous connect operations.
        /// </summary>
        private SocketAsyncEventArgs connectEventArg { get; set; }

        /// <summary>
        /// Indicates whether the client is currently receiving data.
        /// </summary>
        private bool receiving { get; set; }

        /// <summary>
        /// The event argument used for asynchronous receive operations.
        /// </summary>
        private SocketAsyncEventArgs receiveEventArg { get; set; }

        /// <summary>
        /// Indicates whether the client is currently sending data.
        /// </summary>
        private bool sending { get; set; }

        /// <summary>
        /// The event argument used for asynchronous send operations.
        /// </summary>
        private SocketAsyncEventArgs sendEventArg { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        public TcpClient(string address, int port, TcpClientOptions options)
        {
            this.Id = this.CreateRandomId();

            this.Address = address;
            this.Port = port;

            this.Options = options;

            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.sendLock = new object();
            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();
            this.sendBufferFlush = new MemoryBuffer();

            this.networkStatistics = new ChangeNetworkStatistics();

            this.IsSocketDisposed = true;
        }

        /// <summary>
        /// Creates a random identifier for the TCP client instance.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Creates a new socket for the connection.
        /// </summary>
        /// <returns>A new <see cref="Socket"/> instance.</returns>
        private Socket CreateSocket() => new Socket(this.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Gets the network statistics associated with the client.
        /// </summary>
        /// <returns>An object implementing <see cref="INetworkStatistics"/> containing network statistics.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Connects the client to the server synchronously.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        public virtual bool Connect()
        {
            if (this.IsConnected || this.IsConnecting)
                return false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.connectEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = this.EndPoint
            };
            this.receiveEventArg = new SocketAsyncEventArgs();
            this.sendEventArg = new SocketAsyncEventArgs();

            this.connectEventArg.Completed -= this.OnAsyncCompleted;
            this.connectEventArg.Completed += this.OnAsyncCompleted;

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.receiveEventArg.Completed += this.OnAsyncCompleted;

            this.sendEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed += this.OnAsyncCompleted;

            this.Socket = this.CreateSocket();

            this.IsSocketDisposed = false;

            if (this.Socket.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.DualMode = this.Options.DualMode;

            this.IsConnecting = true;

            this.OnConnecting();

            try
            {
                this.Socket.Connect(this.EndPoint);
            }
            catch (SocketException ex)
            {
                this.SendErrorInternal(ex.SocketErrorCode);

                this.connectEventArg.Completed -= this.OnAsyncCompleted;
                this.receiveEventArg.Completed -= this.OnAsyncCompleted;
                this.sendEventArg.Completed -= this.OnAsyncCompleted;

                this.OnDisconnecting();

                this.Socket.Close();
                this.Socket.Dispose();

                this.connectEventArg.Dispose();
                this.receiveEventArg.Dispose();
                this.sendEventArg.Dispose();

                this.OnDisconnected();

                return false;
            }

            this.SetSocketOption();

            this.ReserveBuffer();

            this.networkStatistics.Reset();

            this.IsConnecting = false;
            this.IsConnected = true;

            this.OnConnected();

            this.TryReceive();

            if (this.sendBuffer.IsEmpty())
                this.OnEmpty();

            return true;
        }

        /// <summary>
        /// Disconnects the client from the server synchronously.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        public virtual bool Disconnect()
        {
            if (!this.IsConnected && !this.IsConnecting)
                return false;

            if (this.IsConnecting)
                Socket.CancelConnectAsync(this.connectEventArg);

            this.connectEventArg.Completed -= this.OnAsyncCompleted;
            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed -= this.OnAsyncCompleted;

            this.OnDisconnecting();

            try
            {
                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException) { }

                this.Socket.Close();
                this.Socket.Dispose();

                this.connectEventArg.Dispose();
                this.receiveEventArg.Dispose();
                this.sendEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.IsConnecting = false;
            this.IsConnected = false;

            this.receiving = false;
            this.sending = false;

            this.ClearBuffers();

            this.OnDisconnected();

            return true;
        }

        /// <summary>
        /// Reconnects the client to the server by first disconnecting 
        /// and then reconnecting.
        /// </summary>
        /// <returns>True if the reconnection is successful; otherwise, false.</returns>
        public virtual bool Reconnect()
        {
            this.Disconnect();

            return this.Connect();
        }

        /// <summary>
        /// Connects the client to the server asynchronously.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        public virtual bool ConnectAsync()
        {
            if (this.IsConnected || this.IsConnecting)
                return false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.connectEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = this.EndPoint
            };
            this.receiveEventArg = new SocketAsyncEventArgs();
            this.sendEventArg = new SocketAsyncEventArgs();

            this.connectEventArg.Completed -= this.OnAsyncCompleted;
            this.connectEventArg.Completed += this.OnAsyncCompleted;

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.receiveEventArg.Completed += this.OnAsyncCompleted;

            this.sendEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed += this.OnAsyncCompleted;

            this.Socket = this.CreateSocket();

            this.IsSocketDisposed = false;

            if (this.Socket.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.DualMode = this.Options.DualMode;

            this.IsConnecting = true;

            this.OnConnecting();

            if (!this.Socket.ConnectAsync(this.connectEventArg))
                this.ProcessConnect(this.connectEventArg);

            return true;
        }

        /// <summary>
        /// Disconnects the client from the server asynchronously.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        public virtual bool DisconnectAsync() => this.Disconnect();

        /// <summary>
        /// Reconnects the client to the server asynchronously by first 
        /// disconnecting and then reconnecting.
        /// </summary>
        /// <returns>True if the reconnection is successful; otherwise, false.</returns>
        public virtual bool ReconnectAsync()
        {
            this.DisconnectAsync();

            while (this.IsConnected)
                Thread.Yield();

            return this.ConnectAsync();
        }

        /// <summary>
        /// Sends data to the server synchronously.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the server synchronously starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
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
        /// Sends data to the server asynchronously.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer) => this.SendAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the server asynchronously starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return false;

            if (length == 0)
                return true;

            lock (this.sendLock)
            {
                if (this.sendBuffer.Length + buffer.Length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
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
        /// Attempts to receive data from the server.
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
        /// Attempts to send data to the server.
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

                        this.networkStatistics.SetBytesPending(0);
                        this.networkStatistics.ChangeBytesSending(this.sendBufferFlush.Length);

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
        /// Clears the send and receive buffers.
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
        /// Handles the completion of asynchronous socket operations.
        /// </summary>
        private void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    this.ProcessConnect(e);
                    break;
                case SocketAsyncOperation.Receive:
                    if (this.ProcessReceive(e))
                        this.TryReceive();
                    break;
                case SocketAsyncOperation.Send:
                    if (this.ProcessSend(e))
                        this.TrySend();
                    break;
                default:
                    throw new Exception("Invalid last operation " + e.LastOperation);
            }

        }

        /// <summary>
        /// Processes the connection event.
        /// </summary>
        /// <param name="e">The event args containing the connection details.</param>
        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            this.IsConnecting = false;

            if (e.SocketError == SocketError.Success)
            {
                this.SetSocketOption();

                this.ReserveBuffer();

                this.networkStatistics.Reset();

                this.IsConnected = true;

                this.TryReceive();

                if (this.IsSocketDisposed)
                    return;

                this.OnConnected();

                if (this.sendBuffer.IsEmpty())
                    this.OnEmpty();
            }
            else
            {
                this.SendErrorInternal(e.SocketError);
                this.OnDisconnected();
            }
        }

        /// <summary>
        /// Sets the socket options based on the TCP client options.
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
        /// Reserves the necessary buffer capacity based on the TCP client options.
        /// </summary>
        private void ReserveBuffer()
        {
            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);
            this.sendBuffer.Reserve(this.Options.SendBufferCapacity);
            this.sendBufferFlush.Reserve(this.Options.SendBufferCapacity);
        }

        /// <summary>
        /// Processes the data received from the server.
        /// </summary>
        /// <param name="e">The event args containing the received data.</param>
        /// <returns>True if more data is expected; otherwise, false.</returns>
        private bool ProcessReceive(SocketAsyncEventArgs e)
        {
            if (!this.IsConnected)
                return false;

            var length = e.BytesTransferred;

            if (length > 0)
            {
                this.networkStatistics.ChangeBytesReceived(length);
                this.networkStatistics.IncPacketReceived();

                this.OnReceived(this.receiveBuffer.Buffer, 0, length);

                if (this.receiveBuffer.Capacity == length)
                {
                    if (length * 2 > this.Options.ReceiveBufferLimit && this.Options.ReceiveBufferLimit > 0)
                    {
                        this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                        this.DisconnectAsync();
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
                    this.DisconnectAsync();
            }
            else
            {
                this.SendErrorInternal(e.SocketError);
                this.DisconnectAsync();
            }

            return false;
        }

        /// <summary>
        /// Processes the data sent to the server.
        /// </summary>
        /// <param name="e">The event args containing the sent data.</param>
        /// <returns>True if more data needs to be sent; otherwise, false.</returns>
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
                this.DisconnectAsync();
                return false;
            }
        }

        /// <summary>
        /// Called when the client is in the process of connecting.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnConnecting() { }

        /// <summary>
        /// Called when the client has successfully connected to the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Called when the client is in the process of disconnecting.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Called when the client has successfully disconnected from the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Called when data is received from the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when data is successfully sent to the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="sent">The number of bytes sent.</param>
        /// <param name="pending">The number of bytes pending in the send buffer.</param>
        protected virtual void OnSent(int sent, int pending) { }

        /// <summary>
        /// Called when the send buffer is empty.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnEmpty() { }

        /// <summary>
        /// Called when an error occurs during socket operations.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Sends an internal error event to the <see cref="OnError"/> method.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Disposes the TCP client, releasing all resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the TCP client, releasing managed resources if specified.
        /// </summary>
        /// <param name="disposingManagedResources">True to release managed resources; otherwise, false.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.DisconnectAsync();
                }

                this.IsDisposed = true;
            }
        }

    }

    /// <summary>
    /// Implements an SSL/TLS client for secure communication over a TCP connection.
    /// </summary>
    public class SslClient : ITcpClient, IDisposable
    {
        /// <summary>
        /// Gets the Ssl options used for SSL/TLS encryption and authentication.
        /// </summary>
        public SslOptions SslOptions { get; }

        /// <summary>
        /// Gets a value indicating whether the SSL/TLS handshake is in progress.
        /// </summary>
        public bool IsHandshaking { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the SSL/TLS handshake has been completed.
        /// </summary>
        public bool IsHandshaked { get; private set; }

        /// <summary>
        /// The SSL stream used for handling the secure communication channel.
        /// </summary>
        private SslStream sslStream { get; set; }

        /// <summary>
        /// A unique identifier for the SSL stream instance, used to track connections.
        /// </summary>
        private string sslStreamId { get; set; }

        /// <summary>
        /// A unique identifier for this client instance.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The address of the remote server to which the client is connecting.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// The port number on the remote server to which the client is connecting.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The endpoint representing the address and port of the remote server.
        /// </summary>
        public EndPoint EndPoint { get; }

        /// <summary>
        /// The socket used for network communication.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Configuration options for the TCP client.
        /// </summary>
        public TcpClientOptions Options { get; }

        /// <summary>
        /// Indicates whether the client is currently in the process of connecting.
        /// </summary>
        public bool IsConnecting { get; private set; }

        /// <summary>
        /// Indicates whether the client is currently connected to the server.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Indicates whether the client has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Indicates whether the socket has been disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

        /// <summary>
        /// Interface for tracking and updating network statistics.
        /// </summary>
        private IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// An object used for synchronizing access to the send buffer.
        /// </summary>
        private object sendLock { get; }

        /// <summary>
        /// The buffer used for storing received data.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// The buffer used for storing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBuffer;

        /// <summary>
        /// The buffer used for flushing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBufferFlush;

        /// <summary>
        /// The offset in the send buffer flush process.
        /// </summary>
        private int sendBufferFlushOffset { get; set; }

        /// <summary>
        /// The event argument used for asynchronous connect operations.
        /// </summary>
        private SocketAsyncEventArgs connectEventArg { get; set; }

        /// <summary>
        /// Indicates whether the client is currently receiving data.
        /// </summary>
        private bool receiving { get; set; }

        /// <summary>
        /// Indicates whether the client is currently sending data.
        /// </summary>
        private bool sending { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SslClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">TCP client options for configuring the connection.</param>
        /// <param name="sslOptions">The Ssl options for secure communication.</param>
        public SslClient(string address, int port, TcpClientOptions options, SslOptions sslOptions)
        {
            this.Id = this.CreateRandomId();

            this.SslOptions = sslOptions;
            this.Address = address;
            this.Port = port;

            this.Options = options;

            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.sendLock = new object();
            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();
            this.sendBufferFlush = new MemoryBuffer();

            this.networkStatistics = new ChangeNetworkStatistics();

            this.IsSocketDisposed = true;
        }

        /// <summary>
        /// Creates a random identifier for the SSL client instance.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Creates a new socket for the connection.
        /// </summary>
        /// <returns>A new <see cref="Socket"/> instance.</returns>
        private Socket CreateSocket() => new Socket(this.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Gets the network statistics associated with the client.
        /// </summary>
        /// <returns>An object implementing <see cref="INetworkStatistics"/> containing network statistics.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Connects the client to the server synchronously and initiates 
        /// the SSL/TLS handshake.
        /// </summary>
        /// <returns>True if the connection and handshake are successful; otherwise, false.</returns>
        public virtual bool Connect()
        {
            if (this.IsConnected || this.IsHandshaked || this.IsConnecting || this.IsHandshaking)
                return false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.connectEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = this.EndPoint
            };

            this.connectEventArg.Completed -= this.OnAsyncCompleted;
            this.connectEventArg.Completed += this.OnAsyncCompleted;

            this.Socket = this.CreateSocket();

            this.IsSocketDisposed = false;

            if (this.Socket.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.DualMode = this.Options.DualMode;

            this.IsConnecting = true;

            this.OnConnecting();

            try
            {
                this.Socket.Connect(this.EndPoint);
            }
            catch (SocketException ex)
            {
                this.SendErrorInternal(ex.SocketErrorCode);

                this.connectEventArg.Completed -= this.OnAsyncCompleted;

                this.OnDisconnecting();

                this.Socket.Close();
                this.Socket.Dispose();

                this.connectEventArg.Dispose();

                this.OnDisconnected();

                return false;
            }

            this.SetSocketOption();

            this.ReserveBuffer();

            this.networkStatistics.Reset();

            this.IsConnecting = false;
            this.IsConnected = true;

            this.OnConnected();

            try
            {
                this.sslStreamId = this.CreateRandomId();
                this.sslStream = new SslStream(new NetworkStream(this.Socket, false), false);

                this.OnHandshaking();

                if (this.SslOptions.Certificates != null)
                    this.sslStream.AuthenticateAsClient(this.Address, this.SslOptions.Certificates, this.SslOptions.SslProtocols, true);
                else if (this.SslOptions.X509Certificate != null)
                    this.sslStream.AuthenticateAsClient(this.Address, new X509CertificateCollection(new[] { this.SslOptions.X509Certificate }), this.SslOptions.SslProtocols, true);
                else
                    this.sslStream.AuthenticateAsClient(this.Address);
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.NotConnected);
                this.DisconnectAsync();
                return false;
            }

            this.IsHandshaked = true;

            this.OnHandshaked();

            this.TryReceive();

            if (this.sendBuffer.IsEmpty())
                this.OnEmpty();

            return true;
        }

        /// <summary>
        /// Disconnects the client from the server synchronously.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        public virtual bool Disconnect()
        {
            if (!this.IsConnected && !this.IsConnecting)
                return false;

            if (this.IsConnecting)
                Socket.CancelConnectAsync(this.connectEventArg);

            this.IsConnecting = false;
            this.IsHandshaking = false;

            this.connectEventArg.Completed -= this.OnAsyncCompleted;

            this.OnDisconnecting();

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

                this.connectEventArg.Dispose();

                this.IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            this.IsHandshaked = false;

            this.IsConnecting = false;
            this.IsConnected = false;

            this.receiving = false;
            this.sending = false;

            this.ClearBuffers();

            this.OnDisconnected();

            return true;
        }

        /// <summary>
        /// Reconnects the client to the server by first disconnecting, 
        /// then reconnecting and initiating the SSL/TLS handshake.
        /// </summary>
        /// <returns>True if the reconnection and handshake are successful; otherwise, false.</returns>
        public virtual bool Reconnect()
        {
            this.Disconnect();

            return this.Connect();
        }

        /// <summary>
        /// Connects the client to the server asynchronously and initiates 
        /// the SSL/TLS handshake.
        /// </summary>
        /// <returns>True if the connection and handshake are successful; otherwise, false.</returns>
        public virtual bool ConnectAsync()
        {
            if (this.IsConnected || this.IsHandshaked || this.IsConnecting || this.IsHandshaking)
                return false;

            this.receiveBuffer.Clear();
            this.sendBuffer.Clear();
            this.sendBufferFlush.Clear();

            this.connectEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = this.EndPoint
            };

            this.connectEventArg.Completed -= this.OnAsyncCompleted;
            this.connectEventArg.Completed += this.OnAsyncCompleted;

            this.Socket = this.CreateSocket();

            this.IsSocketDisposed = false;

            if (this.Socket.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.DualMode = this.Options.DualMode;

            this.IsConnecting = true;

            this.OnConnecting();

            if (!this.Socket.ConnectAsync(this.connectEventArg))
                this.ProcessConnect(this.connectEventArg);

            return true;
        }

        /// <summary>
        /// Disconnects the client from the server asynchronously.
        /// </summary>
        /// <returns>True if the disconnection is successful; otherwise, false.</returns>
        public virtual bool DisconnectAsync() => this.Disconnect();

        /// <summary>
        /// Reconnects the client to the server asynchronously by first 
        /// disconnecting, then reconnecting and initiating the SSL/TLS handshake.
        /// </summary>
        /// <returns>True if the reconnection and handshake are successful; otherwise, false.</returns>
        public virtual bool ReconnectAsync()
        {
            this.DisconnectAsync();

            while (this.IsConnected)
                Thread.Yield();

            return this.ConnectAsync();
        }

        /// <summary>
        /// Sends data to the server synchronously through the SSL/TLS stream.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(byte[] buffer) => this.Send(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the server synchronously through the SSL/TLS stream 
        /// starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
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

                this.networkStatistics.ChangeBytesSent(length);
                this.networkStatistics.IncPacketSent();

                this.OnSent(length, (int)(this.networkStatistics.GetBytesPending() + this.networkStatistics.GetBytesSending()));

                return length;
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.Disconnect();
                return 0;
            }
        }

        /// <summary>
        /// Sends data to the server asynchronously through the SSL/TLS stream.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer) => this.SendAsync(buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the server asynchronously through the SSL/TLS stream 
        /// starting from a specific position in the buffer.
        /// </summary>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(byte[] buffer, int position, int length)
        {
            if (!this.IsHandshaked)
                return false;

            if (length == 0)
                return true;

            lock (this.sendLock)
            {
                if (this.sendBuffer.Length + buffer.Length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
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
        /// Attempts to receive data from the server through the SSL/TLS stream.
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
        /// Attempts to send data to the server through the SSL/TLS stream.
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

                    this.networkStatistics.SetBytesPending(this.sendBufferFlush.Length);

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
        /// Clears the send and receive buffers.
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
        /// Handles the completion of asynchronous socket operations.
        /// </summary>
        private void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (this.IsSocketDisposed)
                return;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    this.ProcessConnect(e);
                    break;
                default:
                    throw new Exception("Invalid last operation " + e.LastOperation);
            }

        }

        /// <summary>
        /// Processes the connection event and initiates the SSL/TLS handshake.
        /// </summary>
        /// <param name="e">The event args containing the connection details.</param>
        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            this.IsConnecting = false;

            if (e.SocketError == SocketError.Success)
            {
                this.SetSocketOption();

                this.ReserveBuffer();

                this.networkStatistics.Reset();

                this.IsConnected = true;

                this.OnConnected();

                try
                {
                    this.sslStreamId = this.CreateRandomId();
                    this.sslStream = new SslStream(new NetworkStream(this.Socket, false), false);

                    this.IsHandshaking = true;
                    this.OnHandshaking();

                    if (this.SslOptions.Certificates != null)
                        this.sslStream.BeginAuthenticateAsClient(this.Address, this.SslOptions.Certificates, this.SslOptions.SslProtocols, true, this.ProcessHandshake, this.sslStreamId);
                    else if (this.SslOptions.X509Certificate != null)
                        this.sslStream.BeginAuthenticateAsClient(this.Address, new X509CertificateCollection(new[] { this.SslOptions.X509Certificate }), this.SslOptions.SslProtocols, true, this.ProcessHandshake, this.sslStreamId);
                    else
                        this.sslStream.BeginAuthenticateAsClient(this.Address, this.ProcessHandshake, this.sslStreamId);
                }
                catch (Exception)
                {
                    this.SendErrorInternal(SocketError.NotConnected);
                    this.DisconnectAsync();
                }
            }
            else
            {
                this.SendErrorInternal(e.SocketError);
                this.OnDisconnected();
            }
        }

        /// <summary>
        /// Sets the socket options based on the TCP client options.
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
        /// Reserves the necessary buffer capacity based on the TCP client options.
        /// </summary>
        private void ReserveBuffer()
        {
            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);
            this.sendBuffer.Reserve(this.Options.SendBufferCapacity);
            this.sendBufferFlush.Reserve(this.Options.SendBufferCapacity);
        }

        /// <summary>
        /// Processes the SSL/TLS handshake.
        /// </summary>
        /// <param name="result">The result of the asynchronous handshake operation.</param>
        private void ProcessHandshake(IAsyncResult result)
        {
            try
            {
                this.IsHandshaking = false;

                if (this.IsHandshaked)
                    return;

                var sslStreamId = result.AsyncState as string;
                if (this.sslStreamId != sslStreamId)
                    return;

                this.sslStream.EndAuthenticateAsClient(result);

                this.IsHandshaked = true;

                this.TryReceive();

                if (this.IsSocketDisposed)
                    return;

                this.OnHandshaked();

                if (this.sendBuffer.IsEmpty())
                    this.OnEmpty();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.NotConnected);
                this.DisconnectAsync();
            }
        }

        /// <summary>
        /// Processes the data received from the server through the SSL/TLS stream.
        /// </summary>
        /// <param name="result">The result of the asynchronous receive operation.</param>
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

                    this.OnReceived(this.receiveBuffer.Buffer, 0, length);

                    if (this.receiveBuffer.Capacity == length)
                    {
                        if (length * 2 > this.Options.ReceiveBufferLimit && this.Options.ReceiveBufferLimit > 0)
                        {
                            this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                            this.DisconnectAsync();
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
                    this.DisconnectAsync();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.DisconnectAsync();
            }
        }

        /// <summary>
        /// Processes the data sent to the server through the SSL/TLS stream.
        /// </summary>
        /// <param name="result">The result of the asynchronous send operation.</param>
        private void ProcessSend(IAsyncResult result)
        {
            try
            {
                if (!this.IsHandshaked)
                    return;

                var sslStreamId = result.AsyncState as string;
                if (this.sslStreamId != sslStreamId)
                    return;

                this.sslStream.EndWrite(result);

                var length = this.sendBufferFlush.Length;

                if (length > 0)
                {
                    this.networkStatistics.ChangeBytesSending(-length);
                    this.networkStatistics.ChangeBytesSent(length);
                    this.networkStatistics.IncPacketSent();

                    this.sendBufferFlushOffset += length;

                    if (this.sendBufferFlushOffset == this.sendBufferFlush.Length)
                    {
                        this.sendBufferFlush.Clear();
                        this.sendBufferFlushOffset = 0;
                    }

                    this.OnSent(length, (int)(this.networkStatistics.GetBytesSending() * this.networkStatistics.GetBytesPending()));
                }

                this.TrySend();
            }
            catch (Exception)
            {
                this.SendErrorInternal(SocketError.OperationAborted);
                this.DisconnectAsync();
            }
        }

        /// <summary>
        /// Called when the client is in the process of connecting.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnConnecting() { }

        /// <summary>
        /// Called when the client has successfully connected to the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Called when the SSL/TLS handshake is in progress.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnHandshaking() { }

        /// <summary>
        /// Called when the SSL/TLS handshake has been completed.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnHandshaked() { }

        /// <summary>
        /// Called when the client is in the process of disconnecting.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnDisconnecting() { }

        /// <summary>
        /// Called when the client has successfully disconnected from the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Called when data is received from the server through the SSL/TLS stream.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when data is successfully sent to the server through the SSL/TLS stream.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="sent">The number of bytes sent.</param>
        /// <param name="pending">The number of bytes pending in the send buffer.</param>
        protected virtual void OnSent(int sent, int pending) { }

        /// <summary>
        /// Called when the send buffer is empty.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        protected virtual void OnEmpty() { }

        /// <summary>
        /// Called when an error occurs during socket operations.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        protected virtual void OnError(SocketError error) { }

        /// <summary>
        /// Sends an internal error event to the <see cref="OnError"/> method.
        /// </summary>
        /// <param name="error">The socket error that occurred.</param>
        private void SendErrorInternal(SocketError error)
        {
            if (Utils.IsIgnoreDisconnectError(error)) return;

            this.OnError(error);
        }

        /// <summary>
        /// Disposes the SSL client, releasing all resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the SSL client, releasing managed resources if specified.
        /// </summary>
        /// <param name="disposingManagedResources">True to release managed resources; otherwise, false.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.IsDisposed)
            {
                if (disposingManagedResources)
                {
                    this.DisconnectAsync();
                }

                this.IsDisposed = true;
            }
        }

    }

}

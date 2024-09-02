using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Helper;

namespace XmobiTea.ProtonNetClient
{
    /// <summary>
    /// Defines the interface for a UDP client that can connect, 
    /// disconnect, send, and receive data over a network.
    /// Supports both synchronous and asynchronous operations.
    /// </summary>
    public interface IUdpClient : IClient
    {
        /// <summary>
        /// Sends data to the specified endpoint synchronously.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(EndPoint endPoint, byte[] buffer);

        /// <summary>
        /// Sends data to the specified endpoint synchronously, starting 
        /// from a specific position in the buffer.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(EndPoint endPoint, byte[] buffer, int position, int length);

        /// <summary>
        /// Sends data to the specified endpoint asynchronously.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(EndPoint endPoint, byte[] buffer);

        /// <summary>
        /// Sends data to the specified endpoint asynchronously, starting 
        /// from a specific position in the buffer.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        bool SendAsync(EndPoint endPoint, byte[] buffer, int position, int length);

        /// <summary>
        /// Joins a multicast group by the specified address.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        void JoinMulticastGroup(string address);

        /// <summary>
        /// Leaves a multicast group by the specified address.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        void LeaveMulticastGroup(string address);
    }

    /// <summary>
    /// Implements a UDP client for sending and receiving data over 
    /// a network. Provides both synchronous and asynchronous operations.
    /// </summary>
    public class UdpClient : IUdpClient, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this UDP client instance.
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
        /// Gets or sets the endpoint representing the server's address and port.
        /// </summary>
        public EndPoint EndPoint;

        /// <summary>
        /// Gets the underlying socket used for the connection.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Gets the options used to configure the UDP client.
        /// </summary>
        public UdpClientOptions Options { get; }

        /// <summary>
        /// Gets a value indicating whether the client is currently connecting.
        /// </summary>
        public bool IsConnecting { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the client is connected to the server.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Interface for tracking and updating network statistics.
        /// </summary>
        private IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// The endpoint from which data is received.
        /// </summary>
        private EndPoint receiveEndPoint { get; }

        /// <summary>
        /// The buffer used for storing received data.
        /// </summary>
        private IMemoryBuffer receiveBuffer { get; }

        /// <summary>
        /// The endpoint to which data is sent.
        /// </summary>
        private EndPoint sendEndPoint { get; set; }

        /// <summary>
        /// The buffer used for storing data to be sent.
        /// </summary>
        private IMemoryBuffer sendBuffer { get; }

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
        /// Initializes a new instance of the <see cref="UdpClient"/> class.
        /// </summary>
        /// <param name="address">The server address to connect to.</param>
        /// <param name="port">The port number on the server.</param>
        /// <param name="options">UDP client options for configuring the connection.</param>
        public UdpClient(string address, int port, UdpClientOptions options)
        {
            this.Id = this.CreateRandomId();

            this.Address = address;
            this.Port = port;

            this.Options = options;

            this.EndPoint = new IPEndPoint(IPAddress.Parse(address), port);

            this.receiveBuffer = new MemoryBuffer();
            this.sendBuffer = new MemoryBuffer();

            this.receiveEndPoint = new IPEndPoint((this.EndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);

            this.networkStatistics = new ChangeNetworkStatistics();
        }

        /// <summary>
        /// Creates a random identifier for the UDP client instance.
        /// </summary>
        /// <returns>A random string identifier.</returns>
        private string CreateRandomId() => Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the network statistics associated with the client.
        /// </summary>
        /// <returns>An object implementing <see cref="INetworkStatistics"/> containing network statistics.</returns>
        public INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Creates a new socket for the connection.
        /// </summary>
        /// <returns>A new <see cref="Socket"/> instance.</returns>
        private Socket CreateSocket() => new Socket(this.EndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

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

            this.receiveEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = this.receiveEndPoint
            };

            this.sendEventArg = new SocketAsyncEventArgs();

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.receiveEventArg.Completed += this.OnAsyncCompleted;

            this.sendEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed += this.OnAsyncCompleted;

            this.Socket = this.CreateSocket();

            this.IsSocketDisposed = false;

            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, this.Options.ReuseAddress);
            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, this.Options.ExclusiveAddressUse);

            if (this.Socket.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.DualMode = this.Options.DualMode;

            this.IsConnecting = true;

            this.OnConnecting();

            try
            {
                if (this.Options.Multicast)
                    this.Socket.Bind(this.EndPoint);
                else
                {
                    var endPoint = new IPEndPoint((this.EndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);
                    this.Socket.Bind(endPoint);
                }
            }
            catch (SocketException ex)
            {
                this.SendErrorInternal(ex.SocketErrorCode);

                this.receiveEventArg.Completed -= this.OnAsyncCompleted;
                this.sendEventArg.Completed -= this.OnAsyncCompleted;

                this.OnDisconnecting();

                this.Socket.Close();
                this.Socket.Dispose();

                this.receiveEventArg.Dispose();
                this.sendEventArg.Dispose();

                this.OnDisconnected();

                return false;
            }

            this.receiveBuffer.Reserve(this.Options.ReceiveBufferCapacity);
            this.sendBuffer.Reserve(this.Options.SendBufferCapacity);

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

            this.receiveEventArg.Completed -= this.OnAsyncCompleted;
            this.sendEventArg.Completed -= this.OnAsyncCompleted;

            this.OnDisconnecting();

            try
            {
                this.Socket.Close();
                this.Socket.Dispose();

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
        public virtual bool ConnectAsync() => this.Connect();

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
        /// Joins a multicast group by the specified address.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        public virtual void JoinMulticastGroup(string address)
        {
            if (this.EndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, new IPv6MulticastOption(IPAddress.Parse(address)));
            else
                this.Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(address)));

            this.OnJoinedMulticastGroup(address);
        }

        /// <summary>
        /// Leaves a multicast group by the specified address.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        public virtual void LeaveMulticastGroup(string address)
        {
            if (this.EndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                this.Socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, new IPv6MulticastOption(IPAddress.Parse(address)));
            else
                this.Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, new MulticastOption(IPAddress.Parse(address)));

            this.OnLeftMulticastGroup(address);
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
        public virtual int Send(byte[] buffer, int position, int length) => this.Send(this.EndPoint, buffer, position, length);

        /// <summary>
        /// Sends data to the specified endpoint synchronously.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(EndPoint endPoint, byte[] buffer) => this.Send(endPoint, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the specified endpoint synchronously, starting 
        /// from a specific position in the buffer.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>The number of bytes sent.</returns>
        public virtual int Send(EndPoint endPoint, byte[] buffer, int position, int length)
        {
            if (!this.IsConnected)
                return 0;

            if (length == 0)
                return 0;

            var sent = 0;

            try
            {
                sent = this.Socket.SendTo(buffer, position, length, SocketFlags.None, endPoint);

                if (sent > 0)
                {
                    this.networkStatistics.ChangeBytesSent(sent);
                    this.networkStatistics.IncPacketSent();

                    this.OnSent(endPoint, sent);
                }
            }
            catch (ObjectDisposedException) { return 0; }
            catch (SocketException ex)
            {
                this.SendErrorInternal(ex.SocketErrorCode);
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
        public virtual bool SendAsync(byte[] buffer, int position, int length) => this.SendAsync(this.EndPoint, buffer, position, length);

        /// <summary>
        /// Sends data to the specified endpoint asynchronously.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(EndPoint endPoint, byte[] buffer) => this.SendAsync(endPoint, buffer, 0, buffer.Length);

        /// <summary>
        /// Sends data to the specified endpoint asynchronously, starting 
        /// from a specific position in the buffer.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data is sent.</param>
        /// <param name="buffer">The data to send.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The number of bytes to send.</param>
        /// <returns>True if the data was sent successfully; otherwise, false.</returns>
        public virtual bool SendAsync(EndPoint endPoint, byte[] buffer, int position, int length)
        {
            if (this.sending)
                return false;

            if (!this.IsConnected)
                return false;

            if (length == 0)
                return true;

            if (this.sendBuffer.Length + buffer.Length > this.Options.SendBufferLimit && this.Options.SendBufferLimit > 0)
            {
                this.SendErrorInternal(SocketError.NoBufferSpaceAvailable);
                return false;
            }

            this.sendBuffer.Write(buffer, position, length);

            this.networkStatistics.SetBytesSending(this.sendBuffer.Length);

            this.sendEndPoint = endPoint;

            this.TrySend();

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

            try
            {
                this.receiving = true;

                this.receiveEventArg.SetBuffer(this.receiveBuffer.Buffer, 0, this.receiveBuffer.Capacity);
                if (!this.Socket.ReceiveFromAsync(this.receiveEventArg))
                    this.ProcessReceiveFrom(this.receiveEventArg);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Attempts to send data to the server.
        /// </summary>
        private void TrySend()
        {
            if (this.sending)
                return;

            if (!this.IsConnected)
                return;

            try
            {
                this.sending = true;
                this.sendEventArg.RemoteEndPoint = this.sendEndPoint;
                this.sendEventArg.SetBuffer(this.sendBuffer.Buffer, 0, this.sendBuffer.Length);
                if (!this.Socket.SendToAsync(this.sendEventArg))
                    this.ProcessSendTo(this.sendEventArg);
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Clears the send and receive buffers.
        /// </summary>
        private void ClearBuffers()
        {
            this.sendBuffer.Clear();

            this.networkStatistics.SetBytesPending(0);
            this.networkStatistics.SetBytesSending(0);
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
                case SocketAsyncOperation.ReceiveFrom:
                    this.ProcessReceiveFrom(e);
                    break;
                case SocketAsyncOperation.SendTo:
                    this.ProcessSendTo(e);
                    break;
                default:
                    throw new Exception("Invalid last operation " + e.LastOperation);
            }
        }

        /// <summary>
        /// Processes the data received from the server.
        /// </summary>
        /// <param name="e">The event args containing the received data.</param>
        private void ProcessReceiveFrom(SocketAsyncEventArgs e)
        {
            this.receiving = false;

            if (!this.IsConnected)
                return;

            if (e.SocketError != SocketError.Success)
            {
                this.SendErrorInternal(e.SocketError);
                this.Disconnect();
                return;
            }

            var length = e.BytesTransferred;

            this.networkStatistics.ChangeBytesReceived(length);
            this.networkStatistics.IncPacketReceived();

            this.OnReceived(e.RemoteEndPoint, this.receiveBuffer.Buffer, 0, length);

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

            this.TryReceive();
        }

        /// <summary>
        /// Processes the data sent to the server.
        /// </summary>
        /// <param name="e">The event args containing the sent data.</param>
        private void ProcessSendTo(SocketAsyncEventArgs e)
        {
            this.sending = false;

            if (!this.IsConnected)
                return;

            if (e.SocketError != SocketError.Success)
            {
                this.SendErrorInternal(e.SocketError);
                this.Disconnect();
                return;
            }

            var sent = e.BytesTransferred;

            if (sent > 0)
            {
                this.networkStatistics.SetBytesSending(0);
                this.networkStatistics.ChangeBytesSent(sent);
                this.networkStatistics.IncPacketSent();

                this.sendBuffer.Clear();

                this.OnSent(this.sendEndPoint, sent);
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
        /// Called when the client successfully joins a multicast group.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        protected virtual void OnJoinedMulticastGroup(string address) { }

        /// <summary>
        /// Called when the client successfully leaves a multicast group.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="address">The address of the multicast group.</param>
        protected virtual void OnLeftMulticastGroup(string address) { }

        /// <summary>
        /// Called when data is received from the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="endPoint">The endpoint from which the data is received.</param>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the received data.</param>
        protected virtual void OnReceived(EndPoint endPoint, byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when data is successfully sent to the server.
        /// Can be overridden in derived classes to handle the event.
        /// </summary>
        /// <param name="endPoint">The endpoint to which the data was sent.</param>
        /// <param name="sent">The number of bytes sent.</param>
        protected virtual void OnSent(EndPoint endPoint, int sent) { }

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
        /// Gets a value indicating whether the client has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the socket has been disposed.
        /// </summary>
        public bool IsSocketDisposed { get; private set; } = true;

        /// <summary>
        /// Disposes the UDP client, releasing all resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the UDP client, releasing managed resources if specified.
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

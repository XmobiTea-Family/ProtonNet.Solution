using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XmobiTea.Bean.Attributes;
using XmobiTea.Bean.Support;
using XmobiTea.ProtonNet.Client.Models;
using XmobiTea.ProtonNet.Client.Socket.Clients;
using XmobiTea.ProtonNet.Client.Socket.Services;
using XmobiTea.ProtonNet.Client.Socket.Types;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Client.Socket
{
    /// <summary>
    /// Represents a client peer that manages socket connections, sends and receives operations, 
    /// and handles various socket events such as connection, disconnection, and errors.
    /// Inherits from <see cref="ClientPeer"/> and implements <see cref="ISocketClientPeer"/> and <see cref="IAfterAutoBind"/>.
    /// </summary>
    public class SocketClientPeer : ClientPeer, ISocketClientPeer, IAfterAutoBind
    {
        /// <summary>
        /// Prefix used in logging messages specific to this socket client peer.
        /// </summary>
        protected override string logPrefix => "SOCKET";

        /// <summary>
        /// Service responsible for emitting socket session events.
        /// </summary>
        [AutoBind]
        protected ISocketSessionEmitService socketSessionEmitService { get; private set; }

        /// <summary>
        /// Service responsible for handling operation models.
        /// </summary>
        [AutoBind]
        protected ISocketOperationModelService operationModelService { get; private set; }

        /// <summary>
        /// The underlying socket client associated with this peer.
        /// </summary>
        protected ISocketClient socketClient { get; }

        /// <summary>
        /// Service responsible for managing the ping-pong operation to keep the connection alive.
        /// </summary>
        protected ISocketPingPong socketPingPong { get; }

        /// <summary>
        /// Event triggered when a connection is successfully established.
        /// </summary>
        public OnConnected OnConnected { get; set; }

        /// <summary>
        /// Event triggered when a disconnection occurs.
        /// </summary>
        public OnDisconnected OnDisconnected { get; set; }

        /// <summary>
        /// Event triggered when an operation event is received.
        /// </summary>
        public OnOperationEvent OnOperationEvent { get; set; }

        /// <summary>
        /// Event triggered when a socket error occurs.
        /// </summary>
        public OnError OnError { get; set; }

        /// <summary>
        /// Lock object for synchronizing access to pending operation events.
        /// </summary>
        private object _lockOperationEventPending { get; }

        /// <summary>
        /// List of pending operation events.
        /// </summary>
        private IList<OperationEvent> operationEventPendings { get; }

        /// <summary>
        /// Lock object for synchronizing access to delayed operation event pendings.
        /// </summary>
        private object _lockDelayOperationEventPendings { get; }

        /// <summary>
        /// Queue of delayed operation event pendings.
        /// </summary>
        private Queue<OperationEventPending> delayOperationEventPendings { get; }

        /// <summary>
        /// Callback for immediate connection events.
        /// </summary>
        private OnConnected onImmediatelyConnected { get; set; }

        /// <summary>
        /// Callback for immediate disconnection events.
        /// </summary>
        private OnDisconnected onImmediatelyDisconnected { get; set; }

        /// <summary>
        /// Lock object for synchronizing access to other fields.
        /// </summary>
        private object _lockOther { get; }

        /// <summary>
        /// Indicates if a disconnection notice has been sent.
        /// </summary>
        private bool isDisconnectedNotice { get; set; }

        /// <summary>
        /// Indicates if a connection notice has been sent.
        /// </summary>
        private bool isConnectedNotice { get; set; }

        /// <summary>
        /// Indicates if an error notice has been sent.
        /// </summary>
        private bool isErrorNotice { get; set; }

        /// <summary>
        /// The socket error that occurred.
        /// </summary>
        private System.Net.Sockets.SocketError socketError { get; set; }

        /// <summary>
        /// The reason for the disconnection.
        /// </summary>
        private DisconnectReason disconnectReason { get; set; }

        /// <summary>
        /// The message describing the disconnection.
        /// </summary>
        private string disconnectMessage { get; set; }

        /// <summary>
        /// The session ID provided by the server.
        /// </summary>
        protected string serverSessionId { get; private set; }

        /// <summary>
        /// The connection ID assigned to this peer.
        /// </summary>
        protected int connectionId { get; private set; }

        /// <summary>
        /// The encryption key used for securing communications.
        /// </summary>
        protected byte[] encryptKey { get; }

        /// <summary>
        /// Indicates whether the handshake has been completed.
        /// </summary>
        private bool isHandshake = false;

        /// <summary>
        /// Indicates whether auto-reconnect is enabled.
        /// </summary>
        private bool autoReconnect = false;

        /// <summary>
        /// Task for managing reconnection attempts.
        /// </summary>
        private Task reconnectTask { get; set; }

        /// <summary>
        /// The delay in seconds before attempting to auto-reconnect.
        /// </summary>
        private int autoReconnectInSeconds { get; set; }

        /// <summary>
        /// The current count of reconnection attempts.
        /// </summary>
        private int reconnectCount { get; set; }

        /// <summary>
        /// Options for configuring the UDP client.
        /// </summary>
        protected UdpClientOptions udpClientOptions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClientPeer"/> class with the specified parameters.
        /// </summary>
        private SocketClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions, UdpClientOptions udpClientOptions)
            : base(serverAddress, initRequest, tcpClientOptions)
        {
            this._lockOperationEventPending = new object();
            this.operationEventPendings = new List<OperationEvent>();

            this._lockDelayOperationEventPendings = new object();
            this.delayOperationEventPendings = new Queue<OperationEventPending>();

            this._lockOther = new object();

            this.encryptKey = initRequest.EncryptKey;
            this.udpClientOptions = udpClientOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClientPeer"/> class for a specific transport protocol.
        /// </summary>
        public SocketClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions, UdpClientOptions udpClientOptions, TransportProtocol protocol)
            : this(serverAddress, initRequest, tcpClientOptions, udpClientOptions)
        {
            this.socketClient = this.NewSocketClient(protocol);
            this.socketPingPong = this.NewSocketPingPong();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketClientPeer"/> class for a specific SSL transport protocol.
        /// </summary>
        public SocketClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions, UdpClientOptions udpClientOptions, SslTransportProtocol sslProtocol, SslOptions sslOptions)
            : this(serverAddress, initRequest, tcpClientOptions, udpClientOptions)
        {
            this.socketClient = this.NewSslSocketClient(sslProtocol, sslOptions);
            this.socketPingPong = this.NewSocketPingPong();
        }

        /// <summary>
        /// Executes any additional logic after auto-binding of dependencies.
        /// </summary>
        public void OnAfterAutoBind() => (this.socketPingPong as SocketPingPong).socketSessionEmitService = this.socketSessionEmitService;

        /// <summary>
        /// Retrieves the network statistics associated with this socket client peer.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> representing the network statistics.</returns>
        public override INetworkStatistics GetNetworkStatistics() => this.socketClient.GetNetworkStatistics();

        /// <summary>
        /// Retrieves the underlying socket client associated with this peer.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the socket client.</returns>
        public ISocketClient GetSocketClient() => this.socketClient;

        /// <summary>
        /// Creates a new socket client for the specified transport protocol.
        /// </summary>
        /// <param name="protocol">The transport protocol to use (e.g., TCP, UDP, WS).</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the socket client.</returns>
        private ISocketClient NewSocketClient(TransportProtocol protocol)
        {
            if (!Uri.TryCreate(this.serverAddress, UriKind.Absolute, out var url)) throw new Exception("server address " + this.serverAddress + " does not create uri, please check again.");

            ISocketClient answer;

            if (protocol == TransportProtocol.Tcp) answer = this.NewTcpClient(url.Host, url.Port, this.tcpClientOptions);
            else if (protocol == TransportProtocol.Udp) answer = this.NewUdpClient(url.Host, url.Port, this.udpClientOptions);
            else if (protocol == TransportProtocol.Ws) answer = this.NewWsClient(url.Host, url.Port, this.tcpClientOptions);
            else throw new Exception("Type " + protocol + " not support.");

            ((ISetEncryptKey)answer).SetEncryptKey(this.encryptKey);
            return answer;
        }

        /// <summary>
        /// Creates a new SSL socket client for the specified SSL transport protocol.
        /// </summary>
        /// <param name="sslProtocol">The SSL transport protocol to use (e.g., SSL, WSS).</param>
        /// <param name="sslOptions">The Ssl options containing the certificate and other SSL-related settings.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the SSL socket client.</returns>
        private ISocketClient NewSslSocketClient(SslTransportProtocol sslProtocol, SslOptions sslOptions)
        {
            if (!Uri.TryCreate(this.serverAddress, UriKind.Absolute, out var url)) throw new Exception("server address " + this.serverAddress + " does not create uri, please check again.");

            ISocketClient answer;

            if (sslProtocol == SslTransportProtocol.Ssl) answer = this.NewSslClient(url.Host, url.Port, this.tcpClientOptions, sslOptions);
            else if (sslProtocol == SslTransportProtocol.Wss) answer = this.NewWssClient(url.Host, url.Port, this.tcpClientOptions, sslOptions);
            else throw new Exception("Type " + sslProtocol + " not support.");

            ((ISetEncryptKey)answer).SetEncryptKey(this.encryptKey);
            return answer;
        }

        /// <summary>
        /// Creates a new instance of the ping-pong service for managing keep-alive pings.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketPingPong"/>.</returns>
        protected virtual ISocketPingPong NewSocketPingPong() => new SocketPingPong(this.socketClient);

        /// <summary>
        /// Creates a new TCP socket client.
        /// </summary>
        /// <param name="host">The host address of the server.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="tcpClientOptions">Options for configuring the TCP client.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the TCP client.</returns>
        protected virtual ISocketClient NewTcpClient(string host, int port, TcpClientOptions tcpClientOptions)
        {
            var answer = new SocketTcpClient(host, port, tcpClientOptions);

            answer.onConnected += this.OnSocketClientConnected;
            answer.onDisconnected += this.OnSocketClientDisconnected;
            answer.onReceived += this.OnSocketClientReceived;
            answer.onError += this.OnSocketClientError;

            return answer;
        }

        /// <summary>
        /// Creates a new SSL socket client.
        /// </summary>
        /// <param name="host">The host address of the server.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="tcpClientOptions">Options for configuring the TCP client.</param>
        /// <param name="sslOptions">The Ssl options containing the certificate and other SSL-related settings.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the SSL client.</returns>
        protected virtual ISocketClient NewSslClient(string host, int port, TcpClientOptions tcpClientOptions, SslOptions sslOptions)
        {
            var answer = new SocketSslClient(host, port, tcpClientOptions, sslOptions);

            answer.onConnected += this.OnSocketClientConnected;
            answer.onDisconnected += this.OnSocketClientDisconnected;
            answer.onReceived += this.OnSocketClientReceived;
            answer.onError += this.OnSocketClientError;

            return answer;
        }

        /// <summary>
        /// Creates a new UDP socket client.
        /// </summary>
        /// <param name="host">The host address of the server.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="udpClientOptions">Options for configuring the UDP client.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the UDP client.</returns>
        protected virtual ISocketClient NewUdpClient(string host, int port, UdpClientOptions udpClientOptions)
        {
            var answer = new SocketUdpClient(host, port, udpClientOptions);

            answer.onConnected += this.OnSocketClientConnected;
            answer.onDisconnected += this.OnSocketClientDisconnected;
            answer.onReceived += this.OnSocketClientReceived;
            answer.onError += this.OnSocketClientError;

            return answer;
        }

        /// <summary>
        /// Creates a new WebSocket client.
        /// </summary>
        /// <param name="host">The host address of the server.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="tcpClientOptions">Options for configuring the WebSocket client.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the WebSocket client.</returns>
        protected virtual ISocketClient NewWsClient(string host, int port, TcpClientOptions tcpClientOptions)
        {
            var answer = new SocketWsClient(host, port, tcpClientOptions);

            answer.onConnected += this.OnSocketClientConnected;
            answer.onDisconnected += this.OnSocketClientDisconnected;
            answer.onReceived += this.OnSocketClientReceived;
            answer.onError += this.OnSocketClientError;

            return answer;
        }

        /// <summary>
        /// Creates a new secure WebSocket client.
        /// </summary>
        /// <param name="host">The host address of the server.</param>
        /// <param name="port">The port number to connect to.</param>
        /// <param name="tcpClientOptions">Options for configuring the WebSocket client.</param>
        /// <param name="sslOptions">The Ssl options containing the certificate and other SSL-related settings.</param>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the secure WebSocket client.</returns>
        protected virtual ISocketClient NewWssClient(string host, int port, TcpClientOptions tcpClientOptions, SslOptions sslOptions)
        {
            var answer = new SocketWssClient(host, port, tcpClientOptions, sslOptions);

            answer.onConnected += this.OnSocketClientConnected;
            answer.onDisconnected += this.OnSocketClientDisconnected;
            answer.onReceived += this.OnSocketClientReceived;
            answer.onError += this.OnSocketClientError;

            return answer;
        }

        /// <summary>
        /// Handles the socket client's connected event.
        /// </summary>
        private void OnSocketClientConnected()
        {
            var sendResult = this.socketSessionEmitService.SendOperationHandshake(this.socketClient, new OperationHandshake()
            {
                SessionId = this.sessionId,
                EncryptKey = this.encryptKey,
                AuthToken = this.authToken,
            });

            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for OperationHandshake failed " + sendResult);
        }

        /// <summary>
        /// Handles the socket client's disconnected event.
        /// </summary>
        private void OnSocketClientDisconnected()
        {
            lock (this._lockOther)
                this.isDisconnectedNotice = true;
        }

        /// <summary>
        /// Handles the socket client's received event, processing incoming data.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        private void OnSocketClientReceived(byte[] buffer, int position, int length)
        {
            using (var mStream = new System.IO.MemoryStream(buffer, position, length))
                while (true)
                {
                    OperationHeader header;
                    IOperationModel operationModel;

                    if (!this.rpcProtocolService.TryRead(mStream, out header, out var payload))
                    {
                        this.logger.Error("can not read data body");
                        return;
                    }

                    if (header.SendParameters.Encrypted)
                    {
                        if (!this.rpcProtocolService.TryDeserializeEncryptOperationModel(payload, header.OperationType, header.ProtocolProviderType, header.CryptoProviderType.GetValueOrDefault(), this.encryptKey, out operationModel))
                        {
                            this.logger.Error("can not deserialize encrypt operation model");
                            return;
                        }
                    }
                    else
                    {
                        if (!this.rpcProtocolService.TryDeserializeOperationModel(payload, header.OperationType, header.ProtocolProviderType, out operationModel))
                        {
                            this.logger.Error("can not deserialize operation model");
                            return;
                        }
                    }

                    this.operationModelService.Handle(this, header.OperationType, operationModel, header.SendParameters, header.ProtocolProviderType, header.CryptoProviderType);

                    if (mStream.Position >= length) return;
                }
        }

        /// <summary>
        /// Handles the socket client's error event.
        /// </summary>
        /// <param name="error">The socket error encountered.</param>
        private void OnSocketClientError(System.Net.Sockets.SocketError error)
        {
            lock (this._lockOther)
            {
                this.socketError = error;

                this.isErrorNotice = true;
            }
        }

        /// <summary>
        /// Sends an operation event to the server.
        /// </summary>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">The parameters used for sending the event.</param>
        public void Send(OperationEvent operationEvent, SendParameters sendParameters)
        {
            this.Enqueue(operationEvent, sendParameters);
        }

        /// <summary>
        /// Enqueues an operation event to be sent to the server.
        /// </summary>
        /// <param name="operationEvent">The operation event to enqueue.</param>
        /// <param name="sendParameters">The parameters used for sending the event.</param>
        private void Enqueue(OperationEvent operationEvent, SendParameters sendParameters)
        {
            if (sendParameters == null) sendParameters = new SendParameters();

            var operationEventPending = new OperationEventPending(operationEvent, sendParameters);

            this.LogEnqueue(operationEventPending);

            lock (this._lockDelayOperationEventPendings)
                this.delayOperationEventPendings.Enqueue(operationEventPending);
        }

        /// <summary>
        /// Establishes a connection to the server.
        /// </summary>
        /// <param name="autoReconnect">Specifies whether to automatically reconnect if the connection is lost.</param>
        /// <param name="onConnected">Callback for handling successful connections.</param>
        /// <param name="onDisconnected">Callback for handling disconnections.</param>
        /// <returns>True if the connection was successfully established, otherwise false.</returns>
        public bool Connect(bool autoReconnect = false, OnConnected onConnected = null, OnDisconnected onDisconnected = null)
        {
            this.autoReconnectInSeconds = 0;
            this.reconnectCount = 0;

            this.autoReconnect = autoReconnect;
            this.disconnectReason = DisconnectReason.CantConnectServer;

            this.onImmediatelyConnected = onConnected;
            this.onImmediatelyDisconnected = onDisconnected;

            return this.socketClient.Connect();
        }

        /// <summary>
        /// Reconnects to the server.
        /// </summary>
        /// <param name="autoReconnect">Specifies whether to automatically reconnect if the connection is lost.</param>
        /// <param name="onConnected">Callback for handling successful connections.</param>
        /// <param name="onDisconnected">Callback for handling disconnections.</param>
        /// <returns>True if the reconnection was successful, otherwise false.</returns>
        public bool Reconnect(bool autoReconnect = false, OnConnected onConnected = null, OnDisconnected onDisconnected = null)
        {
            this.autoReconnect = autoReconnect;
            this.disconnectReason = DisconnectReason.CantConnectServer;

            this.onImmediatelyConnected = onConnected;
            this.onImmediatelyDisconnected = onDisconnected;

            return this.socketClient.Reconnect();
        }

        /// <summary>
        /// Handles the internal disconnection logic, disabling auto-reconnect and disposing of the reconnect task.
        /// </summary>
        private void DisconnectInternal()
        {
            this.autoReconnect = false;

            if (this.reconnectTask != null)
            {
                this.reconnectTask.Dispose();
                this.reconnectTask = null;
            }
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        /// <returns>True if the disconnection was successful, otherwise false.</returns>
        public bool Disconnect()
        {
            this.DisconnectInternal();

            this.disconnectReason = DisconnectReason.DisconnectByClient;

            return this.socketClient.Disconnect();
        }

        /// <summary>
        /// Checks if the client peer is currently connected to the server.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        public override bool IsConnected() => this.socketClient.IsConnected() && this.isHandshake;

        /// <summary>
        /// Services the socket client peer, handling various events such as connection, disconnection, errors, and operation events.
        /// </summary>
        public override void Service()
        {
            base.Service();

            this.CheckWaitingOperationEventPending();
            this.CheckWaitingConnectedPending();
            this.CheckWaitingErrorPending();
            this.CheckWaitingDisconnectedPending();

            if (!this.IsConnected()) return;

            this.socketPingPong.Service();

            this.SendDelayOperationEventPending();
        }

        /// <summary>
        /// The tick count at which the next operation event should be sent.
        /// </summary>
        private long nextSendTickCount;

        /// <summary>
        /// Sends any delayed operation events to the server.
        /// </summary>
        private void SendDelayOperationEventPending()
        {
            OperationEventPending operationEventPending = null;

            lock (this._lockDelayOperationEventPendings)
            {
                if (this.delayOperationEventPendings.Count == 0) return;

                operationEventPending = this.delayOperationEventPendings.Dequeue();
            }

            var milliseconds = DateTime.UtcNow.Ticks;

            if (this.nextSendTickCount > milliseconds) return;

            this.nextSendTickCount = milliseconds + (int)(this.updateInterval * 10000000);

            this.LogSend(operationEventPending);

            this.SendOperation(operationEventPending);
        }

        /// <summary>
        /// Checks for any pending operation events and processes them.
        /// </summary>
        private void CheckWaitingOperationEventPending()
        {
            List<OperationEvent> pendingEvents;

            lock (this._lockOperationEventPending)
            {
                if (this.operationEventPendings.Count == 0) return;

                pendingEvents = new List<OperationEvent>(this.operationEventPendings);

                this.operationEventPendings.Clear();
            }

            foreach (var operationEvent in pendingEvents)
            {
                this.LogEvent(operationEvent);

                try
                {
                    this.OnOperationEvent?.Invoke(operationEvent);
                }
                catch (Exception ex)
                {
                    this.logger.Fatal(ex);
                }
            }
        }

        /// <summary>
        /// Checks for any pending connection events and processes them.
        /// </summary>
        private void CheckWaitingConnectedPending()
        {
            int connectionId;
            string serverSessionId;
            bool shouldClearHandlers;

            lock (this._lockOther)
            {
                if (!this.isConnectedNotice) return;

                this.isConnectedNotice = false;
                this.isHandshake = true;

                connectionId = this.connectionId;
                serverSessionId = this.serverSessionId;

                shouldClearHandlers = !this.autoReconnect;

                if (shouldClearHandlers)
                {
                    this.autoReconnectInSeconds = 0;
                    this.reconnectCount = 0;
                }
            }

            this.onImmediatelyConnected?.Invoke(connectionId, serverSessionId);
            this.OnConnected?.Invoke(connectionId, serverSessionId);

            if (shouldClearHandlers)
            {
                this.onImmediatelyConnected = null;
                this.onImmediatelyDisconnected = null;
            }
        }

        /// <summary>
        /// Checks for any pending disconnection events and processes them.
        /// </summary>
        private void CheckWaitingDisconnectedPending()
        {
            bool shouldClearHandlers;
            bool shouldReconnect;
            int reconnectInSeconds;
            int currentReconnectCount;
            DisconnectReason disconnectReason;
            string disconnectMessage;

            lock (this._lockOther)
            {
                if (!this.isDisconnectedNotice) return;

                this.isDisconnectedNotice = false;
                this.isHandshake = false;

                disconnectReason = this.disconnectReason;
                disconnectMessage = this.disconnectMessage;

                if (!this.autoReconnect)
                {
                    shouldClearHandlers = true;
                    shouldReconnect = false;
                    reconnectInSeconds = 0;
                    currentReconnectCount = 0;
                }
                else
                {
                    shouldClearHandlers = false;
                    shouldReconnect = true;
                    reconnectInSeconds = this.autoReconnectInSeconds == 0 ? 3 : this.autoReconnectInSeconds == 3 ? 5 : 7;
                    currentReconnectCount = this.reconnectCount + 1;
                    this.autoReconnectInSeconds = reconnectInSeconds;
                    this.reconnectCount += 1;
                }
            }

            this.onImmediatelyDisconnected?.Invoke(disconnectReason, disconnectMessage);
            this.OnDisconnected?.Invoke(disconnectReason, disconnectMessage);

            if (shouldClearHandlers)
            {
                this.onImmediatelyConnected = null;
                this.onImmediatelyDisconnected = null;
            }

            if (shouldReconnect)
            {
                this.logger.Info($"autoReconnect enable, try reconnect after {reconnectInSeconds} seconds, reconnect times: {currentReconnectCount}.");

                this.reconnectTask = Task.Run(async () =>
                {
                    await Task.Delay(reconnectInSeconds * 1000);
                    this.Reconnect(this.autoReconnect, this.onImmediatelyConnected, this.onImmediatelyDisconnected);
                });
            }
        }

        /// <summary>
        /// Checks for any pending error events and processes them.
        /// </summary>
        private void CheckWaitingErrorPending()
        {
            System.Net.Sockets.SocketError error;

            lock (this._lockOther)
            {
                if (!this.isErrorNotice) return;

                this.isErrorNotice = false;
                error = this.socketError;
            }

            this.OnError?.Invoke(error);
        }

        /// <summary>
        /// Handles the receipt of an operation response internally, matching it with a pending request.
        /// </summary>
        /// <param name="operationResponse">The operation response received from the server.</param>
        internal void OnReceiveOperationResponseInternal(OperationResponse operationResponse)
        {
            var responseId = operationResponse.ResponseId;

            OperationRequestPending operationRequestPending;

            lock (this._lockWaitingResponseOperationRequestPendings)
                operationRequestPending = this.waitingResponseOperationRequestPendings.Find(x => x.GetOperationRequest().RequestId == responseId);

            if (operationRequestPending != null)
            {
                operationRequestPending.OnRecv();

                operationRequestPending.SetOperationResponse(operationResponse);
            }
        }

        /// <summary>
        /// Handles the receipt of an operation event internally, adding it to the list of pending events.
        /// </summary>
        /// <param name="operationEvent">The operation event received from the server.</param>
        internal void OnReceiveOperationEventInternal(OperationEvent operationEvent)
        {
            lock (this._lockOperationEventPending)
                this.operationEventPendings.Add(operationEvent);
        }

        /// <summary>
        /// Handles the receipt of an operation handshake acknowledgment internally, updating the connection state.
        /// </summary>
        /// <param name="operationHandshakeAck">The handshake acknowledgment received from the server.</param>
        internal void OnReceiveOperationHandshakeAckInternal(OperationHandshakeAck operationHandshakeAck)
        {
            this.connectionId = operationHandshakeAck.ConnectionId;
            this.serverSessionId = operationHandshakeAck.ServerSessionId;

            lock (this._lockOther)
                this.isConnectedNotice = true;
        }

        /// <summary>
        /// Handles the receipt of an operation disconnect event internally, initiating disconnection.
        /// </summary>
        /// <param name="operationDisconnect">The disconnect operation received from the server.</param>
        internal void OnReceiveOperationDisconnectInternal(OperationDisconnect operationDisconnect)
        {
            this.disconnectReason = (DisconnectReason)operationDisconnect.Reason;
            this.disconnectMessage = operationDisconnect.Message;

            this.socketClient.Disconnect();
        }

        /// <summary>
        /// Handles the receipt of an operation ping event internally, responding with a pong.
        /// </summary>
        /// <param name="operationPing">The ping operation received from the server.</param>
        internal void OnReceiveOperationPingInternal(OperationPing operationPing)
        {
            this.SendOperationPong(new OperationPong() { });
        }

        /// <summary>
        /// Handles the receipt of an operation pong event internally.
        /// </summary>
        /// <param name="operationPong">The pong operation received from the server.</param>
        internal void OnReceiveOperationPongInternal(OperationPong operationPong)
        {

        }

        /// <summary>
        /// Sends an operation request to the server.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to send.</param>
        protected override void SendOperation(OperationRequestPending operationRequestPending)
        {
            var operationRequest = operationRequestPending.GetOperationRequest();
            var sendParameters = operationRequestPending.GetSendParameters();

            var sendResult = this.socketSessionEmitService.SendOperationRequest(this.socketClient, operationRequest, sendParameters);
            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for SendOperationRequest failed " + sendResult);
        }

        /// <summary>
        /// Sends an operation event to the server.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event to send.</param>
        protected virtual void SendOperation(OperationEventPending operationEventPending)
        {
            var operationEvent = operationEventPending.GetOperationEvent();
            var sendParameters = operationEventPending.GetSendParameters();

            var sendResult = this.socketSessionEmitService.SendOperationEvent(this.socketClient, operationEvent, sendParameters);
            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for SendOperationEvent failed " + sendResult);
        }

        /// <summary>
        /// Sends a ping operation to the server.
        /// </summary>
        /// <param name="operationPing">The ping operation to send.</param>
        internal void SendOperationPing(OperationPing operationPing)
        {
            var sendResult = this.socketSessionEmitService.SendOperationPing(this.socketClient, operationPing);
            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for SendOperationPing failed " + sendResult);
        }

        /// <summary>
        /// Sends a pong operation to the server in response to a ping.
        /// </summary>
        /// <param name="operationPong">The pong operation to send.</param>
        internal void SendOperationPong(OperationPong operationPong)
        {
            var sendResult = this.socketSessionEmitService.SendOperationPong(this.socketClient, operationPong);
            if (sendResult != SendResult.Ok) this.logger.Warn("SendResult for SendOperationPong failed " + sendResult);
        }

        /// <summary>
        /// Logs the enqueuing of an operation event.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was enqueued.</param>
        protected void LogEnqueue(OperationEventPending operationEventPending)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][ENQUEUE EVENT] {this.debugSupport.ToStringOnEnqueue(operationEventPending)}");
        }

        /// <summary>
        /// Logs the sending of an operation event.
        /// </summary>
        /// <param name="operationEventPending">The pending operation event that was sent.</param>
        protected void LogSend(OperationEventPending operationEventPending)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][SEND EVENT] {this.debugSupport.ToStringOnSend(operationEventPending)}");
        }

        /// <summary>
        /// Logs the reception of an operation event.
        /// </summary>
        /// <param name="operationEvent">The operation event that was received.</param>
        protected void LogEvent(OperationEvent operationEvent)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][EVENT] {this.debugSupport.ToStringOnEvent(operationEvent)}");
        }

    }

}

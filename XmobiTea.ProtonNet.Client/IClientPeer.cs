using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XmobiTea.Bean.Attributes;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Client.Models;
using XmobiTea.ProtonNet.Client.Services;
using XmobiTea.ProtonNet.Client.Supports;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Client
{
    /// <summary>
    /// Represents a client peer in the network communication system. 
    /// Provides methods to send requests, manage connections, and access network statistics.
    /// </summary>
    public interface IClientPeer
    {
        /// <summary>
        /// Gets the unique client ID.
        /// </summary>
        /// <returns>The client ID as an integer.</returns>
        int GetClientId();

        /// <summary>
        /// Gets the current ping (round-trip time) to the server.
        /// </summary>
        /// <returns>The ping in milliseconds.</returns>
        int GetPing();

        /// <summary>
        /// Gets the rate at which data is sent by this client peer.
        /// </summary>
        /// <returns>The send rate in frames per second.</returns>
        int GetSendRate();

        /// <summary>
        /// Gets the network statistics associated with this client peer.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> containing network statistics.</returns>
        INetworkStatistics GetNetworkStatistics();

        /// <summary>
        /// Sends an operation request to the server with an optional callback for the response.
        /// </summary>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="onResponse">The action to execute when a response is received.</param>
        /// <param name="sendParameters">Optional parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">The timeout period in seconds.</param>
        void Send(OperationRequest operationRequest, Action<OperationResponse> onResponse = null, SendParameters sendParameters = default, int timeoutInSeconds = 15);

        /// <summary>
        /// Asynchronously sends an operation request to the server.
        /// </summary>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">Optional parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">The timeout period in seconds.</param>
        /// <returns>A Task representing the asynchronous operation, containing the server's response.</returns>
        Task<OperationResponse> SendAsync(OperationRequest operationRequest, SendParameters sendParameters = default, int timeoutInSeconds = 15);

        /// <summary>
        /// Services the client peer, processing any pending tasks or requests.
        /// </summary>
        void Service();

        /// <summary>
        /// Sets the debug support for this client peer.
        /// </summary>
        /// <param name="debugSupport">The debug support to set.</param>
        void SetDebugSupport(IDebugSupport debugSupport);

        /// <summary>
        /// Sets the authentication token for this client peer.
        /// </summary>
        /// <param name="authToken">The authentication token to set.</param>
        void SetAuthToken(string authToken);

        /// <summary>
        /// Sets the rate at which data is sent by this client peer.
        /// </summary>
        /// <param name="sendRate">The send rate in frames per second.</param>
        void SetSendRate(int sendRate);

    }

    /// <summary>
    /// Abstract class representing a client peer in the network communication system.
    /// Handles sending and receiving of operation requests, managing response timings, 
    /// and providing debug support.
    /// </summary>
    public abstract class ClientPeer : IClientPeer
    {
        /// <summary>
        /// Prefix used in logging messages specific to this client peer.
        /// </summary>
        protected abstract string logPrefix { get; }

        /// <summary>
        /// Logger instance for logging activities within the client peer.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Counter for the total number of requests sent.
        /// </summary>
        private ushort totalRequestId { get; set; }

        /// <summary>
        /// Lock object to synchronize access to pending operation requests.
        /// </summary>
        private object _lockDelayOperationRequestPendings { get; }

        /// <summary>
        /// Queue to store pending operation requests that are delayed.
        /// </summary>
        private Queue<OperationRequestPending> delayOperationRequestPendings { get; }

        /// <summary>
        /// Lock object to synchronize access to waiting response operation requests.
        /// </summary>
        protected object _lockWaitingResponseOperationRequestPendings { get; }

        /// <summary>
        /// List of operation requests that are awaiting a response from the server.
        /// </summary>
        protected List<OperationRequestPending> waitingResponseOperationRequestPendings { get; }

        /// <summary>
        /// Lock object to synchronize access to operation requests that need to be removed.
        /// </summary>
        protected object _lockNeedRemoveOperationRequestPendings { get; }

        /// <summary>
        /// List of operation requests that need to be removed after processing.
        /// </summary>
        private List<OperationRequestPending> needRemoveOperationRequestPendings { get; }

        /// <summary>
        /// Debug support instance for logging and monitoring operations.
        /// </summary>
        protected IDebugSupport debugSupport { get; set; }

        /// <summary>
        /// Lock object to synchronize access to the timer queue.
        /// </summary>
        private object _lockExecuteTimerInMsQueue { get; }

        /// <summary>
        /// Queue to store execution times in milliseconds.
        /// </summary>
        private Queue<int> executeTimerInMsQueue { get; }

        /// <summary>
        /// Stores the current ping (round-trip time) to the server.
        /// </summary>
        private int currentPing { get; set; }

        /// <summary>
        /// Interval at which updates are sent, in seconds.
        /// </summary>
        protected float updateInterval { get; private set; }

        /// <summary>
        /// Tick count for the next send operation.
        /// </summary>
        long nextSendTickCount = 0;

        /// <summary>
        /// Authentication token used for server communication.
        /// </summary>
        protected string authToken { get; private set; }

        /// <summary>
        /// Address of the server to which this client peer is connected.
        /// </summary>
        protected string serverAddress { get; }

        /// <summary>
        /// Session ID for the current connection session.
        /// </summary>
        protected string sessionId { get; }

        /// <summary>
        /// Unique client ID assigned to this client peer.
        /// </summary>
        protected int clientId { get; }

        /// <summary>
        /// RPC protocol service used by this client peer.
        /// </summary>
        [AutoBind]
        protected IRpcProtocolService rpcProtocolService { get; set; }

        /// <summary>
        /// Options for configuring TCP client connections.
        /// </summary>
        protected TcpClientOptions tcpClientOptions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeer"/> class.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <param name="initRequest">The initial request containing session and client ID information.</param>
        /// <param name="tcpClientOptions">Options for configuring the TCP client.</param>
        public ClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions)
        {
            this.logger = LogManager.GetLogger(this);

            this.totalRequestId = 0;

            this.serverAddress = serverAddress;

            this.sessionId = initRequest.SessionId;

            this._lockDelayOperationRequestPendings = new object();
            this._lockWaitingResponseOperationRequestPendings = new object();
            this._lockNeedRemoveOperationRequestPendings = new object();
            this._lockExecuteTimerInMsQueue = new object();

            this.delayOperationRequestPendings = new Queue<OperationRequestPending>();
            this.waitingResponseOperationRequestPendings = new List<OperationRequestPending>();
            this.needRemoveOperationRequestPendings = new List<OperationRequestPending>();

            this.executeTimerInMsQueue = new Queue<int>();
            this.currentPing = -1;

            this.clientId = initRequest.ClientId;

            this.tcpClientOptions = tcpClientOptions;

            this.updateInterval = 0.016f;
        }

        /// <summary>
        /// Abstract method to retrieve network statistics for the client peer.
        /// Must be implemented by derived classes.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> containing network statistics.</returns>
        public abstract INetworkStatistics GetNetworkStatistics();

        /// <summary>
        /// Gets the unique client ID for this client peer.
        /// </summary>
        /// <returns>The client ID as an integer.</returns>
        public int GetClientId() => this.clientId;

        /// <summary>
        /// Gets the current ping (round-trip time) to the server.
        /// </summary>
        /// <returns>The ping in milliseconds.</returns>
        public int GetPing() => this.currentPing;

        /// <summary>
        /// Gets the rate at which data is sent by this client peer.
        /// </summary>
        /// <returns>The send rate in frames per second.</returns>
        public int GetSendRate() => (int)(1000 / this.updateInterval);

        /// <summary>
        /// Sets the rate at which data is sent by this client peer.
        /// </summary>
        /// <param name="sendRate">The send rate in frames per second.</param>
        public void SetSendRate(int sendRate) => this.updateInterval = (float)1000 / sendRate / 1000;

        /// <summary>
        /// Calculates the average ping time based on recent execution times.
        /// </summary>
        /// <returns>The average ping in milliseconds.</returns>
        private int GetAveragePing()
        {
            IEnumerable<int> cloneExecuteTimerInMsQueue;
            int count;

            lock (this._lockExecuteTimerInMsQueue)
            {
                count = this.executeTimerInMsQueue.Count;
                if (count == 0) return -1;

                cloneExecuteTimerInMsQueue = new List<int>();
            }

            var sumPing = 0;
            foreach (var executeTimerInMsQueue in cloneExecuteTimerInMsQueue)
                sumPing += executeTimerInMsQueue;

            return sumPing / count;
        }

        /// <summary>
        /// Checks for operation requests that are waiting for a response and processes them.
        /// </summary>
        private void CheckWaitingResponseOperationRequestPending()
        {
            IEnumerable<OperationRequestPending> cloneWaitingResponseOperationRequestPendings;

            lock (this._lockWaitingResponseOperationRequestPendings)
            {
                if (this.waitingResponseOperationRequestPendings.Count == 0) return;

                cloneWaitingResponseOperationRequestPendings = new List<OperationRequestPending>(this.waitingResponseOperationRequestPendings);
            }

            foreach (var operationRequestPending in cloneWaitingResponseOperationRequestPendings)
            {
                if (operationRequestPending.GetOperationResponse() == null)
                {
                    if (operationRequestPending.IsTimeout())
                    {
                        operationRequestPending.OnRecv();

                        var request = operationRequestPending.GetOperationRequest();

                        var response = new OperationResponse(request.OperationCode)
                        {
                            ReturnCode = ReturnCode.OperationTimeout,
                            ResponseId = request.RequestId,
                        };

                        operationRequestPending.SetOperationResponse(response);
                        operationRequestPending.SetResponseSendParameters(operationRequestPending.GetSendParameters());
                    }
                }
                else
                {
                    this.LogRecv(operationRequestPending);

                    lock (this._lockExecuteTimerInMsQueue)
                    {
                        if (this.executeTimerInMsQueue.Count >= 20) this.executeTimerInMsQueue.Dequeue();
                        this.executeTimerInMsQueue.Enqueue(operationRequestPending.GetExecuteTimerInMs());
                    }

                    this.currentPing = this.GetAveragePing();

                    lock (this._lockNeedRemoveOperationRequestPendings)
                        this.needRemoveOperationRequestPendings.Add(operationRequestPending);
                }
            }
        }

        /// <summary>
        /// Checks and removes operation requests that have been processed.
        /// </summary>
        private void CheckNeedRemoveOperationRequestPending()
        {
            IEnumerable<OperationRequestPending> cloneOperationRequestPendings;

            lock (this._lockNeedRemoveOperationRequestPendings)
            {
                if (this.needRemoveOperationRequestPendings.Count == 0) return;

                cloneOperationRequestPendings = new List<OperationRequestPending>(this.needRemoveOperationRequestPendings);

                this.needRemoveOperationRequestPendings.Clear();
            }

            foreach (var operationRequestPending in cloneOperationRequestPendings)
            {
                try
                {
                    operationRequestPending.GetCallback()?.Invoke(operationRequestPending.GetOperationResponse());
                }
                catch (Exception exception)
                {
                    this.logger.Fatal(exception);
                }

                lock (this._lockWaitingResponseOperationRequestPendings)
                    this.waitingResponseOperationRequestPendings.Remove(operationRequestPending);
            }

        }

        /// <summary>
        /// Sends any delayed operation requests when the appropriate time has elapsed.
        /// </summary>
        private void SendDelayOperationRequestPending()
        {
            OperationRequestPending operationRequestPending;

            lock (this._lockDelayOperationRequestPendings)
            {
                if (this.delayOperationRequestPendings.Count == 0) return;

                operationRequestPending = this.delayOperationRequestPendings.Dequeue();
            }

            var milliseconds = DateTime.UtcNow.Ticks;

            if (this.nextSendTickCount > milliseconds) return;

            this.nextSendTickCount = milliseconds + (int)(this.updateInterval * 10000000);

            this.AddWaitingResponseOperationRequestPending(operationRequestPending);

            operationRequestPending.OnSend();

            this.LogSend(operationRequestPending);

            this.SendOperation(operationRequestPending);
        }

        /// <summary>
        /// Sends an operation request to the server.
        /// </summary>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="onOperationResponse">Callback for handling the server's response.</param>
        /// <param name="sendParameters">Parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">Timeout for the request in seconds.</param>
        public void Send(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse, SendParameters sendParameters, int timeoutInSeconds)
        {
            this.Enqueue(operationRequest, onOperationResponse, sendParameters, timeoutInSeconds);
        }

        /// <summary>
        /// Asynchronously sends an operation request to the server.
        /// </summary>
        /// <param name="operationRequest">The operation request to send.</param>
        /// <param name="sendParameters">Parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">Timeout for the request in seconds.</param>
        /// <returns>A Task representing the asynchronous operation, containing the server's response.</returns>
        public Task<OperationResponse> SendAsync(OperationRequest operationRequest, SendParameters sendParameters = default, int timeoutInSeconds = 15)
        {
            var taskCompletionSource = new TaskCompletionSource<OperationResponse>();

            this.Enqueue(operationRequest, operationResponse =>
            {
                taskCompletionSource.SetResult(operationResponse);
            }, sendParameters, timeoutInSeconds);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Enqueues an operation request for sending.
        /// </summary>
        /// <param name="operationRequest">The operation request to enqueue.</param>
        /// <param name="onOperationResponse">Callback for handling the server's response.</param>
        /// <param name="sendParameters">Parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">Timeout for the request in seconds.</param>
        private void Enqueue(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse, SendParameters sendParameters, int timeoutInSeconds)
        {
            if (sendParameters == null) sendParameters = new SendParameters();

            var operationRequestPending = this.CreateNewOperationRequestPending(operationRequest, onOperationResponse, sendParameters, timeoutInSeconds);

            this.LogEnqueue(operationRequestPending);

            if (sendParameters.Immediately)
            {
                this.AddWaitingResponseOperationRequestPending(operationRequestPending);

                operationRequestPending.OnSend();

                this.LogSend(operationRequestPending);

                this.SendOperation(operationRequestPending);
            }
            else
            {
                lock (this._lockDelayOperationRequestPendings)
                    this.delayOperationRequestPendings.Enqueue(operationRequestPending);
            }

        }

        /// <summary>
        /// Adds an operation request to the list of those waiting for a response.
        /// </summary>
        /// <param name="operationRequestPending">The operation request to add.</param>
        protected void AddWaitingResponseOperationRequestPending(OperationRequestPending operationRequestPending)
        {
            lock (this._lockWaitingResponseOperationRequestPendings)
                this.waitingResponseOperationRequestPendings.Add(operationRequestPending);
        }

        /// <summary>
        /// Creates a new instance of <see cref="OperationRequestPending"/> for the given operation request.
        /// </summary>
        /// <param name="operationRequest">The operation request to process.</param>
        /// <param name="onOperationResponse">Callback for handling the server's response.</param>
        /// <param name="sendParameters">Parameters for sending the request.</param>
        /// <param name="timeoutInSeconds">Timeout for the request in seconds.</param>
        /// <returns>A new instance of OperationRequestPending.</returns>
        protected OperationRequestPending CreateNewOperationRequestPending(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse, SendParameters sendParameters, int timeoutInSeconds)
        {
            operationRequest.RequestId = ++this.totalRequestId;

            return new OperationRequestPending(operationRequest, onOperationResponse, sendParameters, timeoutInSeconds);
        }

        /// <summary>
        /// Determines whether the client peer is currently connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        public virtual bool IsConnected() => false;

        /// <summary>
        /// Services the client peer, processing any pending tasks or requests.
        /// </summary>
        public virtual void Service()
        {
            this.CheckWaitingResponseOperationRequestPending();

            this.CheckNeedRemoveOperationRequestPending();

            if (!this.IsConnected()) return;

            this.SendDelayOperationRequestPending();
        }

        /// <summary>
        /// Abstract method to send an operation request to the server.
        /// Must be implemented by derived classes.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to send.</param>
        protected abstract void SendOperation(OperationRequestPending operationRequestPending);

        /// <summary>
        /// Sets the debug support for this client peer.
        /// </summary>
        /// <param name="debugSupport">The debug support to set.</param>
        public void SetDebugSupport(IDebugSupport debugSupport) => this.debugSupport = debugSupport;

        /// <summary>
        /// Logs the enqueuing of an operation request.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to log.</param>
        protected void LogEnqueue(OperationRequestPending operationRequestPending)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][ENQUEUE] {this.debugSupport.ToStringOnEnqueue(operationRequestPending)}");
        }

        /// <summary>
        /// Logs the sending of an operation request.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to log.</param>
        protected void LogSend(OperationRequestPending operationRequestPending)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][SEND] {this.debugSupport.ToStringOnSend(operationRequestPending)}");
        }

        /// <summary>
        /// Logs the reception of an operation response.
        /// </summary>
        /// <param name="operationRequestPending">The pending operation request to log.</param>
        protected void LogRecv(OperationRequestPending operationRequestPending)
        {
            if (this.debugSupport != null)
                this.logger.Info($"[{this.logPrefix}][RECV] {this.debugSupport.ToStringOnRecv(operationRequestPending)}");
        }

        /// <summary>
        /// Sets the authentication token for this client peer.
        /// </summary>
        /// <param name="authToken">The authentication token to set.</param>
        public void SetAuthToken(string authToken)
        {
            this.authToken = authToken;
        }

    }

}

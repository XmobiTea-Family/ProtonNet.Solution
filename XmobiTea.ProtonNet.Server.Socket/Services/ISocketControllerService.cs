using XmobiTea.Bean.Attributes;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Socket.Controllers;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.Socket.Services
{
    /// <summary>
    /// Defines the interface for a service that manages socket controller operations, 
    /// including handling connections, data reception, disconnections, and errors for socket sessions.
    /// </summary>
    public interface ISocketControllerService
    {
        /// <summary>
        /// Invoked when a session is connected.
        /// </summary>
        /// <param name="session">The session that has been connected.</param>
        void OnConnected(ISocketSession session);

        /// <summary>
        /// Invoked when data is received from a session.
        /// </summary>
        /// <param name="session">The session that received the data.</param>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        void OnReceived(ISocketSession session, byte[] buffer, int position, int length);

        /// <summary>
        /// Invoked when a session is disconnected.
        /// </summary>
        /// <param name="session">The session that has been disconnected.</param>
        void OnDisconnected(ISocketSession session);

        /// <summary>
        /// Invoked when a socket error occurs.
        /// </summary>
        /// <param name="session">The session in which the error occurred.</param>
        /// <param name="error">The socket error that occurred.</param>
        void OnError(ISocketSession session, System.Net.Sockets.SocketError error);

    }

    /// <summary>
    /// Implements the <see cref="ISocketControllerService"/> interface to manage socket session events such as 
    /// connection, data reception, disconnection, and errors. This service coordinates the handling of these events 
    /// through registered socket controllers.
    /// </summary>
    class SocketControllerService : ISocketControllerService
    {
        /// <summary>
        /// Represents the tracking information for a session, including the timestamp of the last tick,
        /// the number of requests received in the current second, and the number of pending requests.
        /// </summary>
        class SessionPerSecondAmount
        {
            /// <summary>
            /// Gets or sets the timestamp of the last tick in ticks (1 tick = 100 nanoseconds).
            /// </summary>
            public long LastTick;

            /// <summary>
            /// Gets or sets the number of requests received in the current second.
            /// </summary>
            public int AmountInCurrentSecond;

            /// <summary>
            /// Gets or sets the number of pending requests for the session.
            /// </summary>
            public int PendingRequest;

        }

        /// <summary>
        /// Logger instance for logging information, warnings, and errors related to the socket controller service.
        /// </summary>
        private ILogger logger { get; }

        /// <summary>
        /// A list of socket controllers that manage the handling of socket session events.
        /// </summary>
        private System.Collections.Generic.IList<SocketController> socketControllerLst { get; }

        /// <summary>
        /// A dictionary mapping each session to its corresponding <see cref="SessionPerSecondAmount"/> to track the session's activity.
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<ISocketSession, SessionPerSecondAmount> sessionReceiveAtTimeAmountDict { get; }

        /// <summary>
        /// Automatically binds the socket session emit service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private ISocketSessionEmitService socketSessionEmitService { get; set; }

        /// <summary>
        /// The maximum number of concurrent sessions allowed on the server.
        /// </summary>
        private int maxSession { get; set; }

        /// <summary>
        /// The current total number of sessions connected to the server.
        /// </summary>
        private int totalSession;

        /// <summary>
        /// The maximum number of pending requests that can be queued for processing.
        /// </summary>
        private int maxPendingRequest { get; set; }

        /// <summary>
        /// The current total number of pending requests.
        /// </summary>
        private int pendingRequest;

        /// <summary>
        /// The maximum number of requests per second that a single session can send.
        /// </summary>
        private int maxSessionRequestPerSecond { get; set; }

        /// <summary>
        /// The maximum number of pending requests that a single session can have at any given time.
        /// </summary>
        private int maxSessionPendingRequest { get; set; }

        /// <summary>
        /// The fiber used for handling non-received events, utilizing a specified number of threads.
        /// </summary>
        private IFiber otherFiber { get; set; }

        /// <summary>
        /// The fiber used for handling received events, utilizing a specified number of threads.
        /// </summary>
        private IFiber receivedFiber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketControllerService"/> class.
        /// </summary>
        public SocketControllerService()
        {
            this.logger = LogManager.GetLogger(this);
            this.socketControllerLst = new System.Collections.Generic.List<SocketController>();
            this.sessionReceiveAtTimeAmountDict = new System.Collections.Concurrent.ConcurrentDictionary<ISocketSession, SessionPerSecondAmount>();
        }

        /// <summary>
        /// Sets the fiber for handling non-received events with a specified number of threads.
        /// </summary>
        /// <param name="threadCount">The number of threads to use for the fiber.</param>
        internal void SetOtherFiber(int threadCount)
        {
            var roundRobinFiber = new RoundRobinFiber("OtherFiber", threadCount);
            roundRobinFiber.Start();
            this.otherFiber = roundRobinFiber;
        }

        /// <summary>
        /// Sets the fiber for handling received events with a specified number of threads.
        /// </summary>
        /// <param name="threadCount">The number of threads to use for the fiber.</param>
        internal void SetReceiveFiber(int threadCount)
        {
            var roundRobinFiber = new RoundRobinFiber("ReceivedFiber", threadCount);
            roundRobinFiber.Start();
            this.receivedFiber = roundRobinFiber;
        }

        /// <summary>
        /// Sets the maximum number of concurrent sessions allowed on the server.
        /// </summary>
        /// <param name="maxSession">The maximum number of sessions.</param>
        internal void SetMaxSession(int maxSession) => this.maxSession = maxSession;

        /// <summary>
        /// Sets the maximum number of pending requests that can be queued for processing.
        /// </summary>
        /// <param name="maxPendingRequest">The maximum number of pending requests.</param>
        internal void SetMaxPendingRequest(int maxPendingRequest) => this.maxPendingRequest = maxPendingRequest;

        /// <summary>
        /// Sets the maximum number of requests per second that a single session can send.
        /// </summary>
        /// <param name="maxSessionRequestPerSecond">The maximum number of requests per second per session.</param>
        internal void SetMaxSessionRequestPerSecond(int maxSessionRequestPerSecond) => this.maxSessionRequestPerSecond = maxSessionRequestPerSecond;

        /// <summary>
        /// Sets the maximum number of pending requests that a single session can have at any given time.
        /// </summary>
        /// <param name="maxSessionPendingRequest">The maximum number of pending requests per session.</param>
        internal void SetMaxSessionPendingRequest(int maxSessionPendingRequest) => this.maxSessionPendingRequest = maxSessionPendingRequest;

        /// <summary>
        /// Adds a socket controller to the service.
        /// </summary>
        /// <param name="socketController">The socket controller to add.</param>
        internal void AddSocketController(SocketController socketController)
        {
            this.socketControllerLst.Add(socketController);
        }

        /// <summary>
        /// Handles a new session connection event.
        /// </summary>
        /// <param name="session">The session that has connected.</param>
        public void OnConnected(ISocketSession session)
        {
            var currentSession = System.Threading.Interlocked.Increment(ref this.totalSession);

            if (currentSession > this.maxSession)
            {
                this.logger.Warn($"max session connect in server, drop because max pending request, current: {currentSession}, maxSession: {this.maxSession}");

                var operationDisconnect = new OperationDisconnect()
                {
                    Reason = (byte)DisconnectReason.MaxSession,
                };

                this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect);
                session.Disconnect(500);

                return;
            }

            this.otherFiber.Enqueue(() =>
            {
                foreach (var socketController in this.socketControllerLst)
                {
                    socketController.OnConnected(session);
                }
            });
        }

        /// <summary>
        /// Handles a data reception event for a session.
        /// </summary>
        /// <param name="session">The session that received the data.</param>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        public void OnReceived(ISocketSession session, byte[] buffer, int position, int length)
        {
            // Check current pending request 
            var pendingRequest = System.Threading.Interlocked.Increment(ref this.pendingRequest);
            if (pendingRequest > this.maxPendingRequest)
            {
                System.Threading.Interlocked.Decrement(ref this.pendingRequest);
                this.logger.Warn($"buffer drop because max pending request, current: {pendingRequest}, maxPendingRequest: {this.maxPendingRequest}");
                return;
            }

            // Check current session info
            if (!this.sessionReceiveAtTimeAmountDict.TryGetValue(session, out var sessionPerSecondAmount))
            {
                sessionPerSecondAmount = new SessionPerSecondAmount();
                this.sessionReceiveAtTimeAmountDict[session] = sessionPerSecondAmount;
            }

            // At one second, check how much amountInCurrentSecond this session has sent
            if (sessionPerSecondAmount.LastTick < System.DateTime.UtcNow.Ticks)
            {
                System.Threading.Interlocked.Exchange(ref sessionPerSecondAmount.AmountInCurrentSecond, 0);
                System.Threading.Interlocked.Exchange(ref sessionPerSecondAmount.LastTick, System.DateTime.UtcNow.Ticks + 10000000);
            }

            var sessionAmountInCurrentSecond = System.Threading.Interlocked.Increment(ref sessionPerSecondAmount.AmountInCurrentSecond);
            if (sessionAmountInCurrentSecond > this.maxSessionRequestPerSecond)
            {
                this.logger.Warn($"buffer drop because max request per session per second, current: {sessionAmountInCurrentSecond}, maxSessionRequestPerSecond: {this.maxSessionRequestPerSecond}");
                return;
            }

            // Check session pending request
            var sessionPendingRequest = System.Threading.Interlocked.Increment(ref sessionPerSecondAmount.PendingRequest);
            if (sessionPendingRequest > this.maxSessionPendingRequest)
            {
                sessionPendingRequest = System.Threading.Interlocked.Decrement(ref sessionPerSecondAmount.PendingRequest);
                this.logger.Warn($"buffer drop because max pending request, current: {sessionPendingRequest}, maxSessionPendingRequest: {this.maxSessionPendingRequest}");
                return;
            }

            this.receivedFiber.Enqueue(() =>
            {
                foreach (var socketController in this.socketControllerLst)
                {
                    try
                    {
                        socketController.OnReceived(session, buffer, position, length);
                    }
                    catch (System.Exception ex)
                    {
                        this.logger.Fatal($"handle at {socketController} exception", ex);
                    }
                }

                System.Threading.Interlocked.Decrement(ref this.pendingRequest);
                sessionPendingRequest = System.Threading.Interlocked.Decrement(ref sessionPerSecondAmount.PendingRequest);
            });
        }

        /// <summary>
        /// Handles a socket error event for a session.
        /// </summary>
        /// <param name="session">The session where the error occurred.</param>
        /// <param name="error">The socket error that occurred.</param>
        public void OnError(ISocketSession session, System.Net.Sockets.SocketError error)
        {
            this.otherFiber.Enqueue(() =>
            {
                foreach (var socketController in this.socketControllerLst)
                {
                    socketController.OnError(session, error);
                }
            });
        }

        /// <summary>
        /// Handles a session disconnection event.
        /// </summary>
        /// <param name="session">The session that has been disconnected.</param>
        public void OnDisconnected(ISocketSession session)
        {
            System.Threading.Interlocked.Decrement(ref this.totalSession);
            this.sessionReceiveAtTimeAmountDict.TryRemove(session, out _);

            this.otherFiber.Enqueue(() =>
            {
                foreach (var socketController in this.socketControllerLst)
                {
                    socketController.OnDisconnected(session);
                }
            });
        }

    }

}

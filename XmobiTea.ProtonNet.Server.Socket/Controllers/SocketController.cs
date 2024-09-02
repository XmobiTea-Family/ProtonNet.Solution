using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.Socket.Sessions;

namespace XmobiTea.ProtonNet.Server.Socket.Controllers
{
    /// <summary>
    /// Represents an abstract base class for socket controllers that handle various events
    /// such as connection, data reception, errors, and disconnections.
    /// </summary>
    public abstract class SocketController
    {
        /// <summary>
        /// Gets the logger instance used for logging information, warnings, and errors.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketController"/> class.
        /// </summary>
        protected SocketController() => this.logger = LogManager.GetLogger(this);

        /// <summary>
        /// Called when a session is connected. Can be overridden by derived classes to handle connection logic.
        /// </summary>
        /// <param name="session">The session that was connected.</param>
        public virtual void OnConnected(ISocketSession session) { }

        /// <summary>
        /// Called when data is received from a session. Can be overridden by derived classes to handle data reception logic.
        /// </summary>
        /// <param name="session">The session that received the data.</param>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the received data starts.</param>
        /// <param name="length">The length of the received data in the buffer.</param>
        public virtual void OnReceived(ISocketSession session, byte[] buffer, int position, int length) { }

        /// <summary>
        /// Called when an error occurs in a session. Can be overridden by derived classes to handle error logic.
        /// </summary>
        /// <param name="session">The session where the error occurred.</param>
        /// <param name="error">The socket error that occurred.</param>
        public virtual void OnError(ISocketSession session, System.Net.Sockets.SocketError error) { }

        /// <summary>
        /// Called when a session is disconnected. Can be overridden by derived classes to handle disconnection logic.
        /// </summary>
        /// <param name="session">The session that was disconnected.</param>
        public virtual void OnDisconnected(ISocketSession session) { }

    }

}

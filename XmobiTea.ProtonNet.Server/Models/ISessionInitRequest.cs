using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Defines the properties required for a session initialization request.
    /// </summary>
    public interface ISessionInitRequest
    {
        /// <summary>
        /// Gets the connection ID.
        /// </summary>
        int ConnectionId { get; }

        /// <summary>
        /// Gets the server session ID.
        /// </summary>
        string ServerSessionId { get; }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// Gets the fiber associated with the session.
        /// </summary>
        IFiber Fiber { get; }

    }

    /// <summary>
    /// Implements the <see cref="ISessionInitRequest"/> interface for session initialization requests.
    /// </summary>
    public class SessionInitRequest : ISessionInitRequest
    {
        /// <summary>
        /// Gets or sets the connection ID.
        /// </summary>
        public int ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the server session ID.
        /// </summary>
        public string ServerSessionId { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the fiber associated with the session.
        /// </summary>
        public IFiber Fiber { get; set; }

    }

}

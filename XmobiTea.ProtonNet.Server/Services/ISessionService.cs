using System.Collections.Generic;
using XmobiTea.Collections.Generic;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines the service for managing sessions.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Gets the total number of sessions.
        /// </summary>
        /// <returns>The number of sessions.</returns>
        int GetSessionCount();

        /// <summary>
        /// Gets the number of sessions associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The number of sessions for the given session ID.</returns>
        int GetSessionCount(string sessionId);

        /// <summary>
        /// Maps a session to a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="session">The session to map.</param>
        void MapSession(string sessionId, ISession session);

        /// <summary>
        /// Removes a session.
        /// </summary>
        /// <param name="session">The session to remove.</param>
        void RemoveSession(ISession session);

        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <returns>An enumerable collection of all sessions.</returns>
        IEnumerable<ISession> GetSessions();

        /// <summary>
        /// Gets all sessions associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>An enumerable collection of sessions for the given session ID.</returns>
        IEnumerable<ISession> GetSessions(string sessionId);
    }

    /// <summary>
    /// Implements <see cref="ISessionService"/> to manage sessions.
    /// </summary>
    public class SessionService : ISessionService
    {
        private IList<ISession> sessionLst { get; }
        private IDictionary<string, IList<ISession>> sessionIdWithSessionDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionService"/> class.
        /// </summary>
        public SessionService()
        {
            this.sessionLst = new ThreadSafeList<ISession>();
            this.sessionIdWithSessionDict = new ThreadSafeDictionary<string, IList<ISession>>();
        }

        /// <summary>
        /// Maps a session to a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="session">The session to map.</param>
        public void MapSession(string sessionId, ISession session)
        {
            if (!this.sessionLst.Contains(session)) this.sessionLst.Add(session);

            if (!this.sessionIdWithSessionDict.TryGetValue(sessionId, out var sessionLst))
            {
                sessionLst = new ThreadSafeList<ISession>();
                this.sessionIdWithSessionDict[sessionId] = sessionLst;
            }

            if (!sessionLst.Contains(session)) sessionLst.Add(session);
        }

        /// <summary>
        /// Gets the total number of sessions.
        /// </summary>
        /// <returns>The number of sessions.</returns>
        public int GetSessionCount() => this.sessionLst.Count;

        /// <summary>
        /// Gets the number of sessions associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The number of sessions for the given session ID.</returns>
        public int GetSessionCount(string sessionId)
        {
            if (!this.sessionIdWithSessionDict.TryGetValue(sessionId, out var sessionLst)) return 0;

            return sessionLst.Count;
        }

        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <returns>An enumerable collection of all sessions.</returns>
        public IEnumerable<ISession> GetSessions() => this.sessionLst;

        /// <summary>
        /// Gets all sessions associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>An enumerable collection of sessions for the given session ID.</returns>
        public IEnumerable<ISession> GetSessions(string sessionId)
        {
            this.sessionIdWithSessionDict.TryGetValue(sessionId, out var sessionLst);

            return sessionLst;
        }

        /// <summary>
        /// Removes a session.
        /// </summary>
        /// <param name="session">The session to remove.</param>
        public void RemoveSession(ISession session)
        {
            this.sessionLst.Remove(session);

            var sessionId = session.GetSessionId();

            if (!string.IsNullOrEmpty(sessionId))
                if (this.sessionIdWithSessionDict.TryGetValue(sessionId, out var sessionLst))
                {
                    sessionLst.Remove(session);

                    if (sessionLst.Count == 0) this.sessionIdWithSessionDict.Remove(sessionId);
                }
        }

    }

}

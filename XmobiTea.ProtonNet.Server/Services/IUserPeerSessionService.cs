using System.Collections.Generic;
using XmobiTea.Collections.Generic;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines the service for managing user peers associated with session IDs.
    /// </summary>
    public interface IUserPeerSessionService
    {
        /// <summary>
        /// Gets the count of user peers.
        /// </summary>
        /// <returns>The number of user peers.</returns>
        int GetUserPeerCount();

        /// <summary>
        /// Gets the user peer associated with a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the user peer.</param>
        /// <returns>The user peer associated with the session ID.</returns>
        IUserPeer GetUserPeer(string sessionId);

        /// <summary>
        /// Maps a session ID to a user peer.
        /// </summary>
        /// <param name="sessionId">The session ID to map.</param>
        /// <param name="userPeer">The user peer to associate with the session ID.</param>
        void MapUserPeer(string sessionId, IUserPeer userPeer);

        /// <summary>
        /// Removes the user peer associated with a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the user peer to remove.</param>
        void RemoveUserPeer(string sessionId);

        /// <summary>
        /// Gets all user peers.
        /// </summary>
        /// <returns>A collection of all user peers.</returns>
        IEnumerable<IUserPeer> GetUserPeers();

    }

    /// <summary>
    /// Implements <see cref="IUserPeerSessionService"/> to manage user peers associated with session IDs.
    /// </summary>
    public class UserPeerSessionService : IUserPeerSessionService
    {
        private IDictionary<string, IUserPeer> sessionIdWithUserPeerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPeerSessionService"/> class.
        /// </summary>
        public UserPeerSessionService()
        {
            this.sessionIdWithUserPeerDict = new ThreadSafeDictionary<string, IUserPeer>();
        }

        /// <summary>
        /// Gets the count of user peers.
        /// </summary>
        /// <returns>The number of user peers.</returns>
        public int GetUserPeerCount() => this.sessionIdWithUserPeerDict.Count;

        /// <summary>
        /// Gets the user peer associated with a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the user peer.</param>
        /// <returns>The user peer associated with the session ID.</returns>
        public IUserPeer GetUserPeer(string sessionId)
        {
            this.sessionIdWithUserPeerDict.TryGetValue(sessionId, out var userPeer);
            return userPeer;
        }

        /// <summary>
        /// Maps a session ID to a user peer.
        /// </summary>
        /// <param name="sessionId">The session ID to map.</param>
        /// <param name="userPeer">The user peer to associate with the session ID.</param>
        public void MapUserPeer(string sessionId, IUserPeer userPeer) => this.sessionIdWithUserPeerDict[sessionId] = userPeer;

        /// <summary>
        /// Removes the user peer associated with a session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the user peer to remove.</param>
        public void RemoveUserPeer(string sessionId) => this.sessionIdWithUserPeerDict.Remove(sessionId);

        /// <summary>
        /// Gets all user peers.
        /// </summary>
        /// <returns>A collection of all user peers.</returns>
        public IEnumerable<IUserPeer> GetUserPeers() => this.sessionIdWithUserPeerDict.Values;

    }

}

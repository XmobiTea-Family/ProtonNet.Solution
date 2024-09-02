using XmobiTea.Collections.Generic;
using XmobiTea.Linq;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines the service for managing user peers in the system.
    /// </summary>
    public interface IUserPeerService
    {
        /// <summary>
        /// Gets the count of user peers.
        /// </summary>
        /// <returns>The number of user peers.</returns>
        int Count();

        /// <summary>
        /// Maps a user ID to a user peer.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="userPeer">The user peer to associate with the user ID.</param>
        void MapUserPeer(string userId, IUserPeer userPeer);

        /// <summary>
        /// Removes the user peer associated with a user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        void RemoveUserPeer(string userId);

        /// <summary>
        /// Gets the user peer associated with a user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user peer associated with the user ID.</returns>
        IUserPeer GetUserPeer(string userId);

        /// <summary>
        /// Gets the user peer that matches a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>The user peer that matches the predicate.</returns>
        IUserPeer GetUserPeer(System.Predicate<IUserPeer> match);

        /// <summary>
        /// Gets all user IDs.
        /// </summary>
        /// <returns>A collection of all user IDs.</returns>
        System.Collections.Generic.IEnumerable<string> GetUserIds();

        /// <summary>
        /// Gets all user peers.
        /// </summary>
        /// <returns>A collection of all user peers.</returns>
        System.Collections.Generic.IEnumerable<IUserPeer> GetUserPeers();

        /// <summary>
        /// Gets user peers that match a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>A collection of user peers that match the predicate.</returns>
        System.Collections.Generic.IEnumerable<IUserPeer> GetUserPeers(System.Predicate<IUserPeer> match);

        /// <summary>
        /// Gets user IDs that match a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>A collection of user IDs that match the predicate.</returns>
        System.Collections.Generic.IEnumerable<string> GetUserIds(System.Predicate<IUserPeer> match);

    }

    /// <summary>
    /// Implements <see cref="IUserPeerService"/> to manage user peers.
    /// </summary>
    public class UserPeerService : IUserPeerService
    {
        /// <summary>
        /// Gets a dictionary that maps user IDs to their corresponding <see cref="IUserPeer"/> instances.
        /// </summary>
        protected System.Collections.Generic.IDictionary<string, IUserPeer> userIdWithUserPeerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPeerService"/> class.
        /// </summary>
        public UserPeerService()
        {
            this.userIdWithUserPeerDict = new ThreadSafeDictionary<string, IUserPeer>();
        }

        /// <summary>
        /// Gets the count of user peers.
        /// </summary>
        /// <returns>The number of user peers.</returns>
        public int Count() => this.userIdWithUserPeerDict.Count;

        /// <summary>
        /// Maps a user ID to a user peer.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="userPeer">The user peer to associate with the user ID.</param>
        public void MapUserPeer(string userId, IUserPeer userPeer) => this.userIdWithUserPeerDict[userId] = userPeer;

        /// <summary>
        /// Removes the user peer associated with a user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public void RemoveUserPeer(string userId) => this.userIdWithUserPeerDict.Remove(userId);

        /// <summary>
        /// Gets the user peer associated with a user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user peer associated with the user ID.</returns>
        public IUserPeer GetUserPeer(string userId)
        {
            this.userIdWithUserPeerDict.TryGetValue(userId, out var value);
            return value;
        }

        /// <summary>
        /// Gets the user peer that matches a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>The user peer that matches the predicate.</returns>
        public IUserPeer GetUserPeer(System.Predicate<IUserPeer> match) => this.userIdWithUserPeerDict.Values.Find(match);

        /// <summary>
        /// Gets all user IDs.
        /// </summary>
        /// <returns>A collection of all user IDs.</returns>
        public System.Collections.Generic.IEnumerable<string> GetUserIds() => this.userIdWithUserPeerDict.Keys;

        /// <summary>
        /// Gets all user peers.
        /// </summary>
        /// <returns>A collection of all user peers.</returns>
        public System.Collections.Generic.IEnumerable<IUserPeer> GetUserPeers() => this.userIdWithUserPeerDict.Values;

        /// <summary>
        /// Gets user peers that match a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>A collection of user peers that match the predicate.</returns>
        public System.Collections.Generic.IEnumerable<IUserPeer> GetUserPeers(System.Predicate<IUserPeer> match) => this.userIdWithUserPeerDict.Values.Where(match);

        /// <summary>
        /// Gets user IDs that match a specified predicate.
        /// </summary>
        /// <param name="match">The predicate to match against user peers.</param>
        /// <returns>A collection of user IDs that match the predicate.</returns>
        public System.Collections.Generic.IEnumerable<string> GetUserIds(System.Predicate<IUserPeer> match) => this.GetUserPeers(match).Select(x => x.GetUserId());

    }

}

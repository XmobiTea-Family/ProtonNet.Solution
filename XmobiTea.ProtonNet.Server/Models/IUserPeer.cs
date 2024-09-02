using XmobiTea.ProtonNet.Server.Types;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Defines methods for setting user peer properties.
    /// </summary>
    public interface ISetterUserPeer
    {
        /// <summary>
        /// Sets the user ID.
        /// </summary>
        /// <param name="userId">The user ID to set.</param>
        void SetUserId(string userId);

        /// <summary>
        /// Sets the session ID.
        /// </summary>
        /// <param name="sessionId">The session ID to set.</param>
        void SetSessionId(string sessionId);

        /// <summary>
        /// Sets the peer type.
        /// </summary>
        /// <param name="peerType">The peer type to set.</param>
        void SetPeerType(PeerType peerType);

    }

    /// <summary>
    /// Defines methods for accessing user peer properties.
    /// </summary>
    public interface IUserPeer
    {
        /// <summary>
        /// Gets the session ID.
        /// </summary>
        /// <returns>The session ID.</returns>
        string GetSessionId();

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <returns>The user ID.</returns>
        string GetUserId();

        /// <summary>
        /// Gets the peer type.
        /// </summary>
        /// <returns>The peer type.</returns>
        PeerType GetPeerType();

        /// <summary>
        /// Gets the property associated with the user peer.
        /// </summary>
        /// <returns>The property associated with the user peer.</returns>
        IProperty GetProperty();

        /// <summary>
        /// Checks if the user peer is authenticated.
        /// </summary>
        /// <returns>True if authenticated; otherwise, false.</returns>
        bool IsAuthenticated();

    }

    /// <summary>
    /// Implements the <see cref="IUserPeer"/> and <see cref="ISetterUserPeer"/> interfaces for managing user peer data.
    /// </summary>
    public class UserPeer : IUserPeer, ISetterUserPeer
    {
        private string sessionId { get; set; }
        private string userId { get; set; }
        private bool isAuthenticated { get; set; }
        private PeerType peerType { get; set; }
        private IProperty property { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPeer"/> class.
        /// </summary>
        public UserPeer() => this.property = new Property();

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <returns>The user ID.</returns>
        public string GetUserId() => this.userId;

        /// <summary>
        /// Sets the user ID.
        /// </summary>
        /// <param name="userId">The user ID to set.</param>
        public void SetUserId(string userId) => this.userId = userId;

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        /// <returns>The session ID.</returns>
        public string GetSessionId() => this.sessionId;

        /// <summary>
        /// Sets the session ID.
        /// </summary>
        /// <param name="sessionId">The session ID to set.</param>
        public void SetSessionId(string sessionId) => this.sessionId = sessionId;

        /// <summary>
        /// Gets the peer type.
        /// </summary>
        /// <returns>The peer type.</returns>
        public PeerType GetPeerType() => this.peerType;

        /// <summary>
        /// Sets the peer type.
        /// </summary>
        /// <param name="peerType">The peer type to set.</param>
        public void SetPeerType(PeerType peerType) => this.peerType = peerType;

        /// <summary>
        /// Checks if the user peer is authenticated.
        /// </summary>
        /// <returns>True if authenticated; otherwise, false.</returns>
        public bool IsAuthenticated() => this.isAuthenticated;

        /// <summary>
        /// Sets the authenticated state.
        /// </summary>
        /// <param name="isAuthenticated">The authenticated state to set.</param>
        public void SetAuthenticated(bool isAuthenticated) => this.isAuthenticated = isAuthenticated;

        /// <summary>
        /// Gets the property associated with the user peer.
        /// </summary>
        /// <returns>The property associated with the user peer.</returns>
        public IProperty GetProperty() => this.property;

    }

}

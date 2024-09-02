using XmobiTea.ProtonNet.Token.Attributes;
using XmobiTea.ProtonNet.Token.Models;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Represents the payload of a user peer token, implementing <see cref="ITokenPayload"/>.
    /// </summary>
    public class UserPeerTokenPayload : ITokenPayload
    {
        /// <summary>
        /// Gets or sets the userId.
        /// </summary>
        /// <value>The userId.</value>
        [TokenMember(0)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the peer type as a byte value.
        /// </summary>
        /// <value>The peer type.</value>
        [TokenMember(1)]
        public byte PeerType { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        /// <value>The session ID.</value>
        [TokenMember(2)]
        public string SessionId { get; set; }

    }

}

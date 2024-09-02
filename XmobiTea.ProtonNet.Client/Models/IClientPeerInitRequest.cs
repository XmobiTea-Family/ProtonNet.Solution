namespace XmobiTea.ProtonNet.Client.Models
{
    /// <summary>
    /// Represents an initialization request for a client peer.
    /// </summary>
    public interface IClientPeerInitRequest
    {
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        int ClientId { get; }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        byte[] EncryptKey { get; }

    }

    /// <summary>
    /// Implements the <see cref="IClientPeerInitRequest"/> interface.
    /// </summary>
    class ClientPeerInitRequest : IClientPeerInitRequest
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the encryption key.
        /// </summary>
        public byte[] EncryptKey { get; set; }

    }

}

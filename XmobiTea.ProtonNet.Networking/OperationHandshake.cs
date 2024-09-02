namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents a handshake operation in the networking layer.
    /// </summary>
    public class OperationHandshake : IOperationModel
    {
        /// <summary>
        /// Gets or sets the session ID associated with the handshake.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the encryption key used during the handshake.
        /// </summary>
        public byte[] EncryptKey { get; set; }

        /// <summary>
        /// Gets or sets the authentication token used for validating the handshake.
        /// </summary>
        public string AuthToken { get; set; }

    }

}

namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Enumeration representing the result of a send operation.
    /// </summary>
    public enum SendResult
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The connection was disconnected.
        /// </summary>
        Disconnected = 1,

        /// <summary>
        /// The send buffer is full, preventing the operation from completing.
        /// </summary>
        SendBufferFull = 2,

        /// <summary>
        /// The message is too large to be sent.
        /// </summary>
        MessageTooBig = 3,

        /// <summary>
        /// The session is null, indicating an invalid or missing session.
        /// </summary>
        SessionNull = 4,

        /// <summary>
        /// The operation failed because encryption is not supported.
        /// </summary>
        EncryptionNotSupported = 5,

        /// <summary>
        /// The send operation failed due to an unspecified error.
        /// </summary>
        Failed = 6,

    }

}

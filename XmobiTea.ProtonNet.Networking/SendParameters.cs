namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Struct representing parameters used during a send operation.
    /// </summary>
    public class SendParameters
    {
        /// <summary>
        /// Gets or sets the ID of the channel through which the data is sent.
        /// </summary>
        public byte ChannelId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data is sent unreliably.
        /// </summary>
        public bool Unreliable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data should be encrypted.
        /// </summary>
        public bool Encrypted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data should be sent immediately.
        /// </summary>
        public bool Immediately { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation should be synchronous.
        /// </summary>
        public bool Sync { get; set; }

    }

}

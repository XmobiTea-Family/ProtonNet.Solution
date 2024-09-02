namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Represents a disconnect operation in the networking layer.
    /// </summary>
    public class OperationDisconnect : IOperationModel
    {
        /// <summary>
        /// Gets or sets the reason code for the disconnection.
        /// </summary>
        public byte Reason { get; set; }

        /// <summary>
        /// Gets or sets a message providing additional information about the disconnection.
        /// </summary>
        public string Message { get; set; }

    }

}

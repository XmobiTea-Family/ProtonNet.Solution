namespace XmobiTea.ProtonNet.Networking
{
    /// <summary>
    /// Static class containing return codes for various network operations.
    /// </summary>
    public static class ReturnCode
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        public static readonly byte Ok = 1;

        /// <summary>
        /// The operation timed out.
        /// </summary>
        public static readonly byte OperationTimeout = 2;

        /// <summary>
        /// The operation was invalid.
        /// </summary>
        public static readonly byte OperationInvalid = 3;

        /// <summary>
        /// The operation was invalid when performed on the server.
        /// </summary>
        public static readonly byte OperationInvalidInServer = 4;

        /// <summary>
        /// The operation was not authorized.
        /// </summary>
        public static readonly byte OperationNotAuthorized = 5;

    }

}

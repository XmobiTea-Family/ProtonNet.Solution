namespace XmobiTea.ProtonNet.Client.WebApi.Types
{
    /// <summary>
    /// Static class containing constant values for HTTP header names 
    /// used in communication between the client and server.
    /// </summary>
    static class HeaderNames
    {
        /// <summary>
        /// The header name for the content type.
        /// </summary>
        public static readonly string ContentType = "content-type";

        /// <summary>
        /// The header name for the session ID.
        /// </summary>
        public static readonly string SessionId = "xp-sid";

        /// <summary>
        /// The header name for the encryption key.
        /// </summary>
        public static readonly string EncryptKey = "xp-ek";

        /// <summary>
        /// The header name for the authentication token.
        /// </summary>
        public static readonly string Token = "xp-token";

    }

}

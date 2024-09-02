namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// Provides constant values commonly used in HTTP operations.
    /// </summary>
    static class Constance
    {
        /// <summary>
        /// The default HTTP protocol version used in requests and responses.
        /// </summary>
        public const string DefaultHttpProtocol = "HTTP/1.1";

        /// <summary>
        /// The default content type for text-based content, with UTF-8 character encoding.
        /// </summary>
        public const string DefaultTextContentType = "text/plain; charset=UTF-8";

        /// <summary>
        /// The default content type for cases where no specific content type is provided.
        /// </summary>
        public const string DefaultEmptyContentType = "";

        /// <summary>
        /// The default set of allowed HTTP methods, typically used in response to OPTIONS requests.
        /// </summary>
        public const string DefaultOptionsAllow = "HEAD,GET,POST,PUT,DELETE,OPTIONS,TRACE";

    }

}

namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// Provides a set of commonly used HTTP header names as constants.
    /// </summary>
    public static class HeaderNames
    {
        /// <summary>
        /// Represents the "cache-control" HTTP header.
        /// </summary>
        public static readonly string CacheControl = "cache-control";

        /// <summary>
        /// Represents the "cookie" HTTP header.
        /// </summary>
        public static readonly string Cookie = "cookie";

        /// <summary>
        /// Represents the "set-cookie" HTTP header.
        /// </summary>
        public static readonly string SetCookie = "set-cookie";

        /// <summary>
        /// Represents the "content-type" HTTP header.
        /// </summary>
        public static readonly string ContentType = "content-type";

        /// <summary>
        /// Represents the "content-length" HTTP header.
        /// </summary>
        public static readonly string ContentLength = "content-length";

        /// <summary>
        /// Represents the "allow" HTTP header, typically used in response to OPTIONS requests.
        /// </summary>
        public static readonly string Allow = "allow";

        /// <summary>
        /// Represents the "connection" HTTP header.
        /// </summary>
        public static readonly string Connection = "connection";

        /// <summary>
        /// Represents the "upgrade" HTTP header, commonly used in WebSocket connections.
        /// </summary>
        public static readonly string Upgrade = "upgrade";

        /// <summary>
        /// Represents the "sec-websocket-accept" HTTP header, used in WebSocket handshake responses.
        /// </summary>
        public static readonly string SecWebSocketAccept = "sec-websocket-accept";

        /// <summary>
        /// Represents the "sec-websocket-key" HTTP header, used in WebSocket handshake requests.
        /// </summary>
        public static readonly string SecWebSocketKey = "sec-websocket-key";

        /// <summary>
        /// Represents the "sec-websocket-version" HTTP header, used to specify the WebSocket protocol version.
        /// </summary>
        public static readonly string SecWebSocketVersion = "sec-websocket-version";

    }

}

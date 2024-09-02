namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// Provides a set of commonly used HTTP header values as constants.
    /// </summary>
    public static class HeaderValues
    {
        /// <summary>
        /// Represents the "keep-alive, upgrade" value, typically used in the "connection" HTTP header.
        /// </summary>
        public static readonly string KeepAliveUpgrade = "keep-alive, upgrade";

        /// <summary>
        /// Represents the "upgrade" value, used in the "connection" or "upgrade" HTTP headers.
        /// </summary>
        public static readonly string Upgrade = "upgrade";

        /// <summary>
        /// Represents the "websocket" value, used in the "upgrade" HTTP header to initiate WebSocket connections.
        /// </summary>
        public static readonly string WebSocket = "websocket";

        /// <summary>
        /// Represents the "message/http" value, used in the "content-type" HTTP header for HTTP TRACE requests.
        /// </summary>
        public static readonly string MessageHttp = "message/http";

        /// <summary>
        /// Represents the WebSocket protocol version "13", used in the "sec-websocket-version" HTTP header.
        /// </summary>
        public static readonly string SecWsVersion = "13";
    }
}

namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// This class contains constant byte values representing WebSocket operation codes.
    /// </summary>
    public class WebSocketOpCodes
    {
        /// <summary>
        /// Indicates the final fragment in a WebSocket message.
        /// </summary>
        public const byte FIN = 0x80;

        /// <summary>
        /// Represents a text frame in WebSocket.
        /// </summary>
        public const byte TEXT = 0x01;

        /// <summary>
        /// Represents a binary frame in WebSocket.
        /// </summary>
        public const byte BINARY = 0x02;

        /// <summary>
        /// Indicates a connection close frame in WebSocket.
        /// </summary>
        public const byte CLOSE = 0x08;

        /// <summary>
        /// Represents a ping frame in WebSocket.
        /// </summary>
        public const byte PING = 0x09;

        /// <summary>
        /// Represents a pong frame in WebSocket.
        /// </summary>
        public const byte PONG = 0x0A;

    }

}

using System;

namespace XmobiTea.Binary.MessagePack.Types
{
    /// <summary>
    /// Defines flags representing the supported lengths for data encoding in MessagePack.
    /// </summary>
    [Flags]
    public enum SupportedLengths
    {
        /// <summary>
        /// Supports 1-byte length.
        /// </summary>
        Byte1 = 1,

        /// <summary>
        /// Supports 2-byte length.
        /// </summary>
        Short2 = 2,

        /// <summary>
        /// Supports 4-byte length.
        /// </summary>
        Int4 = 4,

        /// <summary>
        /// Supports 8-byte length.
        /// </summary>
        Long8 = 8,

        /// <summary>
        /// Supports all length types.
        /// </summary>
        All = 15,

        /// <summary>
        /// Supports lengths from 2 bytes and upward.
        /// </summary>
        FromShortUpward = 14,

        /// <summary>
        /// No length is supported.
        /// </summary>
        None = 0,

    }

}

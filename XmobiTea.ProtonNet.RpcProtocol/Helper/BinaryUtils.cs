using System;

namespace XmobiTea.ProtonNet.RpcProtocol.Helper
{
    /// <summary>
    /// Utility class for handling binary operations related to endianness.
    /// </summary>
    static class BinaryUtils
    {
        /// <summary>
        /// Checks if the system architecture is Big Endian.
        /// </summary>
        /// <returns>True if the system is Big Endian; otherwise, false.</returns>
        public static bool IsBigEndian() => !BitConverter.IsLittleEndian;

        /// <summary>
        /// Swaps the byte order of the given buffer if the system architecture is Little Endian.
        /// </summary>
        /// <param name="buffer">The byte array to potentially swap.</param>
        public static void SwapIfLittleEndian(ref byte[] buffer)
        {
            if (IsBigEndian()) return;

            for (int i = 0, k = buffer.Length - 1; i < k; ++i, --k)
            {
                var c = buffer[i];
                buffer[i] = buffer[k];
                buffer[k] = c;
            }
        }

    }

}

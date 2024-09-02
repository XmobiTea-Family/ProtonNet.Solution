namespace XmobiTea.ProtonNet.Token.Helper
{
    /// <summary>
    /// Provides utility methods for handling binary data and endianness.
    /// </summary>
    static class BinaryUtils
    {
        /// <summary>
        /// Determines if the current system architecture is big-endian.
        /// </summary>
        /// <returns>True if the system is big-endian; otherwise, false.</returns>
        public static bool IsBigEndian() => !System.BitConverter.IsLittleEndian;

        /// <summary>
        /// Swaps the byte order of the specified buffer if the system is little-endian.
        /// </summary>
        /// <param name="buffer">The byte array to swap.</param>
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

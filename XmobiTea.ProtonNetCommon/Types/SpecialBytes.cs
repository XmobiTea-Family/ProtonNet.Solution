namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// This static class provides commonly used byte arrays representing specific characters or strings.
    /// </summary>
    static class SpecialBytes
    {
        /// <summary>
        /// Represents the byte array for a space character.
        /// </summary>
        public static readonly byte[] Spacebar = System.Text.Encoding.UTF8.GetBytes(" ");

        /// <summary>
        /// Represents the byte array for a new line sequence (\r\n).
        /// </summary>
        public static readonly byte[] NewLine = System.Text.Encoding.UTF8.GetBytes("\r\n");

        /// <summary>
        /// Represents the byte array for a colon followed by a space (": ").
        /// </summary>
        public static readonly byte[] ColonSpacebar = System.Text.Encoding.UTF8.GetBytes(": ");

        /// <summary>
        /// Represents the byte array for a semicolon followed by a space ("; ").
        /// </summary>
        public static readonly byte[] SemiColonSpacebar = System.Text.Encoding.UTF8.GetBytes("; ");

        /// <summary>
        /// Represents the byte array for an equals sign ("=").
        /// </summary>
        public new static readonly byte[] Equals = System.Text.Encoding.UTF8.GetBytes("=");

    }

}

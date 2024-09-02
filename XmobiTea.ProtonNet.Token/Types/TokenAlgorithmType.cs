namespace XmobiTea.ProtonNet.Token.Types
{
    /// <summary>
    /// Represents the types of algorithms used for token generation and validation.
    /// </summary>
    public enum TokenAlgorithmType : byte
    {
        /// <summary>
        /// Represents the SHA256 hashing algorithm.
        /// </summary>
        SHA256 = 1,

        /// <summary>
        /// Represents the MD5 hashing algorithm.
        /// </summary>
        MD5 = 2,

        /// <summary>
        /// Represents the HMAC hashing algorithm.
        /// </summary>
        HMAC = 3,

        /// <summary>
        /// Represents the SHA1 hashing algorithm.
        /// </summary>
        SHA1 = 4,

        /// <summary>
        /// Represents the SHA384 hashing algorithm.
        /// </summary>
        SHA384 = 5,

        /// <summary>
        /// Represents the SHA512 hashing algorithm.
        /// </summary>
        SHA512 = 6,

        /// <summary>
        /// Represents a keyed hash algorithm.
        /// </summary>
        KeyedHashAlgorithm = 7,

    }

}

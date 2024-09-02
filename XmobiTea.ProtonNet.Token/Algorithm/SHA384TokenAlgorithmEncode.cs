namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using SHA384 for encryption.
    /// </summary>
    class SHA384TokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using SHA384.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data using SHA384 hash.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create a SHA384 instance for hashing.
            using (var hash = System.Security.Cryptography.SHA384.Create())
            {
                // Compute and return the SHA384 hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

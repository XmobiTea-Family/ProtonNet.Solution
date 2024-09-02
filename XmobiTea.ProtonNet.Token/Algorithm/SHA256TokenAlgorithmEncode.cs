namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using SHA256 for encryption.
    /// </summary>
    class SHA256TokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using SHA256.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data using SHA256 hash.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create a SHA256 instance for hashing.
            using (var hash = System.Security.Cryptography.SHA256.Create())
            {
                // Compute and return the SHA256 hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

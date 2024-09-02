namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using SHA1 for encryption.
    /// </summary>
    class SHA1TokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using SHA1.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data using SHA1 hash.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create a SHA1 instance for hashing.
            using (var hash = System.Security.Cryptography.SHA1.Create())
            {
                // Compute and return the SHA1 hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

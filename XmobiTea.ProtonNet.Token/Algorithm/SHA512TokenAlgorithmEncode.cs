namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using SHA512 for encryption.
    /// </summary>
    class SHA512TokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using SHA512.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data using SHA512 hash.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create a SHA512 instance for hashing.
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                // Compute and return the SHA512 hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using KeyedHashAlgorithm for encryption.
    /// </summary>
    class KeyedHashAlgorithmTokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using KeyedHashAlgorithm.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create a KeyedHashAlgorithm instance with default algorithm.
            using (var hash = System.Security.Cryptography.KeyedHashAlgorithm.Create())
            {
                // Compute and return the hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

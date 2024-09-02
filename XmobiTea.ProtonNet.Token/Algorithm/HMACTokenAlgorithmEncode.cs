namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using HMAC for encryption.
    /// </summary>
    class HMACTokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using HMAC.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create an HMAC instance with default algorithm.
            using (var hash = System.Security.Cryptography.HMAC.Create())
            {
                // Compute and return the hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

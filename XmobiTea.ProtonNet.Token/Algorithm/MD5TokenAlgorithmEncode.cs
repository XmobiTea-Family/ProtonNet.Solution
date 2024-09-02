namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Implements the ITokenAlgorithmEncode interface using MD5 for encryption.
    /// </summary>
    class MD5TokenAlgorithmEncode : ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array using MD5.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data using MD5 hash.</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            // Create an MD5 instance for hashing.
            using (var hash = System.Security.Cryptography.MD5.Create())
            {
                // Compute and return the MD5 hash of the byte array.
                return hash.ComputeHash(buffer);
            }
        }

    }

}

namespace XmobiTea.Crypto
{
    /// <summary>
    /// Defines the interface for a cryptographic provider that supports encryption and decryption of data.
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Encrypts the specified data using the provided salt.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="salt">The salt to use during encryption.</param>
        /// <returns>The encrypted data as a byte array.</returns>
        byte[] Encrypt(byte[] data, object salt);

        /// <summary>
        /// Decrypts the specified encrypted data using the provided salt.
        /// </summary>
        /// <param name="encryptedData">The encrypted data to decrypt.</param>
        /// <param name="salt">The salt to use during decryption.</param>
        /// <returns>The decrypted data as a byte array.</returns>
        byte[] Decrypt(byte[] encryptedData, object salt);

        /// <summary>
        /// Attempts to decrypt the specified encrypted data using the provided salt.
        /// </summary>
        /// <param name="encryptedData">The encrypted data to decrypt.</param>
        /// <param name="salt">The salt to use during decryption.</param>
        /// <param name="data">When this method returns, contains the decrypted data if the decryption succeeded, or an empty byte array if it failed.</param>
        /// <returns>True if decryption succeeded; otherwise, false.</returns>
        bool TryDecrypt(byte[] encryptedData, object salt, out byte[] data);

    }

}

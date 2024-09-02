using XmobiTea.Crypto.Aes.Decrypt;
using XmobiTea.Crypto.Aes.Encrypt;

namespace XmobiTea.Crypto.Aes
{
    /// <summary>
    /// Provides AES-based cryptographic operations including encryption and decryption.
    /// </summary>
    public class CryptoProvider : ICryptoProvider
    {
        /// <summary>
        /// Gets the encrypter responsible for AES encryption.
        /// </summary>
        private ICryptoEncrypter encrypter { get; }

        /// <summary>
        /// Gets the decrypter responsible for AES decryption.
        /// </summary>
        private ICryptoDecrypter decrypter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoProvider"/> class.
        /// </summary>
        public CryptoProvider()
        {
            this.encrypter = new CryptoEncrypter();
            this.decrypter = new CryptoDecrypter();
        }

        /// <summary>
        /// Encrypts the specified data using the provided salt (encryption key).
        /// </summary>
        /// <param name="data">The data to be encrypted.</param>
        /// <param name="salt">The salt (encryption key) to be used for encryption.</param>
        /// <returns>The encrypted data as a byte array.</returns>
        public byte[] Encrypt(byte[] data, object salt)
        {
            var salts = (object[])salt;
            var encryptKey = (byte[])salts[0];

            return this.encrypter.Encrypt(data, encryptKey);
        }

        /// <summary>
        /// Decrypts the specified encrypted data using the provided salt (encryption key).
        /// </summary>
        /// <param name="encryptedData">The encrypted data to be decrypted.</param>
        /// <param name="salt">The salt (encryption key) to be used for decryption.</param>
        /// <returns>The decrypted data as a byte array.</returns>
        public byte[] Decrypt(byte[] encryptedData, object salt)
        {
            var salts = (object[])salt;
            var encryptKey = (byte[])salts[0];

            return this.decrypter.Decrypt(encryptedData, encryptKey);
        }

        /// <summary>
        /// Tries to decrypt the specified encrypted data using the provided salt (encryption key).
        /// </summary>
        /// <param name="encryptedData">The encrypted data to be decrypted.</param>
        /// <param name="salt">The salt (encryption key) to be used for decryption.</param>
        /// <param name="data">The decrypted data if decryption is successful, or null if it fails.</param>
        /// <returns>True if decryption was successful; otherwise, false.</returns>
        public bool TryDecrypt(byte[] encryptedData, object salt, out byte[] data)
        {
            try
            {
                data = this.Decrypt(encryptedData, salt);
                return true;
            }
            catch
            {
                data = null;
                return false;
            }
        }

    }

}

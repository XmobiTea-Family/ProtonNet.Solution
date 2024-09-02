using System;
using System.IO;
using System.Security.Cryptography;

namespace XmobiTea.Crypto.Aes.Decrypt
{
    /// <summary>
    /// Defines the interface for a cryptographic decrypter.
    /// </summary>
    interface ICryptoDecrypter
    {
        /// <summary>Decrypts the specified encrypted data using the provided encryption key.</summary>
        /// <param name="encryptedData">The data to be decrypted.</param>
        /// <param name="encryptKey">The encryption key used to decrypt the data.</param>
        /// <returns>The decrypted data as a byte array.</returns>
        byte[] Decrypt(byte[] encryptedData, byte[] encryptKey);

    }

    /// <summary>
    /// Implementation of the ICryptoDecrypter interface for AES decryption.
    /// </summary>
    class CryptoDecrypter : ICryptoDecrypter
    {
        /// <summary>Decrypts the specified encrypted data using the provided AES encryption key.</summary>
        /// <param name="encryptedData">The AES-encrypted data to be decrypted.</param>
        /// <param name="encryptKey">The AES encryption key used to decrypt the data.</param>
        /// <returns>The decrypted data as a byte array.</returns>
        public byte[] Decrypt(byte[] encryptedData, byte[] encryptKey)
        {
            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = encryptKey;

                var iv = new byte[aes.BlockSize / 8];
                Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                aes.IV = iv;

                var secondEncryptData = new byte[encryptedData.Length - iv.Length];
                Buffer.BlockCopy(encryptedData, iv.Length, secondEncryptData, 0, secondEncryptData.Length);

                using (var decryptor = aes.CreateDecryptor())
                {
                    return this.PerformCryptography(decryptor, secondEncryptData);
                }
            }
        }

        /// <summary>
        /// Performs the cryptographic transformation on the specified data.
        /// </summary>
        /// <param name="cryptoTransform">The cryptographic transform to apply.</param>
        /// <param name="data">The data to be transformed.</param>
        /// <returns>The transformed data as a byte array.</returns>
        private byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

    }

}

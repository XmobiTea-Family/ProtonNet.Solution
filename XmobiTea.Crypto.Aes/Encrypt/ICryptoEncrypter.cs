using System;
using System.IO;
using System.Security.Cryptography;

namespace XmobiTea.Crypto.Aes.Encrypt
{
    /// <summary>
    /// Defines the interface for a cryptographic encrypter.
    /// </summary>
    interface ICryptoEncrypter
    {
        /// <summary>Encrypts the specified data using the provided encryption key.</summary>
        /// <param name="data">The data to be encrypted.</param>
        /// <param name="encryptKey">The encryption key used to encrypt the data.</param>
        /// <returns>The encrypted data as a byte array.</returns>
        byte[] Encrypt(byte[] data, byte[] encryptKey);

    }

    /// <summary>
    /// Implementation of the ICryptoEncrypter interface for AES encryption.
    /// </summary>
    class CryptoEncrypter : ICryptoEncrypter
    {
        /// <summary>Encrypts the specified data using the provided AES encryption key.</summary>
        /// <param name="data">The data to be encrypted.</param>
        /// <param name="encryptKey">The AES encryption key used to encrypt the data.</param>
        /// <returns>The encrypted data as a byte array, including the IV.</returns>
        public byte[] Encrypt(byte[] data, byte[] encryptKey)
        {
            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = encryptKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                {
                    var secondEncryptedData = this.PerformCryptography(encryptor, data);

                    var iv = aes.IV;
                    var fullEncryptedData = new byte[iv.Length + secondEncryptedData.Length];

                    Buffer.BlockCopy(iv, 0, fullEncryptedData, 0, iv.Length);
                    Buffer.BlockCopy(secondEncryptedData, 0, fullEncryptedData, iv.Length, secondEncryptedData.Length);

                    return fullEncryptedData;
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

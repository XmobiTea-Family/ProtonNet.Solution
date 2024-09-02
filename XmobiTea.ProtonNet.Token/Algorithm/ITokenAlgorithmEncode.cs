namespace XmobiTea.ProtonNet.Token.Algorithm
{
    /// <summary>
    /// Defines a method for encrypting data.
    /// </summary>
    interface ITokenAlgorithmEncode
    {
        /// <summary>
        /// Encrypts a byte array.
        /// </summary>
        /// <param name="buffer">The byte array to be encrypted.</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        byte[] Encrypt(byte[] buffer);

    }

}

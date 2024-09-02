using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Token.Binary
{
    /// <summary>
    /// Defines methods for deserializing binary data.
    /// </summary>
    interface ITokenBinaryDecode
    {
        /// <summary>
        /// Deserializes a byte array representing a header into an array of objects.
        /// </summary>
        /// <param name="header">The byte array representing the header.</param>
        /// <returns>An array of objects representing the deserialized header.</returns>
        object[] DeserializeHeader(byte[] header);

        /// <summary>
        /// Deserializes a byte array representing the payload into a dictionary.
        /// </summary>
        /// <param name="payload">The byte array representing the payload.</param>
        /// <returns>A dictionary where the key is a byte and the value is an object representing the deserialized payload.</returns>
        IDictionary<byte, object> DeserializePayload(byte[] payload);

    }

}

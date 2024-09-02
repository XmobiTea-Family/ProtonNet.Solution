using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Token.Binary
{
    /// <summary>
    /// Defines methods for serializing data into binary format.
    /// </summary>
    interface ITokenBinaryEncode
    {
        /// <summary>
        /// Serializes an array of objects representing the header into a byte array.
        /// </summary>
        /// <param name="header">An array of objects representing the header.</param>
        /// <returns>A byte array representing the serialized header.</returns>
        byte[] SerializeHeader(object[] header);

        /// <summary>
        /// Serializes a dictionary representing the payload into a byte array.
        /// </summary>
        /// <param name="payload">A dictionary where the key is a byte and the value is an object representing the payload.</param>
        /// <returns>A byte array representing the serialized payload.</returns>
        byte[] SerializePayload(IDictionary<byte, object> payload);

    }

}

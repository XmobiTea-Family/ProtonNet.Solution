using System.Collections.Generic;
using XmobiTea.Binary;

namespace XmobiTea.ProtonNet.Token.Binary
{
    /// <summary>
    /// Implements the ITokenBinaryEncode interface for serializing data into binary format.
    /// </summary>
    class ProtocolTokenBinaryEncode : ITokenBinaryEncode
    {
        private IBinaryConverter binaryConverter { get; }

        /// <summary>
        /// Initializes a new instance of the ProtocolTokenBinaryEncode class.
        /// </summary>
        /// <param name="binaryConverter">The binary converter used for serialization.</param>
        public ProtocolTokenBinaryEncode(IBinaryConverter binaryConverter) => this.binaryConverter = binaryConverter;

        /// <summary>
        /// Serializes an array of objects representing the header into a byte array.
        /// </summary>
        /// <param name="header">An array of objects representing the header.</param>
        /// <returns>A byte array representing the serialized header.</returns>
        public byte[] SerializeHeader(object[] header) => this.binaryConverter.Serialize(header);

        /// <summary>
        /// Serializes a dictionary representing the payload into a byte array.
        /// </summary>
        /// <param name="payload">A dictionary where the key is a byte and the value is an object representing the payload.</param>
        /// <returns>A byte array representing the serialized payload.</returns>
        public byte[] SerializePayload(IDictionary<byte, object> payload) => this.binaryConverter.Serialize(payload);

    }

}

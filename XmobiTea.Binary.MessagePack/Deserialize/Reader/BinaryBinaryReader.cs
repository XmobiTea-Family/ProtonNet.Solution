using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing binary data (byte arrays) from a binary stream.
    /// </summary>
    class BinaryBinaryReader : AbstractCollectionBinaryReader<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public BinaryBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with binary data (byte arrays).
        /// </summary>
        /// <returns>The binary type code for binary data as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Binary;

        /// <summary>
        /// Reads and deserializes binary data (byte array) from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as a byte array.</param>
        /// <returns>A byte array representing the deserialized binary data.</returns>
        public override byte[] Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            long length = 0;

            if (messagePackTypeCode == MessagePackTypeCode.Bin8) length = this.GetCollectionLength(stream, 1);
            else if (messagePackTypeCode == MessagePackTypeCode.Bin16) length = this.GetCollectionLength(stream, 2);
            else if (messagePackTypeCode == MessagePackTypeCode.Bin32) length = this.GetCollectionLength(stream, 4);

            return this.ReadBytes(stream, (ushort)length);
        }

    }

}

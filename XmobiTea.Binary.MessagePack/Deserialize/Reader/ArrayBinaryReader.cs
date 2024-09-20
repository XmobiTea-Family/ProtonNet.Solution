using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing array types from a binary stream.
    /// </summary>
    class ArrayBinaryReader : AbstractCollectionBinaryReader<System.Collections.ICollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public ArrayBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with arrays.
        /// </summary>
        /// <returns>The binary type code for arrays as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Array;

        /// <summary>
        /// Reads and deserializes an array from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as an array.</param>
        /// <returns>A list representing the deserialized array.</returns>
        public override System.Collections.ICollection Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            if (!this.IsMasked(MessagePackTypeCode.ArrayFix, messagePackTypeCode, 0x0F, out var length))
            {
                if (messagePackTypeCode == MessagePackTypeCode.Array16) length = this.GetCollectionLength(stream, 2);
                else if (messagePackTypeCode == MessagePackTypeCode.Array32) length = this.GetCollectionLength(stream, 4);
            }

            var answer = new object[length];

            for (var i = 0; i < length; i++)
            {
                var elementMessagePackTypeCode = this.ReadByte(stream);
                answer[i] = this.binaryDeserializer.GetReader(elementMessagePackTypeCode).Read(stream, elementMessagePackTypeCode);
            }

            return answer;
        }

    }

}

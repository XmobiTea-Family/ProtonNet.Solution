using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing string data from a binary stream.
    /// </summary>
    class StringBinaryReader : AbstractCollectionBinaryReader<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public StringBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with string data.
        /// </summary>
        /// <returns>The binary type code for strings as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.String;

        /// <summary>
        /// Reads and deserializes a string from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as a string.</param>
        /// <returns>A string representing the deserialized data.</returns>
        public override string Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            if (!this.IsMasked(MessagePackTypeCode.StrFix, messagePackTypeCode, 0x1F, out var length))
                if (messagePackTypeCode == MessagePackTypeCode.Str8) length = this.GetCollectionLength(stream, 1);
                else if (messagePackTypeCode == MessagePackTypeCode.Str16) length = this.GetCollectionLength(stream, 2);
                else if (messagePackTypeCode == MessagePackTypeCode.Str32) length = this.GetCollectionLength(stream, 4);

            var buffer = this.ReadBytes(stream, (ushort)length);
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

    }

}

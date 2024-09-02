using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing extension data from a binary stream.
    /// </summary>
    class ExtensionBinaryReader : AbstractCollectionBinaryReader<System.Tuple<sbyte, byte[]>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public ExtensionBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with extension data.
        /// </summary>
        /// <returns>The binary type code for extension data as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Extension;

        /// <summary>
        /// Reads and deserializes extension data from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as an extension.</param>
        /// <returns>A tuple containing the type specifier and the byte array representing the deserialized extension data.</returns>
        public override System.Tuple<sbyte, byte[]> Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            long length = 0;

            if (messagePackTypeCode == MessagePackTypeCode.ExtFix1) length = 1;
            else if (messagePackTypeCode == MessagePackTypeCode.ExtFix2) length = 2;
            else if (messagePackTypeCode == MessagePackTypeCode.ExtFix4) length = 4;
            else if (messagePackTypeCode == MessagePackTypeCode.ExtFix8) length = 8;
            else if (messagePackTypeCode == MessagePackTypeCode.ExtFix16) length = 16;
            else if (messagePackTypeCode == MessagePackTypeCode.Ext8) length = this.GetCollectionLength(stream, 1);
            else if (messagePackTypeCode == MessagePackTypeCode.Ext16) length = this.GetCollectionLength(stream, 2);
            else length = this.GetCollectionLength(stream, 4);

            var typeSpecifier = System.Convert.ToSByte(this.ReadByte(stream));

            return new System.Tuple<sbyte, byte[]>(typeSpecifier, this.ReadBytes(stream, (ushort)length));
        }

    }

}

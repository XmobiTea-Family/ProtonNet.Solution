using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize extension types (tuples of sbyte and byte arrays) into MessagePack binary format.
    /// </summary>
    class ExtensionBinaryWriter : AbstractCollectionBinaryWriter<System.Tuple<sbyte, byte[]>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public ExtensionBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with extension types.
        /// </summary>
        /// <returns>The binary type code for extension types.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Extension;

        /// <summary>
        /// Gets the MessagePack type code for the specified extension value.
        /// </summary>
        /// <param name="value">The extension value (tuple of sbyte and byte array) to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(System.Tuple<sbyte, byte[]> value)
        {
            var length = value.Item2.Length;

            if (length <= 0) return MessagePackTypeCode.Ext8;
            else if (length == 1) return MessagePackTypeCode.ExtFix1;
            else if (length == 2) return MessagePackTypeCode.ExtFix2;
            else if (length == 3) return MessagePackTypeCode.Ext8;
            else if (length == 4) return MessagePackTypeCode.ExtFix4;
            else if (length <= 7) return MessagePackTypeCode.Ext8;
            else if (length == 8) return MessagePackTypeCode.ExtFix8;
            else if (length <= 15) return MessagePackTypeCode.Ext8;
            else if (length == 16) return MessagePackTypeCode.ExtFix16;
            else if (length <= 255) return MessagePackTypeCode.Ext8;
            else if (length <= ushort.MaxValue) return MessagePackTypeCode.Ext16;

            return MessagePackTypeCode.Ext32;
        }

        /// <summary>
        /// Gets the total length of the serialized data for the extension value.
        /// </summary>
        /// <param name="value">The extension value (tuple of sbyte and byte array) for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(System.Tuple<sbyte, byte[]> value)
        {
            var answer = 2 + value.Item2.Length;

            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode == MessagePackTypeCode.Ext8
                || messagePackTypeCode == MessagePackTypeCode.Ext16
                || messagePackTypeCode == MessagePackTypeCode.Ext32)
                answer += this.GetLengthBytes(value.Item2.Length, SupportedLengths.All).Length;

            return answer;
        }

        /// <summary>
        /// Writes the extension value (tuple of sbyte and byte array) to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The extension value (tuple of sbyte and byte array) to serialize.</param>
        public override void Write(System.IO.Stream stream, System.Tuple<sbyte, byte[]> value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);
            this.WriteByte(stream, messagePackTypeCode);

            if (messagePackTypeCode == MessagePackTypeCode.Ext8
                || messagePackTypeCode == MessagePackTypeCode.Ext16
                || messagePackTypeCode == MessagePackTypeCode.Ext32)
                this.WriteBytes(stream, this.GetLengthBytes(value.Item2.Length, SupportedLengths.All));

            this.WriteByte(stream, (byte)value.Item1);
            this.WriteBytes(stream, value.Item2);
        }

    }

}

using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize a string into MessagePack binary format.
    /// </summary>
    class StringBinaryWriter : AbstractCollectionBinaryWriter<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public StringBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with strings.
        /// </summary>
        /// <returns>The binary type code for strings.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.String;

        /// <summary>
        /// Gets the MessagePack type code for a given string.
        /// </summary>
        /// <param name="value">The string value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(string value)
        {
            var length = System.Text.Encoding.UTF8.GetBytes(value).Length;

            if (length < 32) return MessagePackTypeCode.StrFix;
            if (length < 256) return MessagePackTypeCode.Str8;
            if (length <= ushort.MaxValue) return MessagePackTypeCode.Str16;

            return MessagePackTypeCode.Str32;
        }

        /// <summary>
        /// Gets the total length of the serialized data for the given string.
        /// </summary>
        /// <param name="value">The string for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(string value)
        {
            var answer = 1;

            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            var bufferString = System.Text.Encoding.UTF8.GetBytes(value);

            if (messagePackTypeCode != MessagePackTypeCode.StrFix) answer += this.GetLengthBytes(bufferString.Length, SupportedLengths.All).Length;

            answer += bufferString.Length;

            return answer;
        }

        /// <summary>
        /// Writes the string to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The string to serialize.</param>
        public override void Write(System.IO.Stream stream, string value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            var stringBytes = System.Text.Encoding.UTF8.GetBytes(value);

            if (messagePackTypeCode == MessagePackTypeCode.StrFix)
                this.WriteByte(stream, this.GetLength(messagePackTypeCode, stringBytes.Length));
            else
            {
                this.WriteByte(stream, messagePackTypeCode);

                this.WriteBytes(stream, this.GetLengthBytes(stringBytes.Length, SupportedLengths.All));
            }

            this.WriteBytes(stream, stringBytes);
        }

    }

}

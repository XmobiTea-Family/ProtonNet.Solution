using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize dictionary objects into MessagePack binary format.
    /// </summary>
    class MapBinaryWriter : AbstractCollectionBinaryWriter<System.Collections.IDictionary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public MapBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with dictionaries.
        /// </summary>
        /// <returns>The binary type code for dictionaries.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Map;

        /// <summary>
        /// Gets the MessagePack type code for the specified dictionary value.
        /// </summary>
        /// <param name="value">The dictionary value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(System.Collections.IDictionary value)
        {
            var length = value.Count;

            if (length < 16) return MessagePackTypeCode.MapFix;
            else if (length <= ushort.MaxValue) return MessagePackTypeCode.Map16;
            else return MessagePackTypeCode.Map32;
        }

        /// <summary>
        /// Gets the total length of the serialized data for the dictionary value.
        /// </summary>
        /// <param name="value">The dictionary value for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(System.Collections.IDictionary value)
        {
            var answer = 1;

            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode != MessagePackTypeCode.MapFix)
                answer += this.GetLengthBytes(value.Count, SupportedLengths.FromShortUpward).Length;

            foreach (var elementKey in value.Keys)
            {
                var elementValue = value[elementKey];

                if (elementKey == null) answer += this.binarySerializer.GetWriter(BinaryTypeCode.Nil).GetDataLength(elementKey);
                else answer += this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementKey.GetType())).GetDataLength(elementKey);

                if (elementValue == null) answer += this.binarySerializer.GetWriter(BinaryTypeCode.Nil).GetDataLength(elementValue);
                else answer += this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementValue.GetType())).GetDataLength(elementValue);
            }

            return answer;
        }

        /// <summary>
        /// Writes the dictionary value to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The dictionary value to serialize.</param>
        public override void Write(System.IO.Stream stream, System.Collections.IDictionary value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode == MessagePackTypeCode.MapFix) this.WriteByte(stream, this.GetLength(messagePackTypeCode, value.Count));
            else
            {
                this.WriteByte(stream, messagePackTypeCode);
                this.WriteBytes(stream, this.GetLengthBytes(value.Count, SupportedLengths.FromShortUpward));
            }

            foreach (var elementKey in value.Keys)
            {
                var elementValue = value[elementKey];

                if (elementKey == null) this.binarySerializer.GetWriter(BinaryTypeCode.Nil).Write(stream, elementKey);
                else this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementKey.GetType())).Write(stream, elementKey);

                if (elementValue == null) this.binarySerializer.GetWriter(BinaryTypeCode.Nil).Write(stream, elementValue);
                else this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementValue.GetType())).Write(stream, elementValue);
            }
        }

    }

}

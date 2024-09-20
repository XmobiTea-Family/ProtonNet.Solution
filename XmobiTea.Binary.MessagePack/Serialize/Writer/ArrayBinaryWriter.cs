using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize arrays into MessagePack binary format.
    /// </summary>
    class ArrayBinaryWriter : AbstractCollectionBinaryWriter<System.Collections.ICollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public ArrayBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with arrays.
        /// </summary>
        /// <returns>The binary type code for arrays.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Array;

        /// <summary>
        /// Gets the MessagePack type code based on the length of the array.
        /// </summary>
        /// <param name="value">The array for which to determine the type code.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(System.Collections.ICollection value)
        {
            var length = value.Count;

            if (length < 16) return MessagePackTypeCode.ArrayFix;
            else if (length <= ushort.MaxValue) return MessagePackTypeCode.Array16;
            else return MessagePackTypeCode.Array32;
        }

        /// <summary>
        /// Gets the total length of the serialized data for the array.
        /// </summary>
        /// <param name="value">The array for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(System.Collections.ICollection value)
        {
            var answer = 1;

            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode != MessagePackTypeCode.ArrayFix)
                answer += this.GetLengthBytes(value.Count, SupportedLengths.FromShortUpward).Length;

            foreach (var elementValue in value)
                if (elementValue == null) answer += this.binarySerializer.GetWriter(BinaryTypeCode.Nil).GetDataLength(elementValue);
                else answer += this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementValue.GetType())).GetDataLength(elementValue);

            return answer;
        }

        /// <summary>
        /// Writes the array to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The array to serialize.</param>
        public override void Write(System.IO.Stream stream, System.Collections.ICollection value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode == MessagePackTypeCode.ArrayFix) this.WriteByte(stream, this.GetLength(messagePackTypeCode, value.Count));
            else
            {
                this.WriteByte(stream, messagePackTypeCode);
                this.WriteBytes(stream, this.GetLengthBytes(value.Count, SupportedLengths.FromShortUpward));
            }

            foreach (var elementValue in value)
                if (elementValue == null) this.binarySerializer.GetWriter(BinaryTypeCode.Nil).Write(stream, elementValue);
                else this.binarySerializer.GetWriter(BinaryUtils.GetBinaryTypeCode(elementValue.GetType())).Write(stream, elementValue);
        }

    }

}

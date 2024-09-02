using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize binary data (byte arrays) into MessagePack binary format.
    /// </summary>
    class BinaryBinaryWriter : AbstractCollectionBinaryWriter<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public BinaryBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with binary data (byte arrays).
        /// </summary>
        /// <returns>The binary type code for binary data.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Binary;

        /// <summary>
        /// Gets the MessagePack type code based on the length of the binary data.
        /// </summary>
        /// <param name="value">The binary data (byte array) for which to determine the type code.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(byte[] value)
        {
            var length = value.Length;

            if (length < byte.MaxValue) return MessagePackTypeCode.Bin8;
            else if (length <= ushort.MaxValue) return MessagePackTypeCode.Bin16;
            else return MessagePackTypeCode.Bin32;
        }

        /// <summary>
        /// Gets the total length of the serialized data for the binary data (byte array).
        /// </summary>
        /// <param name="value">The binary data (byte array) for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(byte[] value)
        {
            return 1 + this.GetLengthBytes(value.Length, SupportedLengths.All).Length + value.Length;
        }

        /// <summary>
        /// Writes the binary data (byte array) to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The binary data (byte array) to serialize.</param>
        public override void Write(System.IO.Stream stream, byte[] value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            this.WriteByte(stream, messagePackTypeCode);
            this.WriteBytes(stream, this.GetLengthBytes(value.Length, SupportedLengths.All));
            this.WriteBytes(stream, value);
        }

    }

}

using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing floating-point numbers from a binary stream.
    /// </summary>
    class FloatBinaryReader : BinaryReader<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public FloatBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with floating-point numbers.
        /// </summary>
        /// <returns>The binary type code for floating-point numbers as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Float;

        /// <summary>
        /// Reads and deserializes a floating-point number from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as a floating-point number.</param>
        /// <returns>An object representing the deserialized floating-point number, either as a <see cref="double"/> or a <see cref="float"/>.</returns>
        public override object Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            byte[] bytes;

            if (messagePackTypeCode == MessagePackTypeCode.Float64) bytes = this.ReadBytes(stream, 8);
            else bytes = this.ReadBytes(stream, 4);

            BinaryUtils.SwapIfLittleEndian(ref bytes);

            if (messagePackTypeCode == MessagePackTypeCode.Float64) return System.BitConverter.ToDouble(bytes, 0);
            return System.BitConverter.ToSingle(bytes, 0);
        }

    }

}

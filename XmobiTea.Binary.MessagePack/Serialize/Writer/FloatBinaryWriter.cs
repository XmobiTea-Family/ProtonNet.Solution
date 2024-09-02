using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize floating-point numbers (both single and double precision) into MessagePack binary format.
    /// </summary>
    class FloatBinaryWriter : BinaryWriter<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public FloatBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with floating-point numbers.
        /// </summary>
        /// <returns>The binary type code for floating-point numbers.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Float;

        /// <summary>
        /// Gets the MessagePack type code for the specified floating-point value.
        /// </summary>
        /// <param name="value">The floating-point value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(object value)
        {
            if (value is float)
                return MessagePackTypeCode.Float32;
            else
            {
                // if (value > float.MaxValue || value < float.MinValue) 

                return MessagePackTypeCode.Float64;
            }
        }

        /// <summary>
        /// Gets the total length of the serialized data for the floating-point value.
        /// </summary>
        /// <param name="value">The floating-point value for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(object value) => value is float ? 5 : 9;

        /// <summary>
        /// Writes the floating-point value to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The floating-point value to serialize.</param>
        public override void Write(System.IO.Stream stream, object value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);
            this.WriteByte(stream, messagePackTypeCode);

            byte[] buffer;

            if (messagePackTypeCode == MessagePackTypeCode.Float32) buffer = System.BitConverter.GetBytes((float)value);
            else buffer = System.BitConverter.GetBytes((double)value);

            BinaryUtils.SwapIfLittleEndian(ref buffer);

            this.WriteBytes(stream, buffer);
        }

    }

}

using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing integer values from a binary stream.
    /// </summary>
    class IntegerBinaryReader : BinaryReader<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public IntegerBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with integers.
        /// </summary>
        /// <returns>The binary type code for integers as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Integer;

        /// <summary>
        /// Reads and deserializes an integer value from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as an integer.</param>
        /// <returns>An object representing the deserialized integer value, which can be a signed or unsigned integer of various sizes.</returns>
        public override object Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            if ((messagePackTypeCode & 0xE0) == 0xE0) return (sbyte)messagePackTypeCode;
            else if ((messagePackTypeCode & 0x80) == 0) return (byte)(messagePackTypeCode & 0x7F);
            else if (messagePackTypeCode == MessagePackTypeCode.Int8) return (sbyte)this.ReadByte(stream);
            else if (messagePackTypeCode == MessagePackTypeCode.UInt8) return this.ReadByte(stream);
            else
            {
                byte[] buffer;

                if (messagePackTypeCode == MessagePackTypeCode.Int16 || messagePackTypeCode == MessagePackTypeCode.UInt16) buffer = this.ReadBytes(stream, 2);
                else if (messagePackTypeCode == MessagePackTypeCode.Int32 || messagePackTypeCode == MessagePackTypeCode.UInt32) buffer = this.ReadBytes(stream, 4);
                else buffer = this.ReadBytes(stream, 8);

                BinaryUtils.SwapIfLittleEndian(ref buffer);

                if (messagePackTypeCode == MessagePackTypeCode.Int16) return System.BitConverter.ToInt16(buffer, 0);
                else if (messagePackTypeCode == MessagePackTypeCode.UInt16) return System.BitConverter.ToUInt16(buffer, 0);
                else if (messagePackTypeCode == MessagePackTypeCode.Int32) return System.BitConverter.ToInt32(buffer, 0);
                else if (messagePackTypeCode == MessagePackTypeCode.UInt32) return System.BitConverter.ToUInt32(buffer, 0);
                else if (messagePackTypeCode == MessagePackTypeCode.Int64) return System.BitConverter.ToInt64(buffer, 0);
                else return System.BitConverter.ToUInt64(buffer, 0);
            }
        }

    }

}

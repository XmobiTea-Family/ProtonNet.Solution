using System;
using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize integer values (both signed and unsigned) into MessagePack binary format.
    /// </summary>
    class IntegerBinaryWriter : BinaryWriter<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public IntegerBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with integer values.
        /// </summary>
        /// <returns>The binary type code for integer values.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Integer;

        /// <summary>
        /// Determines whether the specified value is a signed integer.
        /// </summary>
        /// <param name="value">The integer value to check.</param>
        /// <returns><c>true</c> if the value is signed; otherwise, <c>false</c>.</returns>
        private bool IsSigned(object value)
        {
            byte messagePackTypeCode;
            long svalue;
            ulong uvalue;

            if (value is sbyte sbyteValue)
            {
                messagePackTypeCode = sbyteValue >= -0x1F && sbyteValue <= 0x1F ? MessagePackTypeCode.NegativeIntFix : MessagePackTypeCode.Int8;
                svalue = sbyteValue;
                uvalue = 0;
            }
            else if (value is short shortValue)
            {
                messagePackTypeCode = MessagePackTypeCode.Int16;
                svalue = shortValue;
                uvalue = 0;
            }
            else if (value is int intValue)
            {
                messagePackTypeCode = MessagePackTypeCode.Int32;
                svalue = intValue;
                uvalue = 0;
            }
            else if (value is long longValue)
            {
                messagePackTypeCode = MessagePackTypeCode.Int64;
                svalue = longValue;
                uvalue = 0;
            }
            else if (value is byte byteValue)
            {
                messagePackTypeCode = (byteValue <= 0x7F) ? MessagePackTypeCode.PositiveIntFix : MessagePackTypeCode.UInt8;
                svalue = 0;
                uvalue = byteValue;
            }
            else if (value is ushort ushortValue)
            {
                messagePackTypeCode = MessagePackTypeCode.UInt16;
                svalue = 0;
                uvalue = ushortValue;
            }
            else if (value is uint uintValue)
            {
                messagePackTypeCode = MessagePackTypeCode.UInt32;
                svalue = 0;
                uvalue = uintValue;
            }
            else if (value is ulong ulongValue)
            {
                messagePackTypeCode = MessagePackTypeCode.UInt64;
                svalue = 0;
                uvalue = ulongValue;
            }
            else
            {
                throw new ArgumentException("Unsupported integer type.");
            }

            return svalue < 0;
        }

        /// <summary>
        /// Gets the MessagePack type code for the specified integer value.
        /// </summary>
        /// <param name="value">The integer value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(object value)
        {
            if (this.IsSigned(value))
            {
                var valueLong = System.Convert.ToInt64(value);

                if (valueLong >= -0x1F && valueLong <= 0x1F)
                    return MessagePackTypeCode.NegativeIntFix;
                else if (valueLong >= sbyte.MinValue && valueLong <= sbyte.MaxValue)
                    return MessagePackTypeCode.Int8;
                else if (valueLong >= short.MinValue && valueLong <= short.MaxValue)
                    return MessagePackTypeCode.Int16;
                else if (valueLong >= int.MinValue && valueLong <= int.MaxValue)
                    return MessagePackTypeCode.Int32;
                else
                    return MessagePackTypeCode.Int64;
            }
            else
            {
                var valueULong = System.Convert.ToUInt64(value);
                if (valueULong <= 0x7F)
                    return MessagePackTypeCode.PositiveIntFix;
                else if (valueULong <= byte.MaxValue)
                    return MessagePackTypeCode.UInt8;
                else if (valueULong <= ushort.MaxValue)
                    return MessagePackTypeCode.UInt16;
                else if (valueULong <= uint.MaxValue)
                    return MessagePackTypeCode.UInt32;
                else
                    return MessagePackTypeCode.UInt64;
            }
        }

        /// <summary>
        /// Gets the total length of the serialized data for the integer value.
        /// </summary>
        /// <param name="value">The integer value for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(object value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode == MessagePackTypeCode.NegativeIntFix || messagePackTypeCode == MessagePackTypeCode.PositiveIntFix)
                return 1;
            else if (messagePackTypeCode == MessagePackTypeCode.Int8 || messagePackTypeCode == MessagePackTypeCode.UInt8)
                return 2;
            else if (messagePackTypeCode == MessagePackTypeCode.Int16 || messagePackTypeCode == MessagePackTypeCode.UInt16)
                return 3;
            else if (messagePackTypeCode == MessagePackTypeCode.Int32 || messagePackTypeCode == MessagePackTypeCode.UInt32)
                return 5;
            else
                return 9;
        }

        /// <summary>
        /// Writes the integer value to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The integer value to serialize.</param>
        public override void Write(System.IO.Stream stream, object value)
        {
            var messagePackTypeCode = this.GetMessagePackTypeCode(value);

            if (messagePackTypeCode == MessagePackTypeCode.NegativeIntFix || messagePackTypeCode == MessagePackTypeCode.PositiveIntFix)
            {
                if ((messagePackTypeCode & 0x7F) == 0)
                {
                    this.WriteByte(stream, (byte)(messagePackTypeCode | System.Convert.ToByte(value)));
                    return;
                }
                else if ((messagePackTypeCode & 0xE0) == 0xE0)
                {
                    var bytes = System.BitConverter.GetBytes(System.Convert.ToInt64(value));
                    BinaryUtils.SwapIfLittleEndian(ref bytes);

                    this.WriteByte(stream, (byte)(messagePackTypeCode | bytes[bytes.Length - 1]));
                    return;
                }
            }

            this.WriteByte(stream, messagePackTypeCode);

            if (messagePackTypeCode == MessagePackTypeCode.Int8)
            {
                this.WriteByte(stream, (byte)System.Convert.ToSByte(value));
            }
            else if (messagePackTypeCode == MessagePackTypeCode.UInt8)
            {
                this.WriteByte(stream, System.Convert.ToByte(value));
            }
            else
            {
                byte[] buffer;

                if (messagePackTypeCode == MessagePackTypeCode.Int16)
                    buffer = System.BitConverter.GetBytes(System.Convert.ToInt16(value));
                else if (messagePackTypeCode == MessagePackTypeCode.UInt16)
                    buffer = System.BitConverter.GetBytes(System.Convert.ToUInt16(value));
                else if (messagePackTypeCode == MessagePackTypeCode.Int32)
                    buffer = System.BitConverter.GetBytes(System.Convert.ToInt32(value));
                else if (messagePackTypeCode == MessagePackTypeCode.UInt32)
                    buffer = System.BitConverter.GetBytes(System.Convert.ToUInt32(value));
                else if (messagePackTypeCode == MessagePackTypeCode.Int64)
                    buffer = System.BitConverter.GetBytes(System.Convert.ToInt64(value));
                else
                    buffer = System.BitConverter.GetBytes(System.Convert.ToUInt64(value));

                BinaryUtils.SwapIfLittleEndian(ref buffer);
                this.WriteBytes(stream, buffer);
            }
        }

    }

}

using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides an abstract base class for implementing binary writers for collection types.
    /// </summary>
    /// <typeparam name="T">The type of the collection to be serialized.</typeparam>
    abstract class AbstractCollectionBinaryWriter<T> : BinaryWriter<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCollectionBinaryWriter{T}"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public AbstractCollectionBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Converts the length of the collection into a byte array, considering the supported lengths and endianness.
        /// </summary>
        /// <param name="length">The length of the collection.</param>
        /// <param name="supported">The supported length formats.</param>
        /// <returns>A byte array representing the length in binary format.</returns>
        protected byte[] GetLengthBytes(long length, SupportedLengths supported)
        {
            if (length < 256 && (supported & SupportedLengths.Byte1) > 0) return new byte[1] { (byte)length };
            byte[] bytes; // from here we should worry about endianness
            if (length <= ushort.MaxValue && (supported & SupportedLengths.Short2) > 0) bytes = System.BitConverter.GetBytes((ushort)length);
            else if (length <= uint.MaxValue && (supported & SupportedLengths.Int4) > 0) bytes = System.BitConverter.GetBytes((uint)length);
            else bytes = System.BitConverter.GetBytes((ulong)length);

            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return bytes;
        }

        /// <summary>
        /// Combines the MessagePack type code with the length to produce the final byte representation.
        /// </summary>
        /// <param name="messagePackTypeCode">The MessagePack type code for the collection.</param>
        /// <param name="length">The length of the collection.</param>
        /// <returns>The combined byte representing the type and length.</returns>
        protected byte GetLength(byte messagePackTypeCode, int length)
        {
            return (byte)(messagePackTypeCode | (byte)length);
        }

    }

}

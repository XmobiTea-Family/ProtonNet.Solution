using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Helper;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Provides a base class for reading binary data for collection types from a stream and interpreting it according to a type code.
    /// </summary>
    /// <typeparam name="T">The type of data to read.</typeparam>
    abstract class AbstractCollectionBinaryReader<T> : BinaryReader<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCollectionBinaryReader{T}"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public AbstractCollectionBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the length of the collection from the binary stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="byteLength">The number of bytes representing the length of the collection.</param>
        /// <returns>The length of the collection as a uint.</returns>
        protected uint GetCollectionLength(System.IO.Stream stream, byte byteLength)
        {
            if (byteLength == 1) return this.ReadByte(stream);

            var bytes = this.ReadBytes(stream, byteLength);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            if (byteLength == 2) return System.BitConverter.ToUInt16(bytes, 0);
            return System.BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Determines whether the provided type code matches the expected definition, and extracts the length value if applicable.
        /// </summary>
        /// <param name="definitionMessageTypeCode">The expected type code definition.</param>
        /// <param name="messagePackTypeCode">The actual type code from the message pack.</param>
        /// <param name="valueMask">The mask to apply to the type code to extract the length.</param>
        /// <param name="length">When this method returns, contains the extracted length if the mask matched; otherwise, zero.</param>
        /// <returns><c>true</c> if the type code matches the definition; otherwise, <c>false</c>.</returns>
        protected bool IsMasked(byte definitionMessageTypeCode, byte messagePackTypeCode, byte valueMask, out uint length)
        {
            length = (uint)(messagePackTypeCode & valueMask);

            if (messagePackTypeCode - length == definitionMessageTypeCode)
                return true;
            else
            {
                length = 0;
                return false;
            }
        }

    }

}

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader.Core
{
    /// <summary>
    /// Defines methods for reading binary data from a stream and interpreting it according to a type code.
    /// </summary>
    interface IBinaryReader
    {
        /// <summary>
        /// Gets the binary type code associated with the reader.
        /// </summary>
        /// <returns>The binary type code as a byte.</returns>
        byte GetBinaryTypeCode();

        /// <summary>
        /// Reads and interprets binary data from the specified stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data.</param>
        /// <returns>The interpreted object.</returns>
        object Read(System.IO.Stream stream, byte messagePackTypeCode);
    }

    /// <summary>
    /// Defines methods for reading binary data of a specific type from a stream and interpreting it according to a type code.
    /// </summary>
    /// <typeparam name="T">The type of data to read.</typeparam>
    interface IBinaryReader<T>
    {
        /// <summary>
        /// Reads and interprets binary data of type <typeparamref name="T"/> from the specified stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data.</param>
        /// <returns>The interpreted object of type <typeparamref name="T"/>.</returns>
        T Read(System.IO.Stream stream, byte messagePackTypeCode);
    }

    /// <summary>
    /// Provides a base class for reading binary data of a specific type from a stream and interpreting it according to a type code.
    /// </summary>
    /// <typeparam name="T">The type of data to read.</typeparam>
    abstract class BinaryReader<T> : IBinaryReader<T>, IBinaryReader
    {
        /// <summary>
        /// Gets the binary deserializer used for interpreting binary data.
        /// </summary>
        protected IBinaryDeserializer binaryDeserializer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryReader{T}"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public BinaryReader(IBinaryDeserializer binaryDeserializer)
        {
            this.binaryDeserializer = binaryDeserializer;
        }

        /// <summary>
        /// Gets the binary type code associated with the reader.
        /// </summary>
        /// <returns>The binary type code as a byte.</returns>
        public abstract byte GetBinaryTypeCode();

        /// <summary>
        /// Reads and interprets binary data of type <typeparamref name="T"/> from the specified stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="binaryTypeCode">The type code used to interpret the binary data.</param>
        /// <returns>The interpreted object of type <typeparamref name="T"/>.</returns>
        public abstract T Read(System.IO.Stream stream, byte binaryTypeCode);

        /// <summary>
        /// Reads and interprets binary data from the specified stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="binaryTypeCode">The type code used to interpret the binary data.</param>
        /// <returns>The interpreted object.</returns>
        object IBinaryReader.Read(System.IO.Stream stream, byte binaryTypeCode) => this.Read(stream, binaryTypeCode);

        /// <summary>
        /// Reads a specified number of bytes from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>An array of bytes read from the stream.</returns>
        protected byte[] ReadBytes(System.IO.Stream stream, ushort length)
        {
            var answer = new byte[length];
            stream.Read(answer, 0, length);
            return answer;
        }

        /// <summary>
        /// Reads a single byte from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The byte read from the stream.</returns>
        protected byte ReadByte(System.IO.Stream stream) => (byte)stream.ReadByte();
    }
}

namespace XmobiTea.Binary.MessagePack.Serialize.Writer.Core
{
    /// <summary>
    /// Defines the contract for a binary writer capable of serializing objects into a binary format.
    /// </summary>
    interface IBinaryWriter
    {
        /// <summary>Gets the binary type code associated with the data type.</summary>
        /// <returns>The binary type code as a byte.</returns>
        byte GetBinaryTypeCode();

        /// <summary>Gets the MessagePack type code for the specified value.</summary>
        /// <param name="value">The value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        byte GetMessagePackTypeCode(object value);

        /// <summary>Gets the length of the data when serialized.</summary>
        /// <param name="value">The value to determine the data length for.</param>
        /// <returns>The length of the data as an integer.</returns>
        int GetDataLength(object value);

        /// <summary>Writes the specified value to the provided stream in binary format.</summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write.</param>
        void Write(System.IO.Stream stream, object value);
    }

    /// <summary>
    /// Defines the contract for a binary writer capable of serializing objects of a specific type <typeparamref name="T"/> into a binary format.
    /// </summary>
    /// <typeparam name="T">The type of object that this writer will serialize.</typeparam>
    interface IBinaryWriter<T>
    {
        /// <summary>Gets the MessagePack type code for the specified value.</summary>
        /// <param name="value">The value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        byte GetMessagePackTypeCode(T value);

        /// <summary>Gets the length of the data when serialized.</summary>
        /// <param name="value">The value to determine the data length for.</param>
        /// <returns>The length of the data as an integer.</returns>
        int GetDataLength(T value);

        /// <summary>Writes the specified value to the provided stream in binary format.</summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write.</param>
        void Write(System.IO.Stream stream, T value);
    }

    /// <summary>
    /// Provides an abstract base class for implementing binary writers for specific data types.
    /// </summary>
    /// <typeparam name="T">The type of object that this writer will serialize.</typeparam>
    abstract class BinaryWriter<T> : IBinaryWriter<T>, IBinaryWriter
    {
        /// <summary>
        /// Gets the binary serializer used by this writer to serialize other objects.
        /// </summary>
        protected IBinarySerializer binarySerializer { get; }

        /// <summary>Gets the binary type code associated with the data type.</summary>
        /// <returns>The binary type code as a byte.</returns>
        public abstract byte GetBinaryTypeCode();

        /// <summary>Gets the MessagePack type code for the specified value.</summary>
        /// <param name="value">The value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public abstract byte GetMessagePackTypeCode(T value);

        /// <summary>Gets the length of the data when serialized.</summary>
        /// <param name="value">The value to determine the data length for.</param>
        /// <returns>The length of the data as an integer.</returns>
        public abstract int GetDataLength(T value);

        /// <summary>Writes the specified value to the provided stream in binary format.</summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write.</param>
        public abstract void Write(System.IO.Stream stream, T value);

        /// <summary>Gets the length of the data when serialized.</summary>
        /// <param name="value">The value to determine the data length for.</param>
        /// <returns>The length of the data as an integer.</returns>
        int IBinaryWriter.GetDataLength(object value) => this.GetDataLength((T)value);

        /// <summary>Writes the specified value to the provided stream in binary format.</summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write.</param>
        void IBinaryWriter.Write(System.IO.Stream stream, object value) => this.Write(stream, (T)value);

        /// <summary>Gets the MessagePack type code for the specified value.</summary>
        /// <param name="value">The value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        byte IBinaryWriter.GetMessagePackTypeCode(object value) => this.GetMessagePackTypeCode((T)value);

        /// <summary>
        /// Writes the specified byte array to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="data">The byte array to write.</param>
        protected void WriteBytes(System.IO.Stream stream, byte[] data) => stream.Write(data, 0, data.Length);

        /// <summary>
        /// Writes the specified byte to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="data">The byte to write.</param>
        protected void WriteByte(System.IO.Stream stream, byte data) => stream.WriteByte(data);

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryWriter{T}"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public BinaryWriter(IBinarySerializer binarySerializer)
        {
            this.binarySerializer = binarySerializer;
        }

    }

}

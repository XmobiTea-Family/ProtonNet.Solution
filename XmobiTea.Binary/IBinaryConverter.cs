namespace XmobiTea.Binary
{
    /// <summary>
    /// Defines methods for serializing and deserializing objects to and from binary data.
    /// </summary>
    public interface IBinaryConverter
    {
        /// <summary>
        /// Serializes the specified object to a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="tObj">The object to serialize.</param>
        /// <returns>A byte array representing the serialized object.</returns>
        byte[] Serialize<T>(T tObj);

        /// <summary>
        /// Serializes the specified object and writes the binary data to a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="stream">The stream to which the binary data is written.</param>
        /// <param name="tObj">The object to serialize.</param>
        void Serialize<T>(System.IO.Stream stream, T tObj);

        /// <summary>
        /// Deserializes an object of the specified type from a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stream">The stream from which the object is deserialized.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(System.IO.Stream stream);

        /// <summary>
        /// Deserializes an object of the specified type from a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="data">The byte array containing the serialized object.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(byte[] data);

        /// <summary>
        /// Tries to parse and deserialize an object of the specified type from a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stream">The stream from which the object is deserialized.</param>
        /// <param name="tValue">When this method returns, contains the deserialized object of type <typeparamref name="T"/> if the parsing succeeded, or the default value of <typeparamref name="T"/> if the parsing failed.</param>
        /// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
        bool TryParse<T>(System.IO.Stream stream, out T tValue);

        /// <summary>
        /// Tries to parse and deserialize an object of the specified type from a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="data">The byte array containing the serialized object.</param>
        /// <param name="tValue">When this method returns, contains the deserialized object of type <typeparamref name="T"/> if the parsing succeeded, or the default value of <typeparamref name="T"/> if the parsing failed.</param>
        /// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
        bool TryParse<T>(byte[] data, out T tValue);

    }

}

using XmobiTea.Binary.MessagePack.Deserialize;
using XmobiTea.Binary.MessagePack.Serialize;

namespace XmobiTea.Binary.MessagePack
{
    /// <summary>
    /// Provides methods for serializing and deserializing objects using MessagePack.
    /// </summary>
    public class BinaryConverter : IBinaryConverter
    {
        /// <summary>
        /// Gets the serializer for converting objects to binary format.
        /// </summary>
        private IBinarySerializer serializer { get; }

        /// <summary>
        /// Gets the deserializer for converting binary data back to objects.
        /// </summary>
        private IBinaryDeserializer deserializer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryConverter"/> class.
        /// </summary>
        public BinaryConverter()
        {
            this.serializer = new BinarySerializer();
            this.deserializer = new BinaryDeserializer();
        }

        /// <summary>
        /// Serializes the specified object to a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="tObj">The object to serialize.</param>
        /// <returns>A byte array containing the serialized data.</returns>
        public byte[] Serialize<T>(T tObj) => this.serializer.Serialize(tObj);

        /// <summary>
        /// Serializes the specified object and writes the binary data to the provided stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="stream">The stream to write the serialized data to.</param>
        /// <param name="tObj">The object to serialize.</param>
        public void Serialize<T>(System.IO.Stream stream, T tObj) => this.serializer.Serialize(stream, tObj);

        /// <summary>
        /// Deserializes data from the provided stream into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stream">The stream containing the binary data.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(System.IO.Stream stream) => this.deserializer.Deserialize<T>(stream);

        /// <summary>
        /// Deserializes the specified byte array into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="data">The byte array containing the binary data.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(byte[] data) => this.deserializer.Deserialize<T>(data);

        /// <summary>
        /// Tries to deserialize data from the provided stream into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="stream">The stream containing the binary data.</param>
        /// <param name="tValue">When this method returns, contains the deserialized object.</param>
        /// <returns><c>true</c> if the deserialization succeeded; otherwise, <c>false</c>.</returns>
        public bool TryParse<T>(System.IO.Stream stream, out T tValue)
        {
            try
            {
                tValue = this.deserializer.Deserialize<T>(stream);
                return true;
            }
            catch
            {
                tValue = default;
                return false;
            }
        }

        /// <summary>
        /// Tries to deserialize the specified byte array into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="data">The byte array containing the binary data.</param>
        /// <param name="tValue">When this method returns, contains the deserialized object.</param>
        /// <returns><c>true</c> if the deserialization succeeded; otherwise, <c>false</c>.</returns>
        public bool TryParse<T>(byte[] data, out T tValue)
        {
            try
            {
                tValue = this.deserializer.Deserialize<T>(data);
                return true;
            }
            catch
            {
                tValue = default;
                return false;
            }
        }

    }

}

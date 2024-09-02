using XmobiTea.Data.Converter.Models;

namespace XmobiTea.Data.Converter
{
    /// <summary>
    /// Provides methods for serializing and deserializing objects and arrays using custom converters.
    /// </summary>
    public class DataConverter : IDataConverter
    {
        /// <summary>
        /// Gets the serializer converter instance.
        /// </summary>
        private ISerializeConverter serializeConverter { get; }

        /// <summary>
        /// Gets the deserializer converter instance.
        /// </summary>
        private IDeserializeConverter deserializeConverter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConverter"/> class.
        /// </summary>
        public DataConverter()
        {
            var dataMemberFieldInfoMapper = new DataMemberFieldInfoTypeMapper();

            this.serializeConverter = new SerializeConverter(dataMemberFieldInfoMapper);
            this.deserializeConverter = new DeserializeConverter(dataMemberFieldInfoMapper);
        }

        /// <summary>
        /// Deserializes a <see cref="GNHashtable"/> to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="gnHashtable">The GNHashtable to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public T DeserializeObject<T>(GNHashtable gnHashtable) => (T)this.DeserializeObject(gnHashtable, typeof(T));

        /// <summary>
        /// Deserializes a <see cref="GNHashtable"/> to an object of the specified type.
        /// </summary>
        /// <param name="gnHashtable">The GNHashtable to deserialize.</param>
        /// <param name="cls">The type of object to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public object DeserializeObject(GNHashtable gnHashtable, System.Type cls) => this.deserializeConverter.DeserializeObject(gnHashtable, cls);

        /// <summary>
        /// Deserializes a <see cref="GNArray"/> to a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="gnArray">The GNArray to deserialize.</param>
        /// <returns>The deserialized list of objects.</returns>
        public System.Collections.IList DeserializeArray<T>(GNArray gnArray) => this.DeserializeArray<T>(gnArray);

        /// <summary>
        /// Deserializes a <see cref="GNArray"/> to a list of objects of the specified type.
        /// </summary>
        /// <param name="gnArray">The GNArray to deserialize.</param>
        /// <param name="cls">The type of objects in the list.</param>
        /// <returns>The deserialized list of objects.</returns>
        public System.Collections.IList DeserializeArray(GNArray gnArray, System.Type cls) => this.deserializeConverter.DeserializeArray(gnArray, cls);

        /// <summary>
        /// Serializes an object to a <see cref="GNHashtable"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized GNHashtable.</returns>
        public GNHashtable SerializeObject(object obj) => this.serializeConverter.SerializeObject(obj);

        /// <summary>
        /// Serializes a list of objects to a <see cref="GNArray"/>.
        /// </summary>
        /// <param name="objLst">The list of objects to serialize.</param>
        /// <returns>The serialized GNArray.</returns>
        public GNArray SerializeArray(System.Collections.IList objLst) => this.serializeConverter.SerializeArray(objLst);

    }

}

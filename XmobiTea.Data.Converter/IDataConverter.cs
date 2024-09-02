namespace XmobiTea.Data.Converter
{
    /// <summary>
    /// Defines methods for serializing and deserializing objects and arrays.
    /// </summary>
    public interface IDataConverter
    {
        /// <summary>
        /// Deserializes a <see cref="GNHashtable"/> to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="gnHashtable">The GNHashtable to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        T DeserializeObject<T>(GNHashtable gnHashtable);

        /// <summary>
        /// Deserializes a <see cref="GNHashtable"/> to an object of the specified type.
        /// </summary>
        /// <param name="gnHashtable">The GNHashtable to deserialize.</param>
        /// <param name="cls">The type of object to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        object DeserializeObject(GNHashtable gnHashtable, System.Type cls);

        /// <summary>
        /// Deserializes a <see cref="GNArray"/> to a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="gnArray">The GNArray to deserialize.</param>
        /// <returns>The deserialized list of objects.</returns>
        System.Collections.IList DeserializeArray<T>(GNArray gnArray);

        /// <summary>
        /// Deserializes a <see cref="GNArray"/> to a list of objects of the specified type.
        /// </summary>
        /// <param name="gnArray">The GNArray to deserialize.</param>
        /// <param name="cls">The type of objects in the list.</param>
        /// <returns>The deserialized list of objects.</returns>
        System.Collections.IList DeserializeArray(GNArray gnArray, System.Type cls);

        /// <summary>
        /// Serializes an object to a <see cref="GNHashtable"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized GNHashtable.</returns>
        GNHashtable SerializeObject(object obj);

        /// <summary>
        /// Serializes a list of objects to a <see cref="GNArray"/>.
        /// </summary>
        /// <param name="objLst">The list of objects to serialize.</param>
        /// <returns>The serialized GNArray.</returns>
        GNArray SerializeArray(System.Collections.IList objLst);

    }

}

using System.Collections.Generic;
using XmobiTea.Data.Converter.Helper;
using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Models
{
    /// <summary>
    /// Interface for converting GNHashtable and GNArray objects to their respective types.
    /// </summary>
    interface IDeserializeConverter
    {
        /// <summary>
        /// Deserializes a GNHashtable to a specified object type.
        /// </summary>
        /// <param name="gnHashtable">The GNHashtable object to deserialize.</param>
        /// <param name="cls">The target class type to deserialize into.</param>
        /// <returns>An object of the specified type deserialized from the GNHashtable.</returns>
        object DeserializeObject(GNHashtable gnHashtable, System.Type cls);

        /// <summary>
        /// Deserializes a GNArray to a specified list type.
        /// </summary>
        /// <param name="gnArray">The GNArray object to deserialize.</param>
        /// <param name="cls">The target class type for the elements in the list.</param>
        /// <returns>A list of objects deserialized from the GNArray.</returns>
        System.Collections.IList DeserializeArray(GNArray gnArray, System.Type cls);
    }

    /// <summary>
    /// Implementation of IDeserializeConverter for converting GNHashtable and GNArray objects to their respective types.
    /// </summary>
    class DeserializeConverter : IDeserializeConverter
    {
        /// <summary>
        /// Delegate for handling different types of deserialization parsing.
        /// </summary>
        private delegate void DeserializeParserDelegate(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer);

        /// <summary>
        /// Dictionary mapping GNFieldDataType to corresponding deserialization handlers.
        /// </summary>
        private IDictionary<GNFieldDataType, DeserializeParserDelegate> deserializerParserDict { get; }

        /// <summary>
        /// Mapper for retrieving metadata about data member fields.
        /// </summary>
        private IDataMemberFieldInfoTypeMapper dataMemberFieldInfoMapper { get; }

        private System.Type typeOfByte { get; }
        private System.Type typeOfSByte { get; }
        private System.Type typeOfShort { get; }
        private System.Type typeOfInt { get; }
        private System.Type typeOfFloat { get; }
        private System.Type typeOfLong { get; }
        private System.Type typeOfDouble { get; }
        private System.Type typeOfBool { get; }
        private System.Type typeOfString { get; }
        private System.Type typeOfGNArray { get; }
        private System.Type typeOfGNHashtable { get; }

        /// <summary>
        /// Initializes a new instance of the DeserializeConverter class.
        /// </summary>
        /// <param name="dataMemberFieldInfoMapper">The mapper to retrieve field metadata information.</param>
        public DeserializeConverter(IDataMemberFieldInfoTypeMapper dataMemberFieldInfoMapper)
        {
            this.typeOfByte = typeof(byte);
            this.typeOfSByte = typeof(sbyte);
            this.typeOfShort = typeof(short);
            this.typeOfInt = typeof(int);
            this.typeOfFloat = typeof(float);
            this.typeOfLong = typeof(long);
            this.typeOfDouble = typeof(double);
            this.typeOfBool = typeof(bool);
            this.typeOfString = typeof(string);
            this.typeOfGNArray = typeof(GNArray);
            this.typeOfGNHashtable = typeof(GNHashtable);

            this.dataMemberFieldInfoMapper = dataMemberFieldInfoMapper;

            this.deserializerParserDict = new Dictionary<GNFieldDataType, DeserializeParserDelegate>();

            this.AddDeserializeParserHandlers();
        }

        /// <summary>
        /// Adds deserialization handlers for different GNFieldDataType values.
        /// </summary>
        private void AddDeserializeParserHandlers()
        {
            this.deserializerParserDict[GNFieldDataType.String] = this.StringDeserializer;
            this.deserializerParserDict[GNFieldDataType.Boolean] = this.BooleanDeserializer;
            this.deserializerParserDict[GNFieldDataType.GNHashtable] = this.GNHashtableDeserializer;
            this.deserializerParserDict[GNFieldDataType.GNArray] = this.GNArrayDeserializer;
            this.deserializerParserDict[GNFieldDataType.Number] = this.NumberDeserializer;
            this.deserializerParserDict[GNFieldDataType.Other] = this.OtherDeserializer;
        }

        /// <summary>
        /// Retrieves the appropriate deserialization handler based on the GNFieldDataType.
        /// </summary>
        /// <param name="gnFieldType">The GNFieldDataType to get the handler for.</param>
        /// <returns>The corresponding deserialization handler.</returns>
        private DeserializeParserDelegate GetDeserializeParserHandler(GNFieldDataType gnFieldType) => this.deserializerParserDict[gnFieldType];

        private void StringDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
            else if (value is string) declaredField.FieldInfo.SetValue(answer, value);
        }

        private void BooleanDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
            else if (value is bool) declaredField.FieldInfo.SetValue(answer, value);
        }

        private void GNHashtableDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null)
                if (declaredField.DefaultValue != null)
                    if (declaredField.FieldInfo.FieldType == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IDictionary)) declaredField.FieldInfo.SetValue(answer, ((GNHashtable)declaredField.DefaultValue).ToData());
                    else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)declaredField.DefaultValue, declaredField.Cls));
                else
                if (declaredField.FieldInfo.FieldType == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IDictionary)) declaredField.FieldInfo.SetValue(answer, ((IGNData)value).ToData());
                else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)value, declaredField.Cls));
        }

        private void GNArrayDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null)
                if (declaredField.DefaultValue != null)
                    if (declaredField.FieldInfo.FieldType == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IList)) declaredField.FieldInfo.SetValue(answer, ((GNArray)declaredField.DefaultValue).ToData());
                    else
                if (declaredField.FieldInfo.FieldType == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, value);
                    else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IList)) declaredField.FieldInfo.SetValue(answer, ((IGNData)value).ToData());
        }

        private void NumberDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
            else if (DetectSupport.isNumber(value)) declaredField.FieldInfo.SetValue(answer, value);
        }

        private void OtherDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer)
        {
            if (value == null)
                if (declaredField.DefaultValue != null)
                    if (declaredField.FieldInfo.FieldType == this.typeOfByte) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfSByte) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfShort) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfInt) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfFloat) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfLong) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfDouble) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfBool) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfString) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IList)) declaredField.FieldInfo.SetValue(answer, ((IGNData)declaredField.DefaultValue).ToData());
                    else if (declaredField.FieldInfo.FieldType == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, declaredField.DefaultValue);
                    else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IDictionary)) declaredField.FieldInfo.SetValue(answer, ((IGNData)declaredField.DefaultValue).ToData());
                    else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)declaredField.DefaultValue, declaredField.Cls));
                else
                if (declaredField.FieldInfo.FieldType == this.typeOfByte) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfSByte) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfShort) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfInt) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfFloat) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfLong) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfDouble) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfBool) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfString) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IList)) declaredField.FieldInfo.SetValue(answer, ((IGNData)value).ToData());
                else if (declaredField.FieldInfo.FieldType == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, value);
                else if (declaredField.FieldInfo.FieldType == typeof(System.Collections.IDictionary)) declaredField.FieldInfo.SetValue(answer, ((IGNData)value).ToData());
                else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)value, declaredField.Cls));
        }

        /// <summary>
        /// Deserializes a GNHashtable to the specified object type.
        /// </summary>
        /// <param name="gnHashtable">The GNHashtable to deserialize.</param>
        /// <param name="cls">The target class type for deserialization.</param>
        /// <returns>The deserialized object.</returns>
        public object DeserializeObject(GNHashtable gnHashtable, System.Type cls)
        {
            if (gnHashtable == null) return null;

            var declaredFields = this.dataMemberFieldInfoMapper.GetGNEnhancedObjectFieldMetadata(cls);

            var answer = System.Activator.CreateInstance(cls);

            foreach (var declaredField in declaredFields)
            {
                var value = gnHashtable.GetObject(declaredField.Code);

                var deserializerParser = this.GetDeserializeParserHandler(declaredField.GNFieldType);

                deserializerParser.Invoke(declaredField, value, answer);
            }

            return answer;
        }

        /// <summary>
        /// Deserializes a GNArray to a list of the specified type.
        /// </summary>
        /// <param name="gnArray">The GNArray to deserialize.</param>
        /// <param name="cls">The target class type for the elements in the list.</param>
        /// <returns>The deserialized list of objects.</returns>
        public System.Collections.IList DeserializeArray(GNArray gnArray, System.Type cls)
        {
            if (gnArray == null) return null;

            var answer = new List<object>();

            for (var i = 0; i < gnArray.Count(); i++)
            {
                var value = gnArray.GetObject(i);
                if (value == null) answer.Add(null);
                else
                {
                    var typeOfValue = value.GetType();

                    if (cls == this.typeOfByte) answer.Add(value);
                    else if (cls == this.typeOfSByte) answer.Add(value);
                    else if (cls == this.typeOfShort) answer.Add(value);
                    else if (cls == this.typeOfInt) answer.Add(value);
                    else if (cls == this.typeOfFloat) answer.Add(value);
                    else if (cls == this.typeOfLong) answer.Add(value);
                    else if (cls == this.typeOfDouble) answer.Add(value);
                    else if (cls == this.typeOfBool) answer.Add(value);
                    else if (cls == this.typeOfString) answer.Add(value);
                    else if (cls == this.typeOfGNArray) answer.Add(value);
                    else if (cls == this.typeOfGNHashtable) answer.Add(value);
                    else if (typeOfValue == typeof(System.Collections.IList))
                        answer.Add(this.DeserializeArray(gnArray.GetGNArray(i), cls));
                    else
                        answer.Add(this.DeserializeObject(gnArray.GetGNHashtable(i), cls));
                }
            }

            return answer;
        }

    }

}

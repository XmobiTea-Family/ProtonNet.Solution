using System.Collections.Generic;
using XmobiTea.Data.Converter.Helper;
using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Models
{
    /// <summary>
    /// Interface for converting objects and lists to GNHashtable and GNArray respectively.
    /// </summary>
    interface ISerializeConverter
    {
        /// <summary>
        /// Serializes an object to a GNHashtable.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A GNHashtable representing the serialized object.</returns>
        GNHashtable SerializeObject(object obj);

        /// <summary>
        /// Serializes a list of objects to a GNArray.
        /// </summary>
        /// <param name="objLst">The list of objects to serialize.</param>
        /// <returns>A GNArray representing the serialized list.</returns>
        GNArray SerializeArray(System.Collections.IList objLst);
    }

    /// <summary>
    /// Implementation of ISerializeConverter for converting objects and lists to GNHashtable and GNArray.
    /// </summary>
    class SerializeConverter : ISerializeConverter
    {
        /// <summary>
        /// Delegate for handling the serialization of individual fields.
        /// </summary>
        /// <param name="declaredField">The metadata of the field being serialized.</param>
        /// <param name="value">The value of the field being serialized.</param>
        /// <param name="gnHashtable">The GNHashtable to which the serialized data will be added.</param>
        private delegate void SerializeParserDelegate(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable);

        /// <summary>
        /// Dictionary mapping GNFieldDataType to appropriate serialization handlers.
        /// </summary>
        private IDictionary<GNFieldDataType, SerializeParserDelegate> serializeParserDict { get; }

        /// <summary>
        /// Mapper for obtaining field metadata for a given type.
        /// </summary>
        private IDataMemberFieldInfoTypeMapper dataMemberFieldInfoMapper { get; }

        /// <summary>
        /// Initializes a new instance of the SerializeConverter class.
        /// </summary>
        /// <param name="dataMemberFieldInfoMapper">The mapper for obtaining field metadata.</param>
        public SerializeConverter(IDataMemberFieldInfoTypeMapper dataMemberFieldInfoMapper)
        {
            this.dataMemberFieldInfoMapper = dataMemberFieldInfoMapper;

            this.serializeParserDict = new Dictionary<GNFieldDataType, SerializeParserDelegate>();

            this.AddSerializeParserHandlers();
        }

        /// <summary>
        /// Adds handlers for serializing different field types.
        /// </summary>
        void AddSerializeParserHandlers()
        {
            this.serializeParserDict[GNFieldDataType.String] = this.StringSerializer;
            this.serializeParserDict[GNFieldDataType.Boolean] = this.BooleanSerializer;
            this.serializeParserDict[GNFieldDataType.GNHashtable] = this.GNHashtableSerializer;
            this.serializeParserDict[GNFieldDataType.GNArray] = this.GNArraySerializer;
            this.serializeParserDict[GNFieldDataType.Number] = this.NumberSerializer;
            this.serializeParserDict[GNFieldDataType.Other] = this.OtherSerializer;
        }

        /// <summary>
        /// Serializes a string field.
        /// </summary>
        private void StringSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
                else
                if (value is string) gnHashtable.Add(declaredField.Code, value);
        }

        /// <summary>
        /// Serializes a boolean field.
        /// </summary>
        private void BooleanSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
                else
                if (value is bool) gnHashtable.Add(declaredField.Code, value);
        }

        /// <summary>
        /// Serializes a GNHashtable field.
        /// </summary>
        private void GNHashtableSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
                else
                if (value is GNHashtable) gnHashtable.Add(declaredField.Code, value);
                else if (value is System.Collections.IDictionary valueDict) gnHashtable.Add(declaredField.Code, GNHashtable.NewBuilder().AddAll(valueDict).Build());
                else gnHashtable.Add(declaredField.Code, this.SerializeObject(value));
        }

        /// <summary>
        /// Serializes a GNArray field.
        /// </summary>
        private void GNArraySerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
                else
                if (value is GNArray) gnHashtable.Add(declaredField.Code, value);
                else if (value is System.Collections.IList valueLst) gnHashtable.Add(declaredField.Code, this.SerializeArray(valueLst));
        }

        /// <summary>
        /// Serializes a numeric field.
        /// </summary>
        private void NumberSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
                else
                if (DetectSupport.isNumber(value)) gnHashtable.Add(declaredField.Code, value);

        }

        /// <summary>
        /// Serializes a field of other types.
        /// </summary>
        private void OtherSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value is string) gnHashtable.Add(declaredField.Code, value);
            else if (value is bool) gnHashtable.Add(declaredField.Code, value);
            else if (DetectSupport.isNumber(value)) gnHashtable.Add(declaredField.Code, value);
            else if (value is IGNData) gnHashtable.Add(declaredField.Code, value);
            else if (value is System.Collections.IList valueLst) gnHashtable.Add(declaredField.Code, this.SerializeArray(valueLst));
            else if (value is System.Collections.IDictionary valueDict) gnHashtable.Add(declaredField.Code, GNHashtable.NewBuilder().AddAll(valueDict).Build());
            else gnHashtable.Add(declaredField.Code, this.SerializeObject(value));
        }

        /// <inheritdoc />
        public GNHashtable SerializeObject(object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            var declaredFields = this.dataMemberFieldInfoMapper.GetGNEnhancedObjectFieldMetadata(type);

            var answer = new GNHashtable();

            foreach (var declaredField in declaredFields)
            {
                var value = declaredField.FieldInfo.GetValue(obj);

                var serializeParser = this.serializeParserDict[declaredField.GNFieldType];
                serializeParser.Invoke(declaredField, value, answer);
            }

            return answer;
        }

        /// <inheritdoc />
        public GNArray SerializeArray(System.Collections.IList objLst)
        {
            if (objLst == null) return null;

            var answer = new GNArray();

            for (var i = 0; i < objLst.Count; i++)
            {
                var obj = objLst[i];

                if (obj == null) answer.Add(null);
                else if (obj is string) answer.Add(obj);
                else if (obj is bool) answer.Add(obj);
                else if (DetectSupport.isNumber(obj)) answer.Add(obj);
                else if (obj is IGNData) answer.Add(obj);
                else if (obj is System.Collections.IList iList) answer.Add(this.SerializeArray(iList));
                else answer.Add(this.SerializeObject(obj));
            }

            return answer;
        }
    }
}

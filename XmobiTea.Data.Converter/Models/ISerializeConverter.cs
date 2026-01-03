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

        private System.Type typeOfByte { get; }
        private System.Type typeOfSByte { get; }
        private System.Type typeOfShort { get; }
        private System.Type typeOfUShort { get; }
        private System.Type typeOfInt { get; }
        private System.Type typeOfUInt { get; }
        private System.Type typeOfFloat { get; }
        private System.Type typeOfLong { get; }
        private System.Type typeOfULong { get; }
        private System.Type typeOfDouble { get; }

        /// <summary>
        /// Initializes a new instance of the SerializeConverter class.
        /// </summary>
        /// <param name="dataMemberFieldInfoMapper">The mapper for obtaining field metadata.</param>
        public SerializeConverter(IDataMemberFieldInfoTypeMapper dataMemberFieldInfoMapper)
        {
            this.typeOfByte = typeof(byte);
            this.typeOfSByte = typeof(sbyte);
            this.typeOfShort = typeof(short);
            this.typeOfUShort = typeof(ushort);
            this.typeOfInt = typeof(int);
            this.typeOfUInt = typeof(uint);
            this.typeOfFloat = typeof(float);
            this.typeOfLong = typeof(long);
            this.typeOfULong = typeof(ulong);
            this.typeOfDouble = typeof(double);

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
            {
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
            }
            else
            {
                if (value is string) gnHashtable.Add(declaredField.Code, value);
                else if (value.GetType().IsEnum) gnHashtable.Add(declaredField.Code, value.ToString());
            }
        }

        /// <summary>
        /// Serializes a boolean field.
        /// </summary>
        private void BooleanSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
            {
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
            }
            else
            {
                if (value is bool) gnHashtable.Add(declaredField.Code, value);
            }
        }

        /// <summary>
        /// Serializes a GNHashtable field.
        /// </summary>
        private void GNHashtableSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
            {
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
            }
            else
            {
                if (value is GNHashtable) gnHashtable.Add(declaredField.Code, value);
                else if (value is System.Collections.IDictionary valueDict) gnHashtable.Add(declaredField.Code, GNHashtable.NewBuilder().AddAll(valueDict).Build());
                else gnHashtable.Add(declaredField.Code, this.SerializeObject(value));
            }
        }

        /// <summary>
        /// Serializes a GNArray field.
        /// </summary>
        private void GNArraySerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
            {
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
            }
            else
            {
                if (value is GNArray) gnHashtable.Add(declaredField.Code, value);
                else if (value is byte[]) gnHashtable.Add(declaredField.Code, value);
                else if (value is System.Collections.IList valueLst) gnHashtable.Add(declaredField.Code, this.SerializeArray(valueLst));
            }
        }

        /// <summary>
        /// Serializes a numeric field.
        /// </summary>
        private void NumberSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value == null)
            {
                if (!declaredField.IsOptional) gnHashtable.Add(declaredField.Code, null);
            }
            else
            {
                if (DetectSupport.isNumber(value)) gnHashtable.Add(declaredField.Code, value);
                else
                {
                    var valueType = value.GetType();

                    if (valueType.IsEnum)
                    {
                        var underlyingValueType = System.Enum.GetUnderlyingType(valueType);

                        if (underlyingValueType == this.typeOfByte) gnHashtable.Add(declaredField.Code, System.Convert.ToByte(value));
                        else if (underlyingValueType == this.typeOfSByte) gnHashtable.Add(declaredField.Code, System.Convert.ToSByte(value));
                        else if (underlyingValueType == this.typeOfShort) gnHashtable.Add(declaredField.Code, System.Convert.ToInt16(value));
                        else if (underlyingValueType == this.typeOfUShort) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt16(value));
                        else if (underlyingValueType == this.typeOfInt) gnHashtable.Add(declaredField.Code, System.Convert.ToInt32(value));
                        else if (underlyingValueType == this.typeOfUInt) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt32(value));
                        else if (underlyingValueType == this.typeOfFloat) gnHashtable.Add(declaredField.Code, System.Convert.ToSingle(value));
                        else if (underlyingValueType == this.typeOfLong) gnHashtable.Add(declaredField.Code, System.Convert.ToInt64(value));
                        else if (underlyingValueType == this.typeOfULong) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt64(value));
                        else if (underlyingValueType == this.typeOfDouble) gnHashtable.Add(declaredField.Code, System.Convert.ToDouble(value));
                    }
                }
            }
        }

        /// <summary>
        /// Serializes a field of other types.
        /// </summary>
        private void OtherSerializer(IGNEnhancedObjectFieldMetadata declaredField, object value, GNHashtable gnHashtable)
        {
            if (value is string) gnHashtable.Add(declaredField.Code, value);
            else if (value is bool) gnHashtable.Add(declaredField.Code, value);
            else if (DetectSupport.IsNumber(value)) gnHashtable.Add(declaredField.Code, value);
            else if (value is IGNData) gnHashtable.Add(declaredField.Code, value);
            else if (value is byte[]) gnHashtable.Add(declaredField.Code, value);
            else if (value is System.Collections.IList valueLst) gnHashtable.Add(declaredField.Code, this.SerializeArray(valueLst));
            else if (value is System.Collections.IDictionary valueDict) gnHashtable.Add(declaredField.Code, GNHashtable.NewBuilder().AddAll(valueDict).Build());
            else
            {
                var valueType = value.GetType();

                if (valueType.IsEnum)
                {
                    var underlyingValueType = System.Enum.GetUnderlyingType(valueType);

                    if (underlyingValueType == this.typeOfByte) gnHashtable.Add(declaredField.Code, System.Convert.ToByte(value));
                    else if (underlyingValueType == this.typeOfSByte) gnHashtable.Add(declaredField.Code, System.Convert.ToSByte(value));
                    else if (underlyingValueType == this.typeOfShort) gnHashtable.Add(declaredField.Code, System.Convert.ToInt16(value));
                    else if (underlyingValueType == this.typeOfUShort) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt16(value));
                    else if (underlyingValueType == this.typeOfInt) gnHashtable.Add(declaredField.Code, System.Convert.ToInt32(value));
                    else if (underlyingValueType == this.typeOfUInt) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt32(value));
                    else if (underlyingValueType == this.typeOfFloat) gnHashtable.Add(declaredField.Code, System.Convert.ToSingle(value));
                    else if (underlyingValueType == this.typeOfLong) gnHashtable.Add(declaredField.Code, System.Convert.ToInt64(value));
                    else if (underlyingValueType == this.typeOfULong) gnHashtable.Add(declaredField.Code, System.Convert.ToUInt64(value));
                    else if (underlyingValueType == this.typeOfDouble) gnHashtable.Add(declaredField.Code, System.Convert.ToDouble(value));
                }
                else gnHashtable.Add(declaredField.Code, this.SerializeObject(value));
            }
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
                object value;

                if (declaredField.FieldInfo != null) value = declaredField.FieldInfo.GetValue(obj);
                else if (declaredField.PropertyInfo != null) value = declaredField.PropertyInfo.GetValue(obj);
                else value = null;

                if (value == null)
                {
                    if (!declaredField.IsOptional)
                    {
                        var serializeParser = this.serializeParserDict[declaredField.GNFieldType];
                        serializeParser.Invoke(declaredField, value, answer);
                    }
                }
                else
                {
                    var serializeParser = this.serializeParserDict[declaredField.GNFieldType];
                    serializeParser.Invoke(declaredField, value, answer);
                }
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
                else if (DetectSupport.IsBinary(obj)) answer.Add(obj);
                else if (DetectSupport.IsNumber(obj)) answer.Add(obj);
                else if (obj is IGNData) answer.Add(obj);
                else if (obj is System.Collections.IList iList) answer.Add(this.SerializeArray(iList));
                else
                {
                    var valueType = obj.GetType();

                    if (valueType.IsEnum)
                    {
                        var underlyingValueType = System.Enum.GetUnderlyingType(valueType);

                        if (underlyingValueType == this.typeOfByte) answer.Add(System.Convert.ToByte(obj));
                        else if (underlyingValueType == this.typeOfSByte) answer.Add(System.Convert.ToSByte(obj));
                        else if (underlyingValueType == this.typeOfShort) answer.Add(System.Convert.ToInt16(obj));
                        else if (underlyingValueType == this.typeOfUShort) answer.Add(System.Convert.ToUInt16(obj));
                        else if (underlyingValueType == this.typeOfInt) answer.Add(System.Convert.ToInt32(obj));
                        else if (underlyingValueType == this.typeOfUInt) answer.Add(System.Convert.ToUInt32(obj));
                        else if (underlyingValueType == this.typeOfFloat) answer.Add(System.Convert.ToSingle(obj));
                        else if (underlyingValueType == this.typeOfLong) answer.Add(System.Convert.ToInt64(obj));
                        else if (underlyingValueType == this.typeOfULong) answer.Add(System.Convert.ToUInt64(obj));
                        else if (underlyingValueType == this.typeOfDouble) answer.Add(System.Convert.ToDouble(obj));
                    }
                    else answer.Add(this.SerializeObject(obj));
                }
            }

            return answer;
        }
    }
}

using System.Linq;
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
        private delegate void DeserializeParserDelegate(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey);

        /// <summary>
        /// Dictionary mapping GNFieldDataType to corresponding deserialization handlers.
        /// </summary>
        private System.Collections.Generic.IDictionary<GNFieldDataType, DeserializeParserDelegate> deserializerParserDict { get; }

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
        private System.Type typeOfDictionary { get; }
        private System.Type typeOfDictionaryStringObject { get; }
        private System.Type typeOfGenericDictionary { get; }
        private System.Type typeOfCollection { get; }
        private System.Type typeOfGenericCollection { get; }
        private System.Type typeOfObject { get; }

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
            this.typeOfDictionary = typeof(System.Collections.IDictionary);
            this.typeOfGenericDictionary = typeof(System.Collections.Generic.IDictionary<,>);
            this.typeOfCollection = typeof(System.Collections.ICollection);
            this.typeOfGenericCollection = typeof(System.Collections.Generic.ICollection<>);
            this.typeOfObject = typeof(object);

            this.dataMemberFieldInfoMapper = dataMemberFieldInfoMapper;

            this.deserializerParserDict = new System.Collections.Generic.Dictionary<GNFieldDataType, DeserializeParserDelegate>();

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

        private void StringDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is string) lastValue = value;
            else throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid, this data must is string, but this data is {value.GetType()}");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: StringNull, this data must not null");
                }

                if (lastValue != null)
                {
                    var lastValueStr = (string)lastValue;

                    var requireMinLength = declaredField.MinLength.GetValueOrDefault();
                    var requireMaxLength = declaredField.MaxLength.GetValueOrDefault();

                    if (requireMinLength > lastValueStr.Length) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: StringMinLength, minLength is {requireMinLength} but this dataLength is {lastValueStr.Length}");
                    if (requireMaxLength < lastValueStr.Length) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: StringMaxLength, maxLength is {requireMaxLength} but this dataLength is {lastValueStr.Length}");
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null) declaredField.FieldInfo.SetValue(answer, lastValue);
                else if (declaredField.PropertyInfo != null) declaredField.PropertyInfo.SetValue(answer, lastValue);
            }
        }

        private void BooleanDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is bool) lastValue = value;
            else throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid, this data must is boolean, but this data is {value.GetType()}");

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null) declaredField.FieldInfo.SetValue(answer, lastValue);
                else if (declaredField.PropertyInfo != null) declaredField.PropertyInfo.SetValue(answer, lastValue);
            }
        }

        private void GNHashtableDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is GNHashtable) lastValue = value;
            else if (value.GetType() == declaredField.Cls) lastValue = value;
            else throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid, this data must is GNHashtable, but this data is {value.GetType()}");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableNull, this data must not null");
                }

                if (lastValue != null)
                {
                    if (value is GNHashtable lastValueGNHashtable)
                    {
                        var requireMinLength = declaredField.MinLength.GetValueOrDefault();
                        var requireMaxLength = declaredField.MaxLength.GetValueOrDefault();

                        if (requireMinLength > lastValueGNHashtable.Count()) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableMinLength, minLength is {requireMinLength} but this dataLength is {lastValueGNHashtable.Count()}");
                        if (requireMaxLength < lastValueGNHashtable.Count()) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableMaxLength, maxLength is {requireMaxLength} but this dataLength is {lastValueGNHashtable.Count()}");
                    }
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        || (declaredField.FieldInfo.FieldType.IsGenericType && (declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfGenericDictionary || declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfDictionary))
                        || (declaredField.FieldInfo.FieldType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericDictionary || x.GetGenericTypeDefinition() == this.typeOfDictionary))))
                        declaredField.FieldInfo.SetValue(answer, ((GNHashtable)lastValue).ToData());
                    else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));
                }
                else if (declaredField.PropertyInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNHashtable) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        || (declaredField.PropertyInfo.PropertyType.IsGenericType && (declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfGenericDictionary || declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfDictionary))
                        || (declaredField.PropertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericDictionary || x.GetGenericTypeDefinition() == this.typeOfDictionary))))
                        declaredField.PropertyInfo.SetValue(answer, ((GNHashtable)lastValue).ToData());
                    else declaredField.PropertyInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));
                }
            }
        }

        private void GNArrayDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is GNArray) lastValue = value;
            else if (value is System.Collections.ICollection) lastValue = value;
            else throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid, this data must is GNArray, but this data is {value.GetType()}");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayNull, this data must not null");
                }

                if (lastValue != null)
                {
                    var requireMinLength = declaredField.MinLength.GetValueOrDefault();
                    var requireMaxLength = declaredField.MaxLength.GetValueOrDefault();

                    if (lastValue is GNArray lastValueGNArray)
                    {
                        if (requireMinLength > lastValueGNArray.Count()) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMinLength, minLength is {requireMinLength} but this dataLength is {lastValueGNArray.Count()}");
                        if (requireMaxLength < lastValueGNArray.Count()) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMaxLength, maxLength is {requireMaxLength} but this dataLength is {lastValueGNArray.Count()}");
                    }
                    else if (lastValue is System.Collections.ICollection lastValueCollection)
                    {
                        if (requireMinLength > lastValueCollection.Count) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMinLength, minLength is {requireMinLength} but this dataLength is {lastValueCollection.Count}");
                        if (requireMaxLength < lastValueCollection.Count) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMaxLength, maxLength is {requireMaxLength} but this dataLength is {lastValueCollection.Count}");
                    }
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        || (declaredField.FieldInfo.FieldType.IsGenericType && (declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfGenericCollection || declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfCollection))
                        || (declaredField.FieldInfo.FieldType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericCollection || x.GetGenericTypeDefinition() == this.typeOfCollection))))
                        declaredField.FieldInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.FieldInfo.FieldType.IsArray));
                }
                else if (declaredField.PropertyInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNArray) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        || (declaredField.PropertyInfo.PropertyType.IsGenericType && (declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfGenericCollection || declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfCollection))
                        || (declaredField.PropertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericCollection || x.GetGenericTypeDefinition() == this.typeOfCollection))))
                        declaredField.PropertyInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.PropertyInfo.PropertyType.IsArray));
                }
            }
        }

        private void NumberDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (DetectSupport.isNumber(value)) lastValue = value;
            else throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid, this data must is number, but this data is {value.GetType()}");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustInt.GetValueOrDefault())
                {
                    if (lastValue != null && !DetectSupport.IsInt(lastValue)) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMustInt, this data must is integer number, not single");
                }

                if (lastValue != null)
                {
                    var requireMinValue = declaredField.MinValue.GetValueOrDefault();
                    var requireMaxValue = declaredField.MaxValue.GetValueOrDefault();

                    if (lastValue is int lastValueInt)
                    {
                        if (requireMinValue > lastValueInt) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueInt) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is long lastValueLong)
                    {
                        if (requireMinValue > lastValueLong) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueLong) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is byte lastValueByte)
                    {
                        if (requireMinValue > lastValueByte) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueByte) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is sbyte lastValueSByte)
                    {
                        if (requireMinValue > lastValueSByte) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueSByte) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is short lastValueShort)
                    {
                        if (requireMinValue > lastValueShort) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueShort) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is ushort lastValueUShort)
                    {
                        if (requireMinValue > lastValueUShort) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueUShort) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is uint lastValueUInt)
                    {
                        if (requireMinValue > lastValueUInt) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueUInt) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is float lastValueFloat)
                    {
                        if (requireMinValue > lastValueFloat) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueFloat) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is ulong lastValueULong)
                    {
                        if (requireMinValue > (long)lastValueULong) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < (long)lastValueULong) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                    else if (lastValue is double lastValueDouble)
                    {
                        if (requireMinValue > lastValueDouble) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue, minValue is {requireMinValue} but this data is {lastValue}");
                        if (requireMaxValue < lastValueDouble) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue, maxValue is {requireMaxValue} but this data is {lastValue}");
                    }
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null) declaredField.FieldInfo.SetValue(answer, lastValue);
                else if (declaredField.PropertyInfo != null) declaredField.PropertyInfo.SetValue(answer, lastValue);
            }
        }

        private void OtherDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.ArgumentException($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired, missing input this data");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else lastValue = value;

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null)
                {
                    if (declaredField.Cls == this.typeOfByte) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfSByte) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfShort) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfInt) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfFloat) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfLong) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfDouble) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfBool) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfString) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        || (declaredField.FieldInfo.FieldType.IsGenericType && (declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfGenericDictionary || declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfDictionary))
                        || (declaredField.FieldInfo.FieldType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericDictionary || x.GetGenericTypeDefinition() == this.typeOfDictionary))))
                        declaredField.FieldInfo.SetValue(answer, ((GNHashtable)lastValue).ToData());
                    else if ((this.typeOfCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        || (declaredField.FieldInfo.FieldType.IsGenericType && (declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfGenericCollection || declaredField.FieldInfo.FieldType.GetGenericTypeDefinition() == this.typeOfCollection))
                        || (declaredField.FieldInfo.FieldType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericCollection || x.GetGenericTypeDefinition() == this.typeOfCollection))))
                        declaredField.FieldInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.FieldInfo.FieldType.IsArray));

                    else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));

                }
                else if (declaredField.PropertyInfo != null)
                {
                    if (declaredField.Cls == this.typeOfByte) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfSByte) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfShort) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfInt) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfFloat) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfLong) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfDouble) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfBool) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfString) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfGNArray) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (declaredField.Cls == this.typeOfGNHashtable) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if ((this.typeOfDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        || (declaredField.PropertyInfo.PropertyType.IsGenericType && (declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfGenericDictionary || declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfDictionary))
                        || (declaredField.PropertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericDictionary || x.GetGenericTypeDefinition() == this.typeOfDictionary))))
                        declaredField.PropertyInfo.SetValue(answer, ((GNHashtable)lastValue).ToData());
                    else if ((this.typeOfCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        || (declaredField.PropertyInfo.PropertyType.IsGenericType && (declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfGenericCollection || declaredField.PropertyInfo.PropertyType.GetGenericTypeDefinition() == this.typeOfCollection))
                        || (declaredField.PropertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericCollection || x.GetGenericTypeDefinition() == this.typeOfCollection))))
                        declaredField.PropertyInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.PropertyInfo.PropertyType.IsArray));

                    else declaredField.PropertyInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));

                }
            }
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
                var containsKey = gnHashtable.ContainsKey(declaredField.Code);

                var value = containsKey ? gnHashtable.GetObject(declaredField.Code) : null;

                var deserializerParser = this.GetDeserializeParserHandler(declaredField.GNFieldType);

                deserializerParser.Invoke(declaredField, value, answer, containsKey);
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

            var answer = new System.Collections.Generic.List<object>();

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
                    else if ((this.typeOfDictionary.IsAssignableFrom(typeOfValue) || this.typeOfGenericDictionary.IsAssignableFrom(typeOfValue))
                        || (typeOfValue.IsGenericType && (typeOfValue.GetGenericTypeDefinition() == this.typeOfGenericDictionary || typeOfValue.GetGenericTypeDefinition() == this.typeOfDictionary))
                        || (typeOfValue.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericDictionary || x.GetGenericTypeDefinition() == this.typeOfDictionary))))
                        answer.Add(((GNHashtable)value).ToData());
                    else if ((this.typeOfCollection.IsAssignableFrom(typeOfValue) || this.typeOfGenericCollection.IsAssignableFrom(typeOfValue))
                        || (typeOfValue.IsGenericType && (typeOfValue.GetGenericTypeDefinition() == this.typeOfGenericCollection || typeOfValue.GetGenericTypeDefinition() == this.typeOfCollection))
                        || (typeOfValue.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == this.typeOfGenericCollection || x.GetGenericTypeDefinition() == this.typeOfCollection))))
                        answer.Add(this.DeserializeArray((GNArray)value, cls));
                    else
                        answer.Add(this.DeserializeObject((GNHashtable)value, cls));
                }
            }

            return answer;
        }

        /// <summary>
        /// Cast the originObjectList to other List with other type.
        /// </summary>
        /// <param name="objectLst">The origin object list to cast.</param>
        /// <param name="cls">The type of value want to cast.</param>
        /// <param name="isArray">This is Array or List</param>
        /// <returns>The IList after cast success.</returns>
        private System.Collections.IList CastList(System.Collections.IList objectLst, System.Type cls, bool isArray)
        {
            if (isArray)
            {
                var answer = System.Array.CreateInstance(cls, objectLst.Count);

                for (int i = 0; i < objectLst.Count; i++)
                    answer.SetValue(System.Convert.ChangeType(objectLst[i], cls), i);

                return answer;
            }
            else
            {
                if (cls == this.typeOfObject) return objectLst;

                var typeOfList = typeof(System.Collections.Generic.List<>).MakeGenericType(cls);
                var answer = (System.Collections.IList)System.Activator.CreateInstance(typeOfList);

                foreach (var item in objectLst)
                    answer.Add(System.Convert.ChangeType(item, cls));

                return answer;
            }
        }

    }

}

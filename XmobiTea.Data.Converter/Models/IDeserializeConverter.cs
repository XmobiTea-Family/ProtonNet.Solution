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
            this.typeOfCollection = typeof(System.Collections.IList);
            this.typeOfGenericCollection = typeof(System.Collections.Generic.IList<>);
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
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is string) lastValue = value;
            else throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: StringNull");
                }

                if (lastValue != null)
                {
                    var lastValueStr = (string)lastValue;

                    if (declaredField.MinLength.GetValueOrDefault() > lastValueStr.Length) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: StringMinLength");

                    if (declaredField.MaxLength.GetValueOrDefault() < lastValueStr.Length) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: StringMaxLength");
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
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is bool) lastValue = value;
            else throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid");

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null) declaredField.FieldInfo.SetValue(answer, lastValue);
                else if (declaredField.PropertyInfo != null) declaredField.PropertyInfo.SetValue(answer, lastValue);
            }
        }

        private void GNHashtableDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is GNHashtable) lastValue = value;
            else throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableNull");
                }

                if (lastValue != null)
                {
                    var lastValueHashtable = (GNHashtable)lastValue;

                    if (declaredField.MinLength.GetValueOrDefault() > lastValueHashtable.Count()) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableMinLength");

                    if (declaredField.MaxLength.GetValueOrDefault() < lastValueHashtable.Count()) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNHashtableMaxLength");
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (this.typeOfDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType)) declaredField.FieldInfo.SetValue(answer, ((IGNData)lastValue).ToData());
                    else declaredField.FieldInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));
                }
                else if (declaredField.PropertyInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNHashtable) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (this.typeOfDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType)) declaredField.PropertyInfo.SetValue(answer, ((IGNData)lastValue).ToData());
                    else declaredField.PropertyInfo.SetValue(answer, this.DeserializeObject((GNHashtable)lastValue, declaredField.Cls));
                }
            }
        }

        private void GNArrayDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (value is GNArray) lastValue = value;
            else throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustNonNull.GetValueOrDefault())
                {
                    if (lastValue == null) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayNull");
                }

                if (lastValue != null)
                {
                    var lastValueArray = (GNArray)lastValue;

                    if (declaredField.MinLength.GetValueOrDefault() > lastValueArray.Count()) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMinLength");

                    if (declaredField.MaxLength.GetValueOrDefault() < lastValueArray.Count()) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: GNArrayMaxLength");
                }
            }

            if (lastValue != null)
            {
                if (declaredField.FieldInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNArray) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (this.typeOfCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        declaredField.FieldInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.FieldInfo.FieldType.IsArray));
                }
                else if (declaredField.PropertyInfo != null)
                {
                    if (declaredField.Cls == this.typeOfGNArray) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (this.typeOfCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        declaredField.PropertyInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.PropertyInfo.PropertyType.IsArray));
                }
            }
        }

        private void NumberDeserializer(IGNEnhancedObjectFieldMetadata declaredField, object value, object answer, bool containsKey)
        {
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

            object lastValue;

            if (value == null) lastValue = declaredField.DefaultValue;
            else if (DetectSupport.isNumber(value)) lastValue = value;
            else throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: TypeInvalid");

            if (declaredField.ActiveConditionValid)
            {
                if (declaredField.MustInt.GetValueOrDefault())
                {
                    if (lastValue != null && !DetectSupport.IsInt(lastValue)) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMustInt");
                }

                if (lastValue != null)
                {
                    if (lastValue is int lastValueInt)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueInt) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueInt) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is long lastValueLong)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueLong) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueLong) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is byte lastValueByte)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueByte) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueByte) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is sbyte lastValueSByte)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueSByte) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueSByte) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is short lastValueShort)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueShort) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueShort) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is ushort lastValueUShort)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueUShort) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueUShort) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is uint lastValueUInt)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueUInt) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueUInt) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is float lastValueFloat)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueFloat) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueFloat) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is ulong lastValueULong)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > (long)lastValueULong) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < (long)lastValueULong) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
                    }
                    else if (lastValue is double lastValueDouble)
                    {
                        if (declaredField.MinValue.GetValueOrDefault() > lastValueDouble) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMinValue");

                        if (declaredField.MaxValue.GetValueOrDefault() < lastValueDouble) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: NumberMaxValue");
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
            if (!declaredField.IsOptional && !containsKey) throw new System.Exception($"can not deserialize data, code: {declaredField.Code}, reason: DataRequired");

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
                    else if (this.typeOfCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.FieldInfo.FieldType))
                        declaredField.FieldInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.FieldInfo.FieldType.IsArray));
                    else if (declaredField.Cls == this.typeOfGNHashtable) declaredField.FieldInfo.SetValue(answer, lastValue);
                    else if (this.typeOfDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.FieldInfo.FieldType)) declaredField.FieldInfo.SetValue(answer, ((IGNData)lastValue).ToData());
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
                    else if (this.typeOfCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericCollection.IsAssignableFrom(declaredField.PropertyInfo.PropertyType))
                        declaredField.PropertyInfo.SetValue(answer, this.CastList(this.DeserializeArray((GNArray)lastValue, declaredField.Cls), declaredField.Cls, declaredField.PropertyInfo.PropertyType.IsArray));
                    else if (declaredField.Cls == this.typeOfGNHashtable) declaredField.PropertyInfo.SetValue(answer, lastValue);
                    else if (this.typeOfDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType) || this.typeOfGenericDictionary.IsAssignableFrom(declaredField.PropertyInfo.PropertyType)) declaredField.PropertyInfo.SetValue(answer, ((IGNData)lastValue).ToData());
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
                    else if (this.typeOfCollection.IsAssignableFrom(typeOfValue) || this.typeOfGenericCollection.IsAssignableFrom(typeOfValue))
                        answer.Add(this.DeserializeArray((GNArray)value, cls));
                    else
                        answer.Add(this.DeserializeObject((GNHashtable)value, cls));
                }
            }

            return answer;
        }

        private System.Collections.IList CastList(System.Collections.IList list, System.Type cls, bool isArray)
        {
            if (isArray)
            {
                var answer = System.Array.CreateInstance(cls, list.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    answer.SetValue(System.Convert.ChangeType(list[i], cls), i);
                }

                return answer;
            }
            else
            {
                var typeOfList = typeof(System.Collections.Generic.List<>).MakeGenericType(cls);
                var answer = (System.Collections.IList)System.Activator.CreateInstance(typeOfList);

                foreach (var item in list)
                {
                    answer.Add(System.Convert.ChangeType(item, cls));
                }

                return answer;
            }
        }

    }

}

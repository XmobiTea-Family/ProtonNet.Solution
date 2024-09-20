using System;
using System.Reflection;
using XmobiTea.Collections.Generic;
using XmobiTea.Data.Converter.Rpc;
using XmobiTea.Data.Converter.Types;
using XmobiTea.Linq;

namespace XmobiTea.Data.Converter.Models
{
    /// <summary>
    /// Interface for mapping data member fields to metadata information.
    /// </summary>
    interface IDataMemberFieldInfoTypeMapper
    {
        /// <summary>
        /// Retrieves metadata for the fields of a specified class.
        /// </summary>
        /// <param name="cls">The class type to retrieve metadata for.</param>
        /// <returns>An array of field metadata information.</returns>
        IGNEnhancedObjectFieldMetadata[] GetGNEnhancedObjectFieldMetadata(System.Type cls);
    }

    /// <summary>
    /// Implements the IDataMemberFieldInfoTypeMapper interface to map data member fields to metadata information.
    /// </summary>
    class DataMemberFieldInfoTypeMapper : IDataMemberFieldInfoTypeMapper
    {
        /// <summary>
        /// A dictionary that stores metadata for each class type.
        /// </summary>
        private System.Collections.Generic.IDictionary<System.Type, IGNEnhancedObjectFieldMetadata[]> declaredFieldsMap { get; }

        private static Type TypeOfDictionary { get; }
        private static Type TypeOfGenericDictionary { get; }

        /// <summary>
        /// Initializes a new instance of the DataMemberFieldInfoTypeMapper class.
        /// </summary>
        public DataMemberFieldInfoTypeMapper() => this.declaredFieldsMap = new ThreadSafeDictionary<System.Type, IGNEnhancedObjectFieldMetadata[]>();

        /// <summary>
        /// Retrieves metadata for the fields of a specified class.
        /// </summary>
        /// <param name="cls">The class type to retrieve metadata for.</param>
        /// <returns>An array of field metadata information.</returns>
        public IGNEnhancedObjectFieldMetadata[] GetGNEnhancedObjectFieldMetadata(System.Type cls)
        {
            if (!this.declaredFieldsMap.TryGetValue(cls, out var declaredFields))
            {
                declaredFields = this.GenerateGNEnhancedObjectFieldMetadata(cls);
                this.declaredFieldsMap[cls] = declaredFields;
            }

            return declaredFields;
        }

        /// <summary>
        /// Generates metadata for the fields of a specified class.
        /// </summary>
        /// <param name="cls">The class type to generate metadata for.</param>
        /// <returns>An array of generated field metadata information.</returns>
        private IGNEnhancedObjectFieldMetadata[] GenerateGNEnhancedObjectFieldMetadata(System.Type cls)
        {
            var fieldInfoLst = new System.Collections.Generic.List<FieldInfo>();
            var propertyInfoLst = new System.Collections.Generic.List<PropertyInfo>();

            var currentCls = cls;

            while (true)
            {
                var allDeclaredFields = currentCls
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(field => field.GetCustomAttribute<DataMemberAttribute>(true) != null);

                foreach (var field in allDeclaredFields)
                {
                    var thisDeclaredField = fieldInfoLst.Find(x => x.Name.Equals(field.Name));
                    if (thisDeclaredField == null) fieldInfoLst.Add(field);
                }

                var allDeclaredProperties = currentCls
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(field => field.GetCustomAttribute<DataMemberAttribute>(true) != null);

                foreach (var property in allDeclaredProperties)
                {
                    var thisDeclaredProperty = fieldInfoLst.Find(x => x.Name.Equals(property.Name));
                    if (thisDeclaredProperty == null) propertyInfoLst.Add(property);
                }

                if (currentCls.BaseType == typeof(object))
                    break;

                currentCls = currentCls.BaseType;
            }

            var answer = new IGNEnhancedObjectFieldMetadata[fieldInfoLst.Count + propertyInfoLst.Count];

            this.SetFieldGNEnhancedObjectFieldMetadata(fieldInfoLst, 0, ref answer);
            this.SetPropertyGNEnhancedObjectFieldMetadata(propertyInfoLst, fieldInfoLst.Count, ref answer);

            return answer;
        }

        /// <summary>
        /// Sets metadata for fields.
        /// </summary>
        /// <param name="fieldInfoLst">The list of field information.</param>
        /// <param name="offset">The offset in the metadata array.</param>
        /// <param name="metadatas">The metadata array to populate.</param>
        private void SetFieldGNEnhancedObjectFieldMetadata(System.Collections.Generic.IList<FieldInfo> fieldInfoLst, int offset, ref IGNEnhancedObjectFieldMetadata[] metadatas)
        {
            for (var i = 0; i < fieldInfoLst.Count; i++)
            {
                GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata;

                var field = fieldInfoLst[i];

                var dataMemberAnno = field.GetCustomAttribute<DataMemberAttribute>(true);

                if (dataMemberAnno is StringDataMemberAttribute stringDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.String, field.FieldType, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = stringDataMemberAnno.DefaultValue;

                    if (stringDataMemberAnno.MustNonNull || stringDataMemberAnno.MinLength != -1 || stringDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (stringDataMemberAnno.MinLength == -1) stringDataMemberAnno.MinLength = 0;
                        if (stringDataMemberAnno.MaxLength == -1) stringDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = stringDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = stringDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = stringDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is BooleanDataMemberAttribute booleanDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Boolean, field.FieldType, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = booleanDataMemberAnno.DefaultValue;
                }
                else if (dataMemberAnno is GNHashtableDataMemberAttribute gnHashtableDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Object, field.FieldType, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnHashtableDataMemberAnno.DefaultValue;

                    if (gnHashtableDataMemberAnno.MustNonNull || gnHashtableDataMemberAnno.MinLength != -1 || gnHashtableDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (gnHashtableDataMemberAnno.MinLength == -1) gnHashtableDataMemberAnno.MinLength = 0;
                        if (gnHashtableDataMemberAnno.MaxLength == -1) gnHashtableDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = gnHashtableDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnHashtableDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnHashtableDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    var elementCls = gnArrayDataMemberAnno.ElementCls;

                    if (elementCls == null) elementCls = field.FieldType.IsGenericType ? field.FieldType.GetGenericArguments()[0]
                                                            : field.FieldType.IsArray ? field.FieldType.GetElementType()
                                                            : field.FieldType;

                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Array, elementCls, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnArrayDataMemberAnno.DefaultValue;

                    if (gnArrayDataMemberAnno.MustNonNull || gnArrayDataMemberAnno.MinLength != -1 || gnArrayDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (gnArrayDataMemberAnno.MinLength == -1) gnArrayDataMemberAnno.MinLength = 0;
                        if (gnArrayDataMemberAnno.MaxLength == -1) gnArrayDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = gnArrayDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnArrayDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnArrayDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Number, field.FieldType, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = numberDataMemberAnno.DefaultValue;

                    if (numberDataMemberAnno.MustInt || numberDataMemberAnno.MinValue != -1 || numberDataMemberAnno.MaxValue != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (numberDataMemberAnno.MinValue == -1) numberDataMemberAnno.MinValue = long.MinValue;
                        if (numberDataMemberAnno.MaxValue == -1) numberDataMemberAnno.MaxValue = long.MaxValue;
                    }

                    gnEnhancedObjectFieldMetadata.MinValue = numberDataMemberAnno.MinValue;
                    gnEnhancedObjectFieldMetadata.MaxValue = numberDataMemberAnno.MaxValue;
                    gnEnhancedObjectFieldMetadata.MustInt = numberDataMemberAnno.MustInt;

                }
                else
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Object, field.FieldType, field, null);
                    gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

                }

                gnEnhancedObjectFieldMetadata.Code = dataMemberAnno.Code;
                gnEnhancedObjectFieldMetadata.IsOptional = dataMemberAnno.IsOptional;
                gnEnhancedObjectFieldMetadata.GNFieldType = dataMemberAnno.GNFieldType;

                metadatas[i + offset] = gnEnhancedObjectFieldMetadata;
            }
        }

        /// <summary>
        /// Sets metadata for properties.
        /// </summary>
        /// <param name="propertyInfoLst">The list of property information.</param>
        /// <param name="offset">The offset in the metadata array.</param>
        /// <param name="metadatas">The metadata array to populate.</param>
        private void SetPropertyGNEnhancedObjectFieldMetadata(System.Collections.Generic.IList<PropertyInfo> propertyInfoLst, int offset, ref IGNEnhancedObjectFieldMetadata[] metadatas)
        {
            for (var i = 0; i < propertyInfoLst.Count; i++)
            {
                GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata;

                var property = propertyInfoLst[i];

                var dataMemberAnno = property.GetCustomAttribute<DataMemberAttribute>(true);

                if (dataMemberAnno is StringDataMemberAttribute stringDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.String, property.PropertyType, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = stringDataMemberAnno.DefaultValue;

                    if (!stringDataMemberAnno.MustNonNull || stringDataMemberAnno.MinLength != -1 || stringDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (stringDataMemberAnno.MinLength == -1) stringDataMemberAnno.MinLength = 0;
                        if (stringDataMemberAnno.MaxLength == -1) stringDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = stringDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = stringDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = stringDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is BooleanDataMemberAttribute booleanDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Boolean, property.PropertyType, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = booleanDataMemberAnno.DefaultValue;
                }
                else if (dataMemberAnno is GNHashtableDataMemberAttribute gnHashtableDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Object, property.PropertyType, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnHashtableDataMemberAnno.DefaultValue;

                    if (!gnHashtableDataMemberAnno.MustNonNull || gnHashtableDataMemberAnno.MinLength != -1 || gnHashtableDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (gnHashtableDataMemberAnno.MinLength == -1) gnHashtableDataMemberAnno.MinLength = 0;
                        if (gnHashtableDataMemberAnno.MaxLength == -1) gnHashtableDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = gnHashtableDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnHashtableDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnHashtableDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    var elementCls = gnArrayDataMemberAnno.ElementCls;

                    if (elementCls == null) elementCls = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0]
                                                            : property.PropertyType.IsArray ? property.PropertyType.GetElementType()
                                                            : property.PropertyType;

                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Array, elementCls, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnArrayDataMemberAnno.DefaultValue;

                    if (!gnArrayDataMemberAnno.MustNonNull || gnArrayDataMemberAnno.MinLength != -1 || gnArrayDataMemberAnno.MaxLength != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (gnArrayDataMemberAnno.MinLength == -1) gnArrayDataMemberAnno.MinLength = 0;
                        if (gnArrayDataMemberAnno.MaxLength == -1) gnArrayDataMemberAnno.MaxLength = 256;
                    }

                    gnEnhancedObjectFieldMetadata.MustNonNull = gnArrayDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnArrayDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnArrayDataMemberAnno.MaxLength;

                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Number, property.PropertyType, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = numberDataMemberAnno.DefaultValue;

                    if (!numberDataMemberAnno.MustInt || numberDataMemberAnno.MinValue != -1 || numberDataMemberAnno.MaxValue != -1)
                    {
                        gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                        if (numberDataMemberAnno.MinValue == -1) numberDataMemberAnno.MinValue = long.MinValue;
                        if (numberDataMemberAnno.MaxValue == -1) numberDataMemberAnno.MaxValue = long.MaxValue;
                    }

                    gnEnhancedObjectFieldMetadata.MinValue = numberDataMemberAnno.MinValue;
                    gnEnhancedObjectFieldMetadata.MaxValue = numberDataMemberAnno.MaxValue;
                    gnEnhancedObjectFieldMetadata.MustInt = numberDataMemberAnno.MustInt;

                }
                else
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Object, property.PropertyType, null, property);
                    gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

                }

                gnEnhancedObjectFieldMetadata.Code = dataMemberAnno.Code;
                gnEnhancedObjectFieldMetadata.IsOptional = dataMemberAnno.IsOptional;
                gnEnhancedObjectFieldMetadata.GNFieldType = dataMemberAnno.GNFieldType;

                metadatas[i + offset] = gnEnhancedObjectFieldMetadata;
            }
        }

    }

}

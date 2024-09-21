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
                if (currentCls == null)
                    break;

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

                    this.SetStringDataMemberAttribute(gnEnhancedObjectFieldMetadata, stringDataMemberAnno);
                }
                else if (dataMemberAnno is BooleanDataMemberAttribute booleanDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Boolean, field.FieldType, field, null);

                    this.SetBooleanDataMemberAttribute(gnEnhancedObjectFieldMetadata, booleanDataMemberAnno);
                }
                else if (dataMemberAnno is GNHashtableDataMemberAttribute gnHashtableDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Object, field.FieldType, field, null);

                    this.SetGNHashtableDataMemberAttribute(gnEnhancedObjectFieldMetadata, gnHashtableDataMemberAnno);
                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    var elementCls = gnArrayDataMemberAnno.ElementCls;

                    if (elementCls == null) elementCls = field.FieldType.IsGenericType ? field.FieldType.GetGenericArguments()[0]
                                                            : field.FieldType.IsArray ? field.FieldType.GetElementType()
                                                            : field.FieldType;

                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Array, elementCls, field, null);

                    this.SetGNArrayDataMemberAttribute(gnEnhancedObjectFieldMetadata, gnArrayDataMemberAnno);
                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Number, field.FieldType, field, null);

                    this.SetNumberDataMemberAttribute(gnEnhancedObjectFieldMetadata, numberDataMemberAnno);
                }
                else
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Object, field.FieldType, field, null);

                    this.SetOtherDataMemberAttribute(gnEnhancedObjectFieldMetadata, dataMemberAnno);
                }

                this.SetupGNEnhancedObjectFieldMetadata(gnEnhancedObjectFieldMetadata, dataMemberAnno);

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

                    this.SetStringDataMemberAttribute(gnEnhancedObjectFieldMetadata, stringDataMemberAnno);

                }
                else if (dataMemberAnno is BooleanDataMemberAttribute booleanDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Boolean, property.PropertyType, null, property);

                    this.SetBooleanDataMemberAttribute(gnEnhancedObjectFieldMetadata, booleanDataMemberAnno);
                }
                else if (dataMemberAnno is GNHashtableDataMemberAttribute gnHashtableDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Object, property.PropertyType, null, property);

                    this.SetGNHashtableDataMemberAttribute(gnEnhancedObjectFieldMetadata, gnHashtableDataMemberAnno);
                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    var elementCls = gnArrayDataMemberAnno.ElementCls;

                    if (elementCls == null) elementCls = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0]
                                                            : property.PropertyType.IsArray ? property.PropertyType.GetElementType()
                                                            : property.PropertyType;

                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Array, elementCls, null, property);

                    this.SetGNArrayDataMemberAttribute(gnEnhancedObjectFieldMetadata, gnArrayDataMemberAnno);
                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Number, property.PropertyType, null, property);

                    this.SetNumberDataMemberAttribute(gnEnhancedObjectFieldMetadata, numberDataMemberAnno);
                }
                else
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Object, property.PropertyType, null, property);

                    this.SetOtherDataMemberAttribute(gnEnhancedObjectFieldMetadata, dataMemberAnno);
                }

                this.SetupGNEnhancedObjectFieldMetadata(gnEnhancedObjectFieldMetadata, dataMemberAnno);

                metadatas[i + offset] = gnEnhancedObjectFieldMetadata;
            }
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using StringDataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The StringDataMemberAttribute providing the field data.</param>
        private void SetStringDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, StringDataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

            if (dataMemberAnno.MustNonNull || dataMemberAnno.MinLength != -1 || dataMemberAnno.MaxLength != -1)
            {
                gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                if (dataMemberAnno.MinLength == -1) dataMemberAnno.MinLength = 0;
                if (dataMemberAnno.MaxLength == -1) dataMemberAnno.MaxLength = 256;
            }

            gnEnhancedObjectFieldMetadata.MustNonNull = dataMemberAnno.MustNonNull;
            gnEnhancedObjectFieldMetadata.MinLength = dataMemberAnno.MinLength;
            gnEnhancedObjectFieldMetadata.MaxLength = dataMemberAnno.MaxLength;
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using BooleanDataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The BooleanDataMemberAttribute providing the field data.</param>
        private void SetBooleanDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, BooleanDataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using GNHashtableDataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The GNHashtableDataMemberAttribute providing the field data.</param>
        private void SetGNHashtableDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, GNHashtableDataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

            if (dataMemberAnno.MustNonNull || dataMemberAnno.MinLength != -1 || dataMemberAnno.MaxLength != -1)
            {
                gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                if (dataMemberAnno.MinLength == -1) dataMemberAnno.MinLength = 0;
                if (dataMemberAnno.MaxLength == -1) dataMemberAnno.MaxLength = 256;
            }

            gnEnhancedObjectFieldMetadata.MustNonNull = dataMemberAnno.MustNonNull;
            gnEnhancedObjectFieldMetadata.MinLength = dataMemberAnno.MinLength;
            gnEnhancedObjectFieldMetadata.MaxLength = dataMemberAnno.MaxLength;
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using GNArrayDataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The GNArrayDataMemberAttribute providing the field data.</param>
        private void SetGNArrayDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, GNArrayDataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

            if (dataMemberAnno.MustNonNull || dataMemberAnno.MinLength != -1 || dataMemberAnno.MaxLength != -1)
            {
                gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                if (dataMemberAnno.MinLength == -1) dataMemberAnno.MinLength = 0;
                if (dataMemberAnno.MaxLength == -1) dataMemberAnno.MaxLength = 256;
            }

            gnEnhancedObjectFieldMetadata.MustNonNull = dataMemberAnno.MustNonNull;
            gnEnhancedObjectFieldMetadata.MinLength = dataMemberAnno.MinLength;
            gnEnhancedObjectFieldMetadata.MaxLength = dataMemberAnno.MaxLength;
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using NumberDataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The NumberDataMemberAttribute providing the field data.</param>
        private void SetNumberDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, NumberDataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;

            if (dataMemberAnno.MustInt || dataMemberAnno.MinValue != -1 || dataMemberAnno.MaxValue != -1)
            {
                gnEnhancedObjectFieldMetadata.ActiveConditionValid = true;

                if (dataMemberAnno.MinValue == -1) dataMemberAnno.MinValue = long.MinValue;
                if (dataMemberAnno.MaxValue == -1) dataMemberAnno.MaxValue = long.MaxValue;
            }

            gnEnhancedObjectFieldMetadata.MinValue = dataMemberAnno.MinValue;
            gnEnhancedObjectFieldMetadata.MaxValue = dataMemberAnno.MaxValue;
            gnEnhancedObjectFieldMetadata.MustInt = dataMemberAnno.MustInt;
        }

        /// <summary>
        /// Set the metadata for GNEnhancedObjectField using a generic DataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The DataMemberAttribute providing the field data.</param>
        private void SetOtherDataMemberAttribute(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, DataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.DefaultValue = dataMemberAnno.DefaultValue;
        }

        /// <summary>
        /// Sets up the basic metadata for GNEnhancedObjectField using DataMemberAttribute.
        /// </summary>
        /// <param name="gnEnhancedObjectFieldMetadata">The metadata object to be updated.</param>
        /// <param name="dataMemberAnno">The DataMemberAttribute providing the initial field data.</param>
        private void SetupGNEnhancedObjectFieldMetadata(GNEnhancedObjectFieldMetadata gnEnhancedObjectFieldMetadata, DataMemberAttribute dataMemberAnno)
        {
            gnEnhancedObjectFieldMetadata.Code = dataMemberAnno.Code;
            gnEnhancedObjectFieldMetadata.IsOptional = dataMemberAnno.IsOptional;
            gnEnhancedObjectFieldMetadata.GNFieldType = dataMemberAnno.GNFieldType;
        }

    }

}

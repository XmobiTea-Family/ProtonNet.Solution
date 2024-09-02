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
        private System.Collections.Generic.IDictionary<System.Type, IGNEnhancedObjectFieldMetadata[]> declaredFieldsMap;

        /// <summary>
        /// Initializes a new instance of the DataMemberFieldInfoTypeMapper class.
        /// </summary>
        public DataMemberFieldInfoTypeMapper()
        {
            this.declaredFieldsMap = new ThreadSafeDictionary<System.Type, IGNEnhancedObjectFieldMetadata[]>();
        }

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
                    gnEnhancedObjectFieldMetadata.MustNonNull = gnHashtableDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnHashtableDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnHashtableDataMemberAnno.MaxLength;
                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Array, gnArrayDataMemberAnno.ElementCls == null ? field.FieldType : gnArrayDataMemberAnno.ElementCls, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnArrayDataMemberAnno.DefaultValue;
                    gnEnhancedObjectFieldMetadata.MustNonNull = gnArrayDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnArrayDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnArrayDataMemberAnno.MaxLength;
                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(field.Name, FieldDataType.Number, field.FieldType, field, null);

                    gnEnhancedObjectFieldMetadata.DefaultValue = numberDataMemberAnno.DefaultValue;
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
                    gnEnhancedObjectFieldMetadata.MustNonNull = gnHashtableDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnHashtableDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnHashtableDataMemberAnno.MaxLength;
                }
                else if (dataMemberAnno is GNArrayDataMemberAttribute gnArrayDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Array, gnArrayDataMemberAnno.ElementCls == null ? property.PropertyType : gnArrayDataMemberAnno.ElementCls, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = gnArrayDataMemberAnno.DefaultValue;
                    gnEnhancedObjectFieldMetadata.MustNonNull = gnArrayDataMemberAnno.MustNonNull;
                    gnEnhancedObjectFieldMetadata.MinLength = gnArrayDataMemberAnno.MinLength;
                    gnEnhancedObjectFieldMetadata.MaxLength = gnArrayDataMemberAnno.MaxLength;
                }
                else if (dataMemberAnno is NumberDataMemberAttribute numberDataMemberAnno)
                {
                    gnEnhancedObjectFieldMetadata = new GNEnhancedObjectFieldMetadata(property.Name, FieldDataType.Number, property.PropertyType, null, property);

                    gnEnhancedObjectFieldMetadata.DefaultValue = numberDataMemberAnno.DefaultValue;
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

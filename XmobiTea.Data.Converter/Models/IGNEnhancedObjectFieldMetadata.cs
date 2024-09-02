using System.Reflection;
using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Models
{
    /// <summary>
    /// Interface defining metadata for fields in a GNEnhanced object.
    /// </summary>
    interface IGNEnhancedObjectFieldMetadata
    {
        /// <summary>
        /// Gets or sets the code representing the field.
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is optional.
        /// </summary>
        bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets the GNFieldDataType representing the type of the field.
        /// </summary>
        GNFieldDataType GNFieldType { get; set; }

        /// <summary>
        /// Gets or sets the default value of the field.
        /// </summary>
        object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the active condition for the field is valid.
        /// </summary>
        bool ActiveConditionValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field must be non-null.
        /// </summary>
        bool? MustNonNull { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the field value if applicable.
        /// </summary>
        int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the field value if applicable.
        /// </summary>
        int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the field if applicable.
        /// </summary>
        double? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the field if applicable.
        /// </summary>
        double? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field value must be an integer.
        /// </summary>
        bool? MustInt { get; set; }

        /// <summary>
        /// Gets the FieldInfo associated with the field.
        /// </summary>
        FieldInfo FieldInfo { get; }

        /// <summary>
        /// Gets the PropertyInfo associated with the property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the FieldDataType representing the type of the field.
        /// </summary>
        FieldDataType FieldType { get; }

        /// <summary>
        /// Gets the type of the class that contains the field.
        /// </summary>
        System.Type Cls { get; }
    }

    /// <summary>
    /// Implementation of IGNEnhancedObjectFieldMetadata that provides metadata for fields in a GNEnhanced object.
    /// </summary>
    class GNEnhancedObjectFieldMetadata : IGNEnhancedObjectFieldMetadata
    {
        /// <inheritdoc />
        public string Code { get; set; }

        /// <inheritdoc />
        public bool IsOptional { get; set; }

        /// <inheritdoc />
        public GNFieldDataType GNFieldType { get; set; }

        /// <inheritdoc />
        public object DefaultValue { get; set; }

        /// <inheritdoc />
        public bool ActiveConditionValid { get; set; }

        /// <inheritdoc />
        public bool? MustNonNull { get; set; }

        /// <inheritdoc />
        public int? MinLength { get; set; }

        /// <inheritdoc />
        public int? MaxLength { get; set; }

        /// <inheritdoc />
        public double? MinValue { get; set; }

        /// <inheritdoc />
        public double? MaxValue { get; set; }

        /// <inheritdoc />
        public bool? MustInt { get; set; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public FieldDataType FieldType { get; }

        /// <inheritdoc />
        public System.Type Cls { get; }

        /// <inheritdoc />
        public FieldInfo FieldInfo { get; }

        /// <inheritdoc />
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Initializes a new instance of the GNEnhancedObjectFieldMetadata class.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="cls">The class type that contains the field.</param>
        /// <param name="fieldInfo">The FieldInfo object associated with the field.</param>
        /// <param name="propertyInfo">The PropertyInfo object associated with the property.</param>
        public GNEnhancedObjectFieldMetadata(string name, FieldDataType fieldType, System.Type cls, FieldInfo fieldInfo, PropertyInfo propertyInfo)
        {
            this.Name = name;
            this.FieldType = fieldType;
            this.Cls = cls;

            this.FieldInfo = fieldInfo;
            this.PropertyInfo = propertyInfo;
        }
    }
}

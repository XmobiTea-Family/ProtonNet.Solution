using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{
    /// <summary>
    /// Base attribute for marking fields that represent data members in an object.
    /// Provides common properties for specifying metadata such as field type, code, and default value.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DataMemberAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Default is GNFieldDataType.Other.
        /// </summary>
        public virtual GNFieldDataType GNFieldType => GNFieldDataType.Other;

        /// <summary>
        /// Gets or sets the code associated with the field.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is optional.
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// Gets or sets the default value for the field.
        /// </summary>
        public object DefaultValue { get; set; } = null;

    }

}

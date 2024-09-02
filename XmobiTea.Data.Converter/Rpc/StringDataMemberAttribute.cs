using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{
    /// <summary>
    /// Attribute for marking fields that represent string data members.
    /// Extends DataMemberAttribute with additional string-specific metadata.
    /// </summary>
    public class StringDataMemberAttribute : DataMemberAttribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Overrides to return GNFieldDataType.String.
        /// </summary>
        public override GNFieldDataType GNFieldType => GNFieldDataType.String;

        /// <summary>
        /// Gets or sets the default string value for the field.
        /// </summary>
        public new string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the string must not be null.
        /// </summary>
        public bool MustNonNull { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum length of the string.
        /// </summary>
        public int MinLength { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum length of the string.
        /// </summary>
        public int MaxLength { get; set; } = -1;

    }

}

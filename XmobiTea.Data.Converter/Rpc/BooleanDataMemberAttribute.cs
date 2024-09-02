using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{
    /// <summary>
    /// Attribute for marking fields that represent boolean data members.
    /// Extends DataMemberAttribute with additional boolean-specific metadata.
    /// </summary>
    public class BooleanDataMemberAttribute : DataMemberAttribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Overrides to return GNFieldDataType.Boolean.
        /// </summary>
        public override GNFieldDataType GNFieldType => GNFieldDataType.Boolean;

        /// <summary>
        /// Gets or sets the default boolean value for the field.
        /// </summary>
        public new bool DefaultValue { get; set; } = false;

    }

}

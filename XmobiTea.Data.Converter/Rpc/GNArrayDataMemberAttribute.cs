using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{

    /// <summary>
    /// Attribute for marking fields that represent GNArray data members.
    /// Extends DataMemberAttribute with additional GNArray-specific metadata.
    /// </summary>
    public class GNArrayDataMemberAttribute : DataMemberAttribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Overrides to return GNFieldDataType.GNArray.
        /// </summary>
        public override GNFieldDataType GNFieldType => GNFieldDataType.GNArray;

        /// <summary>
        /// Gets or sets the default GNArray value for the field.
        /// </summary>
        public new GNArray DefaultValue { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the GNArray must not be null.
        /// </summary>
        public bool MustNonNull { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum length of the GNArray.
        /// </summary>
        public int MinLength { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum length of the GNArray.
        /// </summary>
        public int MaxLength { get; set; } = -1;

        /// <summary>
        /// Gets or sets the type of elements contained within the GNArray.
        /// </summary>
        public System.Type ElementCls { get; set; } = null;

    }

}

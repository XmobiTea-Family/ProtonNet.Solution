using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{

    /// <summary>
    /// Attribute for marking fields that represent GNHashtable data members.
    /// Extends DataMemberAttribute with additional GNHashtable-specific metadata.
    /// </summary>
    public class GNHashtableDataMemberAttribute : DataMemberAttribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Overrides to return GNFieldDataType.GNHashtable.
        /// </summary>
        public override GNFieldDataType GNFieldType => GNFieldDataType.GNHashtable;

        /// <summary>
        /// Gets or sets the default GNHashtable value for the field.
        /// </summary>
        public new GNHashtable DefaultValue { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the GNHashtable must not be null.
        /// </summary>
        public bool MustNonNull { get; set; } = false;

        /// <summary>
        /// Gets or sets the minimum length of the GNHashtable.
        /// </summary>
        public int MinLength { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum length of the GNHashtable.
        /// </summary>
        public int MaxLength { get; set; } = -1;

    }

}

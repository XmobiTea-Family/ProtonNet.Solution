using XmobiTea.Data.Converter.Types;

namespace XmobiTea.Data.Converter.Rpc
{

    /// <summary>
    /// Attribute for marking fields that represent numeric data members.
    /// Extends DataMemberAttribute with additional number-specific metadata.
    /// </summary>
    public class NumberDataMemberAttribute : DataMemberAttribute
    {
        /// <summary>
        /// Gets the type of the field represented by this attribute. Overrides to return GNFieldDataType.Number.
        /// </summary>
        public override GNFieldDataType GNFieldType => GNFieldDataType.Number;

        /// <summary>
        /// Gets or sets the default numeric value for the field.
        /// </summary>
        public new double DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the field.
        /// </summary>
        public double MinValue { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum value for the field.
        /// </summary>
        public double MaxValue { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether the field must represent an integer.
        /// </summary>
        public bool MustInt { get; set; } = false;

    }

}

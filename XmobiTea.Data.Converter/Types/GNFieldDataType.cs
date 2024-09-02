namespace XmobiTea.Data.Converter.Types
{
    /// <summary>
    /// Enumeration representing different types of data fields.
    /// </summary>
    public enum GNFieldDataType
    {
        /// <summary>
        /// Represents a field type that is not specifically categorized.
        /// </summary>
        Other = 0,

        /// <summary>
        /// Represents a numeric data field.
        /// </summary>
        Number = 1,

        /// <summary>
        /// Represents a string data field.
        /// </summary>
        String = 2,

        /// <summary>
        /// Represents a boolean data field.
        /// </summary>
        Boolean = 3,

        /// <summary>
        /// Represents a field that is a GNHashtable.
        /// </summary>
        GNHashtable = 4,

        /// <summary>
        /// Represents a field that is a GNArray.
        /// </summary>
        GNArray = 5,

    }

}

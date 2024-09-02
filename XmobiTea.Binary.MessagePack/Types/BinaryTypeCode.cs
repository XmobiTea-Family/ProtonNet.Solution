namespace XmobiTea.Binary.MessagePack.Types
{
    /// <summary>
    /// Represents the different binary type codes used in the MessagePack format.
    /// </summary>
    static class BinaryTypeCode
    {
        /// <summary>
        /// Represents the binary type code for integers.
        /// </summary>
        public static readonly byte Integer = 0;

        /// <summary>
        /// Represents the binary type code for nil (null) values.
        /// </summary>
        public static readonly byte Nil = 1;

        /// <summary>
        /// Represents the binary type code for boolean values.
        /// </summary>
        public static readonly byte Boolean = 2;

        /// <summary>
        /// Represents the binary type code for floating-point numbers.
        /// </summary>
        public static readonly byte Float = 3;

        /// <summary>
        /// Represents the binary type code for strings.
        /// </summary>
        public static readonly byte String = 4;

        /// <summary>
        /// Represents the binary type code for binary data.
        /// </summary>
        public static readonly byte Binary = 5;

        /// <summary>
        /// Represents the binary type code for arrays.
        /// </summary>
        public static readonly byte Array = 6;

        /// <summary>
        /// Represents the binary type code for maps (dictionaries).
        /// </summary>
        public static readonly byte Map = 7;

        /// <summary>
        /// Represents the binary type code for extension types.
        /// </summary>
        public static readonly byte Extension = 8;

    }

}

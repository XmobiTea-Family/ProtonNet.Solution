using XmobiTea.Data.Helper.Parsers;

namespace XmobiTea.Data.Helper
{
    /// <summary>
    /// Static class providing conversion methods to various data types.
    /// </summary>
    static class Convert
    {
        /// <summary>
        /// The data parser instance used for conversions.
        /// </summary>
        private static IDataParser dataParser { get; }

        /// <summary>
        /// Static constructor to initialize the data parser.
        /// </summary>
        static Convert() => dataParser = new DataParser();

        /// <summary>
        /// Converts the given object to a boolean.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted boolean value.</returns>
        public static bool ToBoolean(object obj) => dataParser.ToBool(obj);

        /// <summary>
        /// Converts the given object to a signed byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed byte value.</returns>
        public static sbyte ToSByte(object obj) => dataParser.ToSByte(obj);

        /// <summary>
        /// Converts the given object to a byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted byte value.</returns>
        public static byte ToByte(object obj) => dataParser.ToByte(obj);

        /// <summary>
        /// Converts the given object to a short (Int16).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted short value.</returns>
        public static short ToInt16(object obj) => dataParser.ToShort(obj);

        /// <summary>
        /// Converts the given object to an unsigned short (UInt16).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned short value.</returns>
        public static ushort ToUInt16(object obj) => dataParser.ToUShort(obj);

        /// <summary>
        /// Converts the given object to an integer (Int32).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted integer value.</returns>
        public static int ToInt32(object obj) => dataParser.ToInt(obj);

        /// <summary>
        /// Converts the given object to an unsigned integer (UInt32).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned integer value.</returns>
        public static uint ToUInt32(object obj) => dataParser.ToUInt(obj);

        /// <summary>
        /// Converts the given object to a long (Int64).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted long value.</returns>
        public static long ToInt64(object obj) => dataParser.ToLong(obj);

        /// <summary>
        /// Converts the given object to an unsigned long (UInt64).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned long value.</returns>
        public static ulong ToUInt64(object obj) => dataParser.ToULong(obj);

        /// <summary>
        /// Converts the given object to a float (Single).
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted float value.</returns>
        public static float ToSingle(object obj) => dataParser.ToFloat(obj);

        /// <summary>
        /// Converts the given object to a double.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        public static double ToDouble(object obj) => dataParser.ToDouble(obj);

        /// <summary>
        /// Converts the given object to a char.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted char value.</returns>
        public static char ToChar(object obj) => dataParser.ToChar(obj);

        /// <summary>
        /// Converts the given object to a string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted string value.</returns>
        public static string ToString(object obj) => dataParser.ToString(obj);

    }

}

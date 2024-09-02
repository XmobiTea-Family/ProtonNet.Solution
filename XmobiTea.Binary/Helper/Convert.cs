using XmobiTea.Binary.Helper.Parsers;

namespace XmobiTea.Binary.Helper
{
    /// <summary>
    /// Provides methods for converting objects to various data types.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// The data parser instance used to perform the conversions.
        /// </summary>
        private static IDataParser dataParser { get; }

        /// <summary>
        /// Initializes static members of the <see cref="Convert"/> class.
        /// </summary>
        static Convert()
        {
            dataParser = new DataParser();
        }

        /// <summary>
        /// Converts the specified object to a Boolean value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted Boolean value.</returns>
        public static bool ToBoolean(object obj) => dataParser.ToBool(obj);

        /// <summary>
        /// Converts the specified object to a signed byte value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed byte value.</returns>
        public static sbyte ToSByte(object obj) => dataParser.ToSByte(obj);

        /// <summary>
        /// Converts the specified object to an unsigned byte value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned byte value.</returns>
        public static byte ToByte(object obj) => dataParser.ToByte(obj);

        /// <summary>
        /// Converts the specified object to a signed 16-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed 16-bit integer value.</returns>
        public static short ToInt16(object obj) => dataParser.ToShort(obj);

        /// <summary>
        /// Converts the specified object to an unsigned 16-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned 16-bit integer value.</returns>
        public static ushort ToUInt16(object obj) => dataParser.ToUShort(obj);

        /// <summary>
        /// Converts the specified object to a signed 32-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed 32-bit integer value.</returns>
        public static int ToInt32(object obj) => dataParser.ToInt(obj);

        /// <summary>
        /// Converts the specified object to an unsigned 32-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned 32-bit integer value.</returns>
        public static uint ToUInt32(object obj) => dataParser.ToUInt(obj);

        /// <summary>
        /// Converts the specified object to a signed 64-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed 64-bit integer value.</returns>
        public static long ToInt64(object obj) => dataParser.ToLong(obj);

        /// <summary>
        /// Converts the specified object to an unsigned 64-bit integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned 64-bit integer value.</returns>
        public static ulong ToUInt64(object obj) => dataParser.ToULong(obj);

        /// <summary>
        /// Converts the specified object to a single-precision floating-point value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted single-precision floating-point value.</returns>
        public static float ToSingle(object obj) => dataParser.ToFloat(obj);

        /// <summary>
        /// Converts the specified object to a double-precision floating-point value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted double-precision floating-point value.</returns>
        public static double ToDouble(object obj) => dataParser.ToDouble(obj);

        /// <summary>
        /// Converts the specified object to a character value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted character value.</returns>
        public static char ToChar(object obj) => dataParser.ToChar(obj);

        /// <summary>
        /// Converts the specified object to a string value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted string value.</returns>
        public static string ToString(object obj) => dataParser.ToString(obj);

    }

}

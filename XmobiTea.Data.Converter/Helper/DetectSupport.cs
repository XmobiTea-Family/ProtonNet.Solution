namespace XmobiTea.Data.Converter.Helper
{
    /// <summary>
    /// Provides utility methods for type detection.
    /// </summary>
    class DetectSupport
    {
        /// <summary>
        /// Determines whether the specified value is a numeric type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// <c>true</c> if the value is a numeric type; otherwise, <c>false</c>.
        /// </returns>
        public static bool isNumber(object value)
        {
            return value is byte
                    || value is sbyte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is float
                    || value is long
                    || value is ulong
                    || value is double;
        }

    }

}

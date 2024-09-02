namespace XmobiTea.ProtonNetCommon.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the specified character suffix from the end of the string, if it exists.
        /// </summary>
        /// <param name="self">The string instance to modify.</param>
        /// <param name="toRemove">The character to remove from the end of the string.</param>
        /// <returns>
        /// A new string without the specified suffix, or the original string if the suffix does not exist.
        /// </returns>
        public static string RemoveSuffix(this string self, char toRemove) =>
#if NETCOREAPP
            // If the string is null or empty, return the original string.
            // Otherwise, check if the string ends with the specified character and remove it if present.
            string.IsNullOrEmpty(self) ? self : (self.EndsWith(toRemove) ? self.Substring(0, self.Length - 1) : self);
#else
            // For non-.NET Core versions, perform the same check but convert the character to a string.
            string.IsNullOrEmpty(self) ? self : (self.EndsWith(toRemove.ToString()) ? self.Substring(0, self.Length - 1) : self);
#endif

    }

}

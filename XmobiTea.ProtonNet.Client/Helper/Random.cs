namespace XmobiTea.ProtonNet.Client.Helper
{
    /// <summary>
    /// Provides methods for generating random numbers within specified ranges.
    /// This class cannot be inherited.
    /// </summary>
    public static class Random
    {
        /// <summary>
        /// Gets the instance of the random number generator.
        /// </summary>
        private static System.Random rd { get; }

        /// <summary>
        /// Static constructor to initialize the random number generator.
        /// </summary>
        static Random() => rd = new System.Random();

        /// <summary>
        /// Generates a random float number within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.</param>
        /// <returns>A single-precision floating point number that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        public static float Range(float min, float max) => (float)(rd.NextDouble() * (max - min) + min);

        /// <summary>
        /// Generates a random integer within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        public static int Range(int min, int max) => rd.Next(min, max);

    }

}

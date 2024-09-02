using System;

namespace XmobiTea.ProtonNetCommon.Extensions
{
    /// <summary>
    /// Provides extension methods for array manipulation.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Fills all elements of the array with the specified value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="arr">The array to be filled.</param>
        /// <param name="value">The value to fill the array with.</param>
        public static void Fill<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        /// <summary>
        /// Creates a new array that is a clone of a portion of the source array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The source array to clone from.</param>
        /// <param name="start">The starting index of the portion to clone.</param>
        /// <param name="end">The ending index (exclusive) of the portion to clone.</param>
        /// <returns>A new array that is a clone of the specified portion of the source array.</returns>
        public static T[] ToClone<T>(this T[] source, int start, int end)
        {
            var answer = new T[end - start];

            Array.Copy(source, start, answer, 0, end - start);

            return answer;
        }

    }

}

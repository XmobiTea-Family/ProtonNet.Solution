using System;
using System.Collections.Generic;

namespace XmobiTea.Linq
{
    /// <summary>
    /// Provides a set of extension methods for working with collections that implement <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence of elements to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var item in source)
                if (predicate(item))
                    yield return item;
        }

        /// <summary>
        /// Casts the elements of an <see cref="System.Collections.IEnumerable"/> to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast the elements of <paramref name="source"/> to.</typeparam>
        /// <param name="source">The <see cref="System.Collections.IEnumerable"/> that contains the elements to cast.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains each element of the source sequence cast to the specified type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static IEnumerable<T> Cast<T>(this System.Collections.IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
                yield return (T)item;
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to search for the specified value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns><c>true</c> if the source sequence contains an element that has the specified value; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static bool Contains<T>(this IEnumerable<T> source, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
                if (EqualityComparer<T>.Default.Equals(item, value))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to search for the specified value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value"/> in the sequence, if found; otherwise, <c>-1</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var index = 0;
            foreach (var item in source)
            {
                if (EqualityComparer<T>.Default.Equals(item, value))
                    return index;

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the zero-based index of the last occurrence of a value in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to search for the specified value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns>The zero-based index of the last occurrence of <paramref name="value"/> in the sequence, if found; otherwise, <c>-1</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static int IndexOfLast<T>(this IEnumerable<T> source, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var foundIndex = -1;
            var index = 0;
            foreach (var item in source)
            {
                if (EqualityComparer<T>.Default.Equals(item, value))
                    foundIndex = index;

                index++;
            }

            return foundIndex;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to search for an element that matches the conditions defined by <paramref name="predicate"/>.</param>
        /// <param name="predicate">The function that defines the conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by the predicate, if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        public static T Find<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to search for an element that matches the conditions defined by <paramref name="predicate"/>.</param>
        /// <param name="predicate">The function that defines the conditions of the element to search for.</param>
        /// <returns>The last element that matches the conditions defined by the predicate, if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        public static T FindLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            T result = default(T);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    result = item;
                }
            }

            return result;
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="T">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="selector"/> is <c>null</c>.</exception>
        public static IEnumerable<T> Select<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            foreach (var item in source)
                yield return selector(item);
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence of elements to convert to a list.</param>
        /// <returns>A <see cref="List{T}"/> that contains elements from the input sequence.</returns>
        public static IList<T> ToList<T>(this IEnumerable<T> source) => new List<T>(source);

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to an array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence of elements to convert to an array.</param>
        /// <returns>An array that contains elements from the input sequence.</returns>
        public static T[] ToArray<T>(this IEnumerable<T> source) => ToList(source).ToArray();

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of elements to return from after skipping the specified count.</param>
        /// <param name="skip">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence after the specified number of elements have been bypassed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="skip"/> is less than 0.</exception>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, int skip)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));

            using (var enumerator = source.GetEnumerator())
            {
                // Bỏ qua 'count' phần tử đầu tiên
                while (skip > 0 && enumerator.MoveNext())
                {
                    skip--;
                }

                // Trả về các phần tử còn lại
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="limit">The number of elements to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the start of the input sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="limit"/> is less than 0.</exception>
        public static IEnumerable<T> Limit<T>(this IEnumerable<T> source, int limit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit));

            foreach (var item in source)
            {
                if (limit-- == 0)
                    yield break;

                yield return item;
            }
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of elements to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var sortedList = new List<TSource>(source);
            sortedList.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(x), keySelector(y)));
            return sortedList;
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A sequence of elements to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var sortedList = new List<TSource>(source);
            sortedList.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(y), keySelector(x)));
            return sortedList;
        }

    }

}

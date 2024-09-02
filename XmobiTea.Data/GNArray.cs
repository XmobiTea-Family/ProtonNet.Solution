using System.Collections.Generic;
using System.Text;

namespace XmobiTea.Data
{
    /// <summary>
    /// Represents a generic array data structure with various utility methods.
    /// </summary>
    public class GNArray : GNData<int>
    {
        /// <summary>
        /// Internal list to store array elements.
        /// </summary>
        private IList<object> _array { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GNArray"/> class.
        /// </summary>
        public GNArray()
        {
            this._array = new List<object>();
        }

        /// <summary>
        /// Adds an object to the array.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(object value)
        {
            this._array.Add(this.CreateUseDataFromOriginData(value));
        }

        /// <summary>
        /// Gets the collection of values in the array.
        /// </summary>
        /// <returns>A collection of objects in the array.</returns>
        public ICollection<object> Values()
        {
            return this._array;
        }

        /// <summary>
        /// Clears all elements from the array.
        /// </summary>
        public void Clear()
        {
            this._array.Clear();
        }

        /// <summary>
        /// Removes an element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>True if the element was removed successfully; otherwise, false.</returns>
        public bool Remove(int index)
        {
            this._array.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        /// <returns>The count of elements in the array.</returns>
        public int Count()
        {
            return this._array.Count;
        }

        /// <summary>
        /// Converts the array to an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the resulting array.</typeparam>
        /// <returns>An array of elements of type <typeparamref name="T"/>.</returns>
        public T[] ToArray<T>()
        {
            var answer = new T[this._array.Count];

            for (var i = 0; i < this._array.Count; i++)
            {
                answer[i] = (T)this.CustomGet(i);
            }

            return answer;
        }

        /// <summary>
        /// Converts the array to a list of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the resulting list.</typeparam>
        /// <returns>A list of elements of type <typeparamref name="T"/>.</returns>
        public IList<T> ToList<T>()
        {
            var answer = new List<T>();

            for (var i = 0; i < this._array.Count; i++)
            {
                answer.Add((T)this.CustomGet(i));
            }

            return answer;
        }

        /// <summary>
        /// Gets the object at the specified index.
        /// </summary>
        /// <param name="k">The index of the object to retrieve.</param>
        /// <returns>The object at the specified index, or null if the index is out of bounds.</returns>
        public override object GetObject(int k)
        {
            if (k >= this._array.Count) return null;
            return this._array[k];
        }

        /// <summary>
        /// Converts the array to a data object.
        /// </summary>
        /// <returns>A list of data objects.</returns>
        public override object ToData()
        {
            var answer = new List<object>();

            foreach (var c in this._array)
            {
                answer.Add(this.CreateDataFromUseData(c));
            }

            return answer;
        }

        /// <summary>
        /// Returns a string representation of the array.
        /// </summary>
        /// <returns>A string representing the array.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (var i = 0; i < this._array.Count; i++)
            {
                if (i != 0) stringBuilder.Append(",");

                var value = this._array[i];

                if (value == null) stringBuilder.Append("null");
                else if (value is IGNData gnData) stringBuilder.Append(gnData.ToString());
                else if (value is string valueStr) stringBuilder.Append("\"" + valueStr + "\"");
                else if (value is bool boolValue) stringBuilder.Append(boolValue.ToString().ToLower());
                else stringBuilder.Append(value);
            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for building a <see cref="GNArray"/>.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// A builder class for constructing instances of <see cref="GNArray"/>.
        /// </summary>
        public class Builder
        {
            private IList<object> _array;

            /// <summary>
            /// Adds a value to the builder's array.
            /// </summary>
            /// <param name="value">The value to add.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder Add(object value)
            {
                this._array.Add(value);
                return this;
            }

            /// <summary>
            /// Adds all elements from the specified list to the builder's array.
            /// </summary>
            /// <param name="list">The list of elements to add.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder AddAll(System.Collections.IList list)
            {
                foreach (var o in list)
                {
                    this.Add(o);
                }

                return this;
            }

            /// <summary>
            /// Builds the <see cref="GNArray"/> instance.
            /// </summary>
            /// <returns>A new <see cref="GNArray"/> instance.</returns>
            public GNArray Build()
            {
                var answer = new GNArray();

                foreach (var o in this._array)
                {
                    answer.Add(o);
                }

                return answer;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() => this._array = new List<object>();
        }

    }
}

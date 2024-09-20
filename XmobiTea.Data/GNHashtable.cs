using System.Collections.Generic;
using System.Text;

namespace XmobiTea.Data
{
    /// <summary>
    /// Represents a generic hashtable data structure with various utility methods.
    /// </summary>
    public class GNHashtable : GNData<string>
    {
        /// <summary>
        /// Internal dictionary to store key-value pairs.
        /// </summary>
        private IDictionary<string, object> _dict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GNHashtable"/> class.
        /// </summary>
        public GNHashtable() => this._dict = new Dictionary<string, object>();

        /// <summary>
        /// Adds a key-value pair to the hashtable.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(string key, object value) => this._dict[key] = this.CreateUseDataFromOriginData(value);

        /// <summary>
        /// Gets the collection of values in the hashtable.
        /// </summary>
        /// <returns>A collection of objects in the hashtable.</returns>
        public ICollection<object> Values() => this._dict.Values;

        /// <summary>
        /// Gets the collection of keys in the hashtable.
        /// </summary>
        /// <returns>A collection of keys in the hashtable.</returns>
        public ICollection<string> Keys() => this._dict.Keys;

        /// <summary>
        /// Determines whether the hashtable contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the hashtable.</param>
        /// <returns>True if the hashtable contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(string key) => this._dict.ContainsKey(key);

        /// <summary>
        /// Clears all elements from the hashtable.
        /// </summary>
        public void Clear() => this._dict.Clear();

        /// <summary>
        /// Removes the element with the specified key from the hashtable.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>True if the element is successfully removed; otherwise, false.</returns>
        public bool Remove(string key) => this._dict.Remove(key);

        /// <summary>
        /// Gets the number of elements contained in the hashtable.
        /// </summary>
        /// <returns>The count of elements in the hashtable.</returns>
        public int Count() => this._dict.Count;

        /// <summary>
        /// Converts the hashtable to a dictionary of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of values in the resulting dictionary.</typeparam>
        /// <returns>A dictionary with string keys and values of type <typeparamref name="T"/>.</returns>
        public IDictionary<string, T> ToDictionary<T>()
        {
            var result = new Dictionary<string, T>();
            foreach (var key in this._dict.Keys) result[key] = (T)this.CustomGet(key);
            return result;
        }

        /// <summary>
        /// Gets the object associated with the specified key.
        /// </summary>
        /// <param name="k">The key of the object to retrieve.</param>
        /// <returns>The object associated with the specified key, or null if the key is not found.</returns>
        public override object GetObject(string k) => this._dict.TryGetValue(k, out var value) ? value : null;

        /// <summary>
        /// Converts the hashtable to a data object.
        /// </summary>
        /// <returns>A dictionary containing the data representation of the hashtable.</returns>
        public override object ToData()
        {
            var result = new Dictionary<string, object>();
            foreach (var entry in this._dict) result.Add(entry.Key, this.CreateDataFromUseData(entry.Value));
            return result;
        }

        /// <summary>
        /// Returns a string representation of the hashtable.
        /// </summary>
        /// <returns>A string representing the hashtable.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder("{");
            var i = 0;
            foreach (var c in this._dict)
            {
                if (i != 0) stringBuilder.Append(",");

                i++;
                var value = c.Value;
                if (value == null) stringBuilder.Append("\"" + c.Key.ToString() + "\"" + ":null");
                else if (value is IGNData gnData) stringBuilder.Append("\"" + c.Key.ToString() + "\"" + ":" + gnData.ToString());
                else if (value is string valueStr) stringBuilder.Append("\"" + c.Key.ToString() + "\"" + ":" + "\"" + valueStr + "\"");
                else if (value is bool boolValue) stringBuilder.Append("\"" + c.Key.ToString() + "\"" + ":" + boolValue.ToString().ToLower());
                else stringBuilder.Append("\"" + c.Key.ToString() + "\"" + ":" + value);
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for building a <see cref="GNHashtable"/>.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// A builder class for constructing instances of <see cref="GNHashtable"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Internal dictionary to store key-value pairs during construction.
            /// </summary>
            private IDictionary<string, object> _dict { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() => this._dict = new Dictionary<string, object>();

            /// <summary>
            /// Adds a key-value pair to the builder's dictionary.
            /// </summary>
            /// <param name="key">The key of the element to add.</param>
            /// <param name="value">The value of the element to add.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder Add(string key, object value)
            {
                this._dict[key] = value;
                return this;
            }

            /// <summary>
            /// Adds all key-value pairs from the specified dictionary to the builder's dictionary.
            /// </summary>
            /// <param name="dict">The dictionary of elements to add.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder AddAll(System.Collections.IDictionary dict)
            {
                foreach (string key in dict.Keys) this.Add(key, dict[key]);
                return this;
            }

            /// <summary>
            /// Adds all key-value pairs from the specified dictionary to the builder's dictionary.
            /// </summary>
            /// <param name="dict">The dictionary of elements to add.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder AddAll<TValue>(System.Collections.Generic.IDictionary<string, TValue> dict)
            {
                foreach (string key in dict.Keys) this.Add(key, dict[key]);
                return this;
            }

            /// <summary>
            /// Builds the <see cref="GNHashtable"/> instance.
            /// </summary>
            /// <returns>A new <see cref="GNHashtable"/> instance.</returns>
            public GNHashtable Build()
            {
                var result = new GNHashtable();
                foreach (var entry in this._dict) result.Add(entry.Key, entry.Value);
                return result;
            }

        }

    }

}

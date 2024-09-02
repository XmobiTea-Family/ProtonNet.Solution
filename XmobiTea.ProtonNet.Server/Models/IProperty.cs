using XmobiTea.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Defines methods for managing property data with a string key.
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to retrieve.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <returns>The data associated with the key.</returns>
        TData Get<TData>(string key);

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to set.</typeparam>
        /// <param name="key">The key to associate with the data.</param>
        /// <param name="data">The data to set.</param>
        void Set<TData>(string key, TData data);

        /// <summary>
        /// Attempts to get the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to retrieve.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <param name="data">The data associated with the key, if found.</param>
        /// <returns>True if the data was found; otherwise, false.</returns>
        bool TryGet<TData>(string key, out TData data);

        /// <summary>
        /// Checks if the specified key exists.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Removes the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was successfully removed; otherwise, false.</returns>
        bool Remove(string key);

    }

    /// <summary>
    /// Implements the <see cref="IProperty"/> interface for managing properties with a thread-safe dictionary.
    /// </summary>
    public class Property : IProperty
    {
        /// <summary>
        /// Internal dictionary to store property values.
        /// </summary>
        protected System.Collections.Generic.IDictionary<string, object> _internalProperty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        public Property() => this._internalProperty = new ThreadSafeDictionary<string, object>();

        /// <summary>
        /// Checks if the specified key exists.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public bool Contains(string key)
        {
            return this._internalProperty.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was successfully removed; otherwise, false.</returns>
        public bool Remove(string key)
        {
            return this._internalProperty.Remove(key);
        }

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to set.</typeparam>
        /// <param name="key">The key to associate with the data.</param>
        /// <param name="data">The data to set.</param>
        public void Set<TData>(string key, TData data)
        {
            this._internalProperty[key] = data;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to retrieve.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <returns>The data associated with the key.</returns>
        public TData Get<TData>(string key)
        {
            if (!this._internalProperty.TryGetValue(key, out var value))
                return default;
            return (TData)value;
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TData">The type of the data to retrieve.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <param name="data">The data associated with the key, if found.</param>
        /// <returns>True if the data was found; otherwise, false.</returns>
        public bool TryGet<TData>(string key, out TData data)
        {
            if (!this._internalProperty.TryGetValue(key, out var value))
            {
                data = default;
                return false;
            }

            if (value is TData tData)
            {
                data = tData;
                return true;
            }

            data = default;
            return false;
        }

    }

}

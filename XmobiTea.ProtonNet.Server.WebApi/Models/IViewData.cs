using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents a collection of key-value pairs used to store data for rendering views.
    /// </summary>
    /// <remarks>
    /// This class allows adding, removing, and retrieving data that is used in the view rendering process.
    /// </remarks>
    public interface IViewData
    {
        /// <summary>
        /// Retrieves the original internal dictionary containing the data.
        /// </summary>
        /// <returns>The internal dictionary that stores key-value pairs.</returns>
        IDictionary<string, object> GetOriginDict();

        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <returns>The value associated with the specified key.</returns>
        object GetData(string key);

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key 
        /// from the internal data dictionary.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">The value associated with the specified key, 
        /// if found; otherwise, the default value of the type.</param>
        /// <returns>True if the key is found in the dictionary; otherwise, false.</returns>
        bool TryGetValue(string key, out object value);

        /// <summary>
        /// Contains key in view data.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if contains.</returns>
        bool Contains(string key);

    }

    /// <summary>
    /// Represents a collection of key-value pairs used to store data for rendering views.
    /// </summary>
    /// <remarks>
    /// This class allows adding, removing, and retrieving data that is used in the view rendering process.
    /// </remarks>
    public sealed class ViewData : IViewData
    {
        private IDictionary<string, object> _dataDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class with an empty data dictionary.
        /// </summary>
        public ViewData() : this(new Dictionary<string, object>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class with the specified data dictionary.
        /// </summary>
        /// <param name="_dataDict">The dictionary to initialize the <see cref="ViewData"/> with.</param>
        public ViewData(IDictionary<string, object> _dataDict) => this._dataDict = _dataDict;

        /// <summary>
        /// Retrieves the original internal dictionary containing the data.
        /// </summary>
        /// <returns>The internal dictionary that stores key-value pairs.</returns>
        public IDictionary<string, object> GetOriginDict() => this._dataDict;

        /// <summary>
        /// Adds or updates a key-value pair in the data dictionary.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value associated with the key.</param>
        /// <returns>The current instance of <see cref="ViewData"/>.</returns>
        public ViewData SetData(string key, object value)
        {
            this._dataDict[key] = value;
            return this;
        }

        /// <summary>
        /// Removes a key-value pair from the data dictionary.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>The current instance of <see cref="ViewData"/>.</returns>
        public ViewData RemoveData(string key)
        {
            this._dataDict.Remove(key);
            return this;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value is to be retrieved.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object GetData(string key) => this._dataDict[key];

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key 
        /// from the internal data dictionary.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">The value associated with the specified key, 
        /// if found; otherwise, the default value of the type.</param>
        /// <returns>True if the key is found in the dictionary; otherwise, false.</returns>
        public bool TryGetValue(string key, out object value) => this._dataDict.TryGetValue(key, out value);

        /// <summary>
        /// Contains key in view data.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if contains.</returns>
        public bool Contains(string key) => this._dataDict.ContainsKey(key);

    }

}

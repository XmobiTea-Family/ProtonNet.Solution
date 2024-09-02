using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents a collection of key-value pairs used to store data for rendering views.
    /// </summary>
    /// <remarks>
    /// This class allows adding, removing, and retrieving data that is used in the view rendering process.
    /// </remarks>
    public sealed class ViewData
    {
        private IDictionary<string, string> _dataDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class with an empty data dictionary.
        /// </summary>
        public ViewData()
        {
            this._dataDict = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class with the specified data dictionary.
        /// </summary>
        /// <param name="_dataDict">The dictionary to initialize the <see cref="ViewData"/> with.</param>
        public ViewData(IDictionary<string, string> _dataDict)
        {
            this._dataDict = _dataDict;
        }

        /// <summary>
        /// Adds or updates a key-value pair in the data dictionary.
        /// </summary>
        /// <param name="key">The key to add or update.</param>
        /// <param name="htmlCode">The value associated with the key.</param>
        /// <returns>The current instance of <see cref="ViewData"/>.</returns>
        public ViewData SetData(string key, string htmlCode)
        {
            this._dataDict[key] = htmlCode;
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
        public string GetData(string key)
        {
            return this._dataDict[key];
        }

        /// <summary>
        /// Gets the data dictionary.
        /// </summary>
        /// <returns>The data dictionary.</returns>
        public IDictionary<string, string> GetDataDict() => this._dataDict;

    }

}

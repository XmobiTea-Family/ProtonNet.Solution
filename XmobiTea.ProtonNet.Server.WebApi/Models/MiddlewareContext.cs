using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents the context for middleware operations, allowing storage and retrieval of data.
    /// </summary>
    /// <remarks>
    /// This class is used to pass data between different middleware components in the request processing pipeline.
    /// </remarks>
    public sealed class MiddlewareContext
    {
        /// <summary>
        /// Dictionary that holds the data in the middleware context.
        /// </summary>
        private IDictionary<string, object> _dataDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewareContext"/> class.
        /// </summary>
        public MiddlewareContext() => this._dataDict = new Dictionary<string, object>();

        /// <summary>
        /// Gets the data associated with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="name">The name of the data.</param>
        /// <returns>The data associated with the specified name.</returns>
        public T GetData<T>(string name) => (T)this._dataDict[name];

        /// <summary>
        /// Sets the data associated with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="name">The name of the data.</param>
        /// <param name="value">The data to set.</param>
        public void SetData<T>(string name, T value) => this._dataDict[name] = value;

        /// <summary>
        /// Removes the data associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the data.</param>
        public void RemoveData(string name) => this._dataDict.Remove(name);

        /// <summary>
        /// Determines whether the context contains data associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the data.</param>
        /// <returns><c>true</c> if the context contains data with the specified name; otherwise, <c>false</c>.</returns>
        public bool Contains(string name) => this._dataDict.ContainsKey(name);

        /// <summary>
        /// Gets all data in the context.
        /// </summary>
        /// <returns>A dictionary containing all data in the context.</returns>
        public IDictionary<string, object> GetData() => this._dataDict;

    }

}

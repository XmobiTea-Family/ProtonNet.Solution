namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents a query parameter item containing a key and its associated value.
    /// </summary>
    /// <remarks>
    /// This class is used to handle query parameters in a request. It supports single string values as well as multiple string values.
    /// </remarks>
    public sealed class QueryItem
    {
        /// <summary>
        /// Gets or sets the key of the query parameter.
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// Gets or sets the value of the query parameter. The value can be a single string or an array of strings.
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the query parameter's value is a single string.
        /// </summary>
        public bool IsSingleString { get; internal set; }

        /// <summary>
        /// Gets the value of the query parameter as a single string. If the value is an array, it joins the elements with commas.
        /// </summary>
        public string AsString => this.IsSingleString ? (string)this.Value : string.Join(",", (string[])this.Value);

        /// <summary>
        /// Gets the value of the query parameter as an array of strings. If the value is a single string, it returns an array with that string.
        /// </summary>
        public string[] AsStringArray
        {
            get
            {
                if (this.Value is string valueStr)
                    return new string[] { valueStr };

                return (string[])this.Value;
            }
        }

    }

}

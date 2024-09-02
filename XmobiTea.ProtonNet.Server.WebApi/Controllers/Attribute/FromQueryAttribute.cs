namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Attribute for specifying that a parameter should be bound from the query string in an HTTP request.
    /// </summary>
    public sealed class FromQueryAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FromQueryAttribute class with the specified parameter name.
        /// </summary>
        /// <param name="name">The name of the query parameter to bind from.</param>
        public FromQueryAttribute(string name) : base(name)
        {
        }

    }

}

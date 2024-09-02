namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Specifies an HTTP POST route for a web request.
    /// </summary>
    public sealed class HttpPostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the HttpPostAttribute class with the specified route name.
        /// </summary>
        /// <param name="name">The name of the route associated with the HTTP POST request.</param>
        public HttpPostAttribute(string name) : base(name)
        {
        }

    }

}

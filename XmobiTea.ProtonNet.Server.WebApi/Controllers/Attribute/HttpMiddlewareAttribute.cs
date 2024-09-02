namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Represents an HTTP middleware attribute for web requests.
    /// </summary>
    public class HttpMiddlewareAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the HttpMiddlewareAttribute class with the specified route name.
        /// </summary>
        /// <param name="name">The name of the route associated with the HTTP middleware.</param>
        public HttpMiddlewareAttribute(string name) : base(name)
        {
        }

    }

}

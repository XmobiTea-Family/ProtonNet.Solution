namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Specifies an HTTP GET route for a web request.
    /// </summary>
    public sealed class HttpWebAttribute : HttpGetAttribute
    {
        /// <summary>
        /// Initializes a new instance of the HttpWebAttribute class with the specified route name.
        /// </summary>
        /// <param name="name">The name of the route associated with the HTTP GET request.</param>
        public HttpWebAttribute(string name) : base(name)
        {
        }

    }

}

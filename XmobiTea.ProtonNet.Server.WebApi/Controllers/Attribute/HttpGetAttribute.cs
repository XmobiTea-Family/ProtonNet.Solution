namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Attribute for marking methods as handling HTTP GET requests.
    /// </summary>
    public class HttpGetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the HttpGetAttribute class with the specified name.
        /// </summary>
        /// <param name="name">The name associated with the HTTP GET request.</param>
        public HttpGetAttribute(string name) : base(name)
        {
        }

    }

}

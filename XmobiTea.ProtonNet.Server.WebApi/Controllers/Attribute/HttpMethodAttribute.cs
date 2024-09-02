using System;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Base class for HTTP method attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class HttpMethodAttribute : System.Attribute
    {
        /// <summary>
        /// Gets or sets the prefix associated with the HTTP method.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the name of the HTTP method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the HttpMethodAttribute class with the specified method name.
        /// </summary>
        /// <param name="name">The name of the HTTP method.</param>
        public HttpMethodAttribute(string name) => this.Name = name;

    }

}

using System;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Specifies a route for a class in a web application.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RouteAttribute : System.Attribute
    {
        /// <summary>
        /// Gets or sets the route prefix associated with the class.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Initializes a new instance of the RouteAttribute class with the specified route prefix.
        /// </summary>
        /// <param name="prefix">The route prefix for the class.</param>
        public RouteAttribute(string prefix) => this.Prefix = prefix;

    }

}

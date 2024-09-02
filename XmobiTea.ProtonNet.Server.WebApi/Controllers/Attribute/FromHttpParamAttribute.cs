using System;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Abstract base attribute for specifying how a parameter should be bound from an HTTP request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public abstract class FromHttpParamAttribute : System.Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the parameter is optional.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter to bind from the HTTP request.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the FromHttpParamAttribute class with the specified parameter name.
        /// </summary>
        /// <param name="name">The name of the parameter to bind from the HTTP request.</param>
        public FromHttpParamAttribute(string name) => this.Name = name;

    }

}

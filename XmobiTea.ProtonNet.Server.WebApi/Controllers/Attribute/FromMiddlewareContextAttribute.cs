namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Attribute for specifying that a parameter should be bound from the middleware context.
    /// </summary>
    public sealed class FromMiddlewareContextAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FromMiddlewareContextAttribute class with the specified parameter name.
        /// </summary>
        /// <param name="name">The name of the parameter in the middleware context to bind from.</param>
        public FromMiddlewareContextAttribute(string name) : base(name)
        {
        }

    }

}

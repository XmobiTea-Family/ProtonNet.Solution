namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Attribute for specifying that a parameter should be bound from a specific HTTP parameter.
    /// </summary>
    public sealed class FromParamAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FromParamAttribute class with the specified parameter name.
        /// </summary>
        /// <param name="name">The name of the HTTP parameter to bind from.</param>
        public FromParamAttribute(string name) : base(name)
        {
        }

    }

}

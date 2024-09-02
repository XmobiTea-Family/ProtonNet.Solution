namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Specifies that a parameter should be bound from the HTTP request headers.
    /// </summary>
    public sealed class FromHeaderAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FromHeaderAttribute class with the specified parameter name.
        /// </summary>
        /// <param name="name">The name of the header parameter to bind from the HTTP request.</param>
        public FromHeaderAttribute(string name) : base(name)
        {
        }

    }

}

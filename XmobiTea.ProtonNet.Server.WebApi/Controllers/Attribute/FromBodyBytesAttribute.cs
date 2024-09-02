namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Specifies that a parameter should be bound from the body of the HTTP request as raw bytes.
    /// </summary>
    public sealed class FromBodyBytesAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FromBodyBytesAttribute class.
        /// </summary>
        /// <remarks>
        /// The name parameter is left empty because this attribute is used to bind the entire request body as raw bytes.
        /// </remarks>
        public FromBodyBytesAttribute() : base(string.Empty)
        {
        }

    }

}

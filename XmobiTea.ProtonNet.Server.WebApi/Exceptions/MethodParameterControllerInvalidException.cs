namespace XmobiTea.ProtonNet.Server.WebApi.Exceptions
{
    /// <summary>
    /// Exception thrown when a method parameter is invalid in the context of a controller.
    /// </summary>
    public class MethodParameterControllerInvalidException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the MethodParameterControllerInvalidException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MethodParameterControllerInvalidException(string message) : base(message)
        {
        }

    }

}

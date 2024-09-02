namespace XmobiTea.ProtonNet.Server.WebApi.Exceptions
{
    /// <summary>
    /// Exception thrown when a method controller is invalid.
    /// </summary>
    public class MethodControllerInvalidException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the MethodControllerInvalidException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MethodControllerInvalidException(string message) : base(message)
        {
        }

    }

}

namespace XmobiTea.ProtonNet.Server.Socket.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a required socket controller is missing.
    /// </summary>
    public class MissingSocketControllerException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingSocketControllerException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingSocketControllerException(string message) : base(message)
        {
        }

    }

}

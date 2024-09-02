namespace XmobiTea.ProtonNet.Token.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a token has expired.
    /// </summary>
    public sealed class TokenExpiredException : TokenException
    {
        // This exception is thrown to indicate that the token being processed
        // has surpassed its expiration time and is no longer valid for use.
    }

}

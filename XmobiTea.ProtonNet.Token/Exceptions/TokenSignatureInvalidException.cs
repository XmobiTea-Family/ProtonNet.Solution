namespace XmobiTea.ProtonNet.Token.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a token's signature is invalid.
    /// </summary>
    public sealed class TokenSignatureInvalidException : TokenException
    {
        // This exception is thrown to indicate that the signature of the token
        // does not match the expected value, typically as a result of tampering
        // or incorrect key usage.
    }

}

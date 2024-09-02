using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token
{
    /// <summary>
    /// Represents the options for configuring token generation and validation.
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        /// Gets or sets the type of binary encoding used for the token.
        /// </summary>
        public TokenBinaryType BinaryType { get; set; }

        /// <summary>
        /// Gets or sets the algorithm type used for token creation and validation.
        /// </summary>
        public TokenAlgorithmType AlgorithmType { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds after which the token expires.
        /// </summary>
        public uint ExpiredAfterSeconds { get; set; }

    }

}

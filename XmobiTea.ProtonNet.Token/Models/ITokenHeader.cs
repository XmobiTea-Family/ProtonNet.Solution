using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token.Models
{
    /// <summary>
    /// Represents the interface for token header information.
    /// </summary>
    public interface ITokenHeader
    {
        /// <summary>
        /// Gets the version of the token.
        /// </summary>
        byte Version { get; }

        /// <summary>
        /// Gets the type of binary encoding used for the token.
        /// </summary>
        TokenBinaryType BinaryType { get; }

        /// <summary>
        /// Gets the type of algorithm used for signing the token.
        /// </summary>
        TokenAlgorithmType AlgorithmType { get; }

        /// <summary>
        /// Gets the expiration time in UTC ticks.
        /// </summary>
        long ExpiredAtUtcTicks { get; }

    }

    /// <summary>
    /// Implementation of the token header interface.
    /// </summary>
    class TokenHeader : ITokenHeader
    {
        /// <summary>
        /// Gets or sets the version of the token.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Gets or sets the type of binary encoding used for the token.
        /// </summary>
        public TokenBinaryType BinaryType { get; set; }

        /// <summary>
        /// Gets or sets the type of algorithm used for signing the token.
        /// </summary>
        public TokenAlgorithmType AlgorithmType { get; set; }

        /// <summary>
        /// Gets or sets the expiration time in UTC ticks.
        /// </summary>
        public long ExpiredAtUtcTicks { get; set; }

    }

}

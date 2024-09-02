using System;
using System.Collections.Generic;
using XmobiTea.Binary;
using XmobiTea.ProtonNet.Token.Binary;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token.Factory
{
    /// <summary>
    /// Provides methods for obtaining binary encoding and decoding implementations 
    /// based on the specified TokenBinaryType.
    /// </summary>
    interface ITokenBinaryFactory
    {
        /// <summary>
        /// Retrieves the binary encoder corresponding to the given TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary encoding.</param>
        /// <returns>An object implementing ITokenBinaryEncode.</returns>
        ITokenBinaryEncode GetEncode(TokenBinaryType binaryType);

        /// <summary>
        /// Retrieves the binary decoder corresponding to the given TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary decoding.</param>
        /// <returns>An object implementing ITokenBinaryDecode.</returns>
        ITokenBinaryDecode GetDecode(TokenBinaryType binaryType);

    }

    /// <summary>
    /// Factory class that provides instances of binary encoding and decoding
    /// based on TokenBinaryType.
    /// </summary>
    class TokenBinaryFactory : ITokenBinaryFactory
    {
        /// <summary>
        /// A dictionary that maps TokenBinaryType to its respective encoding and decoding pair.
        /// </summary>
        private IDictionary<TokenBinaryType, Tuple<ITokenBinaryEncode, ITokenBinaryDecode>> binaryPairDict { get; }

        /// <summary>
        /// Initializes a new instance of the TokenBinaryFactory class.
        /// </summary>
        public TokenBinaryFactory()
        {
            this.binaryPairDict = new Dictionary<TokenBinaryType, Tuple<ITokenBinaryEncode, ITokenBinaryDecode>>();
        }

        /// <summary>
        /// Retrieves the encoding and decoding pair for the specified TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary encoding and decoding.</param>
        /// <returns>A tuple containing the encoder and decoder for the binary type.</returns>
        /// <exception cref="Exception">Thrown when the specified binary type is not registered.</exception>
        private Tuple<ITokenBinaryEncode, ITokenBinaryDecode> GetBinaryPair(TokenBinaryType binaryType)
        {
            if (!this.binaryPairDict.TryGetValue(binaryType, out var binary))
            {
                throw new Exception("Missing binaryType for " + binaryType);
            }

            return binary;
        }

        /// <summary>
        /// Adds a new binary converter for the specified TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary encoding and decoding.</param>
        /// <param name="binaryConverter">The binary converter instance.</param>
        public void AddBinaryConverter(TokenBinaryType binaryType, IBinaryConverter binaryConverter) => this.binaryPairDict[binaryType] = new Tuple<ITokenBinaryEncode, ITokenBinaryDecode>(new ProtocolTokenBinaryEncode(binaryConverter), new ProtocolTokenBinaryDecode(binaryConverter));

        /// <summary>
        /// Gets the binary encoder for the specified TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary encoding.</param>
        /// <returns>An object implementing ITokenBinaryEncode.</returns>
        public ITokenBinaryEncode GetEncode(TokenBinaryType binaryType) => this.GetBinaryPair(binaryType).Item1;

        /// <summary>
        /// Gets the binary decoder for the specified TokenBinaryType.
        /// </summary>
        /// <param name="binaryType">The type of binary decoding.</param>
        /// <returns>An object implementing ITokenBinaryDecode.</returns>
        public ITokenBinaryDecode GetDecode(TokenBinaryType binaryType) => this.GetBinaryPair(binaryType).Item2;

    }

}

using System.Collections.Generic;
using XmobiTea.ProtonNet.Token.Algorithm;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token.Factory
{
    /// <summary>
    /// Provides a method to retrieve an encoder based on the TokenAlgorithmType.
    /// </summary>
    interface ITokenAlgorithmFactory
    {
        /// <summary>
        /// Retrieves the algorithm encoder corresponding to the given TokenAlgorithmType.
        /// </summary>
        /// <param name="algorithmType">The type of algorithm encoding.</param>
        /// <returns>An object implementing ITokenAlgorithmEncode.</returns>
        ITokenAlgorithmEncode GetEncode(TokenAlgorithmType algorithmType);

    }

    /// <summary>
    /// Factory class that provides instances of algorithm encoding based on TokenAlgorithmType.
    /// </summary>
    class TokenAlgorithmFactory : ITokenAlgorithmFactory
    {
        /// <summary>
        /// A dictionary that maps TokenAlgorithmType to its respective encoder implementation.
        /// </summary>
        private IDictionary<TokenAlgorithmType, ITokenAlgorithmEncode> algorithmEncodeDict { get; }

        /// <summary>
        /// Initializes a new instance of the TokenAlgorithmFactory class.
        /// </summary>
        public TokenAlgorithmFactory()
        {
            this.algorithmEncodeDict = new Dictionary<TokenAlgorithmType, ITokenAlgorithmEncode>();
        }

        /// <summary>
        /// Retrieves the encoder for the specified TokenAlgorithmType.
        /// </summary>
        /// <param name="algorithmType">The type of algorithm encoding.</param>
        /// <returns>An object implementing ITokenAlgorithmEncode.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified algorithm type is not supported.</exception>
        private ITokenAlgorithmEncode GetAlgorithmEncode(TokenAlgorithmType algorithmType)
        {
            if (!this.algorithmEncodeDict.TryGetValue(algorithmType, out var binary))
            {
                // Initialize the appropriate algorithm encoder based on the type
                switch (algorithmType)
                {
                    case TokenAlgorithmType.SHA256:
                        binary = new SHA256TokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.MD5:
                        binary = new MD5TokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.HMAC:
                        binary = new HMACTokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.SHA1:
                        binary = new SHA1TokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.SHA384:
                        binary = new SHA384TokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.SHA512:
                        binary = new SHA512TokenAlgorithmEncode();
                        break;
                    case TokenAlgorithmType.KeyedHashAlgorithm:
                        binary = new KeyedHashAlgorithmTokenAlgorithmEncode();
                        break;
                    default:
                        throw new System.ArgumentException("AlgorithmType not support " + algorithmType);
                }

                this.algorithmEncodeDict[algorithmType] = binary;
            }

            return binary;
        }

        /// <summary>
        /// Gets the algorithm encoder for the specified TokenAlgorithmType.
        /// </summary>
        /// <param name="algorithmType">The type of algorithm encoding.</param>
        /// <returns>An object implementing ITokenAlgorithmEncode.</returns>
        public ITokenAlgorithmEncode GetEncode(TokenAlgorithmType algorithmType) => this.GetAlgorithmEncode(algorithmType);

    }

}

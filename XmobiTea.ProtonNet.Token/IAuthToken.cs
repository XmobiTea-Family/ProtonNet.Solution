using XmobiTea.ProtonNet.Token.Models;

namespace XmobiTea.ProtonNet.Token
{
    /// <summary>
    /// Defines the methods for encoding, decoding, and verifying authentication tokens.
    /// </summary>
    public interface IAuthToken
    {
        /// <summary>
        /// Encodes the specified payload into a token using the provided key and options.
        /// </summary>
        /// <typeparam name="T">The type of the payload, which must implement ITokenPayload.</typeparam>
        /// <param name="payload">The payload to encode.</param>
        /// <param name="key">The key used for encoding the token.</param>
        /// <param name="options">Optional token options for customizing the encoding process.</param>
        /// <returns>A string representing the encoded token.</returns>
        string Encode<T>(T payload, string key, TokenOptions options = null) where T : ITokenPayload;

        /// <summary>
        /// Decodes the specified token and extracts the header and payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload, which must implement ITokenPayload.</typeparam>
        /// <param name="token">The token to decode.</param>
        /// <param name="header">Outputs the decoded token header.</param>
        /// <param name="payload">Outputs the decoded payload.</param>
        void Decode<T>(string token, out ITokenHeader header, out T payload) where T : ITokenPayload;

        /// <summary>
        /// Verifies the specified token using the provided key and extracts the header and payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload, which must implement ITokenPayload.</typeparam>
        /// <param name="token">The token to verify.</param>
        /// <param name="key">The key used for verifying the token.</param>
        /// <param name="header">Outputs the verified token header.</param>
        /// <param name="payload">Outputs the verified payload.</param>
        void Verify<T>(string token, string key, out ITokenHeader header, out T payload) where T : ITokenPayload;

    }

}

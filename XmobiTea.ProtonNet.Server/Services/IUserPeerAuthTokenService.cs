using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Token;
using XmobiTea.ProtonNet.Token.Models;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines the service for generating and verifying authentication tokens for user peers.
    /// </summary>
    public interface IUserPeerAuthTokenService
    {
        /// <summary>
        /// Generates an authentication token for a user peer.
        /// </summary>
        /// <param name="payload">The payload to encode in the token.</param>
        /// <param name="options">Optional token options.</param>
        /// <returns>The generated token.</returns>
        string GenerateToken(UserPeerTokenPayload payload, TokenOptions options = null);

        /// <summary>
        /// Verifies the authenticity of a token.
        /// </summary>
        /// <param name="token">The token to verify.</param>
        /// <param name="header">The token header if verification is successful.</param>
        /// <param name="userPeerTokenPayload">The payload decoded from the token if verification is successful.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        bool TryVerifyToken(string token, out ITokenHeader header, out UserPeerTokenPayload userPeerTokenPayload);

    }

    /// <summary>
    /// Implements <see cref="IUserPeerAuthTokenService"/> to handle authentication token generation and verification.
    /// </summary>
    public class UserPeerAuthTokenService : IUserPeerAuthTokenService
    {
        private ILogger logger { get; }
        private IAuthToken authToken { get; }
        private string password { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPeerAuthTokenService"/> class.
        /// </summary>
        public UserPeerAuthTokenService(string password)
        {
            this.logger = LogManager.GetLogger(this);

            this.authToken = this.CreateAuthToken();

            this.password = password;
        }

        /// <summary>
        /// Creates an instance of <see cref="IAuthToken"/> with configured binary converters.
        /// </summary>
        /// <returns>An instance of <see cref="IAuthToken"/>.</returns>
        private IAuthToken CreateAuthToken()
        {
            var answer = new AuthToken();

            answer.SetBinaryConverter(TokenBinaryType.SimplePack, new Binary.SimplePack.BinaryConverter());
            answer.SetBinaryConverter(TokenBinaryType.MessagePack, new Binary.MessagePack.BinaryConverter());

            return answer;
        }

        /// <summary>
        /// Generates an authentication token for a user peer.
        /// </summary>
        /// <param name="payload">The payload to encode in the token.</param>
        /// <param name="options">Optional token options.</param>
        /// <returns>The generated token.</returns>
        public string GenerateToken(UserPeerTokenPayload payload, TokenOptions options = null) => this.authToken.Encode(payload, this.password, options);

        /// <summary>
        /// Verifies the authenticity of a token.
        /// </summary>
        /// <param name="token">The token to verify.</param>
        /// <param name="header">The token header if verification is successful.</param>
        /// <param name="userPeerTokenPayload">The payload decoded from the token if verification is successful.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        public bool TryVerifyToken(string token, out ITokenHeader header, out UserPeerTokenPayload userPeerTokenPayload)
        {
            try
            {
                this.authToken.Verify(token, this.password, out header, out userPeerTokenPayload);

                return true;
            }
            catch (System.Exception ex)
            {
                header = null;
                userPeerTokenPayload = null;

                this.logger.Fatal(ex);
                return false;
            }
        }

    }

}

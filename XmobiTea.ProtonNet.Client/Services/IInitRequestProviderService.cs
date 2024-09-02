using XmobiTea.ProtonNet.Client.Helper;
using XmobiTea.ProtonNet.Client.Models;

namespace XmobiTea.ProtonNet.Client.Services
{
    /// <summary>
    /// Interface for a service that provides initialization requests 
    /// for creating new client peers. 
    /// </summary>
    public interface IInitRequestProviderService
    {
        /// <summary>
        /// Creates a new initialization request for a client peer.
        /// </summary>
        /// <returns>An instance of <see cref="IClientPeerInitRequest"/> containing the initialization data.</returns>
        IClientPeerInitRequest NewClientPeerInitRequest();

    }

    /// <summary>
    /// Service that provides initialization requests for creating new client peers. 
    /// It generates session IDs, encryption keys, and client IDs for the requests.
    /// </summary>
    class InitRequestProviderService : IInitRequestProviderService
    {
        /// <summary>
        /// Length of the session ID to be generated.
        /// </summary>
        private static readonly int SessionIdLength = 16;

        /// <summary>
        /// Length of the encryption key to be generated.
        /// </summary>
        private static readonly int EncryptLength = 16;

        /// <summary>
        /// Provider for generating session IDs.
        /// </summary>
        protected SessionIdProvider sessionIdProvider { get; }

        /// <summary>
        /// Provider for generating encryption keys.
        /// </summary>
        protected EncryptKeyProvider encryptKeyProvider { get; }

        /// <summary>
        /// Provider for generating client IDs.
        /// </summary>
        protected ClientIdProvider clientIdProvider { get; }

        /// <summary>
        /// The session ID generated for this service instance.
        /// </summary>
        private string sessionId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitRequestProviderService"/> class.
        /// </summary>
        public InitRequestProviderService()
        {
            this.sessionIdProvider = new SessionIdProvider();
            this.encryptKeyProvider = new EncryptKeyProvider();
            this.clientIdProvider = new ClientIdProvider();

            this.sessionId = this.sessionIdProvider.GenerateRandomSessionId();
        }

        /// <summary>
        /// Creates a new initialization request for a client peer.
        /// </summary>
        /// <returns>An instance of <see cref="IClientPeerInitRequest"/> containing the initialization data.</returns>
        public IClientPeerInitRequest NewClientPeerInitRequest()
        {
            var answer = new ClientPeerInitRequest();

            answer.ClientId = this.clientIdProvider.GenerateNextClientId();
            answer.SessionId = this.sessionId;
            answer.EncryptKey = this.encryptKeyProvider.GenerateRandomEncryptKey();

            return answer;
        }

        /// <summary>
        /// Nested class responsible for generating random session IDs.
        /// </summary>
        protected class SessionIdProvider
        {
            /// <summary>
            /// The characters used to generate the session ID.
            /// </summary>
            private static readonly string AllCharacters = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

            /// <summary>
            /// Generates a random session ID.
            /// </summary>
            /// <returns>A randomly generated session ID as a string.</returns>
            public string GenerateRandomSessionId()
            {
                var answer = string.Empty;

                for (var i = 0; i < SessionIdLength; i++)
                    answer += AllCharacters[Random.Range(0, AllCharacters.Length)];

                return answer;
            }
        }

        /// <summary>
        /// Nested class responsible for generating random encryption keys.
        /// </summary>
        protected class EncryptKeyProvider
        {
            /// <summary>
            /// The characters used to generate the encryption key.
            /// </summary>
            private static readonly string AllCharacters = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

            /// <summary>
            /// Generates a random encryption key.
            /// </summary>
            /// <returns>A randomly generated encryption key as a byte array.</returns>
            public byte[] GenerateRandomEncryptKey()
            {
                var randomSessionId = string.Empty;

                for (var i = 0; i < EncryptLength; i++)
                    randomSessionId += AllCharacters[Random.Range(0, AllCharacters.Length)];

                return System.Text.Encoding.UTF8.GetBytes(randomSessionId);
            }
        }

        /// <summary>
        /// Nested class responsible for generating unique client IDs.
        /// </summary>
        protected class ClientIdProvider
        {
            /// <summary>
            /// The current client ID counter.
            /// </summary>
            private int clientId { get; set; }

            /// <summary>
            /// Generates the next unique client ID.
            /// </summary>
            /// <returns>The next unique client ID as an integer.</returns>
            public int GenerateNextClientId() => this.clientId++;

        }

    }

}

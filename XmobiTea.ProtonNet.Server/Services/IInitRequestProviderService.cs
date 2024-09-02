using System.Threading;
using XmobiTea.ProtonNet.Server.Helper;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines a service for creating new session initialization requests.
    /// </summary>
    public interface IInitRequestProviderService
    {
        /// <summary>
        /// Creates a new session initialization request.
        /// </summary>
        /// <returns>A new instance of <see cref="ISessionInitRequest"/>.</returns>
        ISessionInitRequest NewSessionInitRequest();

    }

    /// <summary>
    /// Implements <see cref="IInitRequestProviderService"/> to provide new session initialization requests.
    /// </summary>
    public class InitRequestProviderService : IInitRequestProviderService
    {
        /// <summary>
        /// Defines the fixed length for session IDs. This is a constant value of 16.
        /// </summary>
        private static readonly int SessionIdLength = 16;

        /// <summary>
        /// Gets the provider responsible for generating and managing session IDs.
        /// </summary>
        protected SessionIdProvider sessionIdProvider { get; }

        /// <summary>
        /// Gets the provider responsible for generating and managing connection IDs.
        /// </summary>
        protected ConnectionIdProvider connectionIdProvider { get; }

        /// <summary>
        /// Gets the provider responsible for managing fibers, which are lightweight units of execution.
        /// </summary>
        protected FiberProvider fiberProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitRequestProviderService"/> class.
        /// </summary>
        public InitRequestProviderService()
        {
            this.sessionIdProvider = new SessionIdProvider();
            this.connectionIdProvider = new ConnectionIdProvider();
            this.fiberProvider = new FiberProvider();
        }

        /// <summary>
        /// Creates a new session initialization request.
        /// </summary>
        /// <returns>A new instance of <see cref="ISessionInitRequest"/>.</returns>
        public virtual ISessionInitRequest NewSessionInitRequest()
        {
            var answer = new SessionInitRequest()
            {
                ConnectionId = this.connectionIdProvider.GetNextConnectionId(),
                ServerSessionId = this.sessionIdProvider.GenerateRandomSessionId(),
                Fiber = this.fiberProvider.CreateNewFiber(),
            };

            return answer;
        }

        /// <summary>
        /// Provides functionality to generate session IDs.
        /// </summary>
        protected class SessionIdProvider
        {
            private static readonly string AllCharacters = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

            /// <summary>
            /// Generates a random session ID.
            /// </summary>
            /// <returns>A random session ID.</returns>
            public string GenerateRandomSessionId()
            {
                var answer = string.Empty;

                for (var i = 0; i < SessionIdLength; i++)
                    answer += AllCharacters[Random.Range(0, AllCharacters.Length)];

                return answer;
            }
        }

        /// <summary>
        /// Provides functionality to generate connection IDs.
        /// </summary>
        protected class ConnectionIdProvider
        {
            private int connectionId;

            /// <summary>
            /// Gets the next connection ID.
            /// </summary>
            /// <returns>The next connection ID.</returns>
            public int GetNextConnectionId() => Interlocked.Increment(ref this.connectionId);
        }

        /// <summary>
        /// Provides functionality to create new fibers.
        /// </summary>
        protected class FiberProvider
        {
            /// <summary>
            /// Creates and starts a new fiber.
            /// </summary>
            /// <returns>A new instance of <see cref="IFiber"/>.</returns>
            public IFiber CreateNewFiber()
            {
                var answer = new PoolFiber();
                answer.Start();

                return answer;
            }
        }

    }

}

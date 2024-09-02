using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Services;

namespace XmobiTea.ProtonNet.Server.WebApi.Services
{
    /// <summary>
    /// Provides initialization requests for Web API sessions.
    /// </summary>
    class WebApiInitRequestProviderService : InitRequestProviderService, IInitRequestProviderService
    {
        /// <summary>
        /// Creates a new session initialization request.
        /// </summary>
        /// <returns>An <see cref="ISessionInitRequest"/> instance with initialized properties.</returns>
        public override ISessionInitRequest NewSessionInitRequest()
        {
            var answer = new SessionInitRequest()
            {
                ConnectionId = this.connectionIdProvider.GetNextConnectionId(),
                ServerSessionId = this.sessionIdProvider.GenerateRandomSessionId(),
                Fiber = this.fiberProvider.CreateNewFiber(),
            };

            return answer;
        }

    }

}

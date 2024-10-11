using XmobiTea.ProtonNet.Server.Context;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Services;

namespace XmobiTea.ProtonNet.Server.Socket.Context
{
    /// <summary>
    /// Provides an interface for the socket server context, extending <see cref="IServerContext"/>.
    /// </summary>
    public interface ISocketServerContext : IServerContext
    {
        /// <summary>
        /// Gets the socket controller service used by the server.
        /// </summary>
        /// <returns>The <see cref="ISocketControllerService"/> instance.</returns>
        ISocketControllerService GetControllerService();

    }

    /// <summary>
    /// Represents the socket server context, implementing <see cref="ISocketServerContext"/>.
    /// This class is sealed to prevent inheritance.
    /// </summary>
    sealed class SocketServerContext : ServerContext, ISocketServerContext
    {
        private ISocketControllerService controllerService { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="SocketServerContext"/> class from being created.
        /// </summary>
        private SocketServerContext() { }

        /// <summary>
        /// Gets the socket controller service used by the server.
        /// </summary>
        /// <returns>The <see cref="ISocketControllerService"/> instance.</returns>
        public ISocketControllerService GetControllerService() => this.controllerService;

        /// <summary>
        /// Sets the socket controller service used by the server.
        /// </summary>
        /// <param name="controllerService">The <see cref="ISocketControllerService"/> instance to set.</param>
        public void SetControllerService(ISocketControllerService controllerService) => this.controllerService = controllerService;

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class to build a <see cref="SocketServerContext"/>.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Provides a builder pattern for constructing instances of <see cref="SocketServerContext"/>.
        /// </summary>
        public class Builder
        {
            internal Builder() { }

            /// <summary>
            /// Gets or sets the session service used by the server.
            /// </summary>
            public ISessionService SessionService { get; set; }

            /// <summary>
            /// Gets or sets the user peer session service used by the server.
            /// </summary>
            public IUserPeerSessionService UserPeerSessionService { get; set; }

            /// <summary>
            /// Gets or sets the initialization request provider service used by the server.
            /// </summary>
            public IInitRequestProviderService InitRequestProviderService { get; set; }

            /// <summary>
            /// Gets or sets the byte array manager service used by the server.
            /// </summary>
            public IByteArrayManagerService ByteArrayManagerService { get; set; }

            /// <summary>
            /// Sets the session service to be used by the server.
            /// </summary>
            /// <param name="sessionService">The session service to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetSessionService(ISessionService sessionService)
            {
                this.SessionService = sessionService;
                return this;
            }

            /// <summary>
            /// Sets the user peer session service to be used by the server.
            /// </summary>
            /// <param name="userPeerSessionService">The user peer session service to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetUserPeerSessionService(IUserPeerSessionService userPeerSessionService)
            {
                this.UserPeerSessionService = userPeerSessionService;
                return this;
            }

            /// <summary>
            /// Sets the initialization request provider service to be used by the server.
            /// </summary>
            /// <param name="initRequestProviderService">The initialization request provider service to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetInitRequestProviderService(IInitRequestProviderService initRequestProviderService)
            {
                this.InitRequestProviderService = initRequestProviderService;
                return this;
            }

            /// <summary>
            /// Sets the byte array manager service to be used by the server.
            /// </summary>
            /// <param name="byteArrayManagerService">The byte array manager service to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetByteArrayManagerService(IByteArrayManagerService byteArrayManagerService)
            {
                this.ByteArrayManagerService = byteArrayManagerService;
                return this;
            }

            /// <summary>
            /// Builds and returns a new instance of <see cref="SocketServerContext"/> using the provided services.
            /// </summary>
            /// <returns>A new <see cref="SocketServerContext"/> instance.</returns>
            public SocketServerContext Build()
            {
                var answer = new SocketServerContext();

                answer.sessionService = this.SessionService;
                answer.userPeerSessionService = this.UserPeerSessionService;
                answer.initRequestProviderService = this.InitRequestProviderService;
                answer.byteArrayManagerService = this.ByteArrayManagerService;

                return answer;
            }
        }

    }

}

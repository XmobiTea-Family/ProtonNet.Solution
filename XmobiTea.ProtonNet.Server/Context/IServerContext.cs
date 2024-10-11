using XmobiTea.ProtonNet.Server.Services;

namespace XmobiTea.ProtonNet.Server.Context
{
    /// <summary>
    /// Defines the interface for server context services.
    /// </summary>
    public interface IServerContext
    {
        /// <summary>
        /// Gets the session service.
        /// </summary>
        /// <returns>The session service instance.</returns>
        ISessionService GetSessionService();

        /// <summary>
        /// Gets the user peer session service.
        /// </summary>
        /// <returns>The user peer session service instance.</returns>
        IUserPeerSessionService GetUserPeerSessionService();

        /// <summary>
        /// Gets the initialization request provider service.
        /// </summary>
        /// <returns>The initialization request provider service instance.</returns>
        IInitRequestProviderService GetInitRequestProviderService();

        /// <summary>
        /// Gets the byte array manager service.
        /// </summary>
        /// <returns>The byte array manager service instance.</returns>
        IByteArrayManagerService GetByteArrayManagerService();

    }

    /// <summary>
    /// Implements the server context services.
    /// </summary>
    public class ServerContext : IServerContext
    {
        /// <summary>
        /// The session service instance.
        /// </summary>
        protected ISessionService sessionService { get; set; }

        /// <summary>
        /// The user peer session service instance.
        /// </summary>
        protected IUserPeerSessionService userPeerSessionService { get; set; }

        /// <summary>
        /// The initialization request provider service instance.
        /// </summary>
        protected IInitRequestProviderService initRequestProviderService { get; set; }

        /// <summary>
        /// The byte array manager service instance.
        /// </summary>
        protected IByteArrayManagerService byteArrayManagerService { get; set; }

        /// <summary>
        /// Gets the session service.
        /// </summary>
        /// <returns>The session service instance.</returns>
        public ISessionService GetSessionService() => this.sessionService;

        /// <summary>
        /// Gets the user peer session service.
        /// </summary>
        /// <returns>The user peer session service instance.</returns>
        public IUserPeerSessionService GetUserPeerSessionService() => this.userPeerSessionService;

        /// <summary>
        /// Gets the initialization request provider service.
        /// </summary>
        /// <returns>The initialization request provider service instance.</returns>
        public IInitRequestProviderService GetInitRequestProviderService() => this.initRequestProviderService;

        /// <summary>
        /// Gets the byte array manager service.
        /// </summary>
        /// <returns>The byte array manager service instance.</returns>
        public IByteArrayManagerService GetByteArrayManagerService() => this.byteArrayManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerContext"/> class.
        /// </summary>
        protected ServerContext() { }

    }
}

using System.Security.Cryptography.X509Certificates;
using XmobiTea.Bean;
using XmobiTea.Bean.Attributes;
using XmobiTea.Bean.Support;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNet.Server.WebApi.Server;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.WebApi
{
    /// <summary>
    /// Represents the partial implementation of the WebApiServer class, which provides
    /// functionalities for managing the Web API server operations, including startup,
    /// shutdown, and managing various server services.
    /// </summary>
    partial class WebApiServer : IWebApiServer
    {
        private ILogger logger { get; }
        private IBeanContext beanContext { get; }
        private IWebApiServerContext context { get; }
        private IServerNetworkStatistics networkStatistics { get; }

        private ISubServer httpServer { get; }
        private ISubServer httpsServer { get; }

        /// <summary>
        /// Initializes a new instance of the WebApiServer class with the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the server.</param>
        public WebApiServer(StartupSettings startupSettings)
        {
            this.logger = LogManager.GetLogger(this);

            this.logger.Info($"Try run Proton WebApi Server name: {startupSettings.Name}");
            this.ShowBanner();

            this.beanContext = this.CreateBeanContext(startupSettings);
            this.beanContext.SetSingleton(this.beanContext);

            this.beanContext.SetSingleton(this);

            this.context = this.CreateContext(startupSettings);
            this.beanContext.SetSingleton(this.context);

            this.beanContext.SetSingleton(startupSettings);

            this.CreateOtherSingleton(startupSettings);

            this.httpServer = this.CreateHttpServer(startupSettings);
            this.httpsServer = this.CreateHttpsServer(startupSettings);

            this.networkStatistics = this.CreateNetworkStatistics(startupSettings);

        }

        /// <summary>
        /// Gets the Web API server context.
        /// </summary>
        /// <returns>Returns the current IWebApiServerContext instance.</returns>
        public IWebApiServerContext GetContext() => this.context;

        /// <summary>
        /// Gets the network statistics for the server.
        /// </summary>
        /// <returns>Returns the current IServerNetworkStatistics instance.</returns>
        public IServerNetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Starts the Web API server, including both HTTP and HTTPS servers.
        /// </summary>
        public void Start()
        {
            this.httpServer?.Start();
            this.httpsServer?.Start();

            this.logger.Info("Server starting");

            if (System.IO.Directory.Exists("wwwroot")) this.context.GetControllerService().AddStaticFolderContent("wwwroot");
            if (System.IO.Directory.Exists("web")) this.context.GetControllerService().SetupWebsPathContent("web");

            this.SetupSingleton();

            this.OnStarting();

            this.logger.Info("Server started");
            this.OnStarted();

        }

        /// <summary>
        /// Stops the Web API server, including both HTTP and HTTPS servers.
        /// </summary>
        public void Stop()
        {
            this.httpServer?.Stop();
            this.httpsServer?.Stop();

            this.logger.Info("Server stopping");
            this.OnStopping();

            this.logger.Info("Server stopped");
            this.OnStopped();

        }

        /// <summary>
        /// Creates and registers additional singleton services required by the server.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the services.</param>
        private void CreateOtherSingleton(StartupSettings startupSettings)
        {
            var userPeerSessionService = this.CreateUserPeerSessionService(startupSettings);
            this.beanContext.SetSingleton(userPeerSessionService);

            var rpcProtocolService = this.CreateRpcProtocolService(startupSettings);
            this.beanContext.SetSingleton(rpcProtocolService);

            var userPeerService = this.CreateUserPeerService(startupSettings);
            this.beanContext.SetSingleton(userPeerService);

            var userPeerAuthTokenService = this.CreateUserPeerAuthTokenService(startupSettings);
            this.beanContext.SetSingleton(userPeerAuthTokenService);

            var channelService = this.CreateChannelService(startupSettings);
            this.beanContext.SetSingleton(channelService);

            var dataConverter = this.CreateDataConverter(startupSettings);
            this.beanContext.SetSingleton(dataConverter);

            this.CreateApplicationAssemblies(startupSettings, System.AppDomain.CurrentDomain.GetAssemblies());

        }

        /// <summary>
        /// Registers application assemblies and their associated services with the server.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the services.</param>
        /// <param name="assemblies">The collection of assemblies to be registered.</param>
        private void CreateApplicationAssemblies(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var controllerService = this.CreateControllerService(startupSettings, assemblies);
            this.beanContext.SetSingleton(controllerService);
            (this.context as WebApiServerContext).SetControllerService(controllerService);

            var requestService = this.CreateRequestService(startupSettings, assemblies);
            this.beanContext.SetSingleton(requestService);

        }

        /// <summary>
        /// Configures and returns a TcpServerOptions object based on the provided session configuration.
        /// </summary>
        /// <param name="sessionConfig">The session configuration settings.</param>
        /// <returns>Returns a configured TcpServerOptions instance.</returns>
        private TcpServerOptions GetTcpServerOptions(SessionConfigSettings sessionConfig)
        {
            var answer = new TcpServerOptions();

            answer.AcceptorBacklog = sessionConfig.AcceptorBacklog;
            answer.DualMode = sessionConfig.DualMode;
            answer.KeepAlive = sessionConfig.KeepAlive;

            answer.TcpKeepAliveTime = sessionConfig.TcpKeepAliveTime;
            answer.TcpKeepAliveInterval = sessionConfig.TcpKeepAliveInterval;
            answer.TcpKeepAliveRetryCount = sessionConfig.TcpKeepAliveRetryCount;

            answer.NoDelay = sessionConfig.NoDelay;
            answer.ReuseAddress = sessionConfig.ReuseAddress;
            answer.ExclusiveAddressUse = sessionConfig.ExclusiveAddressUse;

            answer.ReceiveBufferLimit = sessionConfig.ReceiveBufferLimit;
            answer.ReceiveBufferCapacity = sessionConfig.ReceiveBufferCapacity;
            answer.SendBufferLimit = sessionConfig.SendBufferLimit;
            answer.SendBufferCapacity = sessionConfig.SendBufferCapacity;

            return answer;
        }

        /// <summary>
        /// Configures and returns an SslOptions object based on the provided SSL configuration.
        /// </summary>
        /// <param name="sslConfig">The SSL configuration settings.</param>
        /// <returns>Returns a configured SslOptions instance.</returns>
        private SslOptions GetSslOptions(SslConfigSettings sslConfig) => new SslOptions(
#if NETCOREAPP || NET48_OR_GREATER
            System.Security.Authentication.SslProtocols.Tls13
#else
            System.Security.Authentication.SslProtocols.Tls12
#endif
            , new X509Certificate2(sslConfig.CertFilePath, sslConfig.CertPassword));

        /// <summary>
        /// Creates and returns an HTTP server instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the HTTP server.</param>
        /// <returns>Returns a configured ISubServer instance for HTTP, or null if disabled.</returns>
        private ISubServer CreateHttpServer(StartupSettings startupSettings)
        {
            if (!startupSettings.HttpServer.Enable) return null;

            return new WebApiHttpServer(startupSettings.HttpServer.Address, startupSettings.HttpServer.Port, this.GetTcpServerOptions(startupSettings.HttpServer.SessionConfig), this.context);
        }

        /// <summary>
        /// Creates and returns an HTTPS server instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the HTTPS server.</param>
        /// <returns>Returns a configured ISubServer instance for HTTPS, or null if disabled.</returns>
        private ISubServer CreateHttpsServer(StartupSettings startupSettings)
        {
            if (!startupSettings.HttpServer.Enable) return null;

            if (!startupSettings.HttpServer.SslConfig.Enable) return null;

            return new WebApiHttpsServer(startupSettings.HttpServer.Address, startupSettings.HttpServer.SslConfig.Port, this.GetTcpServerOptions(startupSettings.HttpServer.SessionConfig), this.GetSslOptions(startupSettings.HttpServer.SslConfig), this.context);
        }

        /// <summary>
        /// Sets up singleton services by scanning for classes with the SingletonAttribute
        /// and automatically binding objects as singletons.
        /// </summary>
        private void SetupSingleton()
        {
            this.CreateSingletonClassesAttribute();
            this.AutoBindSingletonObjects();

        }

        /// <summary>
        /// Scans for classes marked with SingletonAttribute and registers them as singletons.
        /// </summary>
        private void CreateSingletonClassesAttribute()
        {
            var singletonTypes = this.beanContext.ScanClassHasCustomAttribute(typeof(SingletonAttribute), false);

            foreach (var singletonType in singletonTypes)
            {
                if (singletonType.IsAbstract || singletonType.IsInterface) continue;

                this.beanContext.CreateSingleton(singletonType);

                this.logger.Info("BeanContext - auto create SingletonAttribute: " + singletonType.FullName);
            }
        }

        /// <summary>
        /// Automatically binds singleton objects in the bean context.
        /// </summary>
        private void AutoBindSingletonObjects()
        {
            var singletonObjs = this.beanContext.GetSingletons();
            foreach (var singletonObj in singletonObjs)
            {
                this.beanContext.AutoBind(singletonObj);
            }

            foreach (var singletonObj in singletonObjs)
            {
                (singletonObj as IFinalAutoBind)?.OnFinalAutoBind();
            }
        }

        /// <summary>
        /// Displays the startup banner in the logs.
        /// </summary>
        private void ShowBanner()
        {
            var banner =
                "\r\n██████╗ ██████╗  ██████╗ ████████╗ ██████╗ ███╗   ██╗    ███╗   ██╗███████╗████████╗\r\n██╔══██╗██╔══██╗██╔═══██╗╚══██╔══╝██╔═══██╗████╗  ██║    ████╗  ██║██╔════╝╚══██╔══╝\r\n██████╔╝██████╔╝██║   ██║   ██║   ██║   ██║██╔██╗ ██║    ██╔██╗ ██║█████╗     ██║   \r\n██╔═══╝ ██╔══██╗██║   ██║   ██║   ██║   ██║██║╚██╗██║    ██║╚██╗██║██╔══╝     ██║   \r\n██║     ██║  ██║╚██████╔╝   ██║   ╚██████╔╝██║ ╚████║    ██║ ╚████║███████╗   ██║   \r\n╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝  ╚═══╝    ╚═╝  ╚═══╝╚══════╝   ╚═╝   \r\n\r\n __        _______ ____    _    ____ ___   ____  _____ ______     _______ ____  \r\n \\ \\      / / ____| __ )  / \\  |  _ \\_ _| / ___|| ____|  _ \\ \\   / / ____|  _ \\ \r\n  \\ \\ /\\ / /|  _| |  _ \\ / _ \\ | |_) | |  \\___ \\|  _| | |_) \\ \\ / /|  _| | |_) |\r\n   \\ V  V / | |___| |_) / ___ \\|  __/| |   ___) | |___|  _ < \\ V / | |___|  _ < \r\n    \\_/\\_/  |_____|____/_/   \\_\\_|  |___| |____/|_____|_| \\_\\ \\_/  |_____|_| \\_\\\r\n                                                                                \r\n\r\n                Powered by XmobiTea Family\r\n                https://xmobitea.com\r\n";

            this.logger.Info($"\n\n{banner}\n");

        }

        /// <summary>
        /// Provides a hook for additional actions during the server startup process.
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Provides a hook for additional actions after the server has started.
        /// </summary>
        protected virtual void OnStarted() { }

        /// <summary>
        /// Provides a hook for additional actions during the server shutdown process.
        /// </summary>
        protected virtual void OnStopping() { }

        /// <summary>
        /// Provides a hook for additional actions after the server has stopped.
        /// </summary>
        protected virtual void OnStopped() { }

    }

}

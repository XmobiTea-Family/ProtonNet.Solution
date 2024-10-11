﻿using System.Security.Cryptography.X509Certificates;
using XmobiTea.Bean;
using XmobiTea.Bean.Attributes;
using XmobiTea.Bean.Support;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.Socket.Context;
using XmobiTea.ProtonNet.Server.Socket.Server;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetServer.Options;

namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Represents the implementation of the SocketServer class, providing methods to initialize, start, 
    /// and stop the server, along with various utility and configuration methods.
    /// </summary>
    partial class SocketServer : ISocketServer
    {
        private ILogger logger { get; }
        private IBeanContext beanContext { get; }
        private ISocketServerContext context { get; }
        private IServerNetworkStatistics networkStatistics { get; }

        private IServer socketTcpServer { get; }
        private IServer socketSslServer { get; }
        private IServer socketUdpServer { get; }
        private IServer socketWsServer { get; }
        private IServer socketWssServer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class using the specified startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        public SocketServer(StartupSettings startupSettings)
        {
            this.logger = LogManager.GetLogger(this);

            this.logger.Info($"Try run Proton Socket Server name: {startupSettings.Name}");
            this.ShowBanner();

            this.beanContext = this.CreateBeanContext(startupSettings);
            this.beanContext.SetSingleton(this.beanContext);
            this.beanContext.SetSingleton(this);
            this.context = this.CreateContext(startupSettings);
            this.beanContext.SetSingleton(this.context);
            this.beanContext.SetSingleton(startupSettings);

            this.CreateOtherSingleton(startupSettings);

            this.socketTcpServer = this.CreateSocketTcpServer(startupSettings);
            this.socketSslServer = this.CreateSocketSslServer(startupSettings);
            this.socketUdpServer = this.CreateSocketUdpServer(startupSettings);
            this.socketWsServer = this.CreateSocketWsServer(startupSettings);
            this.socketWssServer = this.CreateSocketWssServer(startupSettings);

            this.networkStatistics = this.CreateNetworkStatistics(startupSettings);
        }

        /// <summary>
        /// Starts the server by initiating all configured socket servers and related services.
        /// </summary>
        public void Start()
        {
            this.socketTcpServer?.Start();
            this.socketSslServer?.Start();
            this.socketUdpServer?.Start();
            this.socketWsServer?.Start();
            this.socketWssServer?.Start();

            this.logger.Info("Server starting");

            this.SetupSingleton();

            this.OnStarting();

            this.logger.Info("Server started");
            this.OnStarted();

        }

        /// <summary>
        /// Stops the server by stopping all active socket servers and related services.
        /// </summary>
        public void Stop()
        {
            this.socketTcpServer?.Stop();
            this.socketSslServer?.Stop();
            this.socketUdpServer?.Stop();
            this.socketWsServer?.Stop();
            this.socketWssServer?.Stop();

            this.logger.Info("Server stopping");
            this.OnStopping();

            this.logger.Info("Server stopped");
            this.OnStopped();

        }

        /// <summary>
        /// Gets the server context containing various services and configuration.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketServerContext"/>.</returns>
        public ISocketServerContext GetContext() => this.context;

        /// <summary>
        /// Gets the network statistics service used by the server.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/>.</returns>
        public IServerNetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Creates and initializes additional singleton services required by the server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
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

            var socketOperationModelService = this.CreateSocketOperationModelService(startupSettings);
            this.beanContext.SetSingleton(socketOperationModelService);

            var socketSessionTimeService = this.CreateSocketSessionTimeService(startupSettings);
            this.beanContext.SetSingleton(socketSessionTimeService);

            var socketSessionEmitService = this.CreateSocketSessionEmitService(startupSettings);
            this.beanContext.SetSingleton(socketSessionEmitService);

            this.CreateApplicationAssemblies(startupSettings, System.AppDomain.CurrentDomain.GetAssemblies());

        }

        /// <summary>
        /// Creates and initializes the necessary assemblies for application-specific components.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <param name="assemblies">The assemblies to scan and initialize.</param>
        private void CreateApplicationAssemblies(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var controllerService = this.CreateControllerService(startupSettings, assemblies);
            this.beanContext.SetSingleton(controllerService);
            (this.context as SocketServerContext)?.SetControllerService(controllerService);

            var requestService = this.CreateRequestService(startupSettings, assemblies);
            this.beanContext.SetSingleton(requestService);

            var eventService = this.CreateEventService(startupSettings, assemblies);
            this.beanContext.SetSingleton(eventService);

        }

        /// <summary>
        /// Generates TCP server options based on the provided session configuration.
        /// </summary>
        /// <param name="sessionConfig">The session configuration settings.</param>
        /// <returns>An instance of <see cref="TcpServerOptions"/>.</returns>
        private TcpServerOptions GetTcpServerOptions(SessionConfigSettings sessionConfig)
        {
            var answer = new TcpServerOptions
            {
                AcceptorBacklog = sessionConfig.AcceptorBacklog,
                DualMode = sessionConfig.DualMode,
                KeepAlive = sessionConfig.KeepAlive,
                TcpKeepAliveTime = sessionConfig.TcpKeepAliveTime,
                TcpKeepAliveInterval = sessionConfig.TcpKeepAliveInterval,
                TcpKeepAliveRetryCount = sessionConfig.TcpKeepAliveRetryCount,
                NoDelay = sessionConfig.NoDelay,
                ReuseAddress = sessionConfig.ReuseAddress,
                ExclusiveAddressUse = sessionConfig.ExclusiveAddressUse,
                ReceiveBufferLimit = sessionConfig.ReceiveBufferLimit,
                ReceiveBufferCapacity = sessionConfig.ReceiveBufferCapacity,
                SendBufferLimit = sessionConfig.SendBufferLimit,
                SendBufferCapacity = sessionConfig.SendBufferCapacity
            };

            return answer;
        }

        /// <summary>
        /// Generates UDP server options based on the provided session configuration.
        /// </summary>
        /// <param name="sessionConfig">The UDP session configuration settings.</param>
        /// <returns>An instance of <see cref="UdpServerOptions"/>.</returns>
        private UdpServerOptions GetUdpServerOptions(UdpSessionConfigSettings sessionConfig)
        {
            var answer = new UdpServerOptions
            {
                DualMode = sessionConfig.DualMode,
                ReuseAddress = sessionConfig.ReuseAddress,
                ExclusiveAddressUse = sessionConfig.ExclusiveAddressUse,
                ReceiveBufferLimit = sessionConfig.ReceiveBufferLimit,
                ReceiveBufferCapacity = sessionConfig.ReceiveBufferCapacity,
                SendBufferLimit = sessionConfig.SendBufferLimit
            };

            return answer;
        }

        /// <summary>
        /// Generates SSL options based on the provided SSL configuration.
        /// </summary>
        /// <param name="sslConfig">The SSL configuration settings.</param>
        /// <returns>An instance of <see cref="SslOptions"/>.</returns>
        private SslOptions GetSslOptions(SslConfigSettings sslConfig) => new SslOptions(
#if NETCOREAPP || NET48_OR_GREATER
            System.Security.Authentication.SslProtocols.Tls13
#else
            System.Security.Authentication.SslProtocols.Tls12
#endif
            , new X509Certificate2(sslConfig.CertFilePath, sslConfig.CertPassword));

        /// <summary>
        /// Creates and initializes the server context containing session-related services.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="ISocketServerContext"/>.</returns>
        protected virtual ISocketServerContext CreateContext(StartupSettings startupSettings)
        {
            var sessionService = this.CreateSessionService(startupSettings);
            this.beanContext.SetSingleton(sessionService);

            var userPeerSessionService = this.CreateUserPeerSessionService(startupSettings);
            this.beanContext.SetSingleton(userPeerSessionService);

            var initRequestProviderService = this.CreateInitRequestProviderService(startupSettings);
            this.beanContext.SetSingleton(initRequestProviderService);

            var byteArrayManagerService = this.CreateByteArrayManagerService(startupSettings);
            this.beanContext.SetSingleton(byteArrayManagerService);

            var answer = SocketServerContext.NewBuilder()
                .SetSessionService(sessionService)
                .SetUserPeerSessionService(userPeerSessionService)
                .SetInitRequestProviderService(initRequestProviderService)
                .SetByteArrayManagerService(byteArrayManagerService)
                .Build();

            return answer;
        }

        /// <summary>
        /// Creates and initializes the TCP socket server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServer"/> representing the TCP server, or null if not enabled.</returns>
        private IServer CreateSocketTcpServer(StartupSettings startupSettings)
        {
            if (!startupSettings.TcpServer.Enable) return null;

            return new SocketTcpServer(startupSettings.TcpServer.Address, startupSettings.TcpServer.Port, this.GetTcpServerOptions(startupSettings.TcpServer.SessionConfig), this.context);
        }

        /// <summary>
        /// Creates and initializes the SSL-enabled TCP socket server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServer"/> representing the SSL TCP server, or null if not enabled.</returns>
        private IServer CreateSocketSslServer(StartupSettings startupSettings)
        {
            if (!startupSettings.TcpServer.Enable) return null;
            if (!startupSettings.TcpServer.SslConfig.Enable) return null;

            return new SocketSslServer(startupSettings.TcpServer.Address, startupSettings.TcpServer.SslConfig.Port, this.GetTcpServerOptions(startupSettings.TcpServer.SessionConfig), this.GetSslOptions(startupSettings.TcpServer.SslConfig), this.context);
        }

        /// <summary>
        /// Creates and initializes the UDP socket server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServer"/> representing the UDP server, or null if not enabled.</returns>
        private IServer CreateSocketUdpServer(StartupSettings startupSettings)
        {
            if (!startupSettings.UdpServer.Enable) return null;

            return new SocketUdpServer(startupSettings.UdpServer.Address, startupSettings.UdpServer.Port, this.GetUdpServerOptions(startupSettings.UdpServer.SessionConfig), this.context);
        }

        /// <summary>
        /// Creates and initializes the WebSocket server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServer"/> representing the WebSocket server, or null if not enabled.</returns>
        private IServer CreateSocketWsServer(StartupSettings startupSettings)
        {
            if (!startupSettings.WebSocketServer.Enable) return null;

            return new SocketWsServer(startupSettings.WebSocketServer.Address, startupSettings.WebSocketServer.Port, this.GetTcpServerOptions(startupSettings.WebSocketServer.SessionConfig), this.context);
        }

        /// <summary>
        /// Creates and initializes the SSL-enabled WebSocket server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServer"/> representing the SSL WebSocket server, or null if not enabled.</returns>
        private IServer CreateSocketWssServer(StartupSettings startupSettings)
        {
            if (!startupSettings.WebSocketServer.Enable) return null;
            if (!startupSettings.WebSocketServer.SslConfig.Enable) return null;

            return new SocketWssServer(startupSettings.WebSocketServer.Address, startupSettings.WebSocketServer.Port, this.GetTcpServerOptions(startupSettings.WebSocketServer.SessionConfig), this.GetSslOptions(startupSettings.WebSocketServer.SslConfig), this.context);
        }

        /// <summary>
        /// Sets up singleton instances for classes marked with attributes.
        /// </summary>
        private void SetupSingleton()
        {
            this.CreateSingletonClassesAttribute();
            this.AutoBindSingletonObjects();
        }

        /// <summary>
        /// Automatically creates singleton instances for classes with the <see cref="SingletonAttribute"/>.
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
        /// Automatically binds singleton objects to the bean context.
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
        /// Displays the server banner in the logs.
        /// </summary>
        private void ShowBanner()
        {
            var banner =
                "\r\n██████╗ ██████╗  ██████╗ ████████╗ ██████╗ ███╗   ██╗    ███╗   ██╗███████╗████████╗\r\n██╔══██╗██╔══██╗██╔═══██╗╚══██╔══╝██╔═══██╗████╗  ██║    ████╗  ██║██╔════╝╚══██╔══╝\r\n██████╔╝██████╔╝██║   ██║   ██║   ██║   ██║██╔██╗ ██║    ██╔██╗ ██║█████╗     ██║   \r\n██╔═══╝ ██╔══██╗██║   ██║   ██║   ██║   ██║██║╚██╗██║    ██║╚██╗██║██╔══╝     ██║   \r\n██║     ██║  ██║╚██████╔╝   ██║   ╚██████╔╝██║ ╚████║    ██║ ╚████║███████╗   ██║   \r\n╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝  ╚═══╝    ╚═╝  ╚═══╝╚══════╝   ╚═╝   \r\n\r\n  ____   ___   ____ _  _______ _____   ____  _____ ______     _______ ____  \r\n / ___| / _ \\ / ___| |/ / ____|_   _| / ___|| ____|  _ \\ \\   / / ____|  _ \\ \r\n \\___ \\| | | | |   | ' /|  _|   | |   \\___ \\|  _| | |_) \\ \\ / /|  _| | |_) |\r\n  ___) | |_| | |___| . \\| |___  | |    ___) | |___|  _ < \\ V / | |___|  _ < \r\n |____/ \\___/ \\____|_|\\_\\_____| |_|   |____/|_____|_| \\_\\ \\_/  |_____|_| \\_\\\r\n                                                                            \r\n\r\n                Powered by XmobiTea Family\r\n                https://xmobitea.com\r\n";

            this.logger.Info($"\n\n{banner}\n");
        }

        /// <summary>
        /// Invoked during the server startup process before the server is fully started.
        /// </summary>
        protected virtual void OnStarting()
        {

        }

        /// <summary>
        /// Invoked after the server has successfully started.
        /// </summary>
        protected virtual void OnStarted()
        {

        }

        /// <summary>
        /// Invoked during the server shutdown process before the server is fully stopped.
        /// </summary>
        protected virtual void OnStopping()
        {

        }

        /// <summary>
        /// Invoked after the server has successfully stopped.
        /// </summary>
        protected virtual void OnStopped()
        {

        }
    }

}

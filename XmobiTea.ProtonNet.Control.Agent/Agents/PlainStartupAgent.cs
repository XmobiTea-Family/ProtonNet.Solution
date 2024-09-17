using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Control.Agent.Models;
using XmobiTea.ProtonNet.Control.Agent.Types;
using XmobiTea.ProtonNet.Server.Socket;
using XmobiTea.ProtonNet.Server.WebApi;

namespace XmobiTea.ProtonNet.Control.Agent.Agents
{
    /// <summary>
    /// Implementation of IStartupAgent for managing the startup of different types of servers.
    /// </summary>
    class PlainStartupAgent : IStartupAgent
    {
        private ILogger logger { get; }
        private AppDomain appDomain { get; set; }

        private StartupAgentInfo startupAgentInfo { get; }
        private string[] lookupBinPaths { get; }

        private IWebApiServer webApiServer { get; set; }
        private ISocketServer socketServer { get; set; }

        private PlainStartupAgent(Builder builder)
        {
            this.logger = LogManager.GetLogger(this);

            this.startupAgentInfo = builder.StartupAgentInfo;
            this.lookupBinPaths = this.CreateLookupBinPaths(builder.StartupAgentInfo);

            Environment.CurrentDirectory = this.startupAgentInfo.BinPath;
        }

        /// <summary>
        /// Creates paths for looking up assemblies.
        /// </summary>
        /// <param name="startupAgentInfo">Information used to create lookup paths.</param>
        /// <returns>An array of paths used for assembly lookup.</returns>
        private string[] CreateLookupBinPaths(StartupAgentInfo startupAgentInfo)
        {
            var answer = new string[2];

            answer[0] = startupAgentInfo.BinPath;
            answer[1] = startupAgentInfo.ProtonBinPath;

            return answer;
        }

        /// <summary>
        /// Starts the agent by setting up event handlers and loading the server.
        /// </summary>
        public void Start()
        {
            this.appDomain = AppDomain.CurrentDomain;

            this.appDomain.UnhandledException += new UnhandledExceptionEventHandler(this.UnhandledExceptionHandlerLogException);
            this.appDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.AssemblyLoadHandler);
            this.appDomain.AssemblyResolve += new ResolveEventHandler(this.AssemblyResolveHandler);
            this.appDomain.DomainUnload += new EventHandler(this.DomainUnloadHandler);
            this.appDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(this.ReflectionOnlyAssemblyResolveHandler);
            this.appDomain.ResourceResolve += new ResolveEventHandler(this.ResourceResolveHandler);
            this.appDomain.TypeResolve += new ResolveEventHandler(this.TypeResolveHandler);

            this.LoadAndStartServer();
        }

        /// <summary>
        /// Loads the server assembly and starts the server based on the configuration.
        /// </summary>
        private void LoadAndStartServer()
        {
            this.appDomain.Load(this.startupAgentInfo.AssemblyName);

            var assemblyName = this.startupAgentInfo.ServerType == ServerType.WebApi
                ? "XmobiTea.ProtonNet.Server.WebApi"
                : "XmobiTea.ProtonNet.Server.Socket";

            var assembly = this.appDomain.Load(assemblyName);

            if (this.startupAgentInfo.ServerType == ServerType.WebApi) this.StartWebApiServer();
            else if (this.startupAgentInfo.ServerType == ServerType.Socket) this.StartSocketServer();
            else throw new Exception("Invalid server type " + this.startupAgentInfo.ServerType);
        }

        /// <summary>
        /// Starts the Web API server.
        /// </summary>
        private void StartWebApiServer()
        {
            var serverEntry = Server.WebApi.WebApiServerEntry.NewBuilder()
                .SetStartupSettings(this.LoadWebApiServerStartupSettings())
                .Build();

            var server = serverEntry.GetServer();
            server.Start();

            this.webApiServer = server;

            this.SetupWwwRootPath();
            this.SetupWebsPath();
        }

        private void SetupWwwRootPath()
        {
            var wwwRootPath = this.startupAgentInfo.BinPath + "/wwwroot";

            if (Directory.Exists(wwwRootPath))
                this.webApiServer.GetContext().GetControllerService().AddStaticFolderContent(wwwRootPath);
        }

        private void SetupWebsPath()
        {
            var websPath = this.startupAgentInfo.BinPath + "/web";

            if (Directory.Exists(websPath))
                this.webApiServer.GetContext().GetControllerService().SetupWebsPathContent(websPath);
        }

        /// <summary>
        /// Loads the startup settings for the Web API server.
        /// </summary>
        /// <returns>The startup settings for the Web API server.</returns>
        private Server.WebApi.StartupSettings LoadWebApiServerStartupSettings()
        {
            var webApiStartupSettings = JsonConvert.DeserializeObject<WebApiStartupSettings>(File.ReadAllText(this.startupAgentInfo.StartupSettingsFilePath));
            return this.GenerateStartupSettingsFrom(webApiStartupSettings);
        }

        /// <summary>
        /// Generates Web API server startup settings from the provided configuration.
        /// </summary>
        /// <param name="webApiStartupSettings">Configuration for the Web API server.</param>
        /// <returns>The generated startup settings for the Web API server.</returns>
        private Server.WebApi.StartupSettings GenerateStartupSettingsFrom(WebApiStartupSettings webApiStartupSettings)
        {
            var sessionConfig = Server.WebApi.SessionConfigSettings.NewBuilder()
                .SetAcceptorBacklog(webApiStartupSettings.HttpServer.SessionConfig.AcceptorBacklog)
                .SetDualMode(webApiStartupSettings.HttpServer.SessionConfig.DualMode)
                .SetKeepAlive(webApiStartupSettings.HttpServer.SessionConfig.KeepAlive)
                .SetTcpKeepAliveTime(webApiStartupSettings.HttpServer.SessionConfig.TcpKeepAliveTime)
                .SetTcpKeepAliveInterval(webApiStartupSettings.HttpServer.SessionConfig.TcpKeepAliveInterval)
                .SetTcpKeepAliveRetryCount(webApiStartupSettings.HttpServer.SessionConfig.TcpKeepAliveRetryCount)
                .SetNoDelay(webApiStartupSettings.HttpServer.SessionConfig.NoDelay)
                .SetReuseAddress(webApiStartupSettings.HttpServer.SessionConfig.ReuseAddress)
                .SetExclusiveAddressUse(webApiStartupSettings.HttpServer.SessionConfig.ExclusiveAddressUse)
                .SetReceiveBufferLimit(webApiStartupSettings.HttpServer.SessionConfig.ReceiveBufferLimit)
                .SetReceiveBufferCapacity(webApiStartupSettings.HttpServer.SessionConfig.ReceiveBufferCapacity)
                .SetSendBufferLimit(webApiStartupSettings.HttpServer.SessionConfig.SendBufferLimit)
                .SetSendBufferCapacity(webApiStartupSettings.HttpServer.SessionConfig.SendBufferCapacity)
                .Build();

            var sslConfig = Server.WebApi.SslConfigSettings.NewBuilder()
                .SetEnable(webApiStartupSettings.HttpServer.SslConfig.Enable)
                .SetPort(webApiStartupSettings.HttpServer.SslConfig.Port)
                .SetCerFilePath(webApiStartupSettings.HttpServer.SslConfig.CerFilePath)
                .SetCerPassword(webApiStartupSettings.HttpServer.SslConfig.CerPassword)
                .Build();

            var httpServer = Server.WebApi.HttpServerSettings.NewBuilder()
                .SetAddress(webApiStartupSettings.HttpServer.Address)
                .SetPort(webApiStartupSettings.HttpServer.Port)
                .SetEnable(webApiStartupSettings.HttpServer.Enable)
                .SetSessionConfig(sessionConfig)
                .SetSslConfig(sslConfig)
                .Build();

            var threadPoolSize = Server.WebApi.ThreadPoolSizeSettings.NewBuilder()
                .SetOtherFiber(webApiStartupSettings.ThreadPoolSize.OtherFiber)
                .SetReceivedFiber(webApiStartupSettings.ThreadPoolSize.ReceivedFiber)
                .Build();

            var authToken = Server.WebApi.AuthTokenSettings.NewBuilder()
                .SetPassword(webApiStartupSettings.AuthToken.Password)
                .Build();

            var startupSettings = Server.WebApi.StartupSettings.NewBuilder()
                .SetName(webApiStartupSettings.Name)
                .SetMaxPendingRequest(webApiStartupSettings.MaxPendingRequest)
                .SetMaxSessionPendingRequest(webApiStartupSettings.MaxSessionPendingRequest)
                .SetMaxSessionRequestPerSecond(webApiStartupSettings.MaxSessionRequestPerSecond)
                .SetHttpServer(httpServer)
                .SetThreadPoolSize(threadPoolSize)
                .SetAuthToken(authToken)
                .Build();

            return startupSettings;
        }

        /// <summary>
        /// Starts the Socket server.
        /// </summary>
        private void StartSocketServer()
        {
            var serverEntry = Server.Socket.SocketServerEntry.NewBuilder()
                .SetStartupSettings(this.LoadSocketServerStartupSettings())
                .Build();

            var server = serverEntry.GetServer();
            server.Start();

            this.socketServer = server;
        }

        /// <summary>
        /// Loads the startup settings for the Socket server.
        /// </summary>
        /// <returns>The startup settings for the Socket server.</returns>
        private Server.Socket.StartupSettings LoadSocketServerStartupSettings()
        {
            var socketStartupSettings = JsonConvert.DeserializeObject<SocketStartupSettings>(File.ReadAllText(this.startupAgentInfo.StartupSettingsFilePath));
            return this.GenerateStartupSettingsFrom(socketStartupSettings);
        }

        /// <summary>
        /// Generates Socket server startup settings from the provided configuration.
        /// </summary>
        /// <param name="socketStartupSettings">Configuration for the Socket server.</param>
        /// <returns>The generated startup settings for the Socket server.</returns>
        private Server.Socket.StartupSettings GenerateStartupSettingsFrom(SocketStartupSettings socketStartupSettings)
        {
            var tcpSessionConfig = Server.Socket.SessionConfigSettings.NewBuilder()
                .SetAcceptorBacklog(socketStartupSettings.TcpServer.SessionConfig.AcceptorBacklog)
                .SetDualMode(socketStartupSettings.TcpServer.SessionConfig.DualMode)
                .SetKeepAlive(socketStartupSettings.TcpServer.SessionConfig.KeepAlive)
                .SetTcpKeepAliveTime(socketStartupSettings.TcpServer.SessionConfig.TcpKeepAliveTime)
                .SetTcpKeepAliveInterval(socketStartupSettings.TcpServer.SessionConfig.TcpKeepAliveInterval)
                .SetTcpKeepAliveRetryCount(socketStartupSettings.TcpServer.SessionConfig.TcpKeepAliveRetryCount)
                .SetNoDelay(socketStartupSettings.TcpServer.SessionConfig.NoDelay)
                .SetReuseAddress(socketStartupSettings.TcpServer.SessionConfig.ReuseAddress)
                .SetExclusiveAddressUse(socketStartupSettings.TcpServer.SessionConfig.ExclusiveAddressUse)
                .SetReceiveBufferLimit(socketStartupSettings.TcpServer.SessionConfig.ReceiveBufferLimit)
                .SetReceiveBufferCapacity(socketStartupSettings.TcpServer.SessionConfig.ReceiveBufferCapacity)
                .SetSendBufferLimit(socketStartupSettings.TcpServer.SessionConfig.SendBufferLimit)
                .SetSendBufferCapacity(socketStartupSettings.TcpServer.SessionConfig.SendBufferCapacity)
                .Build();

            var tcpSslConfig = Server.Socket.SslConfigSettings.NewBuilder()
                .SetEnable(socketStartupSettings.TcpServer.SslConfig.Enable)
                .SetPort(socketStartupSettings.TcpServer.SslConfig.Port)
                .SetCerFilePath(socketStartupSettings.TcpServer.SslConfig.CerFilePath)
                .SetCerPassword(socketStartupSettings.TcpServer.SslConfig.CerPassword)
                .Build();

            var tcpServer = Server.Socket.TcpServerSettings.NewBuilder()
                .SetAddress(socketStartupSettings.TcpServer.Address)
                .SetPort(socketStartupSettings.TcpServer.Port)
                .SetEnable(socketStartupSettings.TcpServer.Enable)
                .SetSessionConfig(tcpSessionConfig)
                .SetSslConfig(tcpSslConfig)
                .Build();

            var udpSessionConfig = Server.Socket.UdpSessionConfigSettings.NewBuilder()
                .SetDualMode(socketStartupSettings.UdpServer.SessionConfig.DualMode)
                .SetReuseAddress(socketStartupSettings.UdpServer.SessionConfig.ReuseAddress)
                .SetExclusiveAddressUse(socketStartupSettings.UdpServer.SessionConfig.ExclusiveAddressUse)
                .SetReceiveBufferLimit(socketStartupSettings.UdpServer.SessionConfig.ReceiveBufferLimit)
                .SetReceiveBufferCapacity(socketStartupSettings.UdpServer.SessionConfig.ReceiveBufferCapacity)
                .SetSendBufferLimit(socketStartupSettings.UdpServer.SessionConfig.SendBufferLimit)
                .Build();

            var udpServer = Server.Socket.UdpServerSettings.NewBuilder()
                .SetEnable(socketStartupSettings.UdpServer.Enable)
                .SetAddress(socketStartupSettings.UdpServer.Address)
                .SetPort(socketStartupSettings.UdpServer.Port)
                .SetSessionConfig(udpSessionConfig)
                .Build();

            var webSocketSessionConfig = Server.Socket.SessionConfigSettings.NewBuilder()
                .SetAcceptorBacklog(socketStartupSettings.WebSocketServer.SessionConfig.AcceptorBacklog)
                .SetDualMode(socketStartupSettings.WebSocketServer.SessionConfig.DualMode)
                .SetKeepAlive(socketStartupSettings.WebSocketServer.SessionConfig.KeepAlive)
                .SetTcpKeepAliveTime(socketStartupSettings.WebSocketServer.SessionConfig.TcpKeepAliveTime)
                .SetTcpKeepAliveInterval(socketStartupSettings.WebSocketServer.SessionConfig.TcpKeepAliveInterval)
                .SetTcpKeepAliveRetryCount(socketStartupSettings.WebSocketServer.SessionConfig.TcpKeepAliveRetryCount)
                .SetNoDelay(socketStartupSettings.WebSocketServer.SessionConfig.NoDelay)
                .SetReuseAddress(socketStartupSettings.WebSocketServer.SessionConfig.ReuseAddress)
                .SetExclusiveAddressUse(socketStartupSettings.WebSocketServer.SessionConfig.ExclusiveAddressUse)
                .SetReceiveBufferLimit(socketStartupSettings.WebSocketServer.SessionConfig.ReceiveBufferLimit)
                .SetReceiveBufferCapacity(socketStartupSettings.WebSocketServer.SessionConfig.ReceiveBufferCapacity)
                .SetSendBufferLimit(socketStartupSettings.WebSocketServer.SessionConfig.SendBufferLimit)
                .SetSendBufferCapacity(socketStartupSettings.WebSocketServer.SessionConfig.SendBufferCapacity)
                .Build();

            var webSocketSslConfig = Server.Socket.SslConfigSettings.NewBuilder()
                .SetEnable(socketStartupSettings.WebSocketServer.SslConfig.Enable)
                .SetPort(socketStartupSettings.WebSocketServer.SslConfig.Port)
                .SetCerFilePath(socketStartupSettings.WebSocketServer.SslConfig.CerFilePath)
                .SetCerPassword(socketStartupSettings.WebSocketServer.SslConfig.CerPassword)
                .Build();

            var webSocketServer = Server.Socket.WebSocketServerSettings.NewBuilder()
                .SetEnable(socketStartupSettings.WebSocketServer.Enable)
                .SetAddress(socketStartupSettings.WebSocketServer.Address)
                .SetPort(socketStartupSettings.WebSocketServer.Port)
                .SetMaxFrameSize(socketStartupSettings.WebSocketServer.MaxFrameSize)
                .SetSessionConfig(webSocketSessionConfig)
                .SetSslConfig(webSocketSslConfig)
                .Build();

            var threadPoolSize = Server.Socket.ThreadPoolSizeSettings.NewBuilder()
                .SetOtherFiber(socketStartupSettings.ThreadPoolSize.OtherFiber)
                .SetReceivedFiber(socketStartupSettings.ThreadPoolSize.ReceivedFiber)
                .Build();

            var authToken = Server.Socket.AuthTokenSettings.NewBuilder()
                .SetPassword(socketStartupSettings.AuthToken.Password)
                .Build();

            var startupSettings = Server.Socket.StartupSettings.NewBuilder()
                .SetName(socketStartupSettings.Name)
                .SetMaxSession(socketStartupSettings.MaxSession)
                .SetMaxPendingRequest(socketStartupSettings.MaxPendingRequest)
                .SetMaxSessionPendingRequest(socketStartupSettings.MaxSessionPendingRequest)
                .SetMaxSessionRequestPerSecond(socketStartupSettings.MaxSessionRequestPerSecond)
                .SetMaxUdpSessionRequestPerUser(socketStartupSettings.MaxUdpSessionRequestPerUser)
                .SetMaxTcpSessionRequestPerUser(socketStartupSettings.MaxTcpSessionRequestPerUser)
                .SetMaxWsSessionRequestPerUser(socketStartupSettings.MaxWsSessionRequestPerUser)
                .SetHandshakeTimeoutInSeconds(socketStartupSettings.HandshakeTimeoutInSeconds)
                .SetIdleTimeoutInSeconds(socketStartupSettings.IdleTimeoutInSeconds)
                .SetTcpServer(tcpServer)
                .SetUdpServer(udpServer)
                .SetWebSocketServer(webSocketServer)
                .SetThreadPoolSize(threadPoolSize)
                .SetAuthToken(authToken)
                .Build();

            return startupSettings;
        }

        /// <summary>
        /// Handles unhandled exceptions by logging the error.
        /// </summary>
        /// <param name="sender">The source of the unhandled exception.</param>
        /// <param name="e">The exception details.</param>
        private void UnhandledExceptionHandlerLogException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;

            this.logger.Fatal("UnhandledExceptionHandlerLogException", exception);
            this.logger.Info(exception.StackTrace);
        }

        /// <summary>
        /// Handles the event when an assembly is loaded.
        /// </summary>
        /// <param name="sender">The source of the assembly load event.</param>
        /// <param name="args">Details of the assembly load event.</param>
        private void AssemblyLoadHandler(object sender, AssemblyLoadEventArgs args)
        {
            this.logger.Info(string.Format("AssemblyLoadEvent: " + args.LoadedAssembly.FullName + " from " + args.LoadedAssembly.Location));
        }

        /// <summary>
        /// Handles the event when an assembly is resolved.
        /// </summary>
        /// <param name="sender">The source of the assembly resolve event.</param>
        /// <param name="args">Details of the assembly resolve event.</param>
        /// <returns>The resolved assembly, or null if not resolved.</returns>
        private Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            this.logger.Info(string.Format("AssemblyResolveEvent: ApplicationName = '{0}', DomainId='{1}', args.Name='{2}'", this.appDomain.FriendlyName, this.appDomain.Id, args.Name));

            var str = string.Empty;
            try
            {
                var libNames = args.Name.Split(',');

                for (var index = 0; index < this.lookupBinPaths.Length; index++)
                {
                    str = Path.Combine(this.lookupBinPaths[index], libNames[0].Trim() + ".dll");
                    if (File.Exists(str))
                        return Assembly.LoadFrom(str);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(string.Format("AssemblyResolve: Failed to load file {0}, error={1}", str, ex.Message));
            }
            return null;
        }

        /// <summary>
        /// Handles the event when a type is resolved.
        /// </summary>
        /// <param name="sender">The source of the type resolve event.</param>
        /// <param name="args">Details of the type resolve event.</param>
        /// <returns>The resolved type, or null if not resolved.</returns>
        private Assembly TypeResolveHandler(object sender, ResolveEventArgs args)
        {
            this.logger.Info(string.Format("TypeResolveHandler: ApplicationName = '{0}', DomainId='{1}', args.Name='{2}'", this.appDomain.FriendlyName, this.appDomain.Id, args.Name));
            return null;
        }

        /// <summary>
        /// Handles the event when a resource is resolved.
        /// </summary>
        /// <param name="sender">The source of the resource resolve event.</param>
        /// <param name="args">Details of the resource resolve event.</param>
        /// <returns>The resolved resource, or null if not resolved.</returns>
        private Assembly ResourceResolveHandler(object sender, ResolveEventArgs args)
        {
            this.logger.Info(string.Format("ResourceResolveHandler: ApplicationName = '{0}', DomainId='{1}', args.Name='{2}'", this.appDomain.FriendlyName, this.appDomain.Id, args.Name));
            return null;
        }

        /// <summary>
        /// Handles the event when a reflection-only assembly is resolved.
        /// </summary>
        /// <param name="sender">The source of the reflection-only assembly resolve event.</param>
        /// <param name="args">Details of the reflection-only assembly resolve event.</param>
        /// <returns>The resolved assembly, or null if not resolved.</returns>
        private Assembly ReflectionOnlyAssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            this.logger.Info(string.Format("ReflectionOnlyAssemblyResolveHandler: ApplicationName = '{0}', DomainId='{1}', args.Name='{2}'", this.appDomain.FriendlyName, this.appDomain.Id, args.Name));
            return null;
        }

        /// <summary>
        /// Handles the event when the application domain is unloaded.
        /// </summary>
        /// <param name="sender">The source of the domain unload event.</param>
        /// <param name="e">Details of the domain unload event.</param>
        private void DomainUnloadHandler(object sender, EventArgs e)
        {
            this.logger.Info(string.Format("DomainUnloadHandler: ApplicationName = '{0}', DomainId='{1}'", this.appDomain.FriendlyName, this.appDomain.Id));

            if (this.startupAgentInfo.ServerType == ServerType.Socket) this.socketServer?.Stop();
            else if (this.startupAgentInfo.ServerType == ServerType.WebApi) this.webApiServer?.Stop();
        }

        /// <summary>
        /// Stops the server based on the server type.
        /// </summary>
        public void Stop()
        {
            if (this.startupAgentInfo.ServerType == ServerType.Socket) this.socketServer?.Stop();
            else if (this.startupAgentInfo.ServerType == ServerType.WebApi) this.webApiServer?.Stop();
        }

        /// <summary>
        /// Creates a new Builder instance.
        /// </summary>
        /// <returns>A new Builder instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for creating instances of PlainStartupAgent.
        /// </summary>
        public class Builder
        {
            public StartupAgentInfo StartupAgentInfo { get; set; }

            /// <summary>
            /// Sets the startup agent information for the builder.
            /// </summary>
            /// <param name="startupAgentInfo">The startup agent information to set.</param>
            /// <returns>The updated Builder instance.</returns>
            public Builder SetStartupAgentInformation(StartupAgentInfo startupAgentInfo)
            {
                this.StartupAgentInfo = startupAgentInfo;
                return this;
            }

            internal Builder() { }

            /// <summary>
            /// Builds a new instance of PlainStartupAgent using the builder configuration.
            /// </summary>
            /// <returns>A new instance of PlainStartupAgent.</returns>
            public PlainStartupAgent Build() => new PlainStartupAgent(this);

        }

    }

}

using XmobiTea.Bean;
using XmobiTea.Data.Converter;
using XmobiTea.ProtonNet.Server.Handlers;
using XmobiTea.ProtonNet.Server.Handlers.Attributes;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Controllers;
using XmobiTea.ProtonNet.Server.Socket.Handlers;
using XmobiTea.ProtonNet.Server.Socket.Services;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Represents the partial implementation of the SocketServer class, 
    /// responsible for creating and configuring various services and components 
    /// required for server operation.
    /// </summary>
    partial class SocketServer
    {
        /// <summary>
        /// Creates and initializes the bean context used for dependency injection.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IBeanContext"/>.</returns>
        protected virtual IBeanContext CreateBeanContext(StartupSettings startupSettings) => new BeanContext();

        /// <summary>
        /// Creates and initializes the network statistics service for the server.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IServerNetworkStatistics"/>.</returns>
        protected virtual IServerNetworkStatistics CreateNetworkStatistics(StartupSettings startupSettings)
        {
            var statistics = new System.Collections.Generic.List<IServerNetworkStatistics>();

            if (this.socketTcpServer != null) statistics.Add(this.socketTcpServer.GetNetworkStatistics());
            if (this.socketSslServer != null) statistics.Add(this.socketSslServer.GetNetworkStatistics());
            if (this.socketUdpServer != null) statistics.Add(this.socketUdpServer.GetNetworkStatistics());
            if (this.socketWsServer != null) statistics.Add(this.socketWsServer.GetNetworkStatistics());
            if (this.socketWssServer != null) statistics.Add(this.socketWssServer.GetNetworkStatistics());

            return new XmobiTea.ProtonNet.Server.Models.ServerNetworkStatistics(statistics.ToArray());
        }

        /// <summary>
        /// Creates and initializes the socket operation model service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="ISocketOperationModelService"/>.</returns>
        protected virtual ISocketOperationModelService CreateSocketOperationModelService(StartupSettings startupSettings)
        {
            var answer = new SocketOperationModelService();

            answer.AddHandler(RpcProtocol.Types.OperationType.OperationRequest, this.beanContext.CreateSingleton<OperationRequestHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationResponse, this.beanContext.CreateSingleton<OperationResponseHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationEvent, this.beanContext.CreateSingleton<OperationEventHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationPing, this.beanContext.CreateSingleton<OperationPingHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationPong, this.beanContext.CreateSingleton<OperationPongHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationHandshakeAck, this.beanContext.CreateSingleton<OperationHandshakeAckHandler>());
            answer.AddHandler(RpcProtocol.Types.OperationType.OperationDisconnect, this.beanContext.CreateSingleton<OperationDisconnectHandler>());

            var operationHandshakeHandler = this.beanContext.CreateSingleton<OperationHandshakeHandler>();

            operationHandshakeHandler.SetMaxUdpSessionRequestPerUser(startupSettings.MaxUdpSessionRequestPerUser);
            operationHandshakeHandler.SetMaxTcpSessionRequestPerUser(startupSettings.MaxTcpSessionRequestPerUser);
            operationHandshakeHandler.SetMaxWsSessionRequestPerUser(startupSettings.MaxWsSessionRequestPerUser);

            answer.AddHandler(RpcProtocol.Types.OperationType.OperationHandshake, operationHandshakeHandler);

            return answer;
        }

        /// <summary>
        /// Creates and initializes the data converter service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IDataConverter"/>.</returns>
        protected virtual IDataConverter CreateDataConverter(StartupSettings startupSettings) => new DataConverter();

        /// <summary>
        /// Creates and initializes the socket session time service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="ISocketSessionTimeService"/>.</returns>
        protected virtual ISocketSessionTimeService CreateSocketSessionTimeService(StartupSettings startupSettings)
        {
            var answer = new SocketSessionTimeService();

            answer.SetHandshakeTimeoutInSeconds(startupSettings.HandshakeTimeoutInSeconds);
            answer.SetIdleTimeoutInSeconds(startupSettings.IdleTimeoutInSeconds);

            return answer;
        }

        /// <summary>
        /// Creates and initializes the session service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="ISessionService"/>.</returns>
        protected virtual ISessionService CreateSessionService(StartupSettings startupSettings) => new SessionService();

        /// <summary>
        /// Creates and initializes the user peer session service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IUserPeerSessionService"/>.</returns>
        protected virtual IUserPeerSessionService CreateUserPeerSessionService(StartupSettings startupSettings) => new UserPeerSessionService();

        /// <summary>
        /// Creates and initializes the user peer service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IUserPeerService"/>.</returns>
        protected virtual IUserPeerService CreateUserPeerService(StartupSettings startupSettings) => new UserPeerService();

        /// <summary>
        /// Creates and initializes the init request provider service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IInitRequestProviderService"/>.</returns>
        protected virtual IInitRequestProviderService CreateInitRequestProviderService(StartupSettings startupSettings) => new InitRequestProviderService();

        /// <summary>
        /// Creates and initializes the byte array manager service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IInitRequestProviderService"/>.</returns>
        protected virtual IByteArrayManagerService CreateByteArrayManagerService(StartupSettings startupSettings) => new ByteArrayManagerService();

        /// <summary>
        /// Creates and initializes the socket session emit service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="ISocketSessionEmitService"/>.</returns>
        protected virtual ISocketSessionEmitService CreateSocketSessionEmitService(StartupSettings startupSettings) => new SocketSessionEmitService();

        /// <summary>
        /// Creates and initializes the channel service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IChannelService"/>.</returns>
        protected virtual IChannelService CreateChannelService(StartupSettings startupSettings) => new ChannelService();

        /// <summary>
        /// Creates and returns a new IUserPeerAuthTokenService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the user peer auth token service.</param>
        /// <returns>Returns a new IUserPeerAuthTokenService instance.</returns>
        protected virtual IUserPeerAuthTokenService CreateUserPeerAuthTokenService(StartupSettings startupSettings) => new UserPeerAuthTokenService(startupSettings.AuthToken.Password);

        /// <summary>
        /// Creates and initializes the RPC protocol service.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <returns>An instance of <see cref="IRpcProtocolService"/>.</returns>
        protected virtual IRpcProtocolService CreateRpcProtocolService(StartupSettings startupSettings) => new RpcProtocolService();

        /// <summary>
        /// Creates and initializes the socket controller service, scanning for and adding controllers.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <param name="assemblies">The assemblies to scan for socket controllers.</param>
        /// <returns>An instance of <see cref="ISocketControllerService"/>.</returns>
        protected virtual ISocketControllerService CreateControllerService(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var answer = new SocketControllerService();

            answer.SetOtherFiber(startupSettings.ThreadPoolSize.OtherFiber);
            answer.SetReceiveFiber(startupSettings.ThreadPoolSize.ReceivedFiber);

            answer.SetMaxSession(startupSettings.MaxSession);
            answer.SetMaxPendingRequest(startupSettings.MaxPendingRequest);
            answer.SetMaxSessionRequestPerSecond(startupSettings.MaxSessionRequestPerSecond);
            answer.SetMaxSessionPendingRequest(startupSettings.MaxSessionPendingRequest);

            var socketControllerTypes = this.beanContext.ScanClassFromAssignable(typeof(SocketController), assemblies);
            foreach (var socketControllerType in socketControllerTypes)
            {
                if (socketControllerType.IsAbstract || socketControllerType.IsInterface) continue;

                var socketController = (SocketController)this.beanContext.CreateSingleton(socketControllerType);

                answer.AddSocketController(socketController);

                this.logger.Info("BeanContext - auto create SocketController: " + socketControllerType.FullName);
            }

            return answer;
        }

        /// <summary>
        /// Creates and initializes the request service, scanning for and adding request handlers.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <param name="assemblies">The assemblies to scan for request handlers.</param>
        /// <returns>An instance of <see cref="IRequestService"/>.</returns>
        protected virtual IRequestService CreateRequestService(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var answer = new RequestService();

            var requestHandlerTypes = this.beanContext.ScanClassFromAssignable(typeof(IRequestHandler), assemblies);

            foreach (var requestHandlerType in requestHandlerTypes)
            {
                if (requestHandlerType.IsAbstract || requestHandlerType.IsInterface) continue;

                var isDisableHandler = requestHandlerType.GetCustomAttributes(typeof(DisableHandlerAttribute), false).Length != 0;

                if (isDisableHandler)
                {
                    this.logger.Info("Disable " + requestHandlerType.Name);
                    continue;
                }

                var isAllowAmmonius = requestHandlerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Length != 0;
                var isOnlyServer = requestHandlerType.GetCustomAttributes(typeof(OnlyServerAttribute), false).Length != 0;

                var srvMsg = this.beanContext.CreateSingleton(requestHandlerType) as IRequestHandler;
                answer.AddHandler(srvMsg, isAllowAmmonius, isOnlyServer);

                this.logger.Info("BeanContext - auto create RequestHandler: " + requestHandlerType.FullName);
            }

            return answer;
        }

        /// <summary>
        /// Creates and initializes the event service, scanning for and adding event handlers.
        /// </summary>
        /// <param name="startupSettings">The startup settings configuration.</param>
        /// <param name="assemblies">The assemblies to scan for event handlers.</param>
        /// <returns>An instance of <see cref="IEventService"/>.</returns>
        protected virtual IEventService CreateEventService(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var answer = new EventService();

            var eventHandlerTypes = this.beanContext.ScanClassFromAssignable(typeof(IEventHandler), assemblies);

            foreach (var eventHandlerType in eventHandlerTypes)
            {
                if (eventHandlerType.IsAbstract || eventHandlerType.IsInterface) continue;

                var isDisableHandler = eventHandlerType.GetCustomAttributes(typeof(DisableHandlerAttribute), false).Length != 0;
                if (isDisableHandler)
                {
                    this.logger.Info("Disable " + eventHandlerType.Name);
                    continue;
                }

                var isAllowAmmonius = eventHandlerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Length != 0;
                var isOnlyServer = eventHandlerType.GetCustomAttributes(typeof(OnlyServerAttribute), false).Length != 0;

                var srvMsg = this.beanContext.CreateSingleton(eventHandlerType) as IEventHandler;
                answer.AddHandler(srvMsg, isAllowAmmonius, isOnlyServer);

                this.logger.Info("BeanContext - auto create EventHandler: " + eventHandlerType.FullName);
            }

            return answer;
        }

    }

}

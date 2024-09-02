using System.Collections.Generic;
using XmobiTea.Bean;
using XmobiTea.Data.Converter;
using XmobiTea.ProtonNet.Server.Handlers;
using XmobiTea.ProtonNet.Server.Handlers.Attributes;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.WebApi.Context;
using XmobiTea.ProtonNet.Server.WebApi.Controllers;
using XmobiTea.ProtonNet.Server.WebApi.Services;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi
{
    partial class WebApiServer
    {
        /// <summary>
        /// Creates and returns a new IBeanContext instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the bean context.</param>
        /// <returns>Returns a new IBeanContext instance.</returns>
        protected virtual IBeanContext CreateBeanContext(StartupSettings startupSettings) => new BeanContext();

        /// <summary>
        /// Creates and returns a new IServerNetworkStatistics instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the network statistics.</param>
        /// <returns>Returns a new IServerNetworkStatistics instance.</returns>
        protected virtual IServerNetworkStatistics CreateNetworkStatistics(StartupSettings startupSettings)
        {
            var statistics = new List<IServerNetworkStatistics>();

            if (this.httpServer != null) statistics.Add(this.httpServer.GetNetworkStatistics());
            if (this.httpsServer != null) statistics.Add(this.httpsServer.GetNetworkStatistics());

            return new XmobiTea.ProtonNet.Server.Models.ServerNetworkStatistics(statistics.ToArray());
        }

        /// <summary>
        /// Creates and returns a new IUserPeerSessionService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the user peer session service.</param>
        /// <returns>Returns a new IUserPeerSessionService instance.</returns>
        protected virtual IUserPeerSessionService CreateUserPeerSessionService(StartupSettings startupSettings) => new UserPeerSessionService();

        /// <summary>
        /// Creates and returns a new ISessionService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the session service.</param>
        /// <returns>Returns a new ISessionService instance.</returns>
        protected virtual ISessionService CreateSessionService(StartupSettings startupSettings) => new SessionService();

        /// <summary>
        /// Creates and returns a new IInitRequestProviderService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the init request provider service.</param>
        /// <returns>Returns a new IInitRequestProviderService instance.</returns>
        protected virtual IInitRequestProviderService CreateInitRequestProviderService(StartupSettings startupSettings) => new WebApiInitRequestProviderService();

        /// <summary>
        /// Creates and returns a new IChannelService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the channel service.</param>
        /// <returns>Returns a new IChannelService instance.</returns>
        protected virtual IChannelService CreateChannelService(StartupSettings startupSettings) => new ChannelService();

        /// <summary>
        /// Creates and returns a new IUserPeerAuthTokenService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the user peer auth token service.</param>
        /// <returns>Returns a new IUserPeerAuthTokenService instance.</returns>
        protected virtual IUserPeerAuthTokenService CreateUserPeerAuthTokenService(StartupSettings startupSettings) => new UserPeerAuthTokenService();

        /// <summary>
        /// Creates and returns a new IUserPeerService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the user peer service.</param>
        /// <returns>Returns a new IUserPeerService instance.</returns>
        protected virtual IUserPeerService CreateUserPeerService(StartupSettings startupSettings) => new UserPeerService();

        /// <summary>
        /// Creates and returns a new IRpcProtocolService instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the RPC protocol service.</param>
        /// <returns>Returns a new IRpcProtocolService instance.</returns>
        protected virtual IRpcProtocolService CreateRpcProtocolService(StartupSettings startupSettings) => new RpcProtocolService();

        /// <summary>
        /// Creates and returns a new IDataConverter instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the data converter.</param>
        /// <returns>Returns a new IDataConverter instance.</returns>
        protected virtual IDataConverter CreateDataConverter(StartupSettings startupSettings) => new DataConverter();

        /// <summary>
        /// Creates and returns a new IWebApiServerContext instance based on the provided startup settings.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the Web API server context.</param>
        /// <returns>Returns a new IWebApiServerContext instance.</returns>
        protected virtual IWebApiServerContext CreateContext(StartupSettings startupSettings)
        {
            var sessionService = this.CreateSessionService(startupSettings);
            this.beanContext.SetSingleton(sessionService);

            var userPeerSessionService = this.CreateUserPeerSessionService(startupSettings);
            this.beanContext.SetSingleton(userPeerSessionService);

            var initRequestProviderService = this.CreateInitRequestProviderService(startupSettings);
            this.beanContext.SetSingleton(initRequestProviderService);

            var answer = WebApiServerContext.NewBuilder()
                .SetUserPeerSessionService(userPeerSessionService)
                .SetSessionService(sessionService)
                .SetInitRequestProviderService(initRequestProviderService)
                .Build();

            return answer;
        }

        /// <summary>
        /// Creates and returns a new IWebApiControllerService instance based on the provided startup settings and assemblies.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the Web API controller service.</param>
        /// <param name="assemblies">The collection of assemblies to scan for Web API controllers.</param>
        /// <returns>Returns a new IWebApiControllerService instance.</returns>
        protected virtual IWebApiControllerService CreateControllerService(StartupSettings startupSettings, System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies)
        {
            var answer = new WebApiControllerService();

            answer.SetOtherFiber(startupSettings.ThreadPoolSize.OtherFiber);
            answer.SetReceiveFiber(startupSettings.ThreadPoolSize.ReceivedFiber);

            answer.SetMaxPendingRequest(startupSettings.MaxPendingRequest);
            answer.SetMaxSessionRequestPerSecond(startupSettings.MaxSessionRequestPerSecond);
            answer.SetMaxSessionPendingRequest(startupSettings.MaxSessionPendingRequest);

            var apiControllerTypes = this.beanContext.ScanClassFromAssignable(typeof(WebApiController), assemblies);
            foreach (var apiControllerType in apiControllerTypes)
            {
                if (apiControllerType.IsAbstract || apiControllerType.IsInterface) continue;

                var webApiController = (WebApiController)this.beanContext.CreateSingleton(apiControllerType);

                answer.AddWebApiController(webApiController);

                this.logger.Info("BeanContext - auto create WebApiController: " + apiControllerType.FullName);
            }

            return answer;
        }

        /// <summary>
        /// Creates and returns a new IRequestService instance based on the provided startup settings and assemblies.
        /// </summary>
        /// <param name="startupSettings">The startup settings for configuring the request service.</param>
        /// <param name="assemblies">The collection of assemblies to scan for request handlers.</param>
        /// <returns>Returns a new IRequestService instance.</returns>
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

    }

}

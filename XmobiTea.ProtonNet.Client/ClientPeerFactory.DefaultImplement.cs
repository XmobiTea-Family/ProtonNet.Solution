using XmobiTea.Bean;
using XmobiTea.Data.Converter;
using XmobiTea.ProtonNet.Client.Services;
using XmobiTea.ProtonNet.Client.Socket.Handlers;
using XmobiTea.ProtonNet.Client.Socket.Services;
using XmobiTea.ProtonNet.Client.Supports;
using XmobiTea.ProtonNet.RpcProtocol.Types;
using XmobiTea.ProtonNetClient.Options;

namespace XmobiTea.ProtonNet.Client
{
    public partial class ClientPeerFactory
    {
        /// <summary>
        /// Creates and returns a new instance of UdpClientOptions.
        /// This method can be overridden to provide custom UDP client options.
        /// </summary>
        /// <returns>A new instance of UdpClientOptions.</returns>
        protected virtual UdpClientOptions CreateUdpClientOptions() => new UdpClientOptions();

        /// <summary>
        /// Creates and returns a new instance of TcpClientOptions.
        /// This method can be overridden to provide custom TCP client options.
        /// </summary>
        /// <returns>A new instance of TcpClientOptions.</returns>
        protected virtual TcpClientOptions CreateTcpClientOptions() => new TcpClientOptions();

        /// <summary>
        /// Creates and returns a new instance of IBeanContext.
        /// This method can be overridden to provide a custom BeanContext.
        /// </summary>
        /// <returns>A new instance of IBeanContext.</returns>
        protected virtual IBeanContext CreateBeanContext() => new BeanContext();

        /// <summary>
        /// Creates and returns a new instance of IInitRequestProviderService.
        /// This method can be overridden to provide a custom init request provider service.
        /// </summary>
        /// <returns>A new instance of IInitRequestProviderService.</returns>
        protected virtual IInitRequestProviderService CreateInitRequestProviderService() => new InitRequestProviderService();

        /// <summary>
        /// Creates and returns a new instance of IDataConverter.
        /// This method can be overridden to provide a custom data converter.
        /// </summary>
        /// <returns>A new instance of IDataConverter.</returns>
        protected virtual IDataConverter CreateDataConverter() => new DataConverter();

        /// <summary>
        /// Creates and returns a new instance of IDebugSupport.
        /// This method can be overridden to provide custom debug support.
        /// </summary>
        /// <returns>A new instance of IDebugSupport.</returns>
        protected virtual IDebugSupport CreateDebugSupport() => new DefaultDebugOperationService();

        /// <summary>
        /// Creates and returns a new instance of IRpcProtocolService.
        /// This method can be overridden to provide a custom RPC protocol service.
        /// </summary>
        /// <returns>A new instance of IRpcProtocolService.</returns>
        protected virtual IRpcProtocolService CreateRpcProtocolService() => new RpcProtocolService(new byte[] { 22, 02, 20, 22 });

        /// <summary>
        /// Creates and returns a new instance of IEventService.
        /// This method can be overridden to provide a custom event service. 
        /// It also scans for classes that implement IEventHandler and automatically 
        /// registers them to the event service.
        /// </summary>
        /// <returns>A new instance of IEventService.</returns>
        protected virtual IEventService CreateEventService()
        {
            var answer = new EventService();

            var eventHandlerTypes = this.BeanContext.ScanClassFromAssignable(typeof(IEventHandler));

            foreach (var eventHandlerType in eventHandlerTypes)
            {
                if (eventHandlerType.IsAbstract || eventHandlerType.IsInterface) continue;

                var srvMsg = this.BeanContext.CreateSingleton(eventHandlerType) as IEventHandler;
                answer.AddHandler(srvMsg);

                this.logger.Info("BeanContext - auto create EventService: " + eventHandlerType.FullName);
            }

            return answer;
        }

        /// <summary>
        /// Creates and returns a new instance of ISocketSessionEmitService.
        /// This method can be overridden to provide a custom socket session emit service.
        /// </summary>
        /// <returns>A new instance of ISocketSessionEmitService.</returns>
        protected virtual ISocketSessionEmitService CreateSocketSessionEmitService() => new SocketSessionEmitService();

        /// <summary>
        /// Creates and returns a new instance of ISocketOperationModelService.
        /// This method can be overridden to provide a custom socket operation model service.
        /// It also registers handlers for various operation types.
        /// </summary>
        /// <returns>A new instance of ISocketOperationModelService.</returns>
        protected virtual ISocketOperationModelService CreateSocketOperationModelService()
        {
            var answer = new SocketOperationModelService();

            answer.AddHandler(OperationType.OperationPing, this.BeanContext.CreateSingleton<OperationPingHandler>());
            answer.AddHandler(OperationType.OperationPong, this.BeanContext.CreateSingleton<OperationPongHandler>());
            answer.AddHandler(OperationType.OperationRequest, this.BeanContext.CreateSingleton<OperationRequestHandler>());
            answer.AddHandler(OperationType.OperationResponse, this.BeanContext.CreateSingleton<OperationResponseHandler>());
            answer.AddHandler(OperationType.OperationEvent, this.BeanContext.CreateSingleton<OperationEventHandler>());
            answer.AddHandler(OperationType.OperationHandshake, this.BeanContext.CreateSingleton<OperationHandshakeHandler>());
            answer.AddHandler(OperationType.OperationHandshakeAck, this.BeanContext.CreateSingleton<OperationHandshakeAckHandler>());
            answer.AddHandler(OperationType.OperationDisconnect, this.BeanContext.CreateSingleton<OperationDisconnectHandler>());

            return answer;
        }

    }

}

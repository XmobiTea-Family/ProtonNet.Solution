using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Handlers;
using XmobiTea.ProtonNet.Server.Helper;
using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Types;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines a service for handling requests.
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Retrieves the request handler associated with the specified operation code.
        /// </summary>
        /// <param name="operationCode">The code representing the operation.</param>
        /// <returns>The request handler for the specified operation code.</returns>
        IRequestHandler GetHandler(string operationCode);

        /// <summary>
        /// Handles the specified operation request asynchronously.
        /// </summary>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="userPeer">The user peer initiating the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with a result of type <see cref="OperationResponse"/>.</returns>
        System.Threading.Tasks.Task<OperationResponse> Handle(OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session);
    }

    /// <summary>
    /// Implements <see cref="IRequestService"/> to handle requests.
    /// </summary>
    public class RequestService : IRequestService
    {
        private ILogger logger { get; }

        private System.Collections.Generic.IDictionary<string, IRequestHandler> handlerDict { get; }
        private System.Collections.Generic.IList<string> requestAllowAnonymousCodeLst { get; }
        private System.Collections.Generic.IList<string> requestOnlyServerCodeLst { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestService"/> class.
        /// </summary>
        public RequestService()
        {
            this.logger = LogManager.GetLogger(this);

            this.handlerDict = new System.Collections.Generic.Dictionary<string, IRequestHandler>();
            this.requestAllowAnonymousCodeLst = new System.Collections.Generic.List<string>();
            this.requestOnlyServerCodeLst = new System.Collections.Generic.List<string>();
        }

        /// <summary>
        /// Adds a new request handler to the service.
        /// </summary>
        /// <param name="requestHandler">The request handler to add.</param>
        /// <param name="allowAnonymous">Specifies whether anonymous is allowed for this handler.</param>
        /// <param name="onlyServer">Specifies whether this handler is only for server-side requests.</param>
        public void AddHandler(IRequestHandler requestHandler, bool allowAnonymous = false, bool onlyServer = false)
        {
            var operationCode = requestHandler.GetOperationCode();

            if (this.handlerDict.ContainsKey(operationCode))
                this.logger.Warn("Change exists request handler: " + this.handlerDict[operationCode] + " to new request handler " + requestHandler);

            this.handlerDict[operationCode] = requestHandler;

            if (allowAnonymous) this.requestAllowAnonymousCodeLst.Add(operationCode);
            if (onlyServer) this.requestOnlyServerCodeLst.Add(operationCode);
        }

        /// <summary>
        /// Removes the request handler associated with the specified operation code.
        /// </summary>
        /// <param name="operationCode">The code representing the operation.</param>
        /// <returns>True if the handler was removed; otherwise, false.</returns>
        public bool RemoveHandler(string operationCode)
        {
            return this.handlerDict.Remove(operationCode);
        }

        /// <summary>
        /// Retrieves the request handler associated with the specified operation code.
        /// </summary>
        /// <param name="operationCode">The code representing the operation.</param>
        /// <returns>The request handler for the specified operation code.</returns>
        public IRequestHandler GetHandler(string operationCode)
        {
            this.handlerDict.TryGetValue(operationCode, out var answer);
            return answer;
        }

        /// <summary>
        /// Handles the specified operation request asynchronously.
        /// </summary>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="userPeer">The user peer initiating the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with a result of type <see cref="OperationResponse"/>.</returns>
        public async System.Threading.Tasks.Task<OperationResponse> Handle(OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session)
        {
            var operationCode = operationRequest.OperationCode;

            if (!this.requestAllowAnonymousCodeLst.Contains(operationCode) && !userPeer.IsAuthenticated())
                return OperationHelper.HandleOperationNotAuthorized(operationRequest);

            if ((userPeer.GetPeerType() == PeerType.SocketClient || userPeer.GetPeerType() == PeerType.WebApiClient) && this.requestOnlyServerCodeLst.Contains(operationCode))
                return OperationHelper.HandleOperationInvalid(operationRequest, "client cant send this request");

            var handler = this.GetHandler(operationCode);
            if (handler != null) return await handler.Handle(operationRequest, sendParameters, userPeer, session);

            return null;
        }

    }

}

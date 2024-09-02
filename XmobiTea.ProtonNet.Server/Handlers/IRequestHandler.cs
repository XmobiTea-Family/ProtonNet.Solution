using System;
using XmobiTea.Bean.Attributes;
using XmobiTea.Data.Converter;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Helper;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Handlers
{
    /// <summary>
    /// Defines the interface for request handlers.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Gets the operation code.
        /// </summary>
        /// <returns>The operation code as a string.</returns>
        string GetOperationCode();

        /// <summary>
        /// Handles the operation request asynchronously.
        /// </summary>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with an <see cref="OperationResponse"/> result.</returns>
        System.Threading.Tasks.Task<OperationResponse> Handle(OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session);
    }

    /// <summary>
    /// Provides a base implementation for request handlers.
    /// </summary>
    public abstract class RequestHandler : IRequestHandler
    {
        /// <summary>
        /// Logger instance for logging events.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Gets the operation code.
        /// </summary>
        /// <returns>The operation code as a string.</returns>
        public abstract string GetOperationCode();

        /// <summary>
        /// Handles the operation request asynchronously.
        /// </summary>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with an <see cref="OperationResponse"/> result.</returns>
        public abstract System.Threading.Tasks.Task<OperationResponse> Handle(OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session);

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler"/> class.
        /// </summary>
        public RequestHandler()
        {
            this.logger = LogManager.GetLogger(this);
        }
    }

    /// <summary>
    /// Provides a generic implementation for request handlers with a specific request model.
    /// </summary>
    /// <typeparam name="TRequestModel">The type of the request model.</typeparam>
    public abstract class RequestHandler<TRequestModel> : RequestHandler, IRequestHandler
    {
        /// <summary>
        /// Data converter for converting data.
        /// </summary>
        [AutoBind]
        private IDataConverter dataConverter { get; set; }

        /// <summary>
        /// Handles the operation request asynchronously and converts it to the request model.
        /// </summary>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with an <see cref="OperationResponse"/> result.</returns>
        public override async System.Threading.Tasks.Task<OperationResponse> Handle(OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session)
        {
            var requestModel = this.ConvertToRequestModel(operationRequest);

            if (requestModel == null)
                return OperationHelper.HandleOperationInvalid(operationRequest, "requestModel invalid");

            return await this.Handle(requestModel, operationRequest, sendParameters, userPeer, session);
        }

        /// <summary>
        /// Converts the operation request to the specific request model.
        /// </summary>
        /// <param name="operationRequest">The operation request to convert.</param>
        /// <returns>The request model, or default value if conversion fails.</returns>
        private TRequestModel ConvertToRequestModel(OperationRequest operationRequest)
        {
            try
            {
                return this.dataConverter.DeserializeObject<TRequestModel>(operationRequest.Parameters);
            }
            catch (Exception ex)
            {
                this.logger.Fatal(ex);
            }

            return default;
        }

        /// <summary>
        /// Handles the operation request with the specified request model.
        /// </summary>
        /// <param name="requestModel">The request model to handle.</param>
        /// <param name="operationRequest">The operation request to handle.</param>
        /// <param name="sendParameters">Parameters for sending data.</param>
        /// <param name="userPeer">The user peer associated with the request.</param>
        /// <param name="session">The session associated with the request.</param>
        /// <returns>A task representing the asynchronous operation, with an <see cref="OperationResponse"/> result.</returns>
        public abstract System.Threading.Tasks.Task<OperationResponse> Handle(TRequestModel requestModel, OperationRequest operationRequest, SendParameters sendParameters, IUserPeer userPeer, ISession session);

    }

}

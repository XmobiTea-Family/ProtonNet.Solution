using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Support
{
    /// <summary>
    /// Defines a handler that processes the final response
    /// before sending it back to the client in the Web API pipeline.
    /// </summary>
    public interface IBeforeWebApiControllerResponse
    {
        /// <summary>
        /// Executes after the controller action completes, allowing 
        /// final modifications to the request and response before 
        /// returning the response to the client.
        /// </summary>
        /// <param name="session">The session that received the request.</param>
        /// <param name="request">The incoming HttpRequest object.</param>
        /// <param name="response">The outgoing HttpResponse object.</param>
        void OnPostControllerResponse(IWebApiSession session, HttpRequest request, HttpResponse response);

    }

}

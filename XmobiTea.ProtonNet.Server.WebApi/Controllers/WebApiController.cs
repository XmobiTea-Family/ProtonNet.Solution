using XmobiTea.Bean.Attributes;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render;
using XmobiTea.ProtonNet.Server.WebApi.Models;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers
{
    /// <summary>
    /// Abstract base class for Web API controllers.
    /// </summary>
    public abstract class WebApiController
    {
        /// <summary>
        /// Logger for the controller.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// View engine bound by AutoBind attribute.
        /// </summary>
        [AutoBind]
        private IViewEngine viewEngine { get; set; }

        /// <summary>
        /// Initializes a new instance of the WebApiController class.
        /// </summary>
        protected WebApiController() => this.logger = LogManager.GetLogger(this);

        /// <summary>
        /// Called when a new session is connected.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        public virtual void OnConnected(IWebApiSession session) { }

        /// <summary>
        /// Called when a request is received.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="request">The HTTP request.</param>
        public virtual void OnReceived(IWebApiSession session, ProtonNetCommon.HttpRequest request) { }

        /// <summary>
        /// Called when an error occurs during request processing.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="request">The HTTP request.</param>
        /// <param name="error">The error message.</param>
        public virtual void OnReceivedRequestError(IWebApiSession session, ProtonNetCommon.HttpRequest request, string error) { }

        /// <summary>
        /// Called when a socket error occurs.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        /// <param name="error">The socket error.</param>
        public virtual void OnError(IWebApiSession session, System.Net.Sockets.SocketError error) { }

        /// <summary>
        /// Called when a session is disconnected.
        /// </summary>
        /// <param name="session">The Web API session.</param>
        public virtual void OnDisconnected(IWebApiSession session) { }

        /// <summary>
        /// Creates a new HttpResponse.
        /// </summary>
        /// <returns>A new HttpResponse.</returns>
        protected virtual HttpResponse Response() => new HttpResponse();

        /// <summary>
        /// Creates a ViewResult for rendering a view.
        /// </summary>
        /// <param name="view">The name of the view.</param>
        /// <param name="layout">The name of the layout (optional).</param>
        /// <param name="viewData">The view data (optional).</param>
        /// <returns>A ViewResult with the rendered view content.</returns>
        protected virtual ViewResult View(string view, string layout = null, ViewData viewData = null)
        {
            var answer = new ViewResult();

            try
            {
                var template = this.viewEngine.GetTemplate(view, layout);

                var v = this.viewEngine.Render(template, viewData);

                answer.MakeGetResponse(v.Html, "text/html");
            }
            catch (System.Exception exception)
            {
                this.logger.Fatal(exception);

                answer.MakeErrorResponse(StatusCode.InternalServerError, $"Message: {exception.Message}\nStackTrace: {exception.StackTrace}");
            }

            return answer;
        }

    }

}

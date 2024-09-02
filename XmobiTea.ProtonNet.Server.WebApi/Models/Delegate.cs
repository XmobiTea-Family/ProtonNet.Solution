using System.Threading.Tasks;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents a delegate that defines the next function to be invoked in a middleware pipeline.
    /// </summary>
    /// <returns>The HTTP response produced by the next delegate in the pipeline.</returns>
    public delegate ProtonNetCommon.HttpResponse NextDelegate();

    /// <summary>
    /// Represents a delegate for middleware functions that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/> and
    /// invoke the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <param name="next">The next delegate to be invoked in the pipeline.</param>
    /// <returns>The HTTP response produced by the middleware.</returns>
    public delegate ProtonNetCommon.HttpResponse MiddlewareDelegate(MiddlewareContext context, ProtonNetCommon.HttpRequest req, NextDelegate next);

    /// <summary>
    /// Represents an asynchronous delegate for middleware functions that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/> and
    /// invoke the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <param name="next">The next delegate to be invoked in the pipeline.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response produced by the middleware.</returns>
    public delegate Task<ProtonNetCommon.HttpResponse> MiddlewareDelegateAsync(MiddlewareContext context, ProtonNetCommon.HttpRequest req, NextDelegate next);

    /// <summary>
    /// Represents a delegate for GET request handlers that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/>.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <returns>The HTTP response produced by the GET request handler.</returns>
    public delegate ProtonNetCommon.HttpResponse GetDelegate(MiddlewareContext context, ProtonNetCommon.HttpRequest req);

    /// <summary>
    /// Represents an asynchronous delegate for GET request handlers that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/>.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response produced by the GET request handler.</returns>
    public delegate Task<ProtonNetCommon.HttpResponse> GetDelegateAsync(MiddlewareContext context, ProtonNetCommon.HttpRequest req);

    /// <summary>
    /// Represents a delegate for POST request handlers that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/>.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <returns>The HTTP response produced by the POST request handler.</returns>
    public delegate ProtonNetCommon.HttpResponse PostDelegate(MiddlewareContext context, ProtonNetCommon.HttpRequest req);

    /// <summary>
    /// Represents an asynchronous delegate for POST request handlers that accept a <see cref="MiddlewareContext"/> and <see cref="ProtonNetCommon.HttpRequest"/>.
    /// </summary>
    /// <param name="context">The middleware context.</param>
    /// <param name="req">The HTTP request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response produced by the POST request handler.</returns>
    public delegate Task<ProtonNetCommon.HttpResponse> PostDelegateAsync(MiddlewareContext context, ProtonNetCommon.HttpRequest req);

}

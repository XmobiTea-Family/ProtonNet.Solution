using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetCommon.Extensions
{
    /// <summary>
    /// Provides extension methods for creating various types of HTTP responses.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Creates a new HTTP 200 OK response.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="status">The status code for the response. Defaults to 200 OK.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeOkResponse(this HttpResponse response, StatusCode status = StatusCode.OK)
        {
            response.Clear();
            response.SetBegin(status);
            response.SetBody();

            return response;
        }

        /// <summary>
        /// Creates a new HTTP error response with the specified status code and error message.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="status">The status code for the response.</param>
        /// <param name="error">The error message to include in the response body.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeErrorResponse(this HttpResponse response, StatusCode status, string error = "")
        {
            response.Clear();
            response.SetBegin(status);
            response.SetBody(error);

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK HEAD response.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeHeadResponse(this HttpResponse response)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            response.SetBody();

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK GET response with string content.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="content">The string content to include in the response body.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to text/plain.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeGetResponse(this HttpResponse response, string content = "", string contentType = Constance.DefaultTextContentType)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            if (!string.IsNullOrEmpty(contentType))
                response.SetHeader(HeaderNames.ContentType, contentType);
            response.SetBody(content);

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK GET response with binary content.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="content">The byte array content to include in the response body.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to application/octet-stream.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeGetResponse(this HttpResponse response, byte[] content, string contentType = Constance.DefaultEmptyContentType)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            if (!string.IsNullOrEmpty(contentType))
                response.SetHeader(HeaderNames.ContentType, contentType);
            response.SetBody(content);

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK OPTIONS response.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="allow">The allowed HTTP methods to include in the Allow header. Defaults to a standard set of methods.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeOptionsResponse(this HttpResponse response, string allow = Constance.DefaultOptionsAllow)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            response.SetHeader(HeaderNames.Allow, allow);
            response.SetBody();

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK TRACE response with string content.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="request">The string content to include in the response body, representing the original request.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeTraceResponse(this HttpResponse response, string request)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            response.SetHeader(HeaderNames.ContentType, HeaderValues.MessageHttp);
            response.SetBody(request);

            return response;
        }

        /// <summary>
        /// Creates a new HTTP 200 OK TRACE response with binary content.
        /// </summary>
        /// <param name="response">The HTTP response instance to modify.</param>
        /// <param name="request">The byte array content to include in the response body, representing the original request.</param>
        /// <returns>The modified HTTP response instance.</returns>
        public static HttpResponse MakeTraceResponse(this HttpResponse response, byte[] request)
        {
            response.Clear();
            response.SetBegin(StatusCode.OK);
            response.SetHeader(HeaderNames.ContentType, HeaderValues.MessageHttp);
            response.SetBody(request);

            return response;
        }

    }

}

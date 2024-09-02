using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetCommon.Extensions
{
    /// <summary>
    /// Provides extension methods for creating various types of HTTP requests.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Creates a new HTTP HEAD request.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the HEAD request.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewHeadRequest(this HttpRequest request, string url)
        {
            request.Clear();
            request.SetBegin(MethodNames.Head, url);
            request.SetBody();

            return request;
        }

        /// <summary>
        /// Creates a new HTTP GET request.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the GET request.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewGetRequest(this HttpRequest request, string url)
        {
            request.Clear();
            request.SetBegin(MethodNames.Get, url);
            request.SetBody();

            return request;
        }

        /// <summary>
        /// Creates a new HTTP POST request with string content.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the POST request.</param>
        /// <param name="content">The string content to be sent in the POST request.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to text/plain.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewPostRequest(this HttpRequest request, string url, string content, string contentType = Constance.DefaultTextContentType)
        {
            request.Clear();
            request.SetBegin(MethodNames.Post, url);
            if (!string.IsNullOrEmpty(contentType))
                request.SetHeader(HeaderNames.ContentType, contentType);
            request.SetBody(content);

            return request;
        }

        /// <summary>
        /// Creates a new HTTP POST request with binary content.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the POST request.</param>
        /// <param name="content">The byte array content to be sent in the POST request.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to application/octet-stream.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewPostRequest(this HttpRequest request, string url, byte[] content, string contentType = Constance.DefaultEmptyContentType)
        {
            request.Clear();
            request.SetBegin(MethodNames.Post, url);
            if (!string.IsNullOrEmpty(contentType))
                request.SetHeader(HeaderNames.ContentType, contentType);
            request.SetBody(content);

            return request;
        }

        /// <summary>
        /// Creates a new HTTP PUT request with string content.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the PUT request.</param>
        /// <param name="content">The string content to be sent in the PUT request.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to text/plain.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewPutRequest(this HttpRequest request, string url, string content, string contentType = Constance.DefaultTextContentType)
        {
            request.Clear();
            request.SetBegin(MethodNames.Put, url);
            if (!string.IsNullOrEmpty(contentType))
                request.SetHeader(HeaderNames.ContentType, contentType);
            request.SetBody(content);

            return request;
        }

        /// <summary>
        /// Creates a new HTTP PUT request with binary content.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the PUT request.</param>
        /// <param name="content">The byte array content to be sent in the PUT request.</param>
        /// <param name="contentType">The MIME type of the content. Defaults to application/octet-stream.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewPutRequest(this HttpRequest request, string url, byte[] content, string contentType = Constance.DefaultEmptyContentType)
        {
            request.Clear();
            request.SetBegin(MethodNames.Put, url);
            if (!string.IsNullOrEmpty(contentType))
                request.SetHeader(HeaderNames.ContentType, contentType);
            request.SetBody(content);

            return request;
        }

        /// <summary>
        /// Creates a new HTTP DELETE request.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the DELETE request.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewDeleteRequest(this HttpRequest request, string url)
        {
            request.Clear();
            request.SetBegin(MethodNames.Delete, url);
            request.SetBody();

            return request;
        }

        /// <summary>
        /// Creates a new HTTP OPTIONS request.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the OPTIONS request.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewOptionsRequest(this HttpRequest request, string url)
        {
            request.Clear();
            request.SetBegin(MethodNames.Options, url);
            request.SetBody();

            return request;
        }

        /// <summary>
        /// Creates a new HTTP TRACE request.
        /// </summary>
        /// <param name="request">The HTTP request instance to modify.</param>
        /// <param name="url">The URL for the TRACE request.</param>
        /// <returns>The modified HTTP request instance.</returns>
        public static HttpRequest NewTraceRequest(this HttpRequest request, string url)
        {
            request.Clear();
            request.SetBegin(MethodNames.Trace, url);
            request.SetBody();

            return request;
        }

    }

}

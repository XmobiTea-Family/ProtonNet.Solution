namespace XmobiTea.ProtonNet.Server.WebApi.Types
{
    /// <summary>
    /// Enum representing different types of HTTP parameters.
    /// </summary>
    enum HttpParameterType
    {
        /// <summary>
        /// Represents an HTTP request parameter.
        /// </summary>
        HttpRequest,

        /// <summary>
        /// Represents an HTTP response parameter.
        /// </summary>
        HttpResponse,

        /// <summary>
        /// Represents an HTTP session parameter.
        /// </summary>
        HttpSession,

        /// <summary>
        /// Represents a middleware context parameter.
        /// </summary>
        MiddlewareContext,

        /// <summary>
        /// Represents a delegate that can be used to call the next middleware in the pipeline.
        /// </summary>
        NextDelegate,

        /// <summary>
        /// Represents an attribute that binds the parameter from the body of the HTTP request.
        /// </summary>
        FromBodyAttribute,

        /// <summary>
        /// Represents an attribute that binds the parameter from the headers of the HTTP request.
        /// </summary>
        FromHeaderAttribute,

        /// <summary>
        /// Represents an attribute that binds the parameter from the URL path of the HTTP request.
        /// </summary>
        FromParamAttribute,

        /// <summary>
        /// Represents an attribute that binds the parameter from the query string of the HTTP request.
        /// </summary>
        FromQueryAttribute,

    }

}

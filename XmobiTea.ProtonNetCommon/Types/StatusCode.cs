namespace XmobiTea.ProtonNetCommon.Types
{
    /// <summary>
    /// This enum represents various HTTP status codes as defined by the 
    /// HTTP/1.1 standard and related extensions.
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// Indicates that the client should continue with its request.
        /// </summary>
        Continue = 100,

        /// <summary>
        /// Indicates that the server is switching protocols as requested by the client.
        /// </summary>
        SwitchingProtocols = 101,

        /// <summary>
        /// Indicates that the server has received and is processing the request, 
        /// but no response is available yet.
        /// </summary>
        Processing = 102,

        /// <summary>
        /// A response code to provide early hints of the final HTTP status.
        /// </summary>
        EarlyHints = 103,

        /// <summary>
        /// Indicates that the request has succeeded.
        /// </summary>
        OK = 200,

        /// <summary>
        /// Indicates that the request has been fulfilled and a new resource has been created.
        /// </summary>
        Created = 201,

        /// <summary>
        /// Indicates that the request has been accepted for processing, but the processing is not complete.
        /// </summary>
        Accepted = 202,

        /// <summary>
        /// Indicates that the request was successful but the information may be from a third-party source.
        /// </summary>
        NonAuthoritativeInformation = 203,

        /// <summary>
        /// Indicates that the request was successful but there is no content to send in the response.
        /// </summary>
        NoContent = 204,

        /// <summary>
        /// Indicates that the server successfully processed the request and is instructing the client to reset the view.
        /// </summary>
        ResetContent = 205,

        /// <summary>
        /// Indicates that the server successfully processed a partial GET request.
        /// </summary>
        PartialContent = 206,

        /// <summary>
        /// Indicates that the message body contains multiple status codes.
        /// </summary>
        MultiStatus = 207,

        /// <summary>
        /// Indicates that the members of a DAV binding have already been enumerated.
        /// </summary>
        AlreadyReported = 208,

        /// <summary>
        /// Indicates that the server has fulfilled the GET request for the resource and that the response is a representation of the result of one or more instance manipulations applied to the current instance.
        /// </summary>
        IMUsed = 226,

        /// <summary>
        /// Indicates that the request has more than one possible response.
        /// </summary>
        Ambiguous = 300,

        /// <summary>
        /// Indicates that the request has more than one possible response.
        /// </summary>
        MultipleChoices = 300,

        /// <summary>
        /// Indicates that the resource has been moved to a different URL permanently.
        /// </summary>
        Moved = 301,

        /// <summary>
        /// Indicates that the resource has been moved to a different URL permanently.
        /// </summary>
        MovedPermanently = 301,

        /// <summary>
        /// Indicates that the resource has been found at a different URL temporarily.
        /// </summary>
        Found = 302,

        /// <summary>
        /// Indicates that the resource has been found at a different URL temporarily.
        /// </summary>
        Redirect = 302,

        /// <summary>
        /// Indicates that the resource can be found at a different URL and that the request method should not be changed.
        /// </summary>
        RedirectMethod = 303,

        /// <summary>
        /// Indicates that the resource can be found at a different URL and that the request method should not be changed.
        /// </summary>
        SeeOther = 303,

        /// <summary>
        /// Indicates that the resource has not been modified since the last request.
        /// </summary>
        NotModified = 304,

        /// <summary>
        /// Indicates that the client must use a proxy to access the requested resource.
        /// </summary>
        UseProxy = 305,

        /// <summary>
        /// This status code is no longer used and is reserved.
        /// </summary>
        Unused = 306,

        /// <summary>
        /// Indicates that the request should be repeated with the same method, but to a different URL.
        /// </summary>
        RedirectKeepVerb = 307,

        /// <summary>
        /// Indicates that the request should be repeated with the same method, but to a different URL.
        /// </summary>
        TemporaryRedirect = 307,

        /// <summary>
        /// Indicates that the resource has been moved permanently, and future requests should use a different URL.
        /// </summary>
        PermanentRedirect = 308,

        /// <summary>
        /// Indicates that the server cannot process the request due to a client error.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Indicates that the request requires user authentication.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Indicates that payment is required to access the resource.
        /// </summary>
        PaymentRequired = 402,

        /// <summary>
        /// Indicates that the server understands the request but refuses to authorize it.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// Indicates that the server cannot find the requested resource.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Indicates that the request method is not allowed for the resource.
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// Indicates that the server cannot produce a response matching the list of acceptable values defined in the request's proactive content negotiation headers.
        /// </summary>
        NotAcceptable = 406,

        /// <summary>
        /// Indicates that the client must authenticate with the proxy before sending the request.
        /// </summary>
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// Indicates that the server timed out waiting for the request.
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// Indicates that the request could not be processed because of a conflict with the current state of the resource.
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// Indicates that the requested resource is no longer available and will not be available again.
        /// </summary>
        Gone = 410,

        /// <summary>
        /// Indicates that the request did not specify the length of its content, which is required by the requested resource.
        /// </summary>
        LengthRequired = 411,

        /// <summary>
        /// Indicates that one or more preconditions in the request header fields evaluated to false when tested on the server.
        /// </summary>
        PreconditionFailed = 412,

        /// <summary>
        /// Indicates that the request is larger than the server is willing or able to process.
        /// </summary>
        RequestEntityTooLarge = 413,

        /// <summary>
        /// Indicates that the URI requested by the client is too long to be processed by the server.
        /// </summary>
        RequestUriTooLong = 414,

        /// <summary>
        /// Indicates that the server cannot process the request because the media format of the requested data is not supported by the server.
        /// </summary>
        UnsupportedMediaType = 415,

        /// <summary>
        /// Indicates that the range specified by the Range header field in the request cannot be fulfilled.
        /// </summary>
        RequestedRangeNotSatisfiable = 416,

        /// <summary>
        /// Indicates that the server cannot meet the requirements of the Expect request-header field.
        /// </summary>
        ExpectationFailed = 417,

        /// <summary>
        /// Indicates that the server cannot process the request because it was directed to an inappropriate server.
        /// </summary>
        MisdirectedRequest = 421,

        /// <summary>
        /// Indicates that the server cannot process the request due to a semantic error in the client's request.
        /// </summary>
        UnprocessableEntity = 422,

        /// <summary>
        /// Indicates that the resource is locked.
        /// </summary>
        Locked = 423,

        /// <summary>
        /// Indicates that the request failed because it depended on another request that failed.
        /// </summary>
        FailedDependency = 424,

        /// <summary>
        /// Indicates that the client should switch to a different protocol such as TLS/1.0.
        /// </summary>
        UpgradeRequired = 426,

        /// <summary>
        /// Indicates that the origin server requires the request to be conditional.
        /// </summary>
        PreconditionRequired = 428,

        /// <summary>
        /// Indicates that the user has sent too many requests in a given amount of time.
        /// </summary>
        TooManyRequests = 429,

        /// <summary>
        /// Indicates that the server is unwilling to process the request because its header fields are too large.
        /// </summary>
        RequestHeaderFieldsTooLarge = 431,

        /// <summary>
        /// Indicates that the server is denying access to the resource as a consequence of a legal demand.
        /// </summary>
        UnavailableForLegalReasons = 451,

        /// <summary>
        /// Indicates that the server encountered an unexpected condition that prevented it from fulfilling the request.
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// Indicates that the server does not support the functionality required to fulfill the request.
        /// </summary>
        NotImplemented = 501,

        /// <summary>
        /// Indicates that the server, while acting as a gateway or proxy, received an invalid response from an inbound server it accessed while attempting to fulfill the request.
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// Indicates that the server is currently unable to handle the request due to temporary overloading or maintenance of the server.
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// Indicates that the server, while acting as a gateway or proxy, did not receive a timely response from an upstream server.
        /// </summary>
        GatewayTimeout = 504,

        /// <summary>
        /// Indicates that the server does not support the HTTP protocol version used in the request.
        /// </summary>
        HttpVersionNotSupported = 505,

        /// <summary>
        /// Indicates that the server has an internal configuration error and the request could not be completed.
        /// </summary>
        VariantAlsoNegotiates = 506,

        /// <summary>
        /// Indicates that the server is unable to store the representation needed to complete the request.
        /// </summary>
        InsufficientStorage = 507,

        /// <summary>
        /// Indicates that the server detected an infinite loop while processing the request.
        /// </summary>
        LoopDetected = 508,

        /// <summary>
        /// Indicates that further extensions to the request are required for the server to fulfill it.
        /// </summary>
        NotExtended = 510,

        /// <summary>
        /// Indicates that the client needs to authenticate to gain network access.
        /// </summary>
        NetworkAuthenticationRequired = 511,

    }

}

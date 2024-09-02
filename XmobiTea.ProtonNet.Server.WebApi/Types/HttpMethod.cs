namespace XmobiTea.ProtonNet.Server.WebApi.Types
{
    /// <summary>
    /// Enum representing the various HTTP methods.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// Represents the HTTP HEAD method, used to retrieve headers of a resource.
        /// </summary>
        HEAD,

        /// <summary>
        /// Represents the HTTP GET method, used to retrieve a resource.
        /// </summary>
        GET,

        /// <summary>
        /// Represents the HTTP POST method, used to submit data to be processed by a resource.
        /// </summary>
        POST,

        /// <summary>
        /// Represents the HTTP PUT method, used to update or create a resource.
        /// </summary>
        PUT,

        /// <summary>
        /// Represents the HTTP DELETE method, used to delete a resource.
        /// </summary>
        DELETE,

        /// <summary>
        /// Represents the HTTP OPTIONS method, used to describe the communication options for a resource.
        /// </summary>
        OPTIONS,

        /// <summary>
        /// Represents the HTTP TRACE method, used to perform a diagnostic trace of the path to the resource.
        /// </summary>
        TRACE,

    }

}

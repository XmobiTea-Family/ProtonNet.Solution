using System;
using System.IO;
using System.Reflection;
using XmobiTea.Bean;
using XmobiTea.Linq;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Server.WebApi.Controllers;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render;
using XmobiTea.ProtonNet.Server.WebApi.Exceptions;
using XmobiTea.ProtonNet.Server.WebApi.Extensions;
using XmobiTea.ProtonNet.Server.WebApi.Helper;
using XmobiTea.ProtonNet.Server.WebApi.Models;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;
using XmobiTea.ProtonNet.Server.WebApi.Types;
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;
using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.WebApi.Services
{
    /// <summary>
    /// Defines methods for managing Web API controllers, handling static content, middleware, and request routing.
    /// </summary>
    public interface IWebApiControllerService
    {
        /// <summary>
        /// Adds static content from a folder to be served by the Web API.
        /// </summary>
        /// <param name="path">The path of the folder containing static content.</param>
        /// <param name="prefix">The URL prefix for the static content. Defaults to "/".</param>
        /// <param name="filter">The file filter to apply. Defaults to "*.*".</param>
        /// <param name="timeout">An optional timeout for serving static content.</param>
        void AddStaticFolderContent(string path, string prefix = "/", string filter = "*.*", TimeSpan? timeout = null);

        /// <summary>
        /// Removes static content served from a folder.
        /// </summary>
        /// <param name="path">The path of the folder containing static content.</param>
        /// <param name="prefix">The URL prefix for the static content. Defaults to "/".</param>
        void RemoveStaticFolderContent(string path, string prefix = "/");

        /// <summary>
        /// Adds a static file to be served by the Web API.
        /// </summary>
        /// <param name="filePath">The path of the static file.</param>
        /// <param name="prefix">The URL prefix for the static file. Defaults to "/".</param>
        /// <param name="timeout">An optional timeout for serving the static file.</param>
        void AddStaticFileContent(string filePath, string prefix = "/", TimeSpan? timeout = null);

        /// <summary>
        /// Removes a static file from being served by the Web API.
        /// </summary>
        /// <param name="filePath">The path of the static file.</param>
        /// <param name="prefix">The URL prefix for the static file. Defaults to "/".</param>
        void RemoveStaticFileContent(string filePath, string prefix = "/");

        /// <summary>
        /// Setup the path for all web path
        /// </summary>
        /// <param name="path">The path.</param>
        void SetupWebsPathContent(string path);

        /// <summary>
        /// Configures middleware to be used with a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the middleware.</param>
        /// <param name="middlewareDelegate">The synchronous middleware delegate.</param>
        /// <param name="middlewareDelegateParams">Optional additional middleware delegates.</param>
        void UseMiddleware(string fullPrefix, MiddlewareDelegate middlewareDelegate, params MiddlewareDelegate[] middlewareDelegateParams);

        /// <summary>
        /// Configures asynchronous middleware to be used with a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the middleware.</param>
        /// <param name="middlewareDelegate">The asynchronous middleware delegate.</param>
        /// <param name="middlewareDelegateParams">Optional additional asynchronous middleware delegates.</param>
        void UseMiddleware(string fullPrefix, MiddlewareDelegateAsync middlewareDelegate, params MiddlewareDelegateAsync[] middlewareDelegateParams);

        /// <summary>
        /// Configures a handler for GET requests at a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the GET handler.</param>
        /// <param name="getDelegate">The synchronous GET handler delegate.</param>
        void Get(string fullPrefix, GetDelegate getDelegate);

        /// <summary>
        /// Configures an asynchronous handler for GET requests at a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the GET handler.</param>
        /// <param name="getDelegate">The asynchronous GET handler delegate.</param>
        void Get(string fullPrefix, GetDelegateAsync getDelegate);

        /// <summary>
        /// Configures a handler for POST requests at a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the POST handler.</param>
        /// <param name="postDelegate">The synchronous POST handler delegate.</param>
        void Post(string fullPrefix, PostDelegate postDelegate);

        /// <summary>
        /// Configures an asynchronous handler for POST requests at a specific URL prefix.
        /// </summary>
        /// <param name="fullPrefix">The URL prefix for the POST handler.</param>
        /// <param name="postDelegate">The asynchronous POST handler delegate.</param>
        void Post(string fullPrefix, PostDelegateAsync postDelegate);

        /// <summary>
        /// Handles a new connection to the Web API server.
        /// </summary>
        /// <param name="session">The session representing the connection.</param>
        void OnConnected(IWebApiSession session);

        /// <summary>
        /// Handles a received HTTP request.
        /// </summary>
        /// <param name="session">The session representing the connection.</param>
        /// <param name="request">The received HTTP request.</param>
        void OnReceived(IWebApiSession session, ProtonNetCommon.HttpRequest request);

        /// <summary>
        /// Handles an error encountered while processing a request.
        /// </summary>
        /// <param name="session">The session representing the connection.</param>
        /// <param name="request">The request that caused the error.</param>
        /// <param name="error">The error message.</param>
        void OnReceivedRequestError(IWebApiSession session, ProtonNetCommon.HttpRequest request, string error);

        /// <summary>
        /// Handles a disconnection from the Web API server.
        /// </summary>
        /// <param name="session">The session representing the disconnection.</param>
        void OnDisconnected(IWebApiSession session);

        /// <summary>
        /// Handles a socket error encountered during communication.
        /// </summary>
        /// <param name="session">The session representing the connection.</param>
        /// <param name="error">The socket error encountered.</param>
        void OnError(IWebApiSession session, System.Net.Sockets.SocketError error);

    }

    class WebApiControllerService : IWebApiControllerService
    {
        /// <summary>
        /// Represents the amount of requests received per second in a session.
        /// </summary>
        class SessionPerSecondAmount
        {
            /// <summary>
            /// The timestamp of the last recorded tick.
            /// </summary>
            public long LastTick;

            /// <summary>
            /// The number of requests received in the current second.
            /// </summary>
            public int AmountInCurrentSecond;

            /// <summary>
            /// The number of pending requests.
            /// </summary>
            public int PendingRequest;
        }

        /// <summary>
        /// Logger instance for logging messages.
        /// </summary>
        private ILogger logger { get; }

        /// <summary>
        /// List of WebApiController instances managed by the server.
        /// </summary>
        private System.Collections.Generic.IList<WebApiController> webApiControllerLst { get; }

        /// <summary>
        /// Dictionary tracking the amount of requests received per session per second.
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<IWebApiSession, SessionPerSecondAmount> sessionReceiveAtTimeAmountDict { get; }

        /// <summary>
        /// Controller responsible for handling GET method requests.
        /// </summary>
        private MethodController getMethodController { get; }

        /// <summary>
        /// Controller responsible for handling POST method requests.
        /// </summary>
        private MethodController postMethodController { get; }

        /// <summary>
        /// Controller responsible for handling middleware requests.
        /// </summary>
        private MethodController middlewareMethodController { get; }

        /// <summary>
        /// Maximum number of pending requests allowed.
        /// </summary>
        private int maxPendingRequest { get; set; }

        /// <summary>
        /// Maximum number of requests allowed per session per second.
        /// </summary>
        private int maxSessionRequestPerSecond { get; set; }

        /// <summary>
        /// Maximum number of pending requests allowed per session.
        /// </summary>
        private int maxSessionPendingRequest { get; set; }

        /// <summary>
        /// The current number of pending requests.
        /// </summary>
        private int pendingRequest;

        /// <summary>
        /// Cache for static content used by the server.
        /// </summary>
        private IStaticCache staticCache { get; }

        /// <summary>
        /// Fiber used for operations other than request handling.
        /// </summary>
        private IFiber otherFiber { get; set; }

        /// <summary>
        /// Fiber used for handling received requests.
        /// </summary>
        private IFiber receivedFiber { get; set; }

        /// <summary>
        /// The view engine to render View()
        /// </summary>
        private IViewEngine viewEngine { get; }

        /// <summary>
        /// The bean context
        /// </summary>
        private IBeanContext beanContext { get; }

        /// <summary>
        /// Initializes a new instance of the WebApiControllerService class.
        /// </summary>
        /// <remarks>
        /// Sets up the logger, method controllers, and initializes collections for WebApiControllers and session data.
        /// </remarks>
        public WebApiControllerService(IBeanContext beanContext)
        {
            this.logger = LogManager.GetLogger(this);

            this.beanContext = beanContext;

            this.getMethodController = new MethodController(string.Empty, string.Empty);
            this.postMethodController = new MethodController(string.Empty, string.Empty);
            this.middlewareMethodController = new MethodController(string.Empty, string.Empty);

            this.webApiControllerLst = new System.Collections.Generic.List<WebApiController>();
            this.sessionReceiveAtTimeAmountDict = new System.Collections.Concurrent.ConcurrentDictionary<IWebApiSession, SessionPerSecondAmount>();

            this.staticCache = new StaticCache();

            this.viewEngine = (IViewEngine)this.beanContext.CreateSingleton(typeof(ViewEngine));

        }

        /// <summary>
        /// Sets up a fiber for handling other tasks with a specified number of threads.
        /// </summary>
        /// <param name="threadCount">The number of threads to be used by the fiber.</param>
        internal void SetOtherFiber(int threadCount)
        {
            var roundRobinFiber = new RoundRobinFiber("OtherFiber", threadCount);
            roundRobinFiber.Start();

            this.otherFiber = roundRobinFiber;
        }

        /// <summary>
        /// Sets up a fiber for handling received requests with a specified number of threads.
        /// </summary>
        /// <param name="threadCount">The number of threads to be used by the fiber.</param>
        internal void SetReceiveFiber(int threadCount)
        {
            var roundRobinFiber = new RoundRobinFiber("ReceivedFiber", threadCount);
            roundRobinFiber.Start();

            this.receivedFiber = roundRobinFiber;
        }

        /// <summary>
        /// Sets the maximum number of pending requests allowed.
        /// </summary>
        /// <param name="maxPendingRequest">The maximum number of pending requests.</param>
        internal void SetMaxPendingRequest(int maxPendingRequest) => this.maxPendingRequest = maxPendingRequest;

        /// <summary>
        /// Sets the maximum number of requests allowed per session per second.
        /// </summary>
        /// <param name="maxSessionRequestPerSecond">The maximum number of requests allowed per session per second.</param>
        internal void SetMaxSessionRequestPerSecond(int maxSessionRequestPerSecond) => this.maxSessionRequestPerSecond = maxSessionRequestPerSecond;

        /// <summary>
        /// Sets the maximum number of pending requests allowed per session.
        /// </summary>
        /// <param name="maxPendingRequestPerSession">The maximum number of pending requests allowed per session.</param>
        internal void SetMaxSessionPendingRequest(int maxPendingRequestPerSession) => this.maxSessionPendingRequest = maxPendingRequestPerSession;

        /// <summary>
        /// Adds a WebApiController to the list and registers its HTTP methods.
        /// </summary>
        /// <param name="webApiController">The WebApiController instance to add.</param>
        internal void AddWebApiController(WebApiController webApiController)
        {
            this.webApiControllerLst.Add(webApiController);

            var apiControllerType = webApiController.GetType();

            var routeAttribute = (RouteAttribute)apiControllerType.GetCustomAttribute(typeof(RouteAttribute), true);

            var httpMethods = apiControllerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                                .Where(x => x.GetCustomAttribute<HttpMethodAttribute>(true) != null);

            foreach (var methodInfo in httpMethods)
            {
                var httpMethodAttributes = methodInfo.GetCustomAttributes<HttpMethodAttribute>(true);

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    var httpMethodType =
                        httpMethodAttribute is HttpMiddlewareAttribute ? FunctionType.Middleware
                        : httpMethodAttribute is HttpGetAttribute ? FunctionType.Get
                        : FunctionType.Post;

                    this.AddMethodControllerFor(webApiController, methodInfo, DetectUrlUtils.GetSinglePrefixLst(httpMethodAttribute, routeAttribute), httpMethodType);
                }
            }
        }

        /// <summary>
        /// Adds static folder content to the server with optional caching.
        /// </summary>
        /// <param name="path">The path to the folder.</param>
        /// <param name="prefix">The URL prefix for the folder content.</param>
        /// <param name="filter">The filter for the files to include.</param>
        /// <param name="timeout">The optional cache timeout duration.</param>
        public void AddStaticFolderContent(string path, string prefix = "/", string filter = "*.*", TimeSpan? timeout = null)
        {
            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            timeout = timeout ?? TimeSpan.FromHours(1);

            this.staticCache.AddFolder(prefix, path, filter, true, (key, subPrefix, filePath, fileBuffer) =>
            {
                var response = new HttpResponse();

                response.SetBegin(StatusCode.OK);
                response.SetContentType(Path.GetExtension(key));
                response.SetHeader(ProtonNetCommon.Types.HeaderNames.CacheControl, $"max-age={timeout.Value.Seconds}");
                response.SetBody(fileBuffer);

                return response.Cache.Buffer;
            });
        }

        /// <summary>
        /// Removes static folder content from the server.
        /// </summary>
        /// <param name="path">The path to the folder.</param>
        /// <param name="prefix">The URL prefix for the folder content.</param>
        public void RemoveStaticFolderContent(string path, string prefix = "/")
        {
            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            this.staticCache.RemoveFolder(prefix, path);
        }

        /// <summary>
        /// Adds static file content to the server with optional caching.
        /// </summary>
        /// <param name="filePath">The path to the static file.</param>
        /// <param name="prefix">The URL prefix for the file content.</param>
        /// <param name="timeout">The optional cache timeout duration.</param>
        public void AddStaticFileContent(string filePath, string prefix = "/", TimeSpan? timeout = null)
        {
            filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            timeout = timeout ?? TimeSpan.FromHours(1);

            byte[] GetBufferFileHandler(string key, string prefix1, string filePath1, byte[] fileBuffer)
            {
                var response = new HttpResponse();

                response.SetBegin(StatusCode.OK);
                response.SetContentType(Path.GetExtension(key));
                response.SetHeader(ProtonNetCommon.Types.HeaderNames.CacheControl, $"max-age={timeout.Value.Seconds}");
                response.SetBody(fileBuffer);

                return response.Cache.Buffer;
            }

            this.staticCache.AddFile(prefix, filePath, true, GetBufferFileHandler);
        }

        /// <summary>
        /// Removes a static file from the cache based on the provided file path and prefix.
        /// </summary>
        /// <param name="filePath">The path of the file to be removed.</param>
        /// <param name="prefix">The URL prefix to be associated with the file.</param>
        public void RemoveStaticFileContent(string filePath, string prefix = "/")
        {
            filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            this.staticCache.RemoveFile(prefix, filePath);
        }

        /// <summary>
        /// Setup the webs path
        /// </summary>
        /// <param name="websPath">The webs path.</param>
        public void SetupWebsPathContent(string websPath) => this.viewEngine.SetupWebsPath(websPath);

        /// <summary>
        /// Adds a method controller for a given method in a Web API controller, associating it with specified prefixes and HTTP method types.
        /// </summary>
        /// <param name="originObject">The instance of the object that contains the method.</param>
        /// <param name="methodInfo">The MethodInfo object representing the method to be added.</param>
        /// <param name="singlePrefixLst">A list of URL prefixes associated with the method.</param>
        /// <param name="httpMethodType">The type of HTTP method (e.g., GET, POST, Middleware) associated with the method.</param>
        /// <exception cref="MethodControllerInvalidException">Thrown when the method return type is invalid.</exception>
        /// <exception cref="MethodParameterControllerInvalidException">Thrown when method parameters are invalid for the specified HTTP method type.</exception>
        /// <remarks>
        /// This method validates the method return type and parameters, creating or updating method controllers as necessary.
        /// </remarks>
        internal void AddMethodControllerFor(object originObject, MethodInfo methodInfo, System.Collections.Generic.List<string> singlePrefixLst, FunctionType httpMethodType)
        {
            if (methodInfo.ReturnType != typeof(System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse>) && methodInfo.ReturnType != typeof(ProtonNetCommon.HttpResponse))
                throw new MethodControllerInvalidException("Return type of " + methodInfo.ToString() + " must is type Task<XmobiTea.ProtonServer.HttpResponse> or XmobiTea.ProtonServer.HttpResponse");

            var isMiddlewareAttribute = httpMethodType == FunctionType.Middleware;

            var methodControllerDict =
                isMiddlewareAttribute ? this.middlewareMethodController.methodControllerDict
                : httpMethodType == FunctionType.Get ? this.getMethodController.methodControllerDict
                : httpMethodType == FunctionType.Post ? this.postMethodController.methodControllerDict
                : null;

            var prefix = string.Empty;

            for (var i = 0; i < singlePrefixLst.Count; i++)
            {
                var singlePrefix = singlePrefixLst[i];

                prefix += "/" + singlePrefix;

                if (!methodControllerDict.ContainsKey(singlePrefix))
                {
                    methodControllerDict[singlePrefix] = new MethodController(singlePrefix, prefix)
                    {
                        isPrefixWithParam = DetectUrlUtils.IsParamPrefix(singlePrefix),
                    };
                }

                var methodController = methodControllerDict[singlePrefix];

                if (i == singlePrefixLst.Count - 1)
                {
                    var parameterInfos = methodInfo.GetParameters();
                    var tryGenerateParameterDatas = new TryGenerateParameterDataDelegate[parameterInfos.Length];

                    var methodInformation = new MethodInformation()
                    {
                        methodInfo = methodInfo,
                        parameterInfos = parameterInfos,
                        tryGenerateParameterDatas = tryGenerateParameterDatas,
                        originObject = originObject,
                    };

                    if (httpMethodType != FunctionType.Middleware)
                    {
                        if (methodController.methodInformationLst.Count != 0)
                        {
                            throw new MethodControllerInvalidException("Can not use multiple Function for one prefix path.");
                        }
                    }

                    methodController.methodInformationLst.Add(methodInformation);

                    for (var j = 0; j < parameterInfos.Length; j++)
                    {
                        var parameterInfo = parameterInfos[j];

                        if (j == parameterInfos.Length - 1 && isMiddlewareAttribute)
                        {
                            if (parameterInfo.ParameterType != typeof(NextDelegate))
                            {
                                throw new MethodParameterControllerInvalidException("missing MiddlewareNextFunction [HttpMiddleware].");
                            }
                        }
                        else if (parameterInfo.ParameterType == typeof(ProtonNetCommon.HttpRequest)) tryGenerateParameterDatas[j] = this.TryGenerateHttpRequestParameterData;
                        else if (parameterInfo.ParameterType == typeof(ProtonNetCommon.HttpResponse)) tryGenerateParameterDatas[j] = this.TryGenerateHttpResponseParameterData;
                        else if (parameterInfo.ParameterType == typeof(IWebApiSession)) tryGenerateParameterDatas[j] = this.TryGenerateSessionParameterData;
                        else if (parameterInfo.ParameterType == typeof(MiddlewareContext)) tryGenerateParameterDatas[j] = this.TryGenerateMiddlewareContextParameterData;
                        else if (parameterInfo.ParameterType == typeof(NextDelegate))
                        {
                            if (isMiddlewareAttribute)
                                throw new MethodParameterControllerInvalidException("The last parameter [HttpMiddleware] must is MiddlewareNextFunction.");

                            throw new MethodParameterControllerInvalidException("MiddlewareNextFunction only use on [HttpMiddleware].");
                        }
                        else
                        {
                            var fromHttpParamAttribute = parameterInfo.GetCustomAttribute<FromHttpParamAttribute>();

                            if (fromHttpParamAttribute == null) throw new MethodParameterControllerInvalidException("Are you missing HttpParam?");
                            else
                            {
                                if (fromHttpParamAttribute is FromBodyBytesAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromBodyBytesParameterData;
                                else if (fromHttpParamAttribute is FromHeaderAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromHeaderParameterData;
                                else if (fromHttpParamAttribute is FromParamAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromParamParameterData;
                                else if (fromHttpParamAttribute is FromQueryAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromQueryParameterData;
                                else if (fromHttpParamAttribute is FromMiddlewareContextAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromMiddlewareContextParameterData;
                                else if (fromHttpParamAttribute is FromAutoBindAttribute) tryGenerateParameterDatas[j] = this.TryGenerateFromAutoBindParameterData;
                            }
                        }
                    }
                }
                else
                    methodControllerDict = methodController.methodControllerDict;
            }
        }

        /// <summary>
        /// Tries to generate an HTTP request parameter data from the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request information.</param>
        /// <param name="obj">The output object representing the HTTP request.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateHttpRequestParameterData(GenerateParameterData parameterData, out object obj)
        {
            obj = parameterData.HttpRequest;

            return true;
        }

        /// <summary>
        /// Tries to generate an HTTP response parameter data from the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP response information.</param>
        /// <param name="obj">The output object representing the HTTP response.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateHttpResponseParameterData(GenerateParameterData parameterData, out object obj)
        {
            obj = parameterData.HttpResponse;

            return true;
        }

        /// <summary>
        /// Tries to generate a session parameter data from the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes session information.</param>
        /// <param name="obj">The output object representing the session.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateSessionParameterData(GenerateParameterData parameterData, out object obj)
        {
            obj = parameterData.Session;

            return true;
        }

        /// <summary>
        /// Tries to generate middleware context parameter data from the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes middleware context information.</param>
        /// <param name="obj">The output object representing the middleware context.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateMiddlewareContextParameterData(GenerateParameterData parameterData, out object obj)
        {
            obj = parameterData.MiddlewareContext;

            return true;
        }

        /// <summary>
        /// Tries to generate parameter data from the body of an HTTP request, based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request information.</param>
        /// <param name="obj">The output object representing the body bytes of the HTTP request.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromBodyBytesParameterData(GenerateParameterData parameterData, out object obj)
        {
            obj = parameterData.HttpRequest.GetBodyBytes();

            return true;
        }

        /// <summary>
        /// Tries to generate parameter data from the body of an HTTP request, based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request information.</param>
        /// <param name="obj">The output object representing the body bytes of the HTTP request.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromAutoBindParameterData(GenerateParameterData parameterData, out object obj)
        {
            var fromAutoBindAttribute = parameterData.FromHttpParam as FromAutoBindAttribute;

            var value = fromAutoBindAttribute.Type != null ? this.beanContext.GetSingleton(fromAutoBindAttribute.Type) : this.beanContext.GetSingleton(parameterData.ParameterType);
            if (value == null)
            {
                obj = null;

                if (!parameterData.FromHttpParam.IsOptional)
                {
                    return false;
                }

                return true;
            }
            else
            {
                obj = value;

                return true;
            }
        }

        /// <summary>
        /// Tries to generate parameter data from HTTP headers based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request and header information.</param>
        /// <param name="obj">The output object representing the HTTP header value.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromHeaderParameterData(GenerateParameterData parameterData, out object obj)
        {
            var name = parameterData.FromHttpParam.Name;

            object headerValue = null;

            if (parameterData.ParameterType == typeof(System.Collections.Generic.ICollection<>))
            {
                var parameterType = parameterData.ParameterType;

                if (parameterType.IsArray) headerValue = parameterData.HttpRequest.GetHeaderArray(name, parameterData.ParameterType.GetElementType());
                else headerValue = parameterData.HttpRequest.GetHeaderArray(name, parameterData.ParameterType.GetGenericArguments()[0]);
            }
            else
                headerValue = parameterData.HttpRequest.GetHeader(name, parameterData.ParameterType);

            if (headerValue == null)
            {
                obj = headerValue;

                if (!parameterData.FromHttpParam.IsOptional)
                {
                    return false;
                }

                return true;
            }
            else
            {
                obj = System.Convert.ChangeType(headerValue, parameterData.ParameterType);
                return true;
            }
        }

        /// <summary>
        /// Tries to generate parameter data from URL parameters based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request and parameter information.</param>
        /// <param name="obj">The output object representing the URL parameter value.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromParamParameterData(GenerateParameterData parameterData, out object obj)
        {
            var name = parameterData.FromHttpParam.Name;
            var paramValue = parameterData.HttpRequest.GetParam(name, parameterData.MethodController.fullPrefix, parameterData.ParameterType);

            if (paramValue == null)
            {
                obj = paramValue;

                if (!parameterData.FromHttpParam.IsOptional)
                {
                    return false;
                }

                return true;
            }
            else
            {
                obj = paramValue;
                return true;
            }
        }

        /// <summary>
        /// Tries to generate parameter data from query string parameters based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes query string and parameter information.</param>
        /// <param name="obj">The output object representing the query parameter value.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromQueryParameterData(GenerateParameterData parameterData, out object obj)
        {
            var name = parameterData.FromHttpParam.Name;

            var queryItem = parameterData.QueryLst.Find(x => x.Key == name);

            if (queryItem == null)
            {
                obj = null;

                if (!parameterData.FromHttpParam.IsOptional)
                {
                    return false;
                }

                return true;
            }
            else
            {
                obj = System.Convert.ChangeType(queryItem.Value, parameterData.ParameterType);

                return true;
            }
        }

        /// <summary>
        /// Tries to generate parameter data from middleware context based on the provided GenerateParameterData.
        /// </summary>
        /// <param name="parameterData">The parameter data that includes HTTP request and middleware context information.</param>
        /// <param name="obj">The output object representing the middleware context value.</param>
        /// <returns>True if the parameter data was successfully generated; otherwise, false.</returns>
        private bool TryGenerateFromMiddlewareContextParameterData(GenerateParameterData parameterData, out object obj)
        {
            var name = parameterData.FromHttpParam.Name;

            var middlewareValue = parameterData.HttpRequest.GetMiddleware(name, parameterData.MiddlewareContext, parameterData.ParameterType);

            if (middlewareValue == null)
            {
                obj = null;

                if (!parameterData.FromHttpParam.IsOptional)
                {
                    return false;
                }

                return true;
            }
            else
            {
                obj = middlewareValue;

                return true;
            }
        }

        /// <summary>
        /// Registers middleware delegates to be used for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the middleware should be applied.</param>
        /// <param name="middlewareDelegate">The primary middleware delegate to register.</param>
        /// <param name="middlewareDelegateParams">Additional middleware delegates to register.</param>
        /// <remarks>Each middleware delegate will be registered for the provided URL prefix.</remarks>
        public void UseMiddleware(string fullPrefix, MiddlewareDelegate middlewareDelegate, params MiddlewareDelegate[] middlewareDelegateParams)
        {
            this.AddMethodControllerFor(middlewareDelegate.Target, middlewareDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Middleware);

            for (var i = 0; i < middlewareDelegateParams.Length; i++)
            {
                var thisMiddlewareDelegate = middlewareDelegateParams[i];

                this.AddMethodControllerFor(thisMiddlewareDelegate.Target, thisMiddlewareDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Middleware);
            }
        }

        /// <summary>
        /// Registers asynchronous middleware delegates to be used for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the middleware should be applied.</param>
        /// <param name="middlewareDelegate">The primary asynchronous middleware delegate to register.</param>
        /// <param name="middlewareDelegateParams">Additional asynchronous middleware delegates to register.</param>
        /// <remarks>Each asynchronous middleware delegate will be registered for the provided URL prefix.</remarks>
        public void UseMiddleware(string fullPrefix, MiddlewareDelegateAsync middlewareDelegate, params MiddlewareDelegateAsync[] middlewareDelegateParams)
        {
            this.AddMethodControllerFor(middlewareDelegate.Target, middlewareDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Middleware);

            for (var i = 0; i < middlewareDelegateParams.Length; i++)
            {
                var thisMiddlewareDelegate = middlewareDelegateParams[i];

                this.AddMethodControllerFor(thisMiddlewareDelegate.Target, thisMiddlewareDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Middleware);
            }
        }

        /// <summary>
        /// Registers a GET request handler for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the GET request handler should be applied.</param>
        /// <param name="getDelegate">The delegate to handle GET requests.</param>
        /// <remarks>The GET request handler will be registered for the provided URL prefix.</remarks>
        public void Get(string fullPrefix, GetDelegate getDelegate)
        {
            this.AddMethodControllerFor(getDelegate.Target, getDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Get);
        }

        /// <summary>
        /// Registers an asynchronous GET request handler for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the asynchronous GET request handler should be applied.</param>
        /// <param name="getDelegate">The asynchronous delegate to handle GET requests.</param>
        /// <remarks>The asynchronous GET request handler will be registered for the provided URL prefix.</remarks>
        public void Get(string fullPrefix, GetDelegateAsync getDelegate)
        {
            this.AddMethodControllerFor(getDelegate.Target, getDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Get);
        }

        /// <summary>
        /// Registers a POST request handler for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the POST request handler should be applied.</param>
        /// <param name="postDelegate">The delegate to handle POST requests.</param>
        /// <remarks>The POST request handler will be registered for the provided URL prefix.</remarks>
        public void Post(string fullPrefix, PostDelegate postDelegate)
        {
            this.AddMethodControllerFor(postDelegate.Target, postDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Post);
        }

        /// <summary>
        /// Registers an asynchronous POST request handler for the specified prefix.
        /// </summary>
        /// <param name="fullPrefix">The full URL prefix to which the asynchronous POST request handler should be applied.</param>
        /// <param name="postDelegate">The asynchronous delegate to handle POST requests.</param>
        /// <remarks>The asynchronous POST request handler will be registered for the provided URL prefix.</remarks>
        public void Post(string fullPrefix, PostDelegateAsync postDelegate)
        {
            this.AddMethodControllerFor(postDelegate.Target, postDelegate.Method, DetectUrlUtils.GetSinglePrefixLst(fullPrefix), FunctionType.Post);
        }

        /// <summary>
        /// Handles incoming HTTP requests, checking for rate limits and processing the request.
        /// </summary>
        /// <param name="session">The session that received the request.</param>
        /// <param name="request">The HTTP request to process.</param>
        /// <remarks>Checks for pending requests and session-specific rate limits. If limits are exceeded, appropriate responses are sent. Otherwise, the request is processed and handled by the appropriate method controller.</remarks>
        public void OnReceived(IWebApiSession session, ProtonNetCommon.HttpRequest request)
        {
            // check current pending request 
            var pendingRequest = System.Threading.Interlocked.Increment(ref this.pendingRequest);
            if (pendingRequest > this.maxPendingRequest)
            {
                session.SendResponseAsync(new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.TooManyRequests));

                System.Threading.Interlocked.Decrement(ref this.pendingRequest);

                this.logger.Warn($"buffer drop because max pending request, current: {pendingRequest}, maxPendingRequest: {pendingRequest}");
                return;
            }

            // check current session info
            if (!this.sessionReceiveAtTimeAmountDict.TryGetValue(session, out var sessionPerSecondAmount))
            {
                sessionPerSecondAmount = new SessionPerSecondAmount();
                this.sessionReceiveAtTimeAmountDict[session] = sessionPerSecondAmount;
            }

            // at one second, check how much amountIncurrentSecond this session has sent?
            if (sessionPerSecondAmount.LastTick < System.DateTime.UtcNow.Ticks)
            {
                System.Threading.Interlocked.Exchange(ref sessionPerSecondAmount.AmountInCurrentSecond, 0);
                System.Threading.Interlocked.Exchange(ref sessionPerSecondAmount.LastTick, System.DateTime.UtcNow.Ticks + 10000000);
            }

            var sessionAmountInCurrentSecond = System.Threading.Interlocked.Increment(ref sessionPerSecondAmount.AmountInCurrentSecond);
            if (sessionAmountInCurrentSecond > this.maxSessionRequestPerSecond)
            {
                session.SendResponseAsync(new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.TooManyRequests));

                this.logger.Warn($"buffer drop because max request per session per second, current: {sessionAmountInCurrentSecond}, maxSessionRequestPerSecond: {this.maxSessionRequestPerSecond}");
                return;
            }

            // check session pending request
            var sessionPendingRequest = System.Threading.Interlocked.Increment(ref sessionPerSecondAmount.PendingRequest);
            if (sessionPendingRequest > this.maxSessionPendingRequest)
            {
                sessionPendingRequest = System.Threading.Interlocked.Decrement(ref sessionPerSecondAmount.PendingRequest);

                session.SendResponseAsync(new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.TooManyRequests));

                this.logger.Warn($"buffer drop because max pending request, current: {sessionPendingRequest}, maxSessionPendingRequest: {this.maxSessionPendingRequest}");
                return;
            }

            this.receivedFiber.Enqueue(() =>
            {
                foreach (var webApiController in this.webApiControllerLst)
                {
                    webApiController.OnReceived(session, request);
                }

                var method = request.GetMethod();

                if (method == HttpMethod.GET)
                {
                    var index = request.Url.IndexOf(SpecialChars.QuestionMark);
                    if (this.staticCache.TryGet((index < 0) ? request.Url : request.Url.Substring(0, index), out var buffer))
                    {
                        session.SendAsync(buffer);
                        return;
                    }
                }

                MethodController localMethodController = null;

                if (method == HttpMethod.GET) localMethodController = this.getMethodController;
                else if (method == HttpMethod.POST) localMethodController = this.postMethodController;

                if (localMethodController == null)
                {
                    session.SendResponseAsync(new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.NotFound));

                    System.Threading.Interlocked.Decrement(ref pendingRequest);
                    System.Threading.Interlocked.Decrement(ref sessionPerSecondAmount.PendingRequest);
                }
                else
                    this.InvokeFunction(session, request, localMethodController);
            });
        }

        /// <summary>
        /// Invokes the appropriate method based on the request and sends the response back to the session.
        /// </summary>
        /// <param name="session">The session that received the request.</param>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="localMethodController">The method controller to use for handling the request.</param>
        /// <remarks>Identifies the correct method to invoke based on the request URL and method controller. Handles middleware and method invocation, and sends the response back to the session.</remarks>
        private async void InvokeFunction(IWebApiSession session, ProtonNetCommon.HttpRequest request, MethodController localMethodController)
        {
            var url = System.Uri.UnescapeDataString(request.Url);

            DetectUrlUtils.Detect(url, out var singlePrefixLst, out var queryLst);

            ProtonNetCommon.HttpResponse response = null;

            var methodControllerLst = MethodControllerUtils.GetChildMethodControllerLst(localMethodController, singlePrefixLst);

            var methodController = methodControllerLst.Find(x => x.methodInformationLst.Count != 0);

            if (methodController == null)
            {
                response = new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.NotFound);
            }
            else
            {
                var middlewareMethodControllerLst = MethodControllerUtils.GetMiddlewareChildMethodControllerLst(this.middlewareMethodController, singlePrefixLst).Where(x => x.methodInformationLst.Count != 0);

                response = await this.InvokeFunctionInternal(session, request, middlewareMethodControllerLst, singlePrefixLst, queryLst, methodController).ConfigureAwait(false);
            }

            if (response != null) session.SendResponseAsync(response);

            System.Threading.Interlocked.Decrement(ref this.pendingRequest);

            if (this.sessionReceiveAtTimeAmountDict.TryGetValue(session, out var sessionPerSecondAmount))
                System.Threading.Interlocked.Decrement(ref sessionPerSecondAmount.PendingRequest);
        }

        /// <summary>
        /// Internal method to invoke a function with the provided parameters and method controllers.
        /// </summary>
        /// <param name="session">The session that received the request.</param>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="methodControllers">The list of method controllers to use.</param>
        /// <param name="singlePrefixLst">The list of URL prefixes.</param>
        /// <param name="queryLst">The list of query parameters.</param>
        /// <param name="methodController">The main method controller to use.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response.</returns>
        /// <remarks>Handles parameter extraction and method invocation, including middleware invocation if necessary. Catches and logs exceptions, and returns appropriate HTTP responses.</remarks>
        private async System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse> InvokeFunctionInternal(IWebApiSession session, ProtonNetCommon.HttpRequest request, System.Collections.Generic.IEnumerable<MethodController> methodControllers, System.Collections.Generic.List<string> singlePrefixLst, System.Collections.Generic.List<QueryItem> queryLst, MethodController methodController)
        {
            ProtonNetCommon.HttpResponse response = null;

            var middlewareContext = new MiddlewareContext();

            var methodInvokeInformationLst = this.GetMethodInvokeInformationLst(methodControllers, methodController);

            var currentMethodInvokeInformation = methodInvokeInformationLst[0];

            if (!TryGetParameterObjs(currentMethodInvokeInformation, 0, out var parameterObjs1))
            {
                response = new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.BadRequest);
            }
            else
            {
                try
                {
                    if (currentMethodInvokeInformation.methodInfo.ReturnType == typeof(System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse>))
                        response = await (System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse>)currentMethodInvokeInformation.methodInfo.Invoke(currentMethodInvokeInformation.originObject, parameterObjs1);
                    else
                        response = (ProtonNetCommon.HttpResponse)currentMethodInvokeInformation.methodInfo.Invoke(currentMethodInvokeInformation.originObject, parameterObjs1);
                }
                catch (System.Exception ex)
                {
                    this.logger.Error("ControllerService", ex);

                    response = new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.InternalServerError);
                }
            }

            bool TryGetParameterObjs(MethodInvokeInformation methodInvokeInformation, int id, out object[] parameterObjs)
            {
                var parameterInfos = methodInvokeInformation.parameterInfos;
                var tryGenerateParameterDatas = methodInvokeInformation.tryGenerateParameterDatas;

                parameterObjs = new object[parameterInfos.Length];

                var validMethod = true;

                for (var i = 0; i < tryGenerateParameterDatas.Length; i++)
                {
                    var parameterInfo = parameterInfos[i];
                    if (parameterInfo.ParameterType == typeof(NextDelegate))
                    {
                        var nextMethodInvokeInformation = methodInvokeInformationLst[id + 1];

                        NextDelegate thisNextFunction = () =>
                        {
                            if (!TryGetParameterObjs(nextMethodInvokeInformation, id + 1, out var nextParameterObjs))
                            {
                                return new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.BadRequest);
                            }

                            try
                            {
                                if (nextMethodInvokeInformation.methodInfo.ReturnType == typeof(System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse>))
                                    return ((System.Threading.Tasks.Task<ProtonNetCommon.HttpResponse>)nextMethodInvokeInformation.methodInfo.Invoke(nextMethodInvokeInformation.originObject, nextParameterObjs)).Result;
                                else
                                    return (ProtonNetCommon.HttpResponse)nextMethodInvokeInformation.methodInfo.Invoke(nextMethodInvokeInformation.originObject, nextParameterObjs);
                            }
                            catch (System.Exception ex)
                            {
                                this.logger.Error("ControllerService", ex);

                                return new ProtonNetCommon.HttpResponse().MakeErrorResponse(StatusCode.InternalServerError);
                            }
                        };

                        parameterObjs[i] = thisNextFunction;
                    }
                    else
                    {
                        var tryGenerateParameterData = tryGenerateParameterDatas[i];

                        if (!tryGenerateParameterData(new GenerateParameterData()
                        {
                            HttpRequest = request,
                            HttpResponse = response,
                            Session = session,
                            MiddlewareContext = middlewareContext,
                            FromHttpParam = parameterInfo.GetCustomAttribute<FromHttpParamAttribute>(),
                            ParameterType = parameterInfo.ParameterType,
                            MethodController = methodController,
                            QueryLst = queryLst,
                        }, out var obj))
                        {
                            validMethod = false;

                            break;
                        }

                        parameterObjs[i] = obj;
                    }
                }

                return validMethod;
            }

            return response;
        }

        /// <summary>
        /// Retrieves a list of method invoke information from the provided method controllers.
        /// </summary>
        /// <param name="methodControllers">The list of method controllers to retrieve information from.</param>
        /// <param name="methodController">The main method controller to include in the list.</param>
        /// <returns>A list of method invoke information.</returns>
        /// <remarks>Aggregates method invoke information from all provided method controllers, including the main method controller.</remarks>
        private System.Collections.Generic.IList<MethodInvokeInformation> GetMethodInvokeInformationLst(System.Collections.Generic.IEnumerable<MethodController> methodControllers, MethodController methodController)
        {
            var answer = new System.Collections.Generic.List<MethodInvokeInformation>();

            foreach (var thisMethodController in methodControllers)
            {
                for (var j = 0; j < thisMethodController.methodInformationLst.Count; j++)
                {
                    var thisMethodInformation = thisMethodController.methodInformationLst[j];

                    answer.Add(new MethodInvokeInformation()
                    {
                        fullPrefix = thisMethodController.fullPrefix,
                        originObject = thisMethodInformation.originObject,
                        methodInfo = thisMethodInformation.methodInfo,
                        parameterInfos = thisMethodInformation.parameterInfos,
                        tryGenerateParameterDatas = thisMethodInformation.tryGenerateParameterDatas,
                    });
                }
            }

            {
                var thisMethodInformation = methodController.methodInformationLst[0];

                answer.Add(new MethodInvokeInformation()
                {
                    fullPrefix = methodController.fullPrefix,
                    originObject = thisMethodInformation.originObject,
                    methodInfo = thisMethodInformation.methodInfo,
                    parameterInfos = thisMethodInformation.parameterInfos,
                    tryGenerateParameterDatas = thisMethodInformation.tryGenerateParameterDatas,
                });
            }

            return answer;
        }

        /// <summary>
        /// Handles a new connection to the server.
        /// </summary>
        /// <param name="session">The session for the new connection.</param>
        /// <remarks>Enqueues the connection handling to another fiber and invokes the OnConnected method for each web API controller.</remarks>
        public void OnConnected(IWebApiSession session)
        {
            this.otherFiber.Enqueue(() =>
            {
                foreach (var webApiController in this.webApiControllerLst)
                {
                    webApiController.OnConnected(session);
                }
            });
        }

        /// <summary>
        /// Handles an error that occurred while processing a request.
        /// </summary>
        /// <param name="session">The session that encountered the error.</param>
        /// <param name="request">The HTTP request that caused the error.</param>
        /// <param name="error">The error message.</param>
        /// <remarks>Enqueues the error handling to another fiber and invokes the OnReceivedRequestError method for each web API controller.</remarks>
        public void OnReceivedRequestError(IWebApiSession session, ProtonNetCommon.HttpRequest request, string error)
        {
            this.otherFiber.Enqueue(() =>
            {
                foreach (var webApiController in this.webApiControllerLst)
                {
                    webApiController.OnReceivedRequestError(session, request, error);
                }
            });
        }

        /// <summary>
        /// Handles a disconnection from the server.
        /// </summary>
        /// <param name="session">The session for the disconnected connection.</param>
        /// <remarks>Removes the session from tracking and enqueues the disconnection handling to another fiber, invoking the OnDisconnected method for each web API controller.</remarks>
        public void OnDisconnected(IWebApiSession session)
        {
            this.sessionReceiveAtTimeAmountDict.TryRemove(session, out _);

            this.otherFiber.Enqueue(() =>
            {
                foreach (var webApiController in this.webApiControllerLst)
                {
                    webApiController.OnDisconnected(session);
                }
            });
        }

        /// <summary>
        /// Handles a socket error that occurred during communication.
        /// </summary>
        /// <param name="session">The session that encountered the socket error.</param>
        /// <param name="error">The socket error code.</param>
        /// <remarks>Enqueues the error handling to another fiber and invokes the OnError method for each web API controller.</remarks>
        public void OnError(IWebApiSession session, System.Net.Sockets.SocketError error)
        {
            this.otherFiber.Enqueue(() =>
            {
                foreach (var webApiController in this.webApiControllerLst)
                {
                    webApiController.OnError(session, error);
                }
            });
        }

    }

}

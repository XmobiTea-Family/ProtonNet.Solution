using System;
using XmobiTea.Linq;
using XmobiTea.ProtonNet.Server.WebApi.Helper;
using XmobiTea.ProtonNet.Server.WebApi.Models;
using XmobiTea.ProtonNet.Server.WebApi.Types;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Extensions
{
    /// <summary>
    /// Extensions for HttpRequest
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Converts the HTTP method string to the HttpMethod enum.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <returns>The HTTP method as HttpMethod enum.</returns>
        public static HttpMethod GetMethod(this ProtonNetCommon.HttpRequest request) => (HttpMethod)Enum.Parse(typeof(HttpMethod), request.Method, true);

        /// <summary>
        /// Retrieves the body of the HTTP request as a byte array.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <returns>The body of the request as a byte array.</returns>
        public static byte[] GetBodyBytes(this ProtonNetCommon.HttpRequest request) => request.BodyAsBytes;

        /// <summary>
        /// Retrieves the body of the HTTP request as the specified type. This method is a placeholder and needs implementation.
        /// </summary>
        /// <typeparam name="T">The type to convert the body to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <returns>The body of the request as the specified type.</returns>
        public static T GetBody<T>(this ProtonNetCommon.HttpRequest request) => default;

        /// <summary>
        /// Retrieves the body of the HTTP request as the specified type. This method is a placeholder and needs implementation.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="type">The type to convert the body to.</param>
        /// <returns>The body of the request as the specified type.</returns>
        public static object GetBody(this ProtonNetCommon.HttpRequest request, Type type) => default;

        /// <summary>
        /// Retrieves a specific header from the HTTP request and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the header value to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the header.</param>
        /// <returns>The header value as the specified type.</returns>
        public static T GetHeader<T>(this ProtonNetCommon.HttpRequest request, string name) => (T)request.GetHeader(name, typeof(T));

        /// <summary>
        /// Retrieves a specific header from the HTTP request and converts it to the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the header.</param>
        /// <param name="type">The type to convert the header value to.</param>
        /// <returns>The header value as the specified type.</returns>
        public static object GetHeader(this ProtonNetCommon.HttpRequest request, string name, Type type)
        {
            name = name.ToLower();

            for (var j = 0; j < request.HeaderCount; j++)
            {
                var header = request.GetHeader(j);

                if (header.Item1.ToLower() == name)
                {
                    return Convert.ChangeType(header.Item2, type);
                }
            }

            return default;
        }

        /// <summary>
        /// Checks if a specific header exists in the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the header.</param>
        /// <returns><c>true</c> if the header exists; otherwise, <c>false</c>.</returns>
        public static object ContainsHeader(this ProtonNetCommon.HttpRequest request, string name)
        {
            name = name.ToLower();

            for (var j = 0; j < request.HeaderCount; j++)
            {
                var header = request.GetHeader(j);

                if (header.Item1.ToLower() == name) return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves all values of a specific header and converts them to an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the header values to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the header.</param>
        /// <returns>An array of header values as the specified type.</returns>
        public static T[] GetHeaderArray<T>(this ProtonNetCommon.HttpRequest request, string name)
        {
            name = name.ToLower();

            for (var j = 0; j < request.HeaderCount; j++)
            {
                var header = request.GetHeader(j);

                if (header.Item1.ToLower() == name)
                {
                    var stringArr = header.Item2.Split(SpecialChars.Semicolon);

                    var answer = new T[stringArr.Length];

                    for (var i = 0; i < stringArr.Length; i++)
                    {
                        answer[i] = (T)Convert.ChangeType(stringArr[i].Trim(), typeof(T));
                    }

                    return answer;
                }
            }

            return default;
        }

        /// <summary>
        /// Retrieves all values of a specific header and converts them to an array of the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the header.</param>
        /// <param name="type">The type to convert the header values to.</param>
        /// <returns>An array of header values as the specified type.</returns>
        public static object[] GetHeaderArray(this ProtonNetCommon.HttpRequest request, string name, Type type)
        {
            name = name.ToLower();

            for (var j = 0; j < request.HeaderCount; j++)
            {
                var header = request.GetHeader(j);

                if (header.Item1.ToLower() == name)
                {
                    var stringArr = header.Item2.Split(SpecialChars.Semicolon);

                    var answer = new object[stringArr.Length];

                    for (var i = 0; i < stringArr.Length; i++)
                    {
                        answer[i] = Convert.ChangeType(stringArr[i].Trim(), type);
                    }

                    return answer;
                }
            }

            return default;
        }

        /// <summary>
        /// Retrieves a query parameter value from the HTTP request and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the query parameter value to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the query parameter.</param>
        /// <returns>The query parameter value as the specified type.</returns>
        public static T GetQuery<T>(this ProtonNetCommon.HttpRequest request, string name) => (T)request.GetQuery(name, typeof(T));

        /// <summary>
        /// Retrieves a query parameter value from the HTTP request and converts it to the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="type">The type to convert the query parameter value to.</param>
        /// <returns>The query parameter value as the specified type.</returns>
        public static object GetQuery(this ProtonNetCommon.HttpRequest request, string name, Type type)
        {
            var url = Uri.UnescapeDataString(request.Url);

            DetectUrlUtils.Detect(url, out var singlePrefixLst, out var queryLst);

            var queryItem = queryLst.Find(x => x.Key == name);

            if (queryItem != null)
            {
                return Convert.ChangeType(queryItem.AsString, type);
            }

            return default;
        }

        /// <summary>
        /// Retrieves all values of a query parameter from the HTTP request and converts them to an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the query parameter values to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the query parameter.</param>
        /// <returns>An array of query parameter values as the specified type.</returns>
        public static T[] GetQueryArray<T>(this ProtonNetCommon.HttpRequest request, string name)
        {
            var url = Uri.UnescapeDataString(request.Url);

            DetectUrlUtils.Detect(url, out var singlePrefixLst, out var queryLst);

            var queryItem = queryLst.Find(x => x.Key == name);

            if (queryItem != null)
            {
                var stringArr = queryItem.AsStringArray;

                var answer = new T[stringArr.Length];

                for (var i = 0; i < stringArr.Length; i++)
                {
                    answer[i] = (T)Convert.ChangeType(stringArr[i], typeof(T));
                }

                return answer;
            }

            return default;
        }

        /// <summary>
        /// Retrieves all values of a query parameter from the HTTP request and converts them to an array of the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="type">The type to convert the query parameter values to.</param>
        /// <returns>An array of query parameter values as the specified type.</returns>
        public static object[] GetQueryArray(this ProtonNetCommon.HttpRequest request, string name, Type type)
        {
            var url = Uri.UnescapeDataString(request.Url);

            DetectUrlUtils.Detect(url, out var singlePrefixLst, out var queryLst);

            var queryItem = queryLst.Find(x => x.Key == name);

            if (queryItem != null)
            {
                var stringArr = queryItem.AsStringArray;

                var answer = new object[stringArr.Length];

                for (var i = 0; i < stringArr.Length; i++)
                {
                    answer[i] = Convert.ChangeType(stringArr[i], type);
                }

                return answer;
            }

            return default;
        }

        /// <summary>
        /// Retrieves a parameter value from the HTTP request based on the provided name and full prefix, and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the parameter value to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="fullPrefix">The full prefix that includes the parameter name.</param>
        /// <returns>The parameter value as the specified type.</returns>
        public static T GetParam<T>(this ProtonNetCommon.HttpRequest request, string name, string fullPrefix)
        {
            return (T)request.GetParam(name, fullPrefix, typeof(T));
        }

        /// <summary>
        /// Retrieves a parameter value from the HTTP request based on the provided name and full prefix, and converts it to the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="fullPrefix">The full prefix that includes the parameter name.</param>
        /// <param name="type">The type to convert the parameter value to.</param>
        /// <returns>The parameter value as the specified type.</returns>
        public static object GetParam(this ProtonNetCommon.HttpRequest request, string name, string fullPrefix, Type type)
        {
            var indexOf = fullPrefix.Split('/').IndexOf('{' + name + '}');

            if (indexOf != -1)
            {
                var url = Uri.UnescapeDataString(request.Url);

                DetectUrlUtils.Detect(url, out var singlePrefixLst, out var queryLst);

                return Convert.ChangeType(singlePrefixLst[indexOf - 1], type);
            }

            return default;
        }

        /// <summary>
        /// Retrieves a middleware value from the HTTP request based on the provided name and middleware context, and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the middleware value to.</typeparam>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the middleware value.</param>
        /// <param name="middlewareContext">The middleware context containing the data.</param>
        /// <returns>The middleware value as the specified type.</returns>
        public static T GetMiddleware<T>(this ProtonNetCommon.HttpRequest request, string name, MiddlewareContext middlewareContext)
        {
            return (T)request.GetMiddleware(name, middlewareContext, typeof(T));
        }

        /// <summary>
        /// Retrieves a middleware value from the HTTP request based on the provided name and middleware context, and converts it to the specified type.
        /// </summary>
        /// <param name="request">The HTTP request to process.</param>
        /// <param name="name">The name of the middleware value.</param>
        /// <param name="middlewareContext">The middleware context containing the data.</param>
        /// <param name="type">The type to convert the middleware value to.</param>
        /// <returns>The middleware value as the specified type.</returns>
        public static object GetMiddleware(this ProtonNetCommon.HttpRequest request, string name, MiddlewareContext middlewareContext, Type type)
        {
            if (!middlewareContext.Contains(name)) return null;
            else return middlewareContext.GetData<object>(name);
        }

        /// <summary>
        /// Creates a clone of the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request to clone.</param>
        /// <returns>A new instance of the HTTP request that is a clone of the original.</returns>
        public static ProtonNetCommon.HttpRequest Clone(this ProtonNetCommon.HttpRequest request) => new HttpRequest(request);

    }

}

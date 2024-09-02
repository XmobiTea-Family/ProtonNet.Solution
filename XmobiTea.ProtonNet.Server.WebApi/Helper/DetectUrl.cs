using System.Collections.Generic;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute;
using XmobiTea.ProtonNet.Server.WebApi.Models;

namespace XmobiTea.ProtonNet.Server.WebApi.Helper
{
    /// <summary>
    /// Provides utility methods for detecting and processing URLs and routing attributes.
    /// </summary>
    static class DetectUrl
    {
        /// <summary>
        /// Parses the provided URL into a list of single prefixes and query items.
        /// </summary>
        /// <param name="url">The URL to be parsed.</param>
        /// <param name="singlePrefixLst">Output list of single prefixes parsed from the URL.</param>
        /// <param name="queryLst">Output list of query items parsed from the URL.</param>
        public static void Detect(string url, out List<string> singlePrefixLst, out List<QueryItem> queryLst)
        {
            var urlSpls = url.Split('/');

            var lastName = string.Empty;
            singlePrefixLst = new List<string>();

            for (var i = 0; i < urlSpls.Length; i++)
            {
                var urlElement = urlSpls[i];

                if (!string.IsNullOrEmpty(urlElement))
                {
                    if (i == urlSpls.Length - 1)
                        lastName = urlElement;
                    else
                        singlePrefixLst.Add(urlElement);
                }
            }

            queryLst = new List<QueryItem>();

            var lastNameSpls = lastName.Split('?');

            singlePrefixLst.Add(lastNameSpls[0]);

            if (lastNameSpls.Length > 1)
            {
                lastName = string.Empty;

                for (var i = 1; i < lastNameSpls.Length; i++)
                {
                    lastName += lastNameSpls[i];

                    if (i < lastNameSpls.Length - 1) lastName += '?';
                }

                lastNameSpls = lastName.Split('&');

                for (var i = 0; i < lastNameSpls.Length; i++)
                {
                    var lastNameElement = lastNameSpls[i];

                    if (!lastNameElement.Contains("=")) continue;

                    var lastNameSpl2s = lastNameElement.Split('=');

                    var queryItem = new QueryItem();
                    queryItem.Key = lastNameSpl2s[0];

                    if (lastNameSpl2s[1].Contains(","))
                    {
                        var valueArray = lastNameSpl2s[1].Split(',');
                        queryItem.Value = valueArray;
                    }
                    else
                    {
                        queryItem.IsSingleString = true;
                        queryItem.Value = lastNameSpl2s[1];
                    }

                    queryLst.Add(queryItem);
                }
            }
        }

        /// <summary>
        /// Constructs the full prefix from the given HTTP method and route attributes.
        /// </summary>
        /// <param name="httpMethodAttribute">The HTTP method attribute containing prefix and name.</param>
        /// <param name="routeAttribute">The route attribute containing prefix.</param>
        /// <returns>The constructed full prefix based on the attributes.</returns>
        public static string GetFullPrefix(HttpMethodAttribute httpMethodAttribute, RouteAttribute routeAttribute)
        {
            string answer;

            if (!string.IsNullOrEmpty(httpMethodAttribute.Prefix))
                answer = httpMethodAttribute.Prefix + "/" + httpMethodAttribute.Name;
            else
            {
                if (routeAttribute != null)
                    answer = routeAttribute.Prefix + "/" + httpMethodAttribute.Name;
                else
                    answer = "/" + httpMethodAttribute.Name;
            }

            return answer;
        }

        /// <summary>
        /// Retrieves a list of single prefixes from the given HTTP method and route attributes.
        /// </summary>
        /// <param name="httpMethodAttribute">The HTTP method attribute containing prefix and name.</param>
        /// <param name="routeAttribute">The route attribute containing prefix.</param>
        /// <returns>A list of single prefixes derived from the attributes.</returns>
        public static List<string> GetSinglePrefixLst(HttpMethodAttribute httpMethodAttribute, RouteAttribute routeAttribute)
        {
            var answer = new List<string>();

            var fullPrefix = GetFullPrefix(httpMethodAttribute, routeAttribute);

            answer.AddRange(fullPrefix.Split('/'));

            answer.RemoveAt(0);

            return answer;
        }

        /// <summary>
        /// Determines whether the given single prefix is a parameter prefix.
        /// </summary>
        /// <param name="singlePrefix">The single prefix to be checked.</param>
        /// <returns><c>true</c> if the prefix contains curly braces indicating a parameter; otherwise, <c>false</c>.</returns>
        public static bool IsParamPrefix(string singlePrefix)
        {
            return singlePrefix.Contains("{") && singlePrefix.Contains("}");
        }

    }

}

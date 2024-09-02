using System.Collections.Generic;
using XmobiTea.ProtonNet.Server.WebApi.Extensions;
using XmobiTea.ProtonNet.Server.WebApi.Models;

namespace XmobiTea.ProtonNet.Server.WebApi.Helper
{
    /// <summary>
    /// Provides utility methods for working with <see cref="MethodController"/> instances.
    /// </summary>
    static class MethodControllerUtils
    {
        /// <summary>
        /// Retrieves a list of child <see cref="MethodController"/> instances from the specified <see cref="MethodController"/> 
        /// based on the provided list of prefixes.
        /// </summary>
        /// <param name="methodController">The parent <see cref="MethodController"/> instance.</param>
        /// <param name="singlePrefixLst">A list of prefixes used to find child method controllers.</param>
        /// <returns>A list of child <see cref="MethodController"/> instances.</returns>
        public static List<MethodController> GetChildMethodControllerLst(MethodController methodController, List<string> singlePrefixLst)
        {
            return methodController.GetMethodControllerLst(singlePrefixLst);
        }

        /// <summary>
        /// Retrieves a list of middleware child <see cref="MethodController"/> instances from the specified <see cref="MethodController"/> 
        /// based on the provided list of prefixes.
        /// </summary>
        /// <param name="methodController">The parent <see cref="MethodController"/> instance.</param>
        /// <param name="singlePrefixLst">A list of prefixes used to find middleware child method controllers.</param>
        /// <returns>A list of middleware child <see cref="MethodController"/> instances.</returns>
        public static List<MethodController> GetMiddlewareChildMethodControllerLst(MethodController methodController, List<string> singlePrefixLst)
        {
            return methodController.GetMiddlewareMethodControllerLst(singlePrefixLst);
        }

    }

}

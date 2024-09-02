using System.Collections.Generic;
using XmobiTea.ProtonNet.Server.WebApi.Models;

namespace XmobiTea.ProtonNet.Server.WebApi.Extensions
{
    /// <summary>
    /// Provides extension methods for retrieving method controllers based on prefixes.
    /// </summary>
    static class MethodControllerExtensions
    {
        /// <summary>
        /// Retrieves a list of middleware method controllers that match the given prefix list.
        /// </summary>
        /// <param name="methodController">The root method controller to start searching from.</param>
        /// <param name="prefixLst">The list of prefixes to match against.</param>
        /// <returns>A list of method controllers that match the given prefixes.</returns>
        public static List<MethodController> GetMiddlewareMethodControllerLst(this MethodController methodController, List<string> prefixLst)
        {
            var answer = new List<MethodController>();

            foreach (var c in methodController.methodControllerDict)
            {
                var temp = GetMiddlewareMethodControllerLst(c.Value, prefixLst, 0);

                if (temp != null)
                {
                    answer.AddRange(temp);
                }
            }

            return answer;
        }

        /// <summary>
        /// Recursively searches for middleware method controllers that match the given prefix list.
        /// </summary>
        /// <param name="methodController">The current method controller to search.</param>
        /// <param name="prefixLst">The list of prefixes to match against.</param>
        /// <param name="i">The current index in the prefix list.</param>
        /// <returns>A list of method controllers that match the given prefixes, or null if no match is found.</returns>
        private static List<MethodController> GetMiddlewareMethodControllerLst(MethodController methodController, List<string> prefixLst, int i)
        {
            if (prefixLst.Count == i) return null;

            var prefix = prefixLst[i];

            if (IsValidPrefix(methodController, prefix))
            {
                var answer = new List<MethodController>();

                answer.Add(methodController);

                if (prefixLst.Count != i + 1)
                {
                    foreach (var c in methodController.methodControllerDict)
                    {
                        var temp = GetMiddlewareMethodControllerLst(c.Value, prefixLst, i + 1);

                        if (temp != null)
                        {
                            answer.AddRange(temp);
                        }
                    }
                }

                return answer;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a list of method controllers that match the given prefix list.
        /// </summary>
        /// <param name="methodController">The root method controller to start searching from.</param>
        /// <param name="prefixLst">The list of prefixes to match against.</param>
        /// <returns>A list of method controllers that match the given prefixes.</returns>
        public static List<MethodController> GetMethodControllerLst(this MethodController methodController, List<string> prefixLst)
        {
            var answer = new List<MethodController>();

            foreach (var c in methodController.methodControllerDict)
            {
                var temp = GetMethodControllerLst(c.Value, prefixLst, 0);

                if (temp != null)
                {
                    answer.AddRange(temp);
                }
            }

            return answer;
        }

        /// <summary>
        /// Recursively searches for method controllers that match the given prefix list.
        /// </summary>
        /// <param name="methodController">The current method controller to search.</param>
        /// <param name="prefixLst">The list of prefixes to match against.</param>
        /// <param name="i">The current index in the prefix list.</param>
        /// <returns>A list of method controllers that match the given prefixes, or null if no match is found.</returns>
        private static List<MethodController> GetMethodControllerLst(MethodController methodController, List<string> prefixLst, int i)
        {
            if (prefixLst.Count == i) return null;

            var prefix = prefixLst[i];

            if (IsValidPrefix(methodController, prefix))
            {
                var answer = new List<MethodController>();

                if (prefixLst.Count == i + 1)
                {
                    answer.Add(methodController);
                }
                else
                {
                    foreach (var c in methodController.methodControllerDict)
                    {
                        var temp = GetMethodControllerLst(c.Value, prefixLst, i + 1);

                        if (temp != null)
                        {
                            answer.AddRange(temp);
                        }
                    }
                }

                return answer;
            }

            return null;
        }

        /// <summary>
        /// Checks if the given prefix is valid for the provided method controller.
        /// </summary>
        /// <param name="methodController">The method controller to check against.</param>
        /// <param name="prefix">The prefix to validate.</param>
        /// <returns><c>true</c> if the prefix is valid; otherwise, <c>false</c>.</returns>
        private static bool IsValidPrefix(MethodController methodController, string prefix)
        {
            return methodController.keyPrefix == prefix || methodController.isPrefixWithParam;
        }

    }

}

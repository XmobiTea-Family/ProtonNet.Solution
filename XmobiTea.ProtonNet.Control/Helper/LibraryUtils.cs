using System.Diagnostics;
using System.IO;

namespace XmobiTea.ProtonNet.Control.Helper
{
    /// <summary>
    /// Utility class for various library-related functions.
    /// </summary>
    static class LibraryUtils
    {
        /// <summary>
        /// The relative root path for file operations.
        /// </summary>
        private static readonly string RootPath = "../../..";

        /// <summary>
        /// Gets the directory path of the current process's executable.
        /// </summary>
        /// <returns>The directory path of the current process's executable.</returns>
        private static string GetDllPath()
        {
            var codeBase = Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetDirectoryName(codeBase) + "/";
        }

        /// <summary>
        /// Combines the root path with additional paths and returns the full path.
        /// </summary>
        /// <param name="paths">The additional paths to combine with the root path.</param>
        /// <returns>The combined full path as a string.</returns>
        public static string CombineFromRootPath(params string[] paths)
        {
            var answer = LibraryUtils.GetDllPath();
            answer += LibraryUtils.RootPath;

            foreach (var path in paths)
            {
                answer += "/" + path;
            }

            answer = answer.Replace("\\", "/");

            return answer;
        }

        /// <summary>
        /// Gets the platform-specific path based on the directory of the current process's executable.
        /// </summary>
        /// <returns>The platform-specific path as a string.</returns>
        public static string GetPlatformPath()
        {
            var directoryInfo = new DirectoryInfo(LibraryUtils.GetDllPath());

            return directoryInfo.Parent.Name + "/" + directoryInfo.Name;
        }

    }

}

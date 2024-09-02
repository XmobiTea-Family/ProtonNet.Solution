using System.Diagnostics;
using System.IO;

namespace XmobiTea.ProtonNet.Server.Helper
{
    /// <summary>
    /// Provides utility methods for library operations.
    /// </summary>
    public static class LibraryUtils
    {
        /// <summary>
        /// Gets the directory path of the current executable.
        /// </summary>
        /// <returns>The directory path where the current executable is located.</returns>
        public static string GetDllPath()
        {
            // Get the file path of the current process's main module
            var codeBase = Process.GetCurrentProcess().MainModule.FileName;

            // Return the directory path of the executable
            return Path.GetDirectoryName(codeBase) + "/";
        }

    }

}

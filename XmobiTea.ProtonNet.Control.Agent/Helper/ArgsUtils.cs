namespace XmobiTea.ProtonNet.Control.Agent.Helper
{
    /// <summary>
    /// Utility class for handling command-line arguments.
    /// </summary>
    static class ArgsUtils
    {
        /// <summary>
        /// Retrieves the value of a specified argument from the array of arguments.
        /// </summary>
        /// <param name="args">
        /// The array of command-line arguments.
        /// </param>
        /// <param name="names">
        /// The names of the arguments whose values are to be retrieved.
        /// </param>
        /// <returns>
        /// The value of the argument if found; otherwise, null.
        /// </returns>
        public static string GetArgValue(string[] args, params string[] names)
        {
            for (int i = 0; i < args.Length; i++)
            {
                foreach (var name in names)
                {
                    if (args[i] == name && args.Length > i + 1)
                    {
                        return args[i + 1];
                    }
                }
            }

            return null;
        }

    }

}

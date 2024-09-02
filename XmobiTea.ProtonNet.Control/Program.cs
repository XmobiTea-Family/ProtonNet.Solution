namespace XmobiTea.ProtonNet.Control
{
    /// <summary>
    /// The Program class is the entry point for the ProtonNet Control application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main method is the entry point of the application.
        /// It initializes and runs the application startup process.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        static void Main(string[] args)
        {
            var applicationStartup = ApplicationStartup.NewBuilder()
                    .SetArgs(args)
                    .Build();

            applicationStartup.Run();
        }

    }

}

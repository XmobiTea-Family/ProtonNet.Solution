namespace XmobiTea.Logging.Console
{
    /// <summary>
    /// Factory class to create instances of <see cref="ConsoleLogger"/>.
    /// This class is a singleton, with the single instance accessible via the <see cref="Instance"/> property.
    /// </summary>
    sealed class ConsoleLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="ConsoleLoggerFactory"/>.
        /// </summary>
        public static readonly ConsoleLoggerFactory Instance = new ConsoleLoggerFactory();

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>A new instance of <see cref="ConsoleLogger"/>.</returns>
        public ILogger CreateLogger(string name) => new ConsoleLogger(name);

        /// <summary>
        /// Prevents a default instance of the <see cref="ConsoleLoggerFactory"/> class from being created.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private ConsoleLoggerFactory() { }

    }

}

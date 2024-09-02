namespace XmobiTea.Logging.Empty
{
    /// <summary>
    /// Factory class for creating instances of <see cref="EmptyLogger"/>.
    /// This factory returns a logger that performs no logging operations.
    /// </summary>
    sealed class EmptyLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Singleton instance of <see cref="EmptyLoggerFactory"/>.
        /// </summary>
        public static readonly EmptyLoggerFactory Instance = new EmptyLoggerFactory();

        /// <summary>
        /// Creates an instance of <see cref="EmptyLogger"/> with the specified logger name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>An instance of <see cref="EmptyLogger"/>.</returns>
        public ILogger CreateLogger(string name) => new EmptyLogger(name);

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private EmptyLoggerFactory() { }

    }

}

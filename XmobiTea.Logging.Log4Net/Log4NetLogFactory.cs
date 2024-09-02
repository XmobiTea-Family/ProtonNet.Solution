namespace XmobiTea.Logging.Log4Net
{
    /// <summary>
    /// Provides a factory for creating instances of <see cref="ILogger"/>
    /// that use the log4net logging framework.
    /// </summary>
    public sealed class Log4NetLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// A singleton instance of <see cref="Log4NetLoggerFactory"/>.
        /// </summary>
        public static readonly Log4NetLoggerFactory Instance = new Log4NetLoggerFactory();

        /// <summary>
        /// Creates an <see cref="ILogger"/> instance for the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name associated with the logger.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public ILogger CreateLogger(string name) => new Log4NetLogger(log4net.LogManager.GetLogger(name));

        /// <summary>
        /// Prevents a default instance of the <see cref="Log4NetLoggerFactory"/> class from being created.
        /// </summary>
        private Log4NetLoggerFactory() { }

    }

}

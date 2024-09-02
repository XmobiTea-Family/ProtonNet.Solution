namespace XmobiTea.Logging
{
    /// <summary>
    /// Interface representing a factory for creating loggers.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger with the specified name.
        /// </summary>
        /// <param name="name">The name of the logger to create.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        ILogger CreateLogger(string name);

    }

}

using System;
using System.Threading;
using XmobiTea.Logging.Console;
using XmobiTea.Logging.Empty;

namespace XmobiTea.Logging
{
    /// <summary>
    /// Static class that manages the creation and configuration of loggers.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Counter to track the number of created loggers.
        /// </summary>
        private static int createLoggerCount;

        /// <summary>
        /// The factory used to create logger instances.
        /// </summary>
        private static ILoggerFactory loggerFactory;

        /// <summary>
        /// Static constructor to set the default logger factory.
        /// </summary>
        static LogManager() => SetDefaultLoggerFactory(DefaultLogType.Console);

        /// <summary>
        /// Gets a logger for the specified generic type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the logger.</typeparam>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public static ILogger GetLogger<T>() => GetLogger(typeof(T));

        /// <summary>
        /// Gets a logger for the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object for which to get the logger.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public static ILogger GetLogger<T>(T obj) => GetLogger(obj.GetType());

        /// <summary>
        /// Gets a logger by the specified name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public static ILogger GetLogger(string name) => loggerFactory.CreateLogger(name);

        /// <summary>
        /// Gets a logger for the specified type.
        /// </summary>
        /// <param name="type">The type for which to get the logger.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public static ILogger GetLogger(Type type) => GetLogger(type.FullName);

        /// <summary>
        /// Sets the logger factory to be used for creating loggers.
        /// </summary>
        /// <param name="factory">The logger factory to set.</param>
        public static void SetLoggerFactory(ILoggerFactory factory)
        {
            loggerFactory = factory;

            int num = Interlocked.Exchange(ref createLoggerCount, 0);
            if (num != 0)
            {
                GetLogger("LogManager").WarnFormat(num == 1 ? "LogManager.SetLoggerFactory: 1 ILogger instance created with previous factory!" : "LogManager.SetLoggerFactory: {0} ILogger instances created with previous factory!", num);
            }
        }

        /// <summary>
        /// Sets the default logger factory based on the specified log type.
        /// </summary>
        /// <param name="defaultLogType">The default log type to set.</param>
        public static void SetDefaultLoggerFactory(DefaultLogType defaultLogType) => SetLoggerFactory(defaultLogType == DefaultLogType.Empty ? (ILoggerFactory)EmptyLoggerFactory.Instance : (ILoggerFactory)ConsoleLoggerFactory.Instance);

    }

}

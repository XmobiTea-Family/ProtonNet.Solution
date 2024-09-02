using System;
using System.Text;

namespace XmobiTea.Logging.Console
{
    /// <summary>
    /// A logger implementation that outputs logs to the console with various log levels.
    /// </summary>
    sealed class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Represents the debug log level.
        /// </summary>
        private static readonly string DEBUG = "DEBUG";

        /// <summary>
        /// Represents the info log level.
        /// </summary>
        private static readonly string INFO = "INFO";

        /// <summary>
        /// Represents the error log level.
        /// </summary>
        private static readonly string ERROR = "ERROR";

        /// <summary>
        /// Represents the fatal log level.
        /// </summary>
        private static readonly string FATAL = "FATAL";

        /// <summary>
        /// Represents the warning log level.
        /// </summary>
        private static readonly string WARN = "WARN";

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        private string name { get; }

        /// <summary>
        /// Gets a value indicating whether debug logging is enabled.
        /// </summary>
        public bool IsDebugEnabled => true;

        /// <summary>
        /// Gets a value indicating whether error logging is enabled.
        /// </summary>
        public bool IsErrorEnabled => true;

        /// <summary>
        /// Gets a value indicating whether fatal logging is enabled.
        /// </summary>
        public bool IsFatalEnabled => true;

        /// <summary>
        /// Gets a value indicating whether info logging is enabled.
        /// </summary>
        public bool IsInfoEnabled => true;

        /// <summary>
        /// Gets a value indicating whether warning logging is enabled.
        /// </summary>
        public bool IsWarnEnabled => true;

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public ConsoleLogger(string name) => this.name = name;

        /// <summary>
        /// Generates the final log message string with timestamp, thread info, and log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The log message.</param>
        /// <returns>The formatted log message string.</returns>
        private string GenerateFinalMessage(string level, object message)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(System.DateTime.Now.ToString("HH:mm:ss.ff"));
            stringBuilder.Append(" ");
            var currentThread = System.Threading.Thread.CurrentThread;
            stringBuilder.Append($"[{currentThread.Name}{currentThread.ManagedThreadId}]");
            stringBuilder.Append(" ");
            stringBuilder.Append(level);
            stringBuilder.Append(" ");
            stringBuilder.Append(this.name);
            stringBuilder.Append(" - ");
            stringBuilder.Append(message);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(object message) => System.Console.WriteLine(this.GenerateFinalMessage(DEBUG, message));

        /// <summary>
        /// Logs a debug message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Debug(object message, Exception exception) => System.Console.WriteLine(this.GenerateFinalMessage(DEBUG, $"{message} {exception}"));

        /// <summary>
        /// Logs a formatted debug message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void DebugFormat(string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(DEBUG, string.Format(format, args)));

        /// <summary>
        /// Logs a formatted debug message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(DEBUG, string.Format(provider, format, args)));

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(object message) => System.Console.WriteLine(this.GenerateFinalMessage(ERROR, message));

        /// <summary>
        /// Logs an error message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Error(object message, Exception exception) => System.Console.WriteLine(this.GenerateFinalMessage(ERROR, $"{message} {exception}"));

        /// <summary>
        /// Logs a formatted error message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void ErrorFormat(string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(ERROR, string.Format(format, args)));

        /// <summary>
        /// Logs a formatted error message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(ERROR, string.Format(provider, format, args)));

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(object message) => System.Console.WriteLine(this.GenerateFinalMessage(FATAL, message));

        /// <summary>
        /// Logs a fatal message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Fatal(object message, Exception exception) => System.Console.WriteLine(this.GenerateFinalMessage(FATAL, $"{message} {exception}"));

        /// <summary>
        /// Logs a formatted fatal message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void FatalFormat(string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(FATAL, string.Format(format, args)));

        /// <summary>
        /// Logs a formatted fatal message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(FATAL, string.Format(provider, format, args)));

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(object message) => System.Console.WriteLine(this.GenerateFinalMessage(INFO, message));

        /// <summary>
        /// Logs an informational message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Info(object message, Exception exception) => System.Console.WriteLine(this.GenerateFinalMessage(INFO, $"{message} {exception}"));

        /// <summary>
        /// Logs a formatted informational message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void InfoFormat(string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(INFO, string.Format(format, args)));

        /// <summary>
        /// Logs a formatted informational message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(INFO, string.Format(provider, format, args)));

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(object message) => System.Console.WriteLine(this.GenerateFinalMessage(WARN, message));

        /// <summary>
        /// Logs a warning message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Warn(object message, Exception exception) => System.Console.WriteLine(this.GenerateFinalMessage(WARN, $"{message} {exception}"));

        /// <summary>
        /// Logs a formatted warning message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WarnFormat(string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(WARN, string.Format(format, args)));

        /// <summary>
        /// Logs a formatted warning message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) => System.Console.WriteLine(this.GenerateFinalMessage(WARN, string.Format(provider, format, args)));

    }

}

using System;

namespace XmobiTea.Logging.Log4Net
{
    /// <summary>
    /// Implements the <see cref="ILogger"/> interface using the log4net logging framework.
    /// </summary>
    sealed class Log4NetLogger : ILogger
    {
        /// <summary>
        /// Gets the log4net logger instance used by this <see cref="Log4NetLogger"/>.
        /// </summary>
        private log4net.ILog log { get; }

        /// <summary>
        /// Gets a value indicating whether debug logging is enabled.
        /// </summary>
        public bool IsDebugEnabled => this.log.IsDebugEnabled;

        /// <summary>
        /// Gets a value indicating whether error logging is enabled.
        /// </summary>
        public bool IsErrorEnabled => this.log.IsErrorEnabled;

        /// <summary>
        /// Gets a value indicating whether fatal logging is enabled.
        /// </summary>
        public bool IsFatalEnabled => this.log.IsFatalEnabled;

        /// <summary>
        /// Gets a value indicating whether informational logging is enabled.
        /// </summary>
        public bool IsInfoEnabled => this.log.IsInfoEnabled;

        /// <summary>
        /// Gets a value indicating whether warning logging is enabled.
        /// </summary>
        public bool IsWarnEnabled => this.log.IsWarnEnabled;

        /// <summary>
        /// Gets the name of this logger.
        /// </summary>
        public string Name => this.log.Logger.Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class with the specified log4net logger.
        /// </summary>
        /// <param name="logger">The log4net logger to use.</param>
        public Log4NetLogger(log4net.ILog logger) => this.log = logger;

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(object message) => this.log.Debug(message);

        /// <summary>
        /// Logs a debug message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Debug(object message, Exception exception) => this.log.Debug(message, exception);

        /// <summary>
        /// Logs a formatted debug message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void DebugFormat(string format, params object[] args) => this.log.DebugFormat(format, args);

        /// <summary>
        /// Logs a formatted debug message using the specified format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) => this.log.DebugFormat(provider, format, args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(object message) => this.log.Error(message);

        /// <summary>
        /// Logs an error message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Error(object message, Exception exception) => this.log.Error(message, exception);

        /// <summary>
        /// Logs a formatted error message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void ErrorFormat(string format, params object[] args) => this.log.ErrorFormat(format, args);

        /// <summary>
        /// Logs a formatted error message using the specified format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) => this.log.ErrorFormat(provider, format, args);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(object message) => this.log.Fatal(message);

        /// <summary>
        /// Logs a fatal message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Fatal(object message, Exception exception) => this.log.Fatal(message, exception);

        /// <summary>
        /// Logs a formatted fatal message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void FatalFormat(string format, params object[] args) => this.log.FatalFormat(format, args);

        /// <summary>
        /// Logs a formatted fatal message using the specified format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) => this.log.FatalFormat(provider, format, args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(object message) => this.log.Info(message);

        /// <summary>
        /// Logs an informational message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Info(object message, Exception exception) => this.log.Info(message, exception);

        /// <summary>
        /// Logs a formatted informational message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void InfoFormat(string format, params object[] args) => this.log.InfoFormat(format, args);

        /// <summary>
        /// Logs a formatted informational message using the specified format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) => this.log.InfoFormat(provider, format, args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(object message) => this.log.Warn(message);

        /// <summary>
        /// Logs a warning message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        public void Warn(object message, Exception exception) => this.log.Warn(message, exception);

        /// <summary>
        /// Logs a formatted warning message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void WarnFormat(string format, params object[] args) => this.log.WarnFormat(format, args);

        /// <summary>
        /// Logs a formatted warning message using the specified format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) => this.log.WarnFormat(provider, format, args);

    }

}

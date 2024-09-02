using System;

namespace XmobiTea.Logging
{
    /// <summary>
    /// Provides a logging interface with various levels of logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets a value indicating whether debug logging is enabled.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether error logging is enabled.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether fatal logging is enabled.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether info logging is enabled.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether warning logging is enabled.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Debug(object message);

        /// <summary>
        /// Logs a debug message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Logs a formatted debug message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted debug message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Error(object message);

        /// <summary>
        /// Logs an error message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Logs a formatted error message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted error message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Fatal(object message);

        /// <summary>
        /// Logs a fatal message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Logs a formatted fatal message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted fatal message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Info(object message);

        /// <summary>
        /// Logs an informational message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Logs a formatted informational message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted informational message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Warn(object message);

        /// <summary>
        /// Logs a warning message with an associated exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Logs a formatted warning message.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs a formatted warning message with an <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

    }

}

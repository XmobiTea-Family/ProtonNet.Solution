using System;

namespace XmobiTea.Logging.Empty
{
    /// <summary>
    /// Represents a logger that performs no logging actions.
    /// This class is a placeholder or no-op (no operation) logger, useful for cases where logging needs to be disabled.
    /// </summary>
    sealed class EmptyLogger : ILogger
    {
        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        private string name { get; }

        /// <summary>
        /// Always returns <c>false</c>, indicating that debug logging is not enabled.
        /// </summary>
        public bool IsDebugEnabled => false;

        /// <summary>
        /// Always returns <c>false</c>, indicating that error logging is not enabled.
        /// </summary>
        public bool IsErrorEnabled => false;

        /// <summary>
        /// Always returns <c>false</c>, indicating that fatal logging is not enabled.
        /// </summary>
        public bool IsFatalEnabled => false;

        /// <summary>
        /// Always returns <c>false</c>, indicating that informational logging is not enabled.
        /// </summary>
        public bool IsInfoEnabled => false;

        /// <summary>
        /// Always returns <c>false</c>, indicating that warning logging is not enabled.
        /// </summary>
        public bool IsWarnEnabled => false;

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyLogger"/> class with the specified logger name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public EmptyLogger(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// A no-op method that does nothing when a debug message is logged.
        /// </summary>
        /// <param name="message">The debug message.</param>
        public void Debug(object message)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a debug message and exception are logged.
        /// </summary>
        /// <param name="message">The debug message.</param>
        /// <param name="exception">The associated exception.</param>
        public void Debug(object message, Exception exception)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted debug message is logged.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void DebugFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted debug message with a format provider is logged.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when an error message is logged.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void Error(object message)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when an error message and exception are logged.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="exception">The associated exception.</param>
        public void Error(object message, Exception exception)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted error message is logged.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void ErrorFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted error message with a format provider is logged.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a fatal message is logged.
        /// </summary>
        /// <param name="message">The fatal message.</param>
        public void Fatal(object message)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a fatal message and exception are logged.
        /// </summary>
        /// <param name="message">The fatal message.</param>
        /// <param name="exception">The associated exception.</param>
        public void Fatal(object message, Exception exception)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted fatal message is logged.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void FatalFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted fatal message with a format provider is logged.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when an informational message is logged.
        /// </summary>
        /// <param name="message">The informational message.</param>
        public void Info(object message)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when an informational message and exception are logged.
        /// </summary>
        /// <param name="message">The informational message.</param>
        /// <param name="exception">The associated exception.</param>
        public void Info(object message, Exception exception)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted informational message is logged.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void InfoFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted informational message with a format provider is logged.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a warning message is logged.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void Warn(object message)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a warning message and exception are logged.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="exception">The associated exception.</param>
        public void Warn(object message, Exception exception)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted warning message is logged.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WarnFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// A no-op method that does nothing when a formatted warning message with a format provider is logged.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
        }

    }

}

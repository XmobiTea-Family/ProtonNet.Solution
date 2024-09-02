using System.Net.Sockets;

namespace XmobiTea.ProtonNetCommon.Helper
{
    /// <summary>
    /// Provides utility methods for handling socket errors.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Determines whether the specified socket error should be ignored in the context of a disconnect operation.
        /// </summary>
        /// <param name="error">The socket error to evaluate.</param>
        /// <returns>
        /// True if the error is one of the following: ConnectionAborted, ConnectionRefused, ConnectionReset, OperationAborted, or Shutdown.
        /// Otherwise, false.
        /// </returns>
        public static bool IsIgnoreDisconnectError(SocketError error)
        {
            return error == SocketError.ConnectionAborted ||
                error == SocketError.ConnectionRefused ||
                error == SocketError.ConnectionReset ||
                error == SocketError.OperationAborted ||
                error == SocketError.Shutdown;
        }

    }

}

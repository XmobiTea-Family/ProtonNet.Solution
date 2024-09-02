using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Defines a method for setting the session ID.
    /// </summary>
    public interface ISessionIdSetter
    {
        /// <summary>
        /// Sets the session ID.
        /// </summary>
        /// <param name="sessionId">The session ID to set.</param>
        void SetSessionId(string sessionId);

    }

    /// <summary>
    /// Defines a method for setting the encryption key.
    /// </summary>
    public interface IEncryptKeySetter
    {
        /// <summary>
        /// Sets the encryption key.
        /// </summary>
        /// <param name="encryptKey">The encryption key to set.</param>
        void SetEncryptKey(byte[] encryptKey);

    }

    /// <summary>
    /// Defines a method for retrieving the fiber associated with the session.
    /// </summary>
    public interface ISessionFiber
    {
        /// <summary>
        /// Gets the fiber associated with the session.
        /// </summary>
        /// <returns>The fiber associated with the session.</returns>
        IFiber GetFiber();

    }

    /// <summary>
    /// Defines methods for accessing session information.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Gets the connection ID.
        /// </summary>
        /// <returns>The connection ID.</returns>
        int GetConnectionId();

        /// <summary>
        /// Gets the server session ID.
        /// </summary>
        /// <returns>The server session ID.</returns>
        string GetServerSessionId();

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <returns>The encryption key.</returns>
        byte[] GetEncryptKey();

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        /// <returns>The session ID.</returns>
        string GetSessionId();

        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        /// <returns>The remote IP address.</returns>
        string GetRemoteIP();

        /// <summary>
        /// Gets the remote port.
        /// </summary>
        /// <returns>The remote port.</returns>
        int GetRemotePort();

    }

}

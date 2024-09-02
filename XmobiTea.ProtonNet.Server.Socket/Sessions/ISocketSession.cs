using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.Socket.Models;
using XmobiTea.ProtonNet.Server.Socket.Types;

namespace XmobiTea.ProtonNet.Server.Socket.Sessions
{
    /// <summary>
    /// Defines the interface for a socket session, providing methods for sending data, managing session state,
    /// and handling encryption and session time.
    /// </summary>
    public interface ISocketSession : ISession, ISessionIdSetter, ISessionFiber, IEncryptKeySetter
    {
        /// <summary>
        /// Gets the transport protocol used by this session.
        /// </summary>
        /// <returns>The transport protocol.</returns>
        TransportProtocol GetTransportProtocol();

        /// <summary>
        /// Gets the session time management service.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketSessionTime"/> representing the session time management service.</returns>
        ISocketSessionTime GetSessionTime();

        /// <summary>
        /// Sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Asynchronously sends data to the connected client.
        /// </summary>
        /// <param name="buffer">The data buffer to send.</param>
        /// <returns>True if the data was successfully sent, otherwise false.</returns>
        bool SendAsync(byte[] buffer);

        /// <summary>
        /// Disconnects the session after a specified delay.
        /// </summary>
        /// <param name="afterMilliseconds">The delay in milliseconds before disconnecting.</param>
        /// <returns>True if the session was scheduled to disconnect, otherwise false.</returns>
        bool Disconnect(int afterMilliseconds);

        /// <summary>
        /// Determines whether the session is currently connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        bool IsConnected();

    }

}

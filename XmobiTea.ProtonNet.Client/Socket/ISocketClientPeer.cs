using XmobiTea.ProtonNet.Client.Socket.Clients;
using XmobiTea.ProtonNet.Networking;

namespace XmobiTea.ProtonNet.Client.Socket
{
    /// <summary>
    /// Delegate for handling events when a connection is successfully established.
    /// </summary>
    /// <param name="connectionId">The unique ID of the connection.</param>
    /// <param name="serverSessionId">The session ID provided by the server.</param>
    public delegate void OnConnected(int connectionId, string serverSessionId);

    /// <summary>
    /// Delegate for handling events when a disconnection occurs.
    /// </summary>
    /// <param name="reason">The reason for the disconnection.</param>
    /// <param name="message">A message describing the disconnection.</param>
    public delegate void OnDisconnected(DisconnectReason reason, string message);

    /// <summary>
    /// Delegate for handling incoming operation events.
    /// </summary>
    /// <param name="operationEvent">The operation event received.</param>
    public delegate void OnOperationEvent(OperationEvent operationEvent);

    /// <summary>
    /// Delegate for handling socket errors.
    /// </summary>
    /// <param name="error">The socket error encountered.</param>
    public delegate void OnError(System.Net.Sockets.SocketError error);

    /// <summary>
    /// Interface representing a socket client peer, which is responsible for managing 
    /// socket connections, sending events, and handling callbacks for various socket events.
    /// </summary>
    public interface ISocketClientPeer : IClientPeer
    {
        /// <summary>
        /// Event triggered when a connection is successfully established.
        /// </summary>
        OnConnected OnConnected { get; set; }

        /// <summary>
        /// Event triggered when a disconnection occurs.
        /// </summary>
        OnDisconnected OnDisconnected { get; set; }

        /// <summary>
        /// Event triggered when an operation event is received.
        /// </summary>
        OnOperationEvent OnOperationEvent { get; set; }

        /// <summary>
        /// Event triggered when a socket error occurs.
        /// </summary>
        OnError OnError { get; set; }

        /// <summary>
        /// Retrieves the underlying socket client associated with this peer.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketClient"/> representing the socket client.</returns>
        ISocketClient GetSocketClient();

        /// <summary>
        /// Sends an operation event to the server.
        /// </summary>
        /// <param name="operationEvent">The operation event to send.</param>
        /// <param name="sendParameters">Optional parameters for sending the event.</param>
        void Send(OperationEvent operationEvent, SendParameters sendParameters = default);

        /// <summary>
        /// Establishes a connection to the server.
        /// </summary>
        /// <param name="autoReconnect">Specifies whether to automatically reconnect if the connection is lost.</param>
        /// <param name="onConnected">Callback for handling successful connections.</param>
        /// <param name="onDisconnected">Callback for handling disconnections.</param>
        /// <returns>True if the connection was successfully established, otherwise false.</returns>
        bool Connect(bool autoReconnect = false, OnConnected onConnected = null, OnDisconnected onDisconnected = null);

        /// <summary>
        /// Reconnects to the server.
        /// </summary>
        /// <param name="autoReconnect">Specifies whether to automatically reconnect if the connection is lost.</param>
        /// <param name="onConnected">Callback for handling successful connections.</param>
        /// <param name="onDisconnected">Callback for handling disconnections.</param>
        /// <returns>True if the reconnection was successful, otherwise false.</returns>
        bool Reconnect(bool autoReconnect = false, OnConnected onConnected = null, OnDisconnected onDisconnected = null);

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        /// <returns>True if the disconnection was successful, otherwise false.</returns>
        bool Disconnect();

        /// <summary>
        /// Checks if the client peer is currently connected to the server.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        bool IsConnected();

    }

}

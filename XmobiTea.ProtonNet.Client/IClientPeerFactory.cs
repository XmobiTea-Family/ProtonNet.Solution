using XmobiTea.ProtonNet.Client.Socket;
using XmobiTea.ProtonNet.Client.Socket.Types;
using XmobiTea.ProtonNet.Client.WebApi;

namespace XmobiTea.ProtonNet.Client
{
    /// <summary>
    /// Interface for a factory that creates and manages various types of client peers.
    /// </summary>
    public interface IClientPeerFactory
    {
        /// <summary>
        /// Creates a new Web API client peer.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <returns>An instance of <see cref="IWebApiClientPeer"/> representing the Web API client peer.</returns>
        IWebApiClientPeer NewWebApiClientPeer(string serverAddress);

        /// <summary>
        /// Creates a new Socket client peer.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <param name="protocol">The transport protocol to use (e.g., TCP, UDP).</param>
        /// <returns>An instance of <see cref="ISocketClientPeer"/> representing the Socket client peer.</returns>
        ISocketClientPeer NewSocketClientPeer(string serverAddress, TransportProtocol protocol);

        /// <summary>
        /// Creates a new SSL Socket client peer.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <param name="protocol">The SSL transport protocol to use.</param>
        /// <returns>An instance of <see cref="ISocketClientPeer"/> representing the SSL Socket client peer.</returns>
        ISocketClientPeer NewSslSocketClientPeer(string serverAddress, SslTransportProtocol protocol);

        /// <summary>
        /// Retrieves a client peer by its client ID.
        /// </summary>
        /// <param name="clientId">The ID of the client peer to retrieve.</param>
        /// <returns>An instance of <see cref="IClientPeer"/> representing the client peer with the specified ID.</returns>
        IClientPeer GetClientPeer(int clientId);

        /// <summary>
        /// Destroys a client peer by its client ID.
        /// </summary>
        /// <param name="clientId">The ID of the client peer to destroy.</param>
        /// <returns>True if the client peer was successfully removed; otherwise, false.</returns>
        bool DestroyClientPeer(int clientId);

        /// <summary>
        /// Services all client peers, processing any pending tasks or requests.
        /// </summary>
        void Service();

    }

}

using XmobiTea.ProtonNet.Client.Models;
using XmobiTea.ProtonNetClient.Options;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Client.WebApi
{
    /// <summary>
    /// Delegate for handling timestamp responses from the server.
    /// </summary>
    /// <param name="currentMilliseconds">The current time in milliseconds returned by the server.</param>
    public delegate void OnGetTsResponse(long currentMilliseconds);

    /// <summary>
    /// Delegate for handling ping responses from the server.
    /// </summary>
    /// <param name="status">The status of the ping request, indicating success or failure.</param>
    public delegate void OnPingResponse(bool status);

    /// <summary>
    /// Delegate for handling errors during socket operations.
    /// </summary>
    /// <param name="error">The socket error encountered.</param>
    public delegate void OnError(System.Net.Sockets.SocketError error);

    /// <summary>
    /// Interface for a Web API client peer, extending the basic client peer functionality
    /// with additional methods specific to Web API operations.
    /// </summary>
    public interface IWebApiClientPeer : IClientPeer
    {
        /// <summary>
        /// Sends a ping request to the server to check the connection status.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the ping request.</param>
        void Ping(OnPingResponse onResponse, int timeoutInSeconds);

        /// <summary>
        /// Sends a request to the server to get the current timestamp.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's timestamp response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the request.</param>
        void GetTs(OnGetTsResponse onResponse, int timeoutInSeconds);

    }

    /// <summary>
    /// Abstract base class for a Web API client peer, implementing common functionality 
    /// and providing a base for more specific Web API client peer implementations.
    /// </summary>
    public abstract class AbstractWebApiClientPeer : ClientPeer, IWebApiClientPeer
    {
        /// <summary>
        /// Encryption key used for secure communication.
        /// </summary>
        protected byte[] encryptKey { get; }

        /// <summary>
        /// Encryption key as a string, derived from the byte array.
        /// </summary>
        protected string encryptKeyStr { get; }

        /// <summary>
        /// Network statistics tracking for the client peer.
        /// </summary>
        protected IChangeNetworkStatistics networkStatistics { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWebApiClientPeer"/> class.
        /// </summary>
        /// <param name="serverAddress">The address of the server to connect to.</param>
        /// <param name="initRequest">Initial request containing session and client ID information.</param>
        /// <param name="tcpClientOptions">Options for configuring the TCP client.</param>
        public AbstractWebApiClientPeer(string serverAddress, IClientPeerInitRequest initRequest, TcpClientOptions tcpClientOptions)
            : base(serverAddress, initRequest, tcpClientOptions)
        {
            this.encryptKey = initRequest.EncryptKey;
            this.encryptKeyStr = System.Text.Encoding.UTF8.GetString(this.encryptKey);

            this.networkStatistics = new ChangeNetworkStatistics();
        }

        /// <summary>
        /// Retrieves network statistics for the client peer.
        /// </summary>
        /// <returns>An instance of <see cref="INetworkStatistics"/> containing network statistics.</returns>
        public override INetworkStatistics GetNetworkStatistics() => this.networkStatistics;

        /// <summary>
        /// Indicates whether the client peer is currently connected.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        public override bool IsConnected() => true;

        /// <summary>
        /// Abstract method to be implemented by derived classes for handling 
        /// timestamp requests from the server.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the request.</param>
        public abstract void GetTs(OnGetTsResponse onResponse, int timeoutInSeconds);

        /// <summary>
        /// Abstract method to be implemented by derived classes for handling 
        /// ping requests to the server.
        /// </summary>
        /// <param name="onResponse">Callback method to handle the server's response.</param>
        /// <param name="timeoutInSeconds">Timeout period in seconds for the ping request.</param>
        public abstract void Ping(OnPingResponse onResponse, int timeoutInSeconds);

    }

}

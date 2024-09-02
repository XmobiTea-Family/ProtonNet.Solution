using XmobiTea.ProtonNet.Client.Socket.Clients;
using XmobiTea.ProtonNet.Client.Socket.Services;

namespace XmobiTea.ProtonNet.Client.Socket
{
    /// <summary>
    /// Interface that defines the service method for managing ping-pong 
    /// operations over a socket connection.
    /// </summary>
    interface ISocketPingPong
    {
        /// <summary>
        /// Services the socket ping-pong operation, sending pings at regular intervals.
        /// </summary>
        void Service();

    }

    /// <summary>
    /// Implements the <see cref="ISocketPingPong"/> interface to manage 
    /// the ping-pong operations between the client and server over a socket connection.
    /// </summary>
    class SocketPingPong : ISocketPingPong
    {
        /// <summary>
        /// Interval in ticks for sending ping messages.
        /// </summary>
        private static readonly long SendPingInterval = 5 * 10000 * 1000;

        /// <summary>
        /// The socket client associated with this ping-pong service.
        /// </summary>
        private ISocketClient client { get; }

        /// <summary>
        /// Service responsible for emitting socket session events.
        /// </summary>
        internal ISocketSessionEmitService socketSessionEmitService { get; set; }

        /// <summary>
        /// The tick count at which the next ping should be sent.
        /// </summary>
        private long nextSend { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketPingPong"/> class.
        /// </summary>
        /// <param name="client">The socket client associated with this ping-pong service.</param>
        public SocketPingPong(ISocketClient client) => this.client = client;

        /// <summary>
        /// Sends a ping message to the server.
        /// </summary>
        void SendPind()
        {
            this.socketSessionEmitService.SendOperationPing(this.client, new Networking.OperationPing());
        }

        /// <summary>
        /// Services the socket ping-pong operation by sending a ping if the interval has elapsed.
        /// </summary>
        public void Service()
        {
            if (this.nextSend > System.DateTime.UtcNow.Ticks) return;
            this.nextSend = System.DateTime.UtcNow.Ticks + SendPingInterval;

            this.SendPind();
        }

    }

}

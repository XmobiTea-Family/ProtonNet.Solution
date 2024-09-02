using XmobiTea.Bean.Attributes;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Sessions;
using XmobiTea.Threading;

namespace XmobiTea.ProtonNet.Server.Socket.Services
{
    /// <summary>
    /// Provides an interface for managing the session time service in a socket server.
    /// </summary>
    public interface ISocketSessionTimeService
    {
        // Interface definition for the socket session time service.
    }

    /// <summary>
    /// Implements the <see cref="ISocketSessionTimeService"/> to manage session timeouts
    /// such as handshake and idle timeouts for socket sessions.
    /// </summary>
    class SocketSessionTimeService : ISocketSessionTimeService
    {
        private const int CheckSessionsLoopInMs = 3000;

        private int handshakeTimeoutInTicks { get; set; }
        private int idleTimeoutInTicks { get; set; }

        private IFiber fiber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSessionTimeService"/> class and starts the session checking loop.
        /// </summary>
        public SocketSessionTimeService()
        {
            this.fiber = new ThreadFiber("SocketTimeDisconnectService");

            this.fiber.ScheduleOnInterval(this.CheckSessions, CheckSessionsLoopInMs, CheckSessionsLoopInMs);
        }

        /// <summary>
        /// Sets the handshake timeout in seconds, which will be converted to ticks.
        /// </summary>
        /// <param name="handshakeTimeoutInSeconds">The handshake timeout in seconds.</param>
        internal void SetHandshakeTimeoutInSeconds(int handshakeTimeoutInSeconds) => this.handshakeTimeoutInTicks = handshakeTimeoutInSeconds * 10000000;

        /// <summary>
        /// Sets the idle timeout in seconds, which will be converted to ticks.
        /// </summary>
        /// <param name="idleTimeoutInSeconds">The idle timeout in seconds.</param>
        internal void SetIdleTimeoutInSeconds(int idleTimeoutInSeconds) => this.idleTimeoutInTicks = idleTimeoutInSeconds * 10000000;

        /// <summary>
        /// Automatically binds the session service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private ISessionService sessionService { get; set; }

        /// <summary>
        /// Automatically binds the socket session emit service using the <see cref="AutoBindAttribute"/>.
        /// </summary>
        [AutoBind]
        private ISocketSessionEmitService socketSessionEmitService { get; set; }

        /// <summary>
        /// Checks the sessions periodically to enforce handshake and idle timeouts, disconnecting sessions as necessary.
        /// </summary>
        private void CheckSessions()
        {
            var utcTicks = System.DateTime.UtcNow.Ticks;

            var sessions = this.sessionService.GetSessions();

            foreach (ISocketSession session in sessions)
            {
                var sessionTime = session.GetSessionTime();

                if (sessionTime.GetTicksHandshaked() == 0)
                {
                    if (sessionTime.GetTicksConnected() + this.handshakeTimeoutInTicks < utcTicks)
                    {
                        var operationDisconnect = new OperationDisconnect()
                        {
                            Reason = (byte)DisconnectReason.HandshakeTimeout,
                        };

                        this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect);

                        session.Disconnect(500);
                    }
                }
                else
                {
                    if (sessionTime.GetTicksLastReceived() + this.idleTimeoutInTicks < utcTicks)
                    {
                        var operationDisconnect = new OperationDisconnect()
                        {
                            Reason = (byte)DisconnectReason.IdleTimeout,
                        };

                        this.socketSessionEmitService.SendOperationDisconnect(session, operationDisconnect);

                        session.Disconnect(500);
                    }
                }
            }
        }

    }

}


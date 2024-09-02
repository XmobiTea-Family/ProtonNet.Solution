using XmobiTea.Bean.Attributes;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.Server.Services;
using XmobiTea.ProtonNet.Server.Socket.Services;
using XmobiTea.ProtonNet.Server.Socket.Sessions;

namespace XmobiTea.ProtonNet.Server.Socket.Controllers
{
    /// <summary>
    /// Represents a specialized socket controller for handling operations in the Proton socket server.
    /// </summary>
    class ProtonSocketController : SocketController
    {
        [AutoBind]
        private IRpcProtocolService rpcProtocolService { get; set; }

        [AutoBind]
        private ISocketOperationModelService operationModelService { get; set; }

        [AutoBind]
        private IUserPeerSessionService userPeerSessionService { get; set; }

        [AutoBind]
        private ISessionService sessionService { get; set; }

        [AutoBind]
        private IUserPeerService userPeerService { get; set; }

        [AutoBind]
        private IChannelService channelService { get; set; }

        /// <summary>
        /// Called when a session is connected. Maps the session to the session service if the session ID is available.
        /// </summary>
        /// <param name="session">The session that was connected.</param>
        public override void OnConnected(ISocketSession session)
        {
            if (!string.IsNullOrEmpty(session.GetSessionId()))
                this.sessionService.MapSession(session.GetSessionId(), session);
        }

        /// <summary>
        /// Called when data is received from a session. Deserializes and processes the operation models within the received data.
        /// </summary>
        /// <param name="session">The session that received the data.</param>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the received data starts.</param>
        /// <param name="length">The length of the received data in the buffer.</param>
        public override void OnReceived(ISocketSession session, byte[] buffer, int position, int length)
        {
            using (var mStream = new System.IO.MemoryStream(buffer, position, length))
            {
                while (true)
                {
                    OperationHeader header;
                    IOperationModel operationModel = null;

                    if (!this.rpcProtocolService.TryRead(mStream, out header, out var payload))
                    {
                        this.logger.Error("can not read data body");
                        return;
                    }

                    if (header.SendParameters.Encrypted)
                    {
                        if (!this.rpcProtocolService.TryDeserializeEncryptOperationModel(payload, header.OperationType, header.ProtocolProviderType, header.CryptoProviderType.GetValueOrDefault(), session.GetEncryptKey(), out operationModel))
                        {
                            this.logger.Error("can not deserialize encrypt operation model");
                            return;
                        }
                    }
                    else
                    {
                        if (!this.rpcProtocolService.TryDeserializeOperationModel(payload, header.OperationType, header.ProtocolProviderType, out operationModel))
                        {
                            this.logger.Error("can not deserialize operation model");
                            return;
                        }
                    }

                    this.operationModelService.Handle(session, header.OperationType, operationModel, header.SendParameters, header.ProtocolProviderType, header.CryptoProviderType);

                    if (mStream.Position >= length) return;
                }
            }
        }

        /// <summary>
        /// Called when an error occurs in a session. Currently, no specific error handling is implemented.
        /// </summary>
        /// <param name="session">The session where the error occurred.</param>
        /// <param name="error">The socket error that occurred.</param>
        public override void OnError(ISocketSession session, System.Net.Sockets.SocketError error)
        {
            // No specific error handling implemented.
        }

        /// <summary>
        /// Called when a session is disconnected. Removes the session and cleans up related resources.
        /// </summary>
        /// <param name="session">The session that was disconnected.</param>
        public override void OnDisconnected(ISocketSession session)
        {
            var sessionId = session.GetSessionId();

            if (string.IsNullOrEmpty(sessionId)) return;

            this.sessionService.RemoveSession(session);

            if (this.sessionService.GetSessionCount(sessionId) == 0)
            {
                this.userPeerSessionService.RemoveUserPeer(sessionId);

                var userPeer = this.userPeerSessionService.GetUserPeer(sessionId);

                if (userPeer != null)
                {
                    var userId = userPeer.GetUserId();

                    this.userPeerService.RemoveUserPeer(userId);
                    this.channelService.LeaveAllChannel(userId);
                }
            }
        }

    }

}

using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Interface defining the contract for WebSocket operations and events.
    /// </summary>
    public interface IWebSocket
    {
        /// <summary>
        /// Invoked when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        void OnWsConnecting(HttpRequest request);

        /// <summary>
        /// Invoked when a WebSocket connection has been successfully established.
        /// </summary>
        /// <param name="response">The HTTP response confirming the connection.</param>
        void OnWsConnected(HttpResponse response);

        /// <summary>
        /// Invoked when a WebSocket connection is being established, allowing to modify the response.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        /// <param name="response">The HTTP response confirming the connection.</param>
        /// <returns>True if the connection should be accepted; otherwise, false.</returns>
        bool OnWsConnecting(HttpRequest request, HttpResponse response);

        /// <summary>
        /// Invoked when a WebSocket connection has been successfully established.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        void OnWsConnected(HttpRequest request);

        /// <summary>
        /// Invoked when a WebSocket connection is about to be closed.
        /// </summary>
        void OnWsDisconnecting();

        /// <summary>
        /// Invoked when a WebSocket connection has been closed.
        /// </summary>
        void OnWsDisconnected();

        /// <summary>
        /// Invoked when a WebSocket message has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the received data.</param>
        void OnWsReceived(byte[] buffer, int position, int length);

        /// <summary>
        /// Invoked when a WebSocket close frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the close frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the close frame data.</param>
        /// <param name="status">The close status code.</param>
        void OnWsClose(byte[] buffer, int position, int length, int status = 1000);

        /// <summary>
        /// Invoked when a WebSocket ping frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the ping frame data.</param>
        void OnWsPing(byte[] buffer, int position, int length);

        /// <summary>
        /// Invoked when a WebSocket pong frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the pong frame data.</param>
        void OnWsPong(byte[] buffer, int position, int length);

        /// <summary>
        /// Invoked when a WebSocket error occurs, providing an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        void OnWsError(string error);

        /// <summary>
        /// Invoked when a WebSocket error occurs, providing a SocketError.
        /// </summary>
        /// <param name="error">The SocketError that occurred.</param>
        void OnWsError(SocketError error);

        /// <summary>
        /// Sends an HTTP response to complete the WebSocket upgrade process.
        /// </summary>
        /// <param name="response">The HTTP response to be sent.</param>
        void SendUpgrade(HttpResponse response);

    }

    /// <summary>
    /// An implement of interface WebSocket
    /// </summary>
    public class WebSocket : IWebSocket
    {
        /// <summary>
        /// Provides a thread-safe random number generator for generating random bytes.
        /// </summary>
        class RandomProvider
        {
            /// <summary>
            /// The static random instance used to generate random bytes.
            /// </summary>
            private static Random random { get; }

            static RandomProvider()
            {
                random = new Random();
            }

            /// <summary>
            /// Fills the specified buffer with random bytes.
            /// </summary>
            public static void NextBytes(byte[] buffer) => random.NextBytes(buffer);
        }

        /// <summary>
        /// The magic string used in WebSocket key validation.
        /// </summary>
        private static readonly string WS_MAGIC_STRING_SALT = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        /// <summary>
        /// The origin WebSocket that implements the IWebSocket interface.
        /// </summary>
        private IWebSocket originWs { get; }

        /// <summary>
        /// The operation code for the current WebSocket frame.
        /// </summary>
        private byte opcode { get; set; }

        /// <summary>
        /// The size of the WebSocket frame header.
        /// </summary>
        private int headerSize { get; set; }

        /// <summary>
        /// The size of the WebSocket frame payload.
        /// </summary>
        private int payloadSize { get; set; }

        /// <summary>
        /// The lock object used to synchronize access to the receive buffer.
        /// </summary>
        private object receiveLock { get; }

        /// <summary>
        /// The buffer used to store the received WebSocket frame.
        /// </summary>
        private IMemoryBuffer receiveFrameBuffer { get; }

        /// <summary>
        /// The buffer used to store the final WebSocket frame after processing.
        /// </summary>
        private IMemoryBuffer receiveFinalBuffer { get; }

        /// <summary>
        /// The masking key used to decode the WebSocket frame payload.
        /// </summary>
        private byte[] receiveMask { get; }

        /// <summary>
        /// Indicates whether the WebSocket handshake has been completed.
        /// </summary>
        public bool handshaked { get; set; }

        /// <summary>
        /// Indicates whether the WebSocket frame has been fully received.
        /// </summary>
        private bool frameReceived { get; set; }

        /// <summary>
        /// Indicates whether the WebSocket frame has been fully processed.
        /// </summary>
        private bool finalReceived { get; set; }

        /// <summary>
        /// The lock object used to synchronize access to the send buffer.
        /// </summary>
        public object sendLock { get; }

        /// <summary>
        /// The buffer used to store the WebSocket frame to be sent.
        /// </summary>
        public IMemoryBuffer sendBuffer { get; }

        /// <summary>
        /// The masking key used to encode the WebSocket frame payload.
        /// </summary>
        private byte[] sendMask { get; }

        /// <summary>
        /// The nonce used during the WebSocket handshake.
        /// </summary>
        public byte[] nonce { get; }

        /// <summary>
        /// Initializes a new instance of the WebSocket class with the specified origin WebSocket.
        /// </summary>
        /// <param name="originWs">The origin WebSocket instance.</param>
        public WebSocket(IWebSocket originWs)
        {
            this.originWs = originWs;

            this.receiveLock = new object();

            this.receiveFrameBuffer = new MemoryBuffer();
            this.receiveFinalBuffer = new MemoryBuffer();

            this.receiveMask = new byte[4];
            this.sendLock = new object();
            this.sendBuffer = new MemoryBuffer();

            this.sendMask = new byte[4];
            this.nonce = new byte[16];

            this.ClearWsBuffers();
            this.InitWsNonce();
        }

        /// <summary>
        /// Performs the WebSocket client upgrade.
        /// </summary>
        /// <param name="response">The HTTP response for the upgrade.</param>
        /// <param name="id">The WebSocket ID.</param>
        /// <returns>True if the upgrade was successful, otherwise false.</returns>
        public bool PerformClientUpgrade(HttpResponse response, string id)
        {
            if (response.Status != StatusCode.SwitchingProtocols)
                return false;

            var error = false;
            var accept = false;
            var connection = false;
            var upgrade = false;

            {
                var connectionHeaderValue = response.GetHeader(HeaderNames.Connection);
                if (string.IsNullOrEmpty(connectionHeaderValue) || connectionHeaderValue.ToLower() != HeaderValues.Upgrade)
                {
                    error = true;
                    this.originWs.OnWsError($"Invalid WebSocket handshaked response: '{HeaderNames.Connection}' header value must be '{HeaderValues.Upgrade}'");
                }
                else connection = true;
            }

            if (!error)
            {
                var upgradeHeaderValue = response.GetHeader(HeaderNames.Upgrade);
                if (string.IsNullOrEmpty(upgradeHeaderValue) || upgradeHeaderValue.ToLower() != HeaderValues.WebSocket)
                {
                    error = true;
                    this.originWs.OnWsError($"Invalid WebSocket handshaked response: '{HeaderNames.Connection}' header value must be '{HeaderValues.Upgrade}'");
                }
                else upgrade = true;
            }

            if (!error)
            {
                var secWebSocketAcceptHeaderValue = response.GetHeader(HeaderNames.SecWebSocketAccept);
                if (string.IsNullOrEmpty(secWebSocketAcceptHeaderValue))
                {
                    error = true;
                    this.originWs.OnWsError($"Invalid WebSocket handshaked response: '{HeaderNames.SecWebSocketAccept}' header value must be non empty");
                }
                else
                {
                    var wskey = Convert.ToBase64String(this.nonce) + WS_MAGIC_STRING_SALT;
                    string wshash;
                    using (var sha1 = SHA1.Create())
                        wshash = Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(wskey)));

                    wskey = Encoding.UTF8.GetString(Convert.FromBase64String(secWebSocketAcceptHeaderValue));

                    if (wskey != wshash)
                    {
                        error = true;
                        this.originWs.OnWsError($"Invalid WebSocket handshaked response: '{HeaderNames.SecWebSocketAccept}' value validation failed");
                    }
                    else accept = true;
                }
            }

            if (!accept || !connection || !upgrade)
            {
                if (!error)
                    this.originWs.OnWsError("WebSocket response missing necessary headers");
                return false;
            }

            this.handshaked = true;
            RandomProvider.NextBytes(this.sendMask);
            this.originWs.OnWsConnected(response);

            return true;
        }

        /// <summary>
        /// Performs the WebSocket server upgrade.
        /// </summary>
        /// <param name="request">The HTTP request initiating the upgrade.</param>
        /// <param name="response">The HTTP response for the upgrade.</param>
        /// <returns>True if the upgrade was successful, otherwise false.</returns>
        public bool PerformServerUpgrade(HttpRequest request, HttpResponse response)
        {
            if (request.Method != MethodNames.Get)
                return false;

            var error = false;
            var connection = false;
            var upgrade = false;
            var wsKey = false;
            var wsVersion = false;

            var accept = string.Empty;

            {
                var connectionHeaderValue = request.GetHeader(HeaderNames.Connection);
                if (string.IsNullOrEmpty(connectionHeaderValue) || (connectionHeaderValue.ToLower() != HeaderValues.Upgrade && connectionHeaderValue.ToLower() != HeaderValues.KeepAliveUpgrade))
                {
                    error = true;
                    response.MakeErrorResponse(StatusCode.BadRequest, $"Invalid WebSocket handshaked request: '{HeaderNames.Connection}' header value must be '{HeaderValues.Upgrade}' or '{HeaderValues.KeepAliveUpgrade}'");
                }
                else connection = true;
            }

            {
                var upgradeHeaderValue = request.GetHeader(HeaderNames.Upgrade);
                if (string.IsNullOrEmpty(upgradeHeaderValue) || upgradeHeaderValue.ToLower() != HeaderValues.WebSocket)
                {
                    error = true;
                    response.MakeErrorResponse(StatusCode.BadRequest, $"Invalid WebSocket handshaked request: '{HeaderNames.Upgrade}' header value must be '{HeaderValues.WebSocket}'");
                }
                else upgrade = true;
            }

            {
                var secWebSocketKeyHeaderValue = request.GetHeader(HeaderNames.SecWebSocketKey);
                if (string.IsNullOrEmpty(secWebSocketKeyHeaderValue))
                {
                    error = true;
                    response.MakeErrorResponse(StatusCode.BadRequest, $"Invalid WebSocket handshaked request: '{HeaderNames.SecWebSocketKey}' header value must be non empty");
                }
                else
                {
                    var wskey = secWebSocketKeyHeaderValue + WS_MAGIC_STRING_SALT;
                    byte[] wshash;
                    using (var sha1 = SHA1.Create())
                        wshash = sha1.ComputeHash(Encoding.UTF8.GetBytes(wskey));

                    accept = Convert.ToBase64String(wshash);
                    wsKey = true;
                }
            }

            {
                var secWebSocketVersionHeaderValue = request.GetHeader(HeaderNames.SecWebSocketVersion);
                if (string.IsNullOrEmpty(secWebSocketVersionHeaderValue) || secWebSocketVersionHeaderValue.ToLower() != HeaderValues.SecWsVersion)
                {
                    error = true;
                    response.MakeErrorResponse(StatusCode.BadRequest, $"Invalid WebSocket handshaked request: '{HeaderNames.SecWebSocketVersion}' header value must be '{HeaderValues.SecWsVersion}'");
                }
                else wsVersion = true;
            }

            if (!connection && !upgrade && !wsKey && !wsVersion)
                return false;

            if (!connection || !upgrade || !wsKey || !wsVersion)
            {
                if (!error)
                    response.MakeErrorResponse(StatusCode.BadRequest, "Invalid WebSocket response");
                this.originWs.SendUpgrade(response);
                return false;
            }

            response.Clear();
            response.SetBegin(StatusCode.SwitchingProtocols);
            response.SetHeader(HeaderNames.Connection, HeaderValues.Upgrade);
            response.SetHeader(HeaderNames.Upgrade, HeaderValues.WebSocket);
            response.SetHeader(HeaderNames.SecWebSocketAccept, accept);
            response.SetBody();

            if (!this.originWs.OnWsConnecting(request, response))
                return false;

            this.originWs.SendUpgrade(response);

            this.handshaked = true;

#if NETCOREAPP
            Array.Fill(this.sendMask, (byte)0);
#else
            this.sendMask.Fill((byte)0);
#endif

            this.originWs.OnWsConnected(request);

            return true;
        }

        /// <summary>
        /// Prepares the WebSocket frame to be sent.
        /// </summary>
        /// <param name="opcode">The operation code of the WebSocket frame.</param>
        /// <param name="mask">Indicates whether the frame should be masked.</param>
        /// <param name="buffer">The buffer containing the data to be sent.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data to be sent.</param>
        /// <param name="status">The status code for the frame, if applicable.</param>
        public void PrepareSendFrame(byte opcode, bool mask, byte[] buffer, int position, int length, int status = 0)
        {
            if (!(position == 0 && length == buffer.Length))
                buffer = buffer.ToClone(position, position + length);

            var needSendStatus = ((opcode & WebSocketOpCodes.CLOSE) == WebSocketOpCodes.CLOSE) && ((length > 0) || (status != 0));
            var wsLength = needSendStatus ? (length + 2) : length;

            this.sendBuffer.Clear();

            this.sendBuffer.Write(opcode);

            if (wsLength <= 125)
                this.sendBuffer.Write((byte)((wsLength & 0xFF) | (mask ? 0x80 : 0)));
            else if (wsLength <= 65535)
            {
                this.sendBuffer.Write((byte)(126 | (mask ? 0x80 : 0)));
                this.sendBuffer.Write((byte)((wsLength >> 8) & 0xFF));
                this.sendBuffer.Write((byte)(wsLength & 0xFF));
            }
            else
            {
                this.sendBuffer.Write((byte)(127 | (mask ? 0x80 : 0)));
                for (var i = 7; i >= 0; i--)
                    this.sendBuffer.Write((byte)((wsLength >> (8 * i)) & 0xFF));
            }

            if (mask)
            {
                this.sendBuffer.Write(this.sendMask);
            }

            var wsPosition = this.sendBuffer.Length;
            this.sendBuffer.Resize(this.sendBuffer.Length + wsLength);

            var index = 0;

            if (needSendStatus)
            {
                index += 2;
                this.sendBuffer.Buffer[wsPosition + 0] = (byte)(((status >> 8) & 0xFF) ^ this.sendMask[0]);
                this.sendBuffer.Buffer[wsPosition + 1] = (byte)((status & 0xFF) ^ this.sendMask[1]);
            }

            for (var i = index; i < wsLength; i++)
                this.sendBuffer.Buffer[wsPosition + i] = (byte)(buffer[i - index] ^ this.sendMask[i % 4]);
        }

        /// <summary>
        /// Prepares the WebSocket frame received for processing.
        /// </summary>
        /// <param name="buffer">The buffer containing the received frame.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the received data.</param>
        public void PrepareReceiveFrame(byte[] buffer, int position, int length)
        {
            lock (this.receiveLock)
            {
                int index = 0;

                if (this.frameReceived)
                {
                    this.frameReceived = false;
                    this.headerSize = 0;
                    this.payloadSize = 0;
                    this.receiveFrameBuffer.Clear();
                    Array.Clear(this.receiveMask, 0, this.receiveMask.Length);
                }
                if (this.finalReceived)
                {
                    this.finalReceived = false;
                    this.receiveFinalBuffer.Clear();
                }

                while (length > 0)
                {
                    if (this.frameReceived)
                    {
                        this.frameReceived = false;
                        this.headerSize = 0;
                        this.payloadSize = 0;
                        this.receiveFrameBuffer.Clear();
                        Array.Clear(this.receiveMask, 0, this.receiveMask.Length);
                    }
                    if (this.finalReceived)
                    {
                        this.finalReceived = false;
                        this.receiveFinalBuffer.Clear();
                    }

                    if (this.receiveFrameBuffer.Length < 2)
                    {
                        for (var i = 0; i < 2; i++, index++, length--)
                        {
                            if (length == 0)
                                return;
                            this.receiveFrameBuffer.Write(buffer[position + index]);
                        }
                    }

                    var opcode = (byte)(this.receiveFrameBuffer[0] & 0x0F);
                    var fin = ((this.receiveFrameBuffer[0] >> 7) & 0x01) != 0;
                    var mask = ((this.receiveFrameBuffer[1] >> 7) & 0x01) != 0;
                    var payload = this.receiveFrameBuffer[1] & (~0x80);

                    this.opcode = (opcode != 0) ? opcode : this.opcode;

                    if (payload <= 125)
                    {
                        this.headerSize = 2 + (mask ? 4 : 0);
                        this.payloadSize = payload;
                    }
                    else if (payload == 126)
                    {
                        if (this.receiveFrameBuffer.Length < 4)
                        {
                            for (var i = 0; i < 2; i++, index++, length--)
                            {
                                if (length == 0)
                                    return;
                                this.receiveFrameBuffer.Write(buffer[position + index]);
                            }
                        }

                        payload = ((this.receiveFrameBuffer[2] << 8) | (this.receiveFrameBuffer[3] << 0));
                        this.headerSize = 4 + (mask ? 4 : 0);
                        this.payloadSize = payload;
                    }
                    else if (payload == 127)
                    {
                        if (this.receiveFrameBuffer.Length < 10)
                        {
                            for (var i = 0; i < 8; i++, index++, length--)
                            {
                                if (length == 0)
                                    return;
                                this.receiveFrameBuffer.Write(buffer[position + index]);
                            }
                        }

                        payload = ((this.receiveFrameBuffer[2] << 56) | (this.receiveFrameBuffer[3] << 48) | (this.receiveFrameBuffer[4] << 40) | (this.receiveFrameBuffer[5] << 32) | (this.receiveFrameBuffer[6] << 24) | (this.receiveFrameBuffer[7] << 16) | (this.receiveFrameBuffer[8] << 8) | (this.receiveFrameBuffer[9] << 0));
                        this.headerSize = 10 + (mask ? 4 : 0);
                        this.payloadSize = payload;
                    }

                    if (mask)
                    {
                        if (this.receiveFrameBuffer.Length < this.headerSize)
                        {
                            for (var i = 0; i < 4; i++, index++, length--)
                            {
                                if (length == 0)
                                    return;
                                this.receiveFrameBuffer.Write(buffer[position + index]);
                                this.receiveMask[i] = buffer[position + index];
                            }
                        }
                    }

                    var total = this.headerSize + this.payloadSize;
                    var minimumLength = Math.Min(total - this.receiveFrameBuffer.Length, length);

                    this.receiveFrameBuffer.Write(
#if NETCOREAPP
                    buffer[(position + index)..(position + index + minimumLength)]
#else
                        buffer.ToClone((position + index), (position + index + minimumLength))
#endif
                        );
                    index += minimumLength;
                    length -= minimumLength;

                    if (this.receiveFrameBuffer.Length == total)
                    {
                        if (mask)
                        {
                            for (var i = 0; i < this.payloadSize; i++)
                                this.receiveFinalBuffer.Write((byte)(this.receiveFrameBuffer[this.headerSize + i] ^ this.receiveMask[i % 4]));
                        }
                        else
                            this.receiveFinalBuffer.Write(this.receiveFrameBuffer.ToArray(this.headerSize, this.payloadSize));

                        this.frameReceived = true;

                        if (fin)
                        {
                            this.finalReceived = true;

                            switch (this.opcode)
                            {
                                case WebSocketOpCodes.PING:
                                    {
                                        this.originWs.OnWsPing(this.receiveFinalBuffer.Buffer, 0, this.receiveFinalBuffer.Length);
                                        break;
                                    }
                                case WebSocketOpCodes.PONG:
                                    {
                                        this.originWs.OnWsPong(this.receiveFinalBuffer.Buffer, 0, this.receiveFinalBuffer.Length);
                                        break;
                                    }
                                case WebSocketOpCodes.CLOSE:
                                    {
                                        var sindex = 0;
                                        var status = 1000;

                                        if (this.receiveFinalBuffer.Length >= 2)
                                        {
                                            sindex += 2;
                                            status = (this.receiveFinalBuffer[0] << 8) | (this.receiveFinalBuffer[1] << 0);
                                        }

                                        this.originWs.OnWsClose(this.receiveFinalBuffer.Buffer, sindex, this.receiveFinalBuffer.Length - sindex, status);
                                        break;
                                    }
                                case WebSocketOpCodes.BINARY:
                                case WebSocketOpCodes.TEXT:
                                    {
                                        this.originWs.OnWsReceived(this.receiveFinalBuffer.Buffer, 0, this.receiveFinalBuffer.Length);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears the WebSocket buffers.
        /// </summary>
        public void ClearWsBuffers()
        {
            var acquiredReceiveLock = false;

            try
            {
                Monitor.TryEnter(this.receiveLock, ref acquiredReceiveLock);
                if (acquiredReceiveLock)
                {
                    this.frameReceived = false;
                    this.finalReceived = false;
                    this.headerSize = 0;
                    this.payloadSize = 0;
                    this.receiveFrameBuffer.Clear();
                    this.receiveFinalBuffer.Clear();
                    Array.Clear(this.receiveMask, 0, this.receiveMask.Length);
                }
            }
            finally
            {
                if (acquiredReceiveLock)
                    Monitor.Exit(this.receiveLock);
            }

            lock (this.sendLock)
            {
                this.sendBuffer.Clear();
                Array.Clear(this.sendMask, 0, this.sendMask.Length);
            }
        }

        /// <summary>
        /// Initializes the WebSocket nonce with random bytes.
        /// </summary>
        public void InitWsNonce() => RandomProvider.NextBytes(this.nonce);

        /// <summary>
        /// Invoked when a WebSocket connection is being established.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        public void OnWsConnecting(HttpRequest request) { }

        /// <summary>
        /// Invoked when a WebSocket connection has been successfully established.
        /// </summary>
        /// <param name="response">The HTTP response confirming the connection.</param>
        public void OnWsConnected(HttpResponse response) { }

        /// <summary>
        /// Invoked when a WebSocket connection is being established, allowing to modify the response.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        /// <param name="response">The HTTP response confirming the connection.</param>
        /// <returns>True if the connection should be accepted; otherwise, false.</returns>
        public bool OnWsConnecting(HttpRequest request, HttpResponse response) => true;

        /// <summary>
        /// Invoked when a WebSocket connection has been successfully established.
        /// </summary>
        /// <param name="request">The HTTP request initiating the connection.</param>
        public void OnWsConnected(HttpRequest request) { }

        /// <summary>
        /// Invoked when a WebSocket connection is about to be closed.
        /// </summary>
        public void OnWsDisconnecting() { }

        /// <summary>
        /// Invoked when a WebSocket connection has been closed.
        /// </summary>
        public void OnWsDisconnected() { }

        /// <summary>
        /// Invoked when a WebSocket message has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the received data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the received data.</param>
        public void OnWsReceived(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Invoked when a WebSocket close frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the close frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the close frame data.</param>
        /// <param name="status">The close status code.</param>
        public void OnWsClose(byte[] buffer, int position, int length, int status = 1000) { }

        /// <summary>
        /// Invoked when a WebSocket ping frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the ping frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the ping frame data.</param>
        public void OnWsPing(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Invoked when a WebSocket pong frame has been received.
        /// </summary>
        /// <param name="buffer">The buffer containing the pong frame data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the pong frame data.</param>
        public void OnWsPong(byte[] buffer, int position, int length) { }

        /// <summary>
        /// Invoked when a WebSocket error occurs, providing an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        public void OnWsError(string error) { }

        /// <summary>
        /// Invoked when a WebSocket error occurs, providing a SocketError.
        /// </summary>
        /// <param name="error">The SocketError that occurred.</param>
        public void OnWsError(SocketError error) { }

        /// <summary>
        /// Sends an HTTP response to complete the WebSocket upgrade process.
        /// </summary>
        /// <param name="response">The HTTP response to be sent.</param>
        public void SendUpgrade(HttpResponse response) { }

    }

}

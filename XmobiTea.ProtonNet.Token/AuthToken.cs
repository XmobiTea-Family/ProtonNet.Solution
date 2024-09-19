using System.IO;
using XmobiTea.Binary;
using XmobiTea.ProtonNet.Token.Deserialize;
using XmobiTea.ProtonNet.Token.Exceptions;
using XmobiTea.ProtonNet.Token.Factory;
using XmobiTea.ProtonNet.Token.Helper;
using XmobiTea.ProtonNet.Token.Models;
using XmobiTea.ProtonNet.Token.Serialize;
using XmobiTea.ProtonNet.Token.Services;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token
{
    /// <summary>
    /// Implements the IAuthToken interface for encoding, decoding, and verifying authentication tokens.
    /// </summary>
    public class AuthToken : IAuthToken
    {
        /// <summary>
        /// The token version.
        /// </summary>
        private static readonly byte Version = 1;

        /// <summary>
        /// Factory for creating token binaries.
        /// </summary>
        private ITokenBinaryFactory tokenBinaryFactory { get; }

        /// <summary>
        /// Serializer for encoding token headers and payloads.
        /// </summary>
        private ITokenSerializer tokenSerializer { get; }

        /// <summary>
        /// Deserializer for decoding token headers and payloads.
        /// </summary>
        private ITokenDeserializer tokenDeserializer { get; }

        /// <summary>
        /// Initializes a new instance of the AuthToken class.
        /// </summary>
        public AuthToken()
        {
            var tokenMemberPropertyService = new TokenMemberPropertyService();

            this.tokenBinaryFactory = new TokenBinaryFactory();
            var algorithmFactory = new TokenAlgorithmFactory();

            this.tokenSerializer = new TokenSerializer(tokenMemberPropertyService, this.tokenBinaryFactory, algorithmFactory);
            this.tokenDeserializer = new TokenDeserializer(tokenMemberPropertyService, this.tokenBinaryFactory);
        }

        /// <summary>
        /// Sets the binary converter for the specified binary type.
        /// </summary>
        /// <param name="binaryType">The binary type.</param>
        /// <param name="binaryConverter">The binary converter.</param>
        public void SetBinaryConverter(TokenBinaryType binaryType, IBinaryConverter binaryConverter)
        {
            ((TokenBinaryFactory)this.tokenBinaryFactory).AddBinaryConverter(binaryType, binaryConverter);
        }

        /// <summary>
        /// Encodes the specified payload into a token.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="payload">The payload to encode.</param>
        /// <param name="key">The key used for signing the token.</param>
        /// <param name="options">Optional encoding options.</param>
        /// <returns>The encoded token as a string.</returns>
        public string Encode<T>(T payload, string key, TokenOptions options = null) where T : ITokenPayload
        {
            if (options == null)
                options = new TokenOptions()
                {
                    AlgorithmType = TokenAlgorithmType.SHA256,
                    BinaryType = TokenBinaryType.SimplePack,
                    ExpiredAfterSeconds = 24 * 60 * 60,
                };

            var header = new TokenHeader()
            {
                Version = Version,
                BinaryType = options.BinaryType,
                AlgorithmType = options.AlgorithmType,
                ExpiredAtUtcTicks = System.DateTime.UtcNow.AddSeconds(options.ExpiredAfterSeconds).Ticks,
            };

            var headerBytes = this.tokenSerializer.GenerateHeader(options.BinaryType, header);
            var payloadBytes = this.tokenSerializer.GeneratePayload(options.BinaryType, payload);
            var signatureBytes = this.tokenSerializer.GenerateSignature(options.AlgorithmType, headerBytes, payloadBytes, key);

            var signatureStr = System.Text.Encoding.UTF8.GetString(signatureBytes);
            var signatureMd5Str = MD5.CreateMD5Hash(signatureStr);
            var signatureMd5Bytes = System.Text.Encoding.UTF8.GetBytes(signatureMd5Str);

            byte[] tokenBytes;

            using (var mStream = new MemoryStream())
            {
                mStream.WriteByte((byte)options.BinaryType);

                var headerDataBytes = System.BitConverter.GetBytes((ushort)headerBytes.Length);
                BinaryUtils.SwapIfLittleEndian(ref headerDataBytes);
                mStream.Write(headerDataBytes, 0, 2);
                mStream.Write(headerBytes, 0, headerBytes.Length);

                var payloadDataBytes = System.BitConverter.GetBytes((ushort)payloadBytes.Length);
                BinaryUtils.SwapIfLittleEndian(ref payloadDataBytes);
                mStream.Write(payloadDataBytes, 0, 2);
                mStream.Write(payloadBytes, 0, payloadBytes.Length);

                mStream.Write(signatureMd5Bytes, 0, signatureMd5Bytes.Length);

                tokenBytes = mStream.ToArray();
            }

            var token = System.Convert.ToBase64String(tokenBytes);

            return token;
        }

        /// <summary>
        /// Decodes the specified token into a header and payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="token">The token to decode.</param>
        /// <param name="header">Outputs the token header.</param>
        /// <param name="payload">Outputs the token payload.</param>
        /// <exception cref="TokenStringInvalidException">Thrown if the token is invalid.</exception>
        public void Decode<T>(string token, out ITokenHeader header, out T payload) where T : ITokenPayload
        {
            var tokenBytes = System.Convert.FromBase64String(token);

            TokenBinaryType binaryType;
            byte[] headerBytes;
            byte[] payloadBytes;

            using (var mStream = new MemoryStream(tokenBytes))
            {
                binaryType = (TokenBinaryType)(byte)mStream.ReadByte();

                var headerDataBytes = this.ReadBytes(mStream, 2);
                BinaryUtils.SwapIfLittleEndian(ref headerDataBytes);
                var headerLength = System.BitConverter.ToUInt16(headerDataBytes, 0);
                headerBytes = this.ReadBytes(mStream, headerLength);

                var payloadDataBytes = this.ReadBytes(mStream, 2);
                BinaryUtils.SwapIfLittleEndian(ref payloadDataBytes);
                var payloadLength = System.BitConverter.ToUInt16(payloadDataBytes, 0);
                payloadBytes = this.ReadBytes(mStream, payloadLength);
            }

            try
            {
                header = this.tokenDeserializer.GenerateHeader(binaryType, headerBytes);
                payload = this.tokenDeserializer.GeneratePayload<T>(binaryType, payloadBytes);
            }
            catch
            {
                throw new TokenStringInvalidException();
            }
        }

        /// <summary>
        /// Verifies the specified token using the provided key and outputs the header and payload.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="token">The token to verify.</param>
        /// <param name="key">The key used for verification.</param>
        /// <param name="header">Outputs the verified token header.</param>
        /// <param name="payload">Outputs the verified token payload.</param>
        /// <exception cref="TokenSignatureInvalidException">Thrown if the token signature is invalid.</exception>
        /// <exception cref="TokenExpiredException">Thrown if the token has expired.</exception>
        public void Verify<T>(string token, string key, out ITokenHeader header, out T payload) where T : ITokenPayload
        {
            var tokenBytes = System.Convert.FromBase64String(token);

            TokenBinaryType binaryType;
            byte[] headerBytes;
            byte[] payloadBytes;

            byte[] signatureMd5Bytes;

            using (var mStream = new MemoryStream(tokenBytes))
            {
                binaryType = (TokenBinaryType)(byte)mStream.ReadByte();

                var headerDataBytes = this.ReadBytes(mStream, 2);
                BinaryUtils.SwapIfLittleEndian(ref headerDataBytes);
                var headerLength = System.BitConverter.ToUInt16(headerDataBytes, 0);
                headerBytes = this.ReadBytes(mStream, headerLength);

                var payloadDataBytes = this.ReadBytes(mStream, 2);
                BinaryUtils.SwapIfLittleEndian(ref payloadDataBytes);
                var payloadLength = System.BitConverter.ToUInt16(payloadDataBytes, 0);
                payloadBytes = this.ReadBytes(mStream, payloadLength);

                signatureMd5Bytes = this.ReadBytes(mStream, (int)(mStream.Length - mStream.Position));
            }

            try
            {
                header = this.tokenDeserializer.GenerateHeader(binaryType, headerBytes);
                payload = this.tokenDeserializer.GeneratePayload<T>(binaryType, payloadBytes);
            }
            catch
            {
                throw new TokenStringInvalidException();
            }

            var signatureMd5Str = System.Text.Encoding.UTF8.GetString(signatureMd5Bytes);

            var signatureBytes = this.tokenSerializer.GenerateSignature(header.AlgorithmType, headerBytes, payloadBytes, key);
            var signatureStr = System.Text.Encoding.UTF8.GetString(signatureBytes);
            var originSignatureMd5Str = MD5.CreateMD5Hash(signatureStr);

            if (signatureMd5Str != originSignatureMd5Str)
            {
                throw new TokenSignatureInvalidException();
            }

            if (header.ExpiredAtUtcTicks < System.DateTime.UtcNow.Ticks)
            {
                throw new TokenExpiredException();
            }
        }

        /// <summary>
        /// Reads a specified number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>A byte array containing the data read from the stream.</returns>
        protected byte[] ReadBytes(Stream stream, int length)
        {
            var answer = new byte[length];
            stream.Read(answer, 0, length);
            return answer;
        }

    }

}

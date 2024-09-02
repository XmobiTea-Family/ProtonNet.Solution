using System.Reflection;
using XmobiTea.ProtonNet.Token.Attributes;
using XmobiTea.ProtonNet.Token.Factory;
using XmobiTea.ProtonNet.Token.Helper;
using XmobiTea.ProtonNet.Token.Models;
using XmobiTea.ProtonNet.Token.Services;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token.Serialize
{
    /// <summary>
    /// Interface for token serialization operations
    /// </summary>
    interface ITokenSerializer
    {
        /// <summary>
        /// Generates the header bytes for the token
        /// </summary>
        /// <param name="binaryType">The type of binary encoding</param>
        /// <param name="header">Token header information</param>
        /// <returns>Serialized byte array of the token header</returns>
        byte[] GenerateHeader(TokenBinaryType binaryType, ITokenHeader header);

        /// <summary>
        /// Generates the payload bytes for the token
        /// </summary>
        /// <typeparam name="T">Type of the token payload</typeparam>
        /// <param name="binaryType">The type of binary encoding</param>
        /// <param name="payload">Token payload data</param>
        /// <returns>Serialized byte array of the token payload</returns>
        byte[] GeneratePayload<T>(TokenBinaryType binaryType, T payload) where T : ITokenPayload;

        /// <summary>
        /// Generates the signature bytes for the token
        /// </summary>
        /// <param name="algorithmType">The type of algorithm used</param>
        /// <param name="headerToken">Serialized header token bytes</param>
        /// <param name="payloadToken">Serialized payload token bytes</param>
        /// <param name="key">Key used for generating the signature</param>
        /// <returns>Generated signature byte array</returns>
        byte[] GenerateSignature(TokenAlgorithmType algorithmType, byte[] headerToken, byte[] payloadToken, string key);

    }

    /// <summary>
    /// Implementation of the token serializer interface
    /// </summary>
    class TokenSerializer : ITokenSerializer
    {
        /// <summary>
        /// Service for handling token member properties
        /// </summary>
        private ITokenMemberPropertyService tokenMemberPropertyService { get; }

        /// <summary>
        /// Factory for binary encoding
        /// </summary>
        private ITokenBinaryFactory binaryFactory { get; }

        /// <summary>
        /// Factory for algorithm encoding
        /// </summary>
        private ITokenAlgorithmFactory algorithmFactory { get; }

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="tokenMemberPropertyService">Token member property service</param>
        /// <param name="binaryFactory">Binary encoding factory</param>
        /// <param name="algorithmFactory">Algorithm encoding factory</param>
        public TokenSerializer(ITokenMemberPropertyService tokenMemberPropertyService, ITokenBinaryFactory binaryFactory, ITokenAlgorithmFactory algorithmFactory)
        {
            this.tokenMemberPropertyService = tokenMemberPropertyService;
            this.binaryFactory = binaryFactory;
            this.algorithmFactory = algorithmFactory;
        }

        /// <summary>
        /// Generates the header bytes for the token
        /// </summary>
        /// <param name="binaryType">The type of binary encoding</param>
        /// <param name="header">Token header information</param>
        /// <returns>Serialized byte array of the token header</returns>
        public byte[] GenerateHeader(TokenBinaryType binaryType, ITokenHeader header)
        {
            var headerObjs = this.ConvertTokenHeader(header);

            var binaryEncoder = this.binaryFactory.GetEncode(binaryType);

            return binaryEncoder.SerializeHeader(headerObjs);
        }

        /// <summary>
        /// Generates the payload bytes for the token
        /// </summary>
        /// <typeparam name="T">Type of the token payload</typeparam>
        /// <param name="binaryType">The type of binary encoding</param>
        /// <param name="payload">Token payload data</param>
        /// <returns>Serialized byte array of the token payload</returns>
        public byte[] GeneratePayload<T>(TokenBinaryType binaryType, T payload) where T : ITokenPayload
        {
            var payloadDict = this.ConvertTokenPayload(payload);

            var binaryEncoder = this.binaryFactory.GetEncode(binaryType);

            return binaryEncoder.SerializePayload(payloadDict);
        }

        /// <summary>
        /// Generates the signature bytes for the token
        /// </summary>
        /// <param name="algorithmType">The type of algorithm used</param>
        /// <param name="headerToken">Serialized header token bytes</param>
        /// <param name="payloadToken">Serialized payload token bytes</param>
        /// <param name="key">Key used for generating the signature</param>
        /// <returns>Generated signature byte array</returns>
        public byte[] GenerateSignature(TokenAlgorithmType algorithmType, byte[] headerToken, byte[] payloadToken, string key)
        {
            var keyMD5 = MD5.CreateMD5Hash(key);

            var keyBase64String = System.Convert.FromBase64String(keyMD5);
            var keyUTF8String = System.Text.Encoding.UTF8.GetBytes(keyMD5);

            byte[] signature;

            using (var mStream = new System.IO.MemoryStream())
            {
                mStream.Write(keyUTF8String, 0, keyUTF8String.Length);
                mStream.Write(headerToken, 0, headerToken.Length);
                mStream.Write(keyBase64String, 0, keyBase64String.Length);
                mStream.Write(payloadToken, 0, payloadToken.Length);

                signature = mStream.ToArray();
            }

            var algorithmEncoder = this.algorithmFactory.GetEncode(algorithmType);

            return algorithmEncoder.Encrypt(signature);
        }

        /// <summary>
        /// Converts the token header to an array of objects
        /// </summary>
        /// <param name="header">Token header information</param>
        /// <returns>Array of objects representing the token header</returns>
        private object[] ConvertTokenHeader(ITokenHeader header)
        {
            var answer = new object[] { (byte)header.Version, (byte)header.BinaryType, (byte)header.AlgorithmType, (long)header.ExpiredAtUtcTicks };

            return answer;
        }

        /// <summary>
        /// Converts the token payload to a dictionary of objects
        /// </summary>
        /// <param name="payload">Token payload data</param>
        /// <returns>Dictionary of objects representing the token payload</returns>
        private System.Collections.Generic.IDictionary<byte, object> ConvertTokenPayload(ITokenPayload payload)
        {
            var answer = new System.Collections.Generic.Dictionary<byte, object>();

            var type = payload.GetType();

            var properties = this.tokenMemberPropertyService.GetProperties(type);

            foreach (var property in properties)
            {
                var tokenMemberAttribute = property.GetCustomAttribute<TokenMemberAttribute>();

                var value = property.GetValue(payload);

                if (value == null)
                    answer[tokenMemberAttribute.Code] = null;
                else
                {
                    if (value is ITokenPayload childPayload)
                        answer[tokenMemberAttribute.Code] = this.ConvertTokenPayload(childPayload);
                    else
                        answer[tokenMemberAttribute.Code] = value;
                }
            }

            return answer;
        }

    }

}

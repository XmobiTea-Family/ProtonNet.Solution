using System;
using System.Collections.Generic;
using System.Reflection;
using XmobiTea.ProtonNet.Token.Attributes;
using XmobiTea.ProtonNet.Token.Factory;
using XmobiTea.ProtonNet.Token.Models;
using XmobiTea.ProtonNet.Token.Services;
using XmobiTea.ProtonNet.Token.Types;

namespace XmobiTea.ProtonNet.Token.Deserialize
{
    /// <summary>
    /// Provides methods for deserializing token headers and payloads.
    /// </summary>
    interface ITokenDeserializer
    {
        /// <summary>
        /// Generates a token header from the given binary data.
        /// </summary>
        /// <param name="binaryType">The binary type used for the token.</param>
        /// <param name="header">The binary data representing the header.</param>
        /// <returns>An instance of ITokenHeader representing the deserialized header.</returns>
        ITokenHeader GenerateHeader(TokenBinaryType binaryType, byte[] header);

        /// <summary>
        /// Generates a token payload of the specified type from the given binary data.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="binaryType">The binary type used for the token.</param>
        /// <param name="payload">The binary data representing the payload.</param>
        /// <returns>An instance of the specified type representing the deserialized payload.</returns>
        T GeneratePayload<T>(TokenBinaryType binaryType, byte[] payload) where T : ITokenPayload;
    }

    /// <summary>
    /// Implementation of ITokenDeserializer that handles the deserialization of token headers and payloads.
    /// </summary>
    class TokenDeserializer : ITokenDeserializer
    {
        private ITokenMemberPropertyService tokenMemberPropertyService { get; }
        private ITokenBinaryFactory binaryFactory { get; }

        /// <summary>
        /// Initializes a new instance of the TokenDeserializer class with the specified services.
        /// </summary>
        /// <param name="tokenMemberPropertyService">Service for retrieving token member properties.</param>
        /// <param name="binaryFactory">Factory for creating binary encoders and decoders.</param>
        public TokenDeserializer(ITokenMemberPropertyService tokenMemberPropertyService, ITokenBinaryFactory binaryFactory)
        {
            this.tokenMemberPropertyService = tokenMemberPropertyService;
            this.binaryFactory = binaryFactory;
        }

        /// <summary>
        /// Deserializes the token header using the specified binary type and header data.
        /// </summary>
        /// <param name="binaryType">The binary type used for the token.</param>
        /// <param name="header">The binary data representing the header.</param>
        /// <returns>An instance of ITokenHeader representing the deserialized header.</returns>
        public ITokenHeader GenerateHeader(TokenBinaryType binaryType, byte[] header)
        {
            var binaryDecoder = this.binaryFactory.GetDecode(binaryType);
            var headerObjs = binaryDecoder.DeserializeHeader(header);
            return this.ConvertTokenHeader(headerObjs);
        }

        /// <summary>
        /// Deserializes the token payload using the specified binary type and payload data.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="binaryType">The binary type used for the token.</param>
        /// <param name="payload">The binary data representing the payload.</param>
        /// <returns>An instance of the specified type representing the deserialized payload.</returns>
        public T GeneratePayload<T>(TokenBinaryType binaryType, byte[] payload) where T : ITokenPayload
        {
            var binaryDecoder = this.binaryFactory.GetDecode(binaryType);
            var payloadDict = binaryDecoder.DeserializePayload(payload);
            return (T)this.ConvertTokenPayload(payloadDict, typeof(T));
        }

        /// <summary>
        /// Converts an array of objects into an instance of ITokenHeader.
        /// </summary>
        /// <param name="headerObjs">The array of objects representing the header.</param>
        /// <returns>An instance of ITokenHeader representing the header.</returns>
        private ITokenHeader ConvertTokenHeader(object[] headerObjs)
        {
            var answer = new TokenHeader()
            {
                Version = Convert.ToByte(headerObjs[0]),
                BinaryType = (TokenBinaryType)Convert.ToByte(headerObjs[1]),
                AlgorithmType = (TokenAlgorithmType)Convert.ToByte(headerObjs[2]),
                ExpiredAtUtcTicks = Convert.ToInt64(headerObjs[3]),
            };

            return answer;
        }

        /// <summary>
        /// Converts a dictionary of byte keys and object values into an instance of the specified payload type.
        /// </summary>
        /// <param name="payloadDict">The dictionary of byte keys and object values representing the payload.</param>
        /// <param name="type">The type of the payload to create.</param>
        /// <returns>An instance of the specified payload type.</returns>
        private object ConvertTokenPayload(IDictionary<byte, object> payloadDict, Type type)
        {
            var answer = Activator.CreateInstance(type);

            var properties = this.tokenMemberPropertyService.GetProperties(type);

            foreach (var property in properties)
            {
                var tokenMemberAttribute = property.GetCustomAttribute<TokenMemberAttribute>();

                if (payloadDict.ContainsKey(tokenMemberAttribute.Code))
                {
                    var value = payloadDict[tokenMemberAttribute.Code];

                    if (value != null)
                    {
                        if (value is IDictionary<byte, object> childPayloadDict)
                        {
                            property.SetValue(answer, this.ConvertTokenPayload(childPayloadDict, property.PropertyType));
                        }
                        else
                        {
                            property.SetValue(answer, Convert.ChangeType(value, property.PropertyType));
                        }
                    }
                }
            }

            return answer;
        }

    }

}

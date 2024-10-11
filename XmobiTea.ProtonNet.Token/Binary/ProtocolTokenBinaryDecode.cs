﻿using System.Collections.Generic;
using XmobiTea.Binary;

namespace XmobiTea.ProtonNet.Token.Binary
{
    /// <summary>
    /// Implements the ITokenBinaryDecode interface for deserializing binary data.
    /// </summary>
    class ProtocolTokenBinaryDecode : ITokenBinaryDecode
    {
        private IBinaryConverter binaryConverter { get; }

        /// <summary>
        /// Initializes a new instance of the ProtocolTokenBinaryDecode class.
        /// </summary>
        /// <param name="binaryConverter">The binary converter used for deserialization.</param>
        public ProtocolTokenBinaryDecode(IBinaryConverter binaryConverter) => this.binaryConverter = binaryConverter;

        /// <summary>
        /// Deserializes a byte array representing a header into an array of objects.
        /// </summary>
        /// <param name="header">The byte array representing the header.</param>
        /// <returns>An array of objects representing the deserialized header.</returns>
        public object[] DeserializeHeader(byte[] header) => this.binaryConverter.Deserialize<object[]>(header);

        /// <summary>
        /// Deserializes a byte array representing the payload into a dictionary.
        /// </summary>
        /// <param name="payload">The byte array representing the payload.</param>
        /// <returns>A dictionary where the key is a byte and the value is an object representing the deserialized payload.</returns>
        public IDictionary<byte, object> DeserializePayload(byte[] payload)
        {
            var answer = this.binaryConverter.Deserialize<object>(payload);

            if (answer is IDictionary<byte, object> answerDict)
                return answerDict;

            return this.ConvertDictionary<byte, object>((IDictionary<object, object>)answer);
        }

        private IDictionary<K, V> ConvertDictionary<K, V>(IDictionary<object, object> dict)
        {
            var answer = new Dictionary<K, V>();

            var typeofK = typeof(K);
            var typeofV = typeof(V);

            foreach (var kvp in dict)
            {
                var key = (K)System.Convert.ChangeType(kvp.Key, typeofK);
                var value = (V)System.Convert.ChangeType(kvp.Value, typeofV);
                answer.Add(key, value);
            }

            return answer;
        }
    }

}

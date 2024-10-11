using System;
using System.Collections.Generic;
using XmobiTea.Binary;
using XmobiTea.Data;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.RpcProtocol
{
    /// <summary>
    /// Interface defining the contract for deserialization 
    /// support of operation models in the XmobiTea.ProtonNet library.
    /// </summary>
    interface IOperationDeserializeSupport
    {
        /// <summary>
        /// Deserializes the given payload into an operation model using the specified operation type and binary converter.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        IOperationModel Deserialize(OperationType operationType, IBinaryConverter binaryConverter, byte[] payload);

    }

    /// <summary>
    /// Implementation of IOperationDeserializeSupport that provides 
    /// deserialization support for various operation types.
    /// </summary>
    class OperationDeserializeSupport : IOperationDeserializeSupport
    {
        /// <summary>
        /// Delegate for handling the deserialization of operation models.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        delegate IOperationModel OperationModelDeserialize(IBinaryConverter binaryConverter, byte[] payload);

        /// <summary>
        /// Dictionary mapping operation types to their corresponding deserialization handlers.
        /// </summary>
        private IDictionary<OperationType, OperationModelDeserialize> operationModelDeserializeDict { get; }

        /// <summary>
        /// Initializes a new instance of the OperationDeserializeSupport class.
        /// </summary>
        public OperationDeserializeSupport()
        {
            this.operationModelDeserializeDict = new Dictionary<OperationType, OperationModelDeserialize>();
            this.AddOperationModelDeserializes();
        }

        /// <summary>
        /// Adds the deserialization handlers for the supported operation types.
        /// </summary>
        private void AddOperationModelDeserializes()
        {
            this.operationModelDeserializeDict[OperationType.OperationPing] = this.OperationPingDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationPong] = this.OperationPongDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationHandshake] = this.OperationHandshakeDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationHandshakeAck] = this.OperationHandshakeAckDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationDisconnect] = this.OperationDisconnectDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationRequest] = this.OperationRequestDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationResponse] = this.OperationResponseDeserialize;
            this.operationModelDeserializeDict[OperationType.OperationEvent] = this.OperationEventDeserialize;
        }

        /// <summary>
        /// Deserializes the given payload into an operation model using the specified operation type and binary converter.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        /// <exception cref="ArgumentException">Thrown when the operation type is not supported.</exception>
        public IOperationModel Deserialize(OperationType operationType, IBinaryConverter binaryConverter, byte[] payload)
        {
            if (!this.operationModelDeserializeDict.TryGetValue(operationType, out var handler))
                throw new ArgumentException("OperationType " + operationType + " not supported.");

            return handler.Invoke(binaryConverter, payload);
        }

        /// <summary>
        /// Deserialization handler for OperationPing.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationPingDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            return new OperationPing();
        }

        /// <summary>
        /// Deserialization handler for OperationPong.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationPongDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            return new OperationPong();
        }

        /// <summary>
        /// Deserialization handler for OperationRequest.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationRequestDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            var parametersDict = (System.Collections.IDictionary)objArrs[1];
            var parameters = parametersDict == null ? null : GNHashtable.NewBuilder().AddAll(parametersDict).Build();

            return new OperationRequest(XmobiTea.Binary.Helper.Convert.ToString(objArrs[0]), parameters)
            {
                RequestId = XmobiTea.Binary.Helper.Convert.ToUInt16(objArrs[2]),
            };
        }

        /// <summary>
        /// Deserialization handler for OperationResponse.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationResponseDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            var parametersDict = (System.Collections.IDictionary)objArrs[1];
            var parameters = parametersDict == null ? null : GNHashtable.NewBuilder().AddAll(parametersDict).Build();

            return new OperationResponse(XmobiTea.Binary.Helper.Convert.ToString(objArrs[0]), parameters)
            {
                ResponseId = XmobiTea.Binary.Helper.Convert.ToUInt16(objArrs[2]),
                ReturnCode = XmobiTea.Binary.Helper.Convert.ToByte(objArrs[3]),
                DebugMessage = XmobiTea.Binary.Helper.Convert.ToString(objArrs[4]),
            };
        }

        /// <summary>
        /// Deserialization handler for OperationEvent.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationEventDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            var parametersDict = (System.Collections.IDictionary)objArrs[1];
            var parameters = parametersDict == null ? null : GNHashtable.NewBuilder().AddAll(parametersDict).Build();

            return new OperationEvent(XmobiTea.Binary.Helper.Convert.ToString(objArrs[0]), parameters);
        }

        /// <summary>
        /// Deserialization handler for OperationHandshake.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationHandshakeDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            return new OperationHandshake()
            {
                SessionId = XmobiTea.Binary.Helper.Convert.ToString(objArrs[0]),
                EncryptKey = (byte[])objArrs[1],
                AuthToken = XmobiTea.Binary.Helper.Convert.ToString(objArrs[2]),
            };
        }

        /// <summary>
        /// Deserialization handler for OperationHandshakeAck.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationHandshakeAckDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            return new OperationHandshakeAck()
            {
                ConnectionId = XmobiTea.Binary.Helper.Convert.ToInt32(objArrs[0]),
                ServerSessionId = XmobiTea.Binary.Helper.Convert.ToString(objArrs[1]),
            };
        }

        /// <summary>
        /// Deserialization handler for OperationDisconnect.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for deserialization.</param>
        /// <param name="payload">The payload to deserialize.</param>
        /// <returns>An instance of IOperationModel representing the deserialized operation model.</returns>
        private IOperationModel OperationDisconnectDeserialize(IBinaryConverter binaryConverter, byte[] payload)
        {
            var objArrs = binaryConverter.Deserialize<object[]>(payload);

            return new OperationDisconnect()
            {
                Reason = XmobiTea.Binary.Helper.Convert.ToByte(objArrs[0]),
                Message = XmobiTea.Binary.Helper.Convert.ToString(objArrs[1]),
            };
        }

    }

}

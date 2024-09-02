using System;
using System.Collections.Generic;
using XmobiTea.Binary;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.RpcProtocol
{
    /// <summary>
    /// Interface defining the contract for serialization 
    /// support of operation models in the XmobiTea.ProtonNet library.
    /// </summary>
    interface IOperationSerializeSupport
    {
        /// <summary>
        /// Serializes the given operation model using the specified operation type and binary converter.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        byte[] Serialize(OperationType operationType, IBinaryConverter binaryConverter, IOperationModel operationModel);
    }

    /// <summary>
    /// Implementation of IOperationSerializeSupport that provides 
    /// serialization support for various operation types.
    /// </summary>
    class OperationSerializeSupport : IOperationSerializeSupport
    {
        /// <summary>
        /// Delegate for handling the serialization of operation models.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        delegate byte[] OperationModelSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel);

        /// <summary>
        /// Dictionary mapping operation types to their corresponding serialization handlers.
        /// </summary>
        private IDictionary<OperationType, OperationModelSerialize> operationModelSerializeDict { get; }

        /// <summary>
        /// Initializes a new instance of the OperationSerializeSupport class.
        /// </summary>
        public OperationSerializeSupport()
        {
            this.operationModelSerializeDict = new Dictionary<OperationType, OperationModelSerialize>();
            this.AddOperationModelSerializes();
        }

        /// <summary>
        /// Adds the serialization handlers for the supported operation types.
        /// </summary>
        private void AddOperationModelSerializes()
        {
            this.operationModelSerializeDict[OperationType.OperationPing] = this.OperationPingSerialize;
            this.operationModelSerializeDict[OperationType.OperationPong] = this.OperationPongSerialize;
            this.operationModelSerializeDict[OperationType.OperationHandshake] = this.OperationHandshakeSerialize;
            this.operationModelSerializeDict[OperationType.OperationHandshakeAck] = this.OperationHandshakeAckSerialize;
            this.operationModelSerializeDict[OperationType.OperationDisconnect] = this.OperationDisconnectSerialize;
            this.operationModelSerializeDict[OperationType.OperationRequest] = this.OperationRequestSerialize;
            this.operationModelSerializeDict[OperationType.OperationResponse] = this.OperationResponseSerialize;
            this.operationModelSerializeDict[OperationType.OperationEvent] = this.OperationEventSerialize;
        }

        /// <summary>
        /// Serializes the given operation model using the specified operation type and binary converter.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        /// <exception cref="Exception">Thrown when the operation type is not supported.</exception>
        public byte[] Serialize(OperationType operationType, IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            if (!this.operationModelSerializeDict.TryGetValue(operationType, out var handler))
                throw new Exception("OperationType " + operationType + " not supported.");

            return handler.Invoke(binaryConverter, operationModel);
        }

        /// <summary>
        /// Serialization handler for OperationPing.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationPingSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            return null;
        }

        /// <summary>
        /// Serialization handler for OperationPong.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationPongSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            return null;
        }

        /// <summary>
        /// Serialization handler for OperationHandshake.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationHandshakeSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationHandshake = (OperationHandshake)operationModel;

            return binaryConverter.Serialize(new object[] {
                (string)operationHandshake.SessionId,
                (byte[])operationHandshake.EncryptKey,
                (string)operationHandshake.AuthToken,
            });
        }

        /// <summary>
        /// Serialization handler for OperationHandshakeAck.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationHandshakeAckSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationHandshakeAck = (OperationHandshakeAck)operationModel;

            return binaryConverter.Serialize(new object[] {
                (int)operationHandshakeAck.ConnectionId,
                (string)operationHandshakeAck.ServerSessionId,
            });
        }

        /// <summary>
        /// Serialization handler for OperationDisconnect.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationDisconnectSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationDisconnect = (OperationDisconnect)operationModel;

            return binaryConverter.Serialize(new object[] {
                (byte)operationDisconnect.Reason,
                (string)operationDisconnect.Message,
            });
        }

        /// <summary>
        /// Serialization handler for OperationRequest.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationRequestSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationRequest = (OperationRequest)operationModel;

            return binaryConverter.Serialize(new object[] {
                (string)operationRequest.OperationCode,
                (Dictionary<string, object>)(operationRequest.Parameters == null ? null : operationRequest.Parameters.ToData()),
                (ushort)operationRequest.RequestId,
            });
        }

        /// <summary>
        /// Serialization handler for OperationResponse.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationResponseSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationResponse = (OperationResponse)operationModel;

            return binaryConverter.Serialize(new object[] {
                (string)operationResponse.OperationCode,
                (Dictionary<string, object>)(operationResponse.Parameters == null ? null : operationResponse.Parameters.ToData()),
                (ushort)operationResponse.ResponseId,
                (byte)operationResponse.ReturnCode,
                (string)operationResponse.DebugMessage,
            });
        }

        /// <summary>
        /// Serialization handler for OperationEvent.
        /// </summary>
        /// <param name="binaryConverter">The binary converter to use for serialization.</param>
        /// <param name="operationModel">The operation model to serialize.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] OperationEventSerialize(IBinaryConverter binaryConverter, IOperationModel operationModel)
        {
            var operationEvent = (OperationEvent)operationModel;

            return binaryConverter.Serialize(new object[] {
                (string)operationEvent.EventCode,
                (Dictionary<string, object>)(operationEvent.Parameters == null ? null : operationEvent.Parameters.ToData()),
            });
        }

    }

}

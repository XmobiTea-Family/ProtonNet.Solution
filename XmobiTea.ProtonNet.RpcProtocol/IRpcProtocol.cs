using System.IO;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.RpcProtocol
{
    /// <summary>
    /// Interface defining the contract for RPC 
    /// communication protocols in the XmobiTea.ProtonNet library.
    /// </summary>
    public interface IRpcProtocol
    {
        /// <summary>
        /// Writes an operation model and its header to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized.</param>
        /// <param name="sendParameters">The send parameters.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        void Write(Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType);

        /// <summary>
        /// Writes and encrypts an operation model and its header to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized and encrypted.</param>
        /// <param name="sendParameters">The send parameters.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <param name="salt">The salt value used for encryption.</param>
        void WriteEncrypt(Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, object salt);

        /// <summary>
        /// Attempts to read an operation from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="header">The operation header.</param>
        /// <param name="payload">The operation payload.</param>
        /// <returns>True if the operation was successfully read; otherwise, false.</returns>
        bool TryRead(Stream stream, out OperationHeader header, out byte[] payload);

        /// <summary>
        /// Attempts to deserialize the operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload to deserialize.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel);

        /// <summary>
        /// Attempts to deserialize and decrypt the operation model from the provided encrypted payload.
        /// </summary>
        /// <param name="payloadEncrypt">The encrypted payload to deserialize and decrypt.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <param name="salt">The salt value used for decryption.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization and decryption were successful; otherwise, false.</returns>
        bool TryDeserializeEncryptOperationModel(byte[] payloadEncrypt, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, object salt, out IOperationModel operationModel);

    }

}

using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Defines the service for handling RPC protocol operations.
    /// </summary>
    public interface IRpcProtocolService
    {
        /// <summary>
        /// Writes an operation model to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to write.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        void Write(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType);

        /// <summary>
        /// Writes an encrypted operation model to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to write.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="cryptoProviderType">The crypto provider type.</param>
        /// <param name="encryptKey">The key used for encryption.</param>
        void WriteEncrypt(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey);

        /// <summary>
        /// Attempts to read an operation header and payload from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="header">The operation header.</param>
        /// <param name="payload">The payload data.</param>
        /// <returns>True if the read operation was successful; otherwise, false.</returns>
        bool TryRead(System.IO.Stream stream, out OperationHeader header, out byte[] payload);

        /// <summary>
        /// Attempts to deserialize an operation model from the specified payload.
        /// </summary>
        /// <param name="payload">The payload data.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel);

        /// <summary>
        /// Attempts to deserialize an encrypted operation model from the specified payload.
        /// </summary>
        /// <param name="payload">The payload data.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="cryptoProviderType">The crypto provider type.</param>
        /// <param name="encryptKey">The key used for decryption.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        bool TryDeserializeEncryptOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey, out IOperationModel operationModel);

    }

    /// <summary>
    /// Implements <see cref="IRpcProtocolService"/> to handle RPC protocol operations.
    /// </summary>
    public class RpcProtocolService : IRpcProtocolService
    {
        private IRpcProtocol rpcProtocol { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcProtocolService"/> class.
        /// </summary>
        public RpcProtocolService()
        {
            this.rpcProtocol = this.CreateRpcProtocol();
        }

        private IRpcProtocol CreateRpcProtocol()
        {
            var answer = new RpcProtocol.RpcProtocol();

            answer.SetProtocolProvider(ProtocolProviderType.SimplePack, new Binary.SimplePack.BinaryConverter());
            answer.SetProtocolProvider(ProtocolProviderType.MessagePack, new Binary.MessagePack.BinaryConverter());

            answer.SetCryptoProvider(CryptoProviderType.Aes, new Crypto.Aes.CryptoProvider());

            return answer;
        }

        /// <summary>
        /// Attempts to deserialize an encrypted operation model from the specified payload.
        /// </summary>
        /// <param name="payload">The payload data.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="cryptoProviderType">The crypto provider type.</param>
        /// <param name="encryptKey">The key used for decryption.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        public bool TryDeserializeEncryptOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey, out IOperationModel operationModel) =>
            this.rpcProtocol.TryDeserializeEncryptOperationModel(payload, operationType, protocolProviderType, cryptoProviderType, new object[] { encryptKey }, out operationModel);

        /// <summary>
        /// Attempts to deserialize an operation model from the specified payload.
        /// </summary>
        /// <param name="payload">The payload data.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        public bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel) =>
            this.rpcProtocol.TryDeserializeOperationModel(payload, operationType, protocolProviderType, out operationModel);

        /// <summary>
        /// Attempts to read an operation header and payload from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="header">The operation header.</param>
        /// <param name="payload">The payload data.</param>
        /// <returns>True if the read operation was successful; otherwise, false.</returns>
        public bool TryRead(System.IO.Stream stream, out OperationHeader header, out byte[] payload) =>
            this.rpcProtocol.TryRead(stream, out header, out payload);

        /// <summary>
        /// Writes an operation model to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to write.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        public void Write(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType) =>
            this.rpcProtocol.Write(stream, operationType, operationModel, sendParameters, protocolProviderType);

        /// <summary>
        /// Writes an encrypted operation model to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to write.</param>
        /// <param name="sendParameters">The parameters for sending data.</param>
        /// <param name="protocolProviderType">The protocol provider type.</param>
        /// <param name="cryptoProviderType">The crypto provider type.</param>
        /// <param name="encryptKey">The key used for encryption.</param>
        public void WriteEncrypt(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey) =>
            this.rpcProtocol.WriteEncrypt(stream, operationType, operationModel, sendParameters, protocolProviderType, cryptoProviderType, new object[] { encryptKey });

    }

}

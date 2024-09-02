using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.Client.Services
{
    /// <summary>
    /// Interface for handling the serialization and deserialization of 
    /// operation models in an RPC (Remote Procedure Call) communication protocol.
    /// Provides methods for writing data to a stream, optionally with encryption,
    /// and for reading and deserializing data from a stream.
    /// </summary>
    public interface IRpcProtocolService
    {
        /// <summary>
        /// Writes an operation model to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the operation model is written.</param>
        /// <param name="operationType">The type of operation being written.</param>
        /// <param name="operationModel">The operation model to be written.</param>
        /// <param name="sendParameters">Parameters for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        void Write(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType);

        /// <summary>
        /// Writes an encrypted operation model to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the operation model is written.</param>
        /// <param name="operationType">The type of operation being written.</param>
        /// <param name="operationModel">The operation model to be written.</param>
        /// <param name="sendParameters">Parameters for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption.</param>
        /// <param name="encryptKey">The encryption key used for encrypting the data.</param>
        void WriteEncrypt(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey);

        /// <summary>
        /// Tries to read an operation from a stream and extracts the operation header and payload.
        /// </summary>
        /// <param name="stream">The stream from which the operation is read.</param>
        /// <param name="header">The header of the operation that was read.</param>
        /// <param name="payload">The payload of the operation that was read.</param>
        /// <returns>True if the operation was successfully read, otherwise false.</returns>
        bool TryRead(System.IO.Stream stream, out OperationHeader header, out byte[] payload);

        /// <summary>
        /// Tries to deserialize an operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload containing the serialized operation model.</param>
        /// <param name="operationType">The type of operation to deserialize.</param>
        /// <param name="protocolProviderType">The protocol provider type used for deserialization.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if the operation model was successfully deserialized, otherwise false.</returns>
        bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel);

        /// <summary>
        /// Tries to deserialize an encrypted operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload containing the serialized operation model.</param>
        /// <param name="operationType">The type of operation to deserialize.</param>
        /// <param name="protocolProviderType">The protocol provider type used for deserialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for decryption.</param>
        /// <param name="encryptKey">The encryption key used for decrypting the data.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if the operation model was successfully deserialized and decrypted, otherwise false.</returns>
        bool TryDeserializeEncryptOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey, out IOperationModel operationModel);
    }

    /// <summary>
    /// Implements the <see cref="IRpcProtocolService"/> interface, providing methods for 
    /// handling RPC protocol operations, including serialization, deserialization, 
    /// and encryption of operation models.
    /// </summary>
    public class RpcProtocolService : IRpcProtocolService
    {
        /// <summary>
        /// Custom salt data used for encryption operations.
        /// </summary>
        private byte[] saltCustomData { get; }

        /// <summary>
        /// The RPC protocol instance used for performing protocol operations.
        /// </summary>
        private IRpcProtocol rpcProtocol { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcProtocolService"/> class.
        /// </summary>
        /// <param name="saltCustomData">Custom salt data used for encryption operations.</param>
        public RpcProtocolService(byte[] saltCustomData)
        {
            this.rpcProtocol = this.CreateRpcProtocol();
            this.saltCustomData = saltCustomData;
        }

        /// <summary>
        /// Creates and initializes an instance of the RPC protocol with the necessary 
        /// protocol providers and crypto providers.
        /// </summary>
        /// <returns>An initialized <see cref="IRpcProtocol"/> instance.</returns>
        private IRpcProtocol CreateRpcProtocol()
        {
            var answer = new XmobiTea.ProtonNet.RpcProtocol.RpcProtocol();

            answer.SetProtocolProvider(ProtocolProviderType.SimplePack, new XmobiTea.Binary.SimplePack.BinaryConverter());
            answer.SetProtocolProvider(ProtocolProviderType.MessagePack, new XmobiTea.Binary.MessagePack.BinaryConverter());

            answer.SetCryptoProvider(CryptoProviderType.Aes, new Crypto.Aes.CryptoProvider());

            return answer;
        }

        /// <summary>
        /// Tries to deserialize and decrypt an encrypted operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload containing the encrypted operation model.</param>
        /// <param name="operationType">The type of operation to deserialize.</param>
        /// <param name="protocolProviderType">The protocol provider type used for deserialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for decryption.</param>
        /// <param name="encryptKey">The encryption key used for decrypting the data.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if the operation model was successfully deserialized and decrypted, otherwise false.</returns>
        public bool TryDeserializeEncryptOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey, out IOperationModel operationModel) => this.rpcProtocol.TryDeserializeEncryptOperationModel(payload, operationType, protocolProviderType, cryptoProviderType, new object[] { encryptKey, this.saltCustomData }, out operationModel);

        /// <summary>
        /// Tries to deserialize an operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload containing the serialized operation model.</param>
        /// <param name="operationType">The type of operation to deserialize.</param>
        /// <param name="protocolProviderType">The protocol provider type used for deserialization.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if the operation model was successfully deserialized, otherwise false.</returns>
        public bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel) => this.rpcProtocol.TryDeserializeOperationModel(payload, operationType, protocolProviderType, out operationModel);

        /// <summary>
        /// Tries to read an operation from a stream and extracts the operation header and payload.
        /// </summary>
        /// <param name="stream">The stream from which the operation is read.</param>
        /// <param name="header">The header of the operation that was read.</param>
        /// <param name="payload">The payload of the operation that was read.</param>
        /// <returns>True if the operation was successfully read, otherwise false.</returns>
        public bool TryRead(System.IO.Stream stream, out OperationHeader header, out byte[] payload) => this.rpcProtocol.TryRead(stream, out header, out payload);

        /// <summary>
        /// Writes an operation model to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the operation model is written.</param>
        /// <param name="operationType">The type of operation being written.</param>
        /// <param name="operationModel">The operation model to be written.</param>
        /// <param name="sendParameters">Parameters for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        public void Write(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType) => this.rpcProtocol.Write(stream, operationType, operationModel, sendParameters, protocolProviderType);

        /// <summary>
        /// Writes an encrypted operation model to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the operation model is written.</param>
        /// <param name="operationType">The type of operation being written.</param>
        /// <param name="operationModel">The operation model to be written.</param>
        /// <param name="sendParameters">Parameters for sending the operation.</param>
        /// <param name="protocolProviderType">The protocol provider type used for serialization.</param>
        /// <param name="cryptoProviderType">The crypto provider type used for encryption.</param>
        /// <param name="encryptKey">The encryption key used for encrypting the data.</param>
        public void WriteEncrypt(System.IO.Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, byte[] encryptKey) => this.rpcProtocol.WriteEncrypt(stream, operationType, operationModel, sendParameters, protocolProviderType, cryptoProviderType, new object[] { encryptKey, this.saltCustomData });

    }

}

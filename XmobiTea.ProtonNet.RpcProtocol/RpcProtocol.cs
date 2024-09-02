using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary;
using XmobiTea.Crypto;
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Helper;
using XmobiTea.ProtonNet.RpcProtocol.Models;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.RpcProtocol
{
    /// <summary>
    /// RpcProtocol class implements 
    /// the IRpcProtocol interface to 
    /// handle RPC communication protocols.
    /// </summary>
    public class RpcProtocol : IRpcProtocol
    {
        /// <summary>
        /// Dictionary to store protocol providers 
        /// based on the ProtocolProviderType.
        /// </summary>
        private IDictionary<ProtocolProviderType, IBinaryConverter> protocolProviderDict { get; }

        /// <summary>
        /// Dictionary to store crypto providers 
        /// based on the CryptoProviderType.
        /// </summary>
        private IDictionary<CryptoProviderType, ICryptoProvider> cryptoProviderDict { get; }

        /// <summary>
        /// Support object for deserialization of operations.
        /// </summary>
        private IOperationDeserializeSupport operationDeserializeSupport { get; }

        /// <summary>
        /// Support object for serialization of operations.
        /// </summary>
        private IOperationSerializeSupport operationSerializeSupport { get; }

        /// <summary>
        /// Initializes a new instance of the RpcProtocol class.
        /// </summary>
        public RpcProtocol()
        {
            this.protocolProviderDict = new Dictionary<ProtocolProviderType, IBinaryConverter>();
            this.cryptoProviderDict = new Dictionary<CryptoProviderType, ICryptoProvider>();

            this.operationDeserializeSupport = new OperationDeserializeSupport();
            this.operationSerializeSupport = new OperationSerializeSupport();
        }

        /// <summary>
        /// Sets the protocol provider for the specified ProtocolProviderType.
        /// </summary>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="binaryConverter">The binary converter instance.</param>
        public void SetProtocolProvider(ProtocolProviderType protocolProviderType, IBinaryConverter binaryConverter)
        {
            this.protocolProviderDict[protocolProviderType] = binaryConverter;
        }

        /// <summary>
        /// Retrieves the protocol provider for the specified ProtocolProviderType.
        /// </summary>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <returns>The binary converter instance.</returns>
        /// <exception cref="System.Exception">Thrown when the protocol provider is not found.</exception>
        private IBinaryConverter GetProtocolProvider(ProtocolProviderType protocolProviderType)
        {
            if (!this.protocolProviderDict.ContainsKey(protocolProviderType))
                throw new System.Exception("ProtocolProvider for type " + protocolProviderType + " not found.");

            return this.protocolProviderDict[protocolProviderType];
        }

        /// <summary>
        /// Retrieves the crypto provider for the specified CryptoProviderType.
        /// </summary>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <returns>The crypto provider instance.</returns>
        /// <exception cref="System.Exception">Thrown when the crypto provider is not found.</exception>
        private ICryptoProvider GetCryptoProvider(CryptoProviderType cryptoProviderType)
        {
            if (!this.cryptoProviderDict.ContainsKey(cryptoProviderType))
                throw new System.Exception("CryptoProvider for type " + cryptoProviderType + " not found.");

            return this.cryptoProviderDict[cryptoProviderType];
        }

        /// <summary>
        /// Sets the crypto provider for the specified CryptoProviderType.
        /// </summary>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <param name="cryptoProvider">The crypto provider instance.</param>
        public void SetCryptoProvider(CryptoProviderType cryptoProviderType, ICryptoProvider cryptoProvider)
        {
            this.cryptoProviderDict[cryptoProviderType] = cryptoProvider;
        }

        /// <summary>
        /// Serializes the operation model into a byte array using the specified protocol provider.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <returns>A byte array representing the serialized operation model.</returns>
        private byte[] SerializeOperationModel(OperationType operationType, IOperationModel operationModel, ProtocolProviderType protocolProviderType)
        {
            var protocolProvider = this.GetProtocolProvider(protocolProviderType);

            return this.operationSerializeSupport.Serialize(operationType, protocolProvider, operationModel);
        }

        /// <summary>
        /// Serializes and encrypts the operation model into a byte array using the specified protocol and crypto providers.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <param name="salt">The salt value used for encryption.</param>
        /// <returns>A byte array representing the serialized and encrypted operation model.</returns>
        private byte[] SerializeEncryptOperationModel(OperationType operationType, IOperationModel operationModel, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, object salt)
        {
            var payload = this.SerializeOperationModel(operationType, operationModel, protocolProviderType);

            if (payload == null) return null;

            var cryptoProvider = this.GetCryptoProvider(cryptoProviderType);

            return cryptoProvider.Encrypt(payload, salt);
        }

        /// <summary>
        /// Determines the length byte for the given length.
        /// </summary>
        /// <param name="length">The length of the payload.</param>
        /// <returns>The length byte.</returns>
        private byte GetLengthByte(int length)
        {
            if (length <= 0) return 0;
            if (length <= byte.MaxValue) return 1;
            else if (length <= ushort.MaxValue) return 2;
            return 3;
        }

        /// <summary>
        /// Writes the operation header and payload to the provided stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="header">The operation header.</param>
        /// <param name="payload">The payload to write.</param>
        private void Write(Stream stream, OperationHeader header, byte[] payload)
        {
            var sendParameters = header.SendParameters;

            byte byte0 = 0;

            var lengthByte = this.GetLengthByte(header.PayloadLength);
            var operationType = (byte)header.OperationType;
            var protocolProviderType = (byte)header.ProtocolProviderType;

            if ((lengthByte & 1 << 1) != 0) byte0 |= 1 << 7;
            if ((lengthByte & 1 << 0) != 0) byte0 |= 1 << 6;

            if ((operationType & 1 << 2) != 0) byte0 |= 1 << 5;
            if ((operationType & 1 << 1) != 0) byte0 |= 1 << 4;
            if ((operationType & 1 << 0) != 0) byte0 |= 1 << 3;

            if ((protocolProviderType & 1 << 2) != 0) byte0 |= 1 << 2;
            if ((protocolProviderType & 1 << 1) != 0) byte0 |= 1 << 1;
            if ((protocolProviderType & 1 << 0) != 0) byte0 |= 1 << 0;

            stream.WriteByte(byte0);

            if (header.OperationType == OperationType.OperationRequest
                || header.OperationType == OperationType.OperationResponse
                || header.OperationType == OperationType.OperationEvent)
            {
                byte byte1 = 0;
                if (header.CryptoProviderType != null)
                {
                    var cryptoProviderType = (byte)header.CryptoProviderType.GetValueOrDefault();

                    if ((cryptoProviderType & 1 << 2) != 0) byte1 |= 1 << 7;
                    if ((cryptoProviderType & 1 << 1) != 0) byte1 |= 1 << 6;
                    if ((cryptoProviderType & 1 << 0) != 0) byte1 |= 1 << 5;
                }

                if (sendParameters.Unreliable) byte1 |= 1 << 4;
                if (sendParameters.Encrypted) byte1 |= 1 << 3;
                if (sendParameters.Immediately) byte1 |= 1 << 2;
                if (sendParameters.Sync) byte1 |= 1 << 1;

                stream.WriteByte(byte1);
            }

            if (lengthByte == 0)
            {

            }
            else
            {
                var lengthBytes = System.BitConverter.GetBytes(header.PayloadLength);
                BinaryUtils.SwapIfLittleEndian(ref lengthBytes);

                if (lengthByte == 1)
                {
                    stream.WriteByte(lengthBytes[3]);
                }
                else if (lengthByte == 2)
                {
                    stream.WriteByte(lengthBytes[2]);
                    stream.WriteByte(lengthBytes[3]);
                }
                else
                {
                    stream.WriteByte(lengthBytes[1]);
                    stream.WriteByte(lengthBytes[2]);
                    stream.WriteByte(lengthBytes[3]);
                }

                stream.Write(payload, 0, payload.Length);
            }
        }

        /// <summary>
        /// Writes the operation model and its header to the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized.</param>
        /// <param name="sendParameters">The send parameters.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        public void Write(Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType)
        {
            var header = this.GetOperationHeader(operationType, operationModel, sendParameters, protocolProviderType, null);

            var payload = this.SerializeOperationModel(operationType, operationModel, protocolProviderType);

            if (payload == null) header.PayloadLength = 0;
            else header.PayloadLength = payload.Length;

            this.Write(stream, header, payload);
        }

        /// <summary>
        /// Writes and encrypts the operation model and its header to the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model to be serialized and encrypted.</param>
        /// <param name="sendParameters">The send parameters.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="cryptoProviderType">The type of the crypto provider.</param>
        /// <param name="salt">The salt value used for encryption.</param>
        public void WriteEncrypt(Stream stream, OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, object salt)
        {
            var header = this.GetOperationHeader(operationType, operationModel, sendParameters, protocolProviderType, cryptoProviderType);

            var payload = this.SerializeEncryptOperationModel(operationType, operationModel, protocolProviderType, cryptoProviderType, salt);
            if (payload == null) header.PayloadLength = 0;
            else header.PayloadLength = payload.Length;

            this.Write(stream, header, payload);
        }

        /// <summary>
        /// Creates an OperationHeader for the given operation and model.
        /// </summary>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="operationModel">The operation model.</param>
        /// <param name="sendParameters">The send parameters.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="cryptoProviderType">The type of the crypto provider, if any.</param>
        /// <returns>An instance of OperationHeader.</returns>
        private OperationHeader GetOperationHeader(OperationType operationType, IOperationModel operationModel, SendParameters sendParameters, ProtocolProviderType protocolProviderType, CryptoProviderType? cryptoProviderType)
        {
            var answer = new OperationHeader();

            answer.SendParameters = sendParameters;
            answer.OperationType = operationType;

            answer.ProtocolProviderType = protocolProviderType;
            answer.CryptoProviderType = cryptoProviderType;

            return answer;
        }

        /// <summary>
        /// Attempts to read an operation from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="header">The operation header.</param>
        /// <param name="payload">The operation payload.</param>
        /// <returns>True if the operation was successfully read; otherwise, false.</returns>
        public bool TryRead(Stream stream, out OperationHeader header, out byte[] payload)
        {
            // Attempt to read the operation from the stream.
            try
            {
                header = new OperationHeader();

                var byte0 = (byte)stream.ReadByte();

                byte lengthByte = 0;
                byte operationType = 0;
                byte protocolProviderType = 0;

                if ((byte0 & 1 << 7) != 0) lengthByte |= 1 << 1;
                if ((byte0 & 1 << 6) != 0) lengthByte |= 1 << 0;

                if ((byte0 & 1 << 5) != 0) operationType |= 1 << 2;
                if ((byte0 & 1 << 4) != 0) operationType |= 1 << 1;
                if ((byte0 & 1 << 3) != 0) operationType |= 1 << 0;

                if ((byte0 & 1 << 2) != 0) protocolProviderType |= 1 << 2;
                if ((byte0 & 1 << 1) != 0) protocolProviderType |= 1 << 1;
                if ((byte0 & 1 << 0) != 0) protocolProviderType |= 1 << 0;

                header.OperationType = (OperationType)operationType;
                header.ProtocolProviderType = (ProtocolProviderType)protocolProviderType;

                if (header.OperationType == OperationType.OperationRequest
                    || header.OperationType == OperationType.OperationResponse
                    || header.OperationType == OperationType.OperationEvent)
                {
                    var byte1 = (byte)stream.ReadByte();

                    var sendParameters = new SendParameters();

                    sendParameters.Unreliable = (byte1 & 1 << 4) != 0;
                    sendParameters.Encrypted = (byte1 & 1 << 3) != 0;
                    sendParameters.Immediately = (byte1 & 1 << 2) != 0;
                    sendParameters.Sync = (byte1 & 1 << 1) != 0;

                    if (sendParameters.Encrypted)
                    {
                        byte cryptoProviderType = 0;

                        if ((byte1 & 1 << 7) != 0) cryptoProviderType |= 1 << 2;
                        if ((byte1 & 1 << 6) != 0) cryptoProviderType |= 1 << 1;
                        if ((byte1 & 1 << 5) != 0) cryptoProviderType |= 1 << 0;

                        header.CryptoProviderType = (CryptoProviderType)cryptoProviderType;
                    }
                    header.SendParameters = sendParameters;
                }

                if (lengthByte == 0)
                {
                    header.PayloadLength = 0;
                    payload = null;
                }
                else
                {
                    byte[] lengthBytes;
                    if (lengthByte == 1) lengthBytes = new byte[] { (byte)stream.ReadByte() };
                    else if (lengthByte == 2) lengthBytes = new byte[] { (byte)stream.ReadByte(), (byte)stream.ReadByte() };
                    else lengthBytes = new byte[] { 0, (byte)stream.ReadByte(), (byte)stream.ReadByte(), (byte)stream.ReadByte() };

                    BinaryUtils.SwapIfLittleEndian(ref lengthBytes);

                    if (lengthByte == 1) header.PayloadLength = lengthBytes[0];
                    else if (lengthByte == 2) header.PayloadLength = System.BitConverter.ToUInt16(lengthBytes, 0);
                    else header.PayloadLength = System.BitConverter.ToInt32(lengthBytes, 0);

                    payload = new byte[header.PayloadLength];

                    if (stream.Read(payload, 0, header.PayloadLength) != header.PayloadLength)
                        throw new System.Exception("Payload length data not match with remain bytes.");
                }

                return true;
            }
            catch
            {
                header = null;
                payload = null;

                return false;
            }
        }

        /// <summary>
        /// Attempts to deserialize the operation model from the provided payload.
        /// </summary>
        /// <param name="payload">The payload to deserialize.</param>
        /// <param name="operationType">The type of the operation.</param>
        /// <param name="protocolProviderType">The type of the protocol provider.</param>
        /// <param name="operationModel">The deserialized operation model.</param>
        /// <returns>True if deserialization was successful; otherwise, false.</returns>
        public bool TryDeserializeOperationModel(byte[] payload, OperationType operationType, ProtocolProviderType protocolProviderType, out IOperationModel operationModel)
        {
            try
            {
                var protocolProvider = this.GetProtocolProvider(protocolProviderType);
                operationModel = this.operationDeserializeSupport.Deserialize(operationType, protocolProvider, payload);

                return true;
            }
            catch
            {
                operationModel = null;
                return false;
            }
        }

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
        public bool TryDeserializeEncryptOperationModel(byte[] payloadEncrypt, OperationType operationType, ProtocolProviderType protocolProviderType, CryptoProviderType cryptoProviderType, object salt, out IOperationModel operationModel)
        {
            try
            {
                var cryptoProvider = this.GetCryptoProvider(cryptoProviderType);

                if (!cryptoProvider.TryDecrypt(payloadEncrypt, salt, out var payload))
                {
                    operationModel = null;
                    return false;
                }

                return this.TryDeserializeOperationModel(payload, operationType, protocolProviderType, out operationModel);
            }
            catch
            {
                operationModel = null;
                return false;
            }
        }

    }

}

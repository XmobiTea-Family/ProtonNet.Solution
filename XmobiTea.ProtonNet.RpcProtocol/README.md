# XmobiTea.ProtonNet.RpcProtocol

## Project Name
XmobiTea.ProtonNet.RpcProtocol

## Installation Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/XmobiTea.ProtonNet.RpcProtocol.git
   ```
2. Navigate to the project directory:
   ```bash
   cd XmobiTea.ProtonNet.RpcProtocol
   ```
3. Install dependencies (if any):
   ```bash
   dotnet restore
   ```

## Features
- **Operation Models**: Define and manage different RPC operation types.
- **Serialization and Deserialization**: Support for serializing and deserializing operation models.
- **Protocol and Crypto Providers**: Allows for different protocol and crypto provider types.

## Usage Examples
1. **Creating an OperationHeader**:
   ```csharp
   var header = new OperationHeader
   {
       PayloadLength = 1024,
       SendParameters = new SendParameters { ... },
       OperationType = OperationType.OperationRequest,
       ProtocolProviderType = ProtocolProviderType.SimplePack,
       CryptoProviderType = CryptoProviderType.Aes
   };
   ```

2. **Serializing an Operation**:
   ```csharp
   var serializeSupport = new OperationSerializeSupport();
   var serializedData = serializeSupport.Serialize(OperationType.OperationRequest, binaryConverter, operationModel);
   ```

3. **Deserializing an Operation**:
   ```csharp
   var deserializeSupport = new OperationDeserializeSupport();
   var operationModel = deserializeSupport.Deserialize(OperationType.OperationRequest, binaryConverter, payload);
   ```

## Supported Data Types
- `OperationType`
- `ProtocolProviderType`
- `CryptoProviderType`
- `IOperationModel`

## Extensibility
- Add new operation types by extending the `OperationType` enum.
- Implement additional serializers and deserializers by extending `OperationSerializeSupport` and `OperationDeserializeSupport`.

## Contributing Guidelines
1. Fork the repository.
2. Create a new branch for your feature or fix.
3. Make your changes and test thoroughly.
4. Submit a pull request with a detailed description of your changes.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments
- Thanks to the contributors and the community for their support.

# XmobiTea.ProtonNet.Networking

## Installation

To install the `XmobiTea.ProtonNet.Networking` library, include it in your project by referencing the appropriate NuGet package or by adding the source files to your solution.

```bash
dotnet add package XmobiTea.ProtonNet.Networking
```

## Features

- **Operation Models**: Supports various operation models such as `OperationRequest`, `OperationResponse`, `OperationEvent`, `OperationPing`, `OperationPong`, `OperationHandshake`, and `OperationDisconnect`.
- **Extensions**: Includes extension methods for setting and manipulating operation codes, parameters, and more.
- **Enums**: Provides enumerations like `DisconnectReason` and `SendResult` for standardized communication.

## Usage

### Example: Creating and Using OperationRequest

```csharp
using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.Networking.Extensions;
using XmobiTea.Data;

var operationRequest = new OperationRequest()
    .SetOperationCode("SampleCode")
    .AddParameter("Key1", "Value1")
    .AddParameter("Key2", 1234);

var operationResponse = new OperationResponse()
    .SetOperationCode("SampleResponse")
    .SetReturnCode(1)
    .SetDebugMessage("Success");
```

### Example: Handling Disconnections

```csharp
var disconnectReason = DisconnectReason.MaxSession;
Console.WriteLine($"Disconnected due to: {disconnectReason}");
```

## Supported Data Types

- **OperationRequest**: Manages the request operations with codes and parameters.
- **OperationResponse**: Manages the response from operations with codes, return codes, and debug messages.
- **OperationEvent**: Handles events with associated codes and parameters.
- **Enums**: Includes types like `DisconnectReason` and `SendResult` for handling specific scenarios.
- **GNHashtable**: A flexible data structure for storing parameters in key-value pairs.

## Extensibility

`XmobiTea.ProtonNet.Networking` is designed to be extensible. You can easily add custom operations, extend the existing models, and implement additional features by following the structure provided in the library.

## Contributing

Contributions are welcome! Please follow the [contribution guidelines](CONTRIBUTING.md) and ensure that your code adheres to the project's coding standards.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

Thanks to the open-source community for providing valuable feedback and contributions that make this project better.

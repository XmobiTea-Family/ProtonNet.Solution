
# XmobiTea.ProtonNet.Server.Socket

## Installation Instructions
To install `XmobiTea.ProtonNet.Server.Socket`, clone the repository and include the project in your solution.

```bash
git clone https://github.com/your-repository/XmobiTea.ProtonNet.Server.Socket.git
```

Ensure all dependencies are restored before building the project.

## Features
- **Socket Server Implementation**: Provides a robust and scalable socket server framework.
- **Supports Multiple Protocols**: Handles TCP, UDP, WebSocket, SSL/TLS, and more.
- **Customizable Controllers**: Extend and customize socket controllers to handle specific protocol logic.
- **Session Management**: Efficient session handling with timeout and disconnect logic.
- **Security**: Supports encryption and secure communication channels.
- **Event-Driven Architecture**: Handles socket events such as connection, disconnection, data reception, and errors.

## Usage Examples
### Example 1: Creating a Basic Socket Server
```csharp
var startupSettings = StartupSettings.NewBuilder()
    .SetName("MySocketServer")
    .SetTcpServer(TcpServerSettings.NewBuilder()
        .SetEnable(true)
        .SetAddress("127.0.0.1")
        .SetPort(8080)
        .Build())
    .Build();

var socketServer = new SocketServer(startupSettings);
socketServer.Start();
```

### Example 2: Handling Received Data
```csharp
public class CustomSocketController : SocketController
{
    public override void OnReceived(ISocketSession session, byte[] buffer, int position, int length)
    {
        var data = Encoding.UTF8.GetString(buffer, position, length);
        Console.WriteLine($"Received data: {data}");
    }
}
```

## Supported Data Types
- **byte[]**: For binary data transmission.
- **string**: For text data transmission, typically UTF-8 encoded.
- **Custom Protocol Messages**: Serialize and deserialize custom protocol messages using the provided interfaces.

## Extensibility
- **Custom Controllers**: Inherit from `SocketController` to define your own behavior for handling connections, data, and errors.
- **Operation Model Handlers**: Create handlers by inheriting from `AbstractOperationModelHandler` to process specific operations.
- **Session Services**: Implement custom session services by extending `ISessionService`.

## Contributing Guidelines
Contributions are welcome! Please fork the repository and submit a pull request with your changes. Ensure all tests pass and follow the coding conventions used in the project.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

## Acknowledgments
Special thanks to the XmobiTea community for their continuous support and contributions.

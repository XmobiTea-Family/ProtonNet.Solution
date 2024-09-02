# XmobiTea.ProtonNet.Client

## Installation

To install `XmobiTea.ProtonNet.Client`, you can use the following command:

```bash
dotnet add package XmobiTea.ProtonNet.Client
```

## Features

- **Socket Clients**: Provides multiple types of socket clients, including TCP, UDP, WebSocket, and WebSocket Secure (WSS) clients.
- **Encryption Support**: Includes support for setting encryption keys for secure communication.
- **Event Handling**: Allows handling of connection, disconnection, data reception, and error events.
- **Reconnect Capability**: Supports automatic reconnection with customizable intervals.
- **Customizable Send Parameters**: Provides options for synchronous and asynchronous data sending.

## Usage

### Example: Creating a TCP Socket Client

```csharp
using XmobiTea.ProtonNet.Client.Socket.Clients;

var client = new SocketTcpClient("example.com", 12345, new TcpClientOptions());
client.SetEncryptKey(new byte[] { /* encryption key bytes */ });

client.onConnected += () => Console.WriteLine("Connected to the server.");
client.onDisconnected += () => Console.WriteLine("Disconnected from the server.");
client.onReceived += (buffer, position, length) => Console.WriteLine("Data received from server.");
client.onError += error => Console.WriteLine($"Socket error: {error}");

client.Connect();
```

### Example: Creating a WebSocket Secure (WSS) Client

```csharp
using XmobiTea.ProtonNet.Client.Socket.Clients;
using XmobiTea.ProtonNetCommon;

var sslContext = new SslContext(/* SSL parameters */);
var client = new SocketWssClient("wss://example.com", 443, new TcpClientOptions(), sslContext);
client.SetEncryptKey(new byte[] { /* encryption key bytes */ });

client.onConnected += () => Console.WriteLine("Connected to the server.");
client.onDisconnected += () => Console.WriteLine("Disconnected from the server.");
client.onReceived += (buffer, position, length) => Console.WriteLine("Data received from server.");
client.onError += error => Console.WriteLine($"Socket error: {error}");

client.Connect();
```

## Supported Data Types

- **TcpClientOptions**: Configuration options for TCP clients.
- **UdpClientOptions**: Configuration options for UDP clients.
- **SslContext**: SSL context used for secure communication in WSS clients.
- **SendParameters**: Parameters controlling the sending of data, including synchronous or asynchronous modes and encryption options.

## Extensibility

`XmobiTea.ProtonNet.Client` is designed with extensibility in mind. You can create custom implementations of socket clients or extend the existing ones by inheriting from the provided classes such as `SocketTcpClient`, `SocketUdpClient`, `SocketWsClient`, and `SocketWssClient`.

## Contributing

We welcome contributions to `XmobiTea.ProtonNet.Client`. If you want to contribute, please follow these guidelines:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add new feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Create a new Pull Request.

## License

`XmobiTea.ProtonNet.Client` is licensed under the MIT License. See the `LICENSE` file for more details.

## Acknowledgments

We would like to thank all the contributors and users of `XmobiTea.ProtonNet.Client` for their support and feedback.

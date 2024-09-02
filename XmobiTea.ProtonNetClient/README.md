# XmobiTea.ProtonNetClient

## Overview
**XmobiTea.ProtonNetClient** is a powerful .NET library that provides interfaces and implementations for creating and managing TCP, UDP, HTTP, and WebSocket connections. This library is designed to support both synchronous and asynchronous connections, offering high performance for modern network applications.

## Installation
You can install `XmobiTea.ProtonNetClient` via the NuGet Package Manager:

```bash
Install-Package XmobiTea.ProtonNetClient
```

Or add it directly to your `.csproj` file:

```xml
<PackageReference Include="XmobiTea.ProtonNetClient" Version="1.0.0" />
```

## Features
- **TCP Client**: Supports TCP connections with robust configuration options.
- **UDP Client**: Manages UDP connections with multicast group join and leave capabilities.
- **HTTP Client**: Simple and efficient HTTP client implementation.
- **WebSocket Client**: Supports WebSocket and WebSocket Secure (WSS) connections, including handling frames like text, binary, ping/pong, and close.
- **SSL/TLS**: Integrated SSL/TLS security for secure connections.

## Usage
### TCP Client
Initialize and connect to a TCP server:

```csharp
var options = new TcpClientOptions
{
    KeepAlive = true,
    NoDelay = true
};

var tcpClient = new TcpClient("127.0.0.1", 8080, options);
tcpClient.Connect();
```

### UDP Client
Initialize and send data via UDP:

```csharp
var udpOptions = new UdpClientOptions
{
    ReuseAddress = true,
    Multicast = true
};

var udpClient = new UdpClient("224.0.0.1", 8080, udpOptions);
udpClient.Connect();
udpClient.Send(udpClient.EndPoint, Encoding.UTF8.GetBytes("Hello, World!"));
```

### WebSocket Client
Initialize and connect to a WebSocket server:

```csharp
var wsOptions = new TcpClientOptions();
var wsClient = new WsClient("ws://echo.websocket.org", 80, wsOptions);
wsClient.Connect();
wsClient.SendText(Encoding.UTF8.GetBytes("Hello WebSocket!"));
```

## Supported Data Types
- `byte[]` for binary and text payloads.
- `string` for connection strings and endpoint URLs.

## Extensibility
The library is designed to be extensible. You can inherit from base classes like `TcpClient`, `UdpClient`, `WsClient`, and `HttpsClient` to customize specific behaviors.

## Contributing
To contribute to this project:
1. Fork this repository.
2. Create a new branch for your feature or bugfix (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature-name`).
5. Create a Pull Request on GitHub.

## License
`XmobiTea.ProtonNetClient` is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments
- Thanks to the .NET community for the tools and libraries that supported the development of this project.

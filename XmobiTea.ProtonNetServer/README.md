
# XmobiTea.ProtonNetServer

## Project Overview
XmobiTea.ProtonNetServer is a lightweight, high-performance TCP, UDP, and WebSocket server library for .NET. It provides a flexible and scalable solution for building custom servers, handling thousands of simultaneous connections with ease. The library supports secure connections using SSL/TLS and is designed to be easily extensible, allowing developers to implement custom protocols and behaviors.

## Installation

To install XmobiTea.ProtonNetServer, you can use NuGet:

```bash
dotnet add package XmobiTea.ProtonNetServer
```

Or via the NuGet Package Manager in Visual Studio:

1. Right-click on your project in Solution Explorer and select "Manage NuGet Packages".
2. Search for `XmobiTea.ProtonNetServer`.
3. Click "Install".

## Features

- **TCP, UDP, and WebSocket Support:** Provides robust implementations for TCP, UDP, and WebSocket protocols.
- **SSL/TLS Support:** Secure your connections with SSL/TLS using built-in support for secure protocols.
- **High Performance:** Optimized for handling thousands of simultaneous connections efficiently.
- **Extensible Architecture:** Easily extend and customize the server to support custom protocols and behaviors.
- **Asynchronous Operations:** Fully supports asynchronous I/O operations, ensuring scalability and responsiveness.
- **Session Management:** Built-in session management for maintaining state across connections.
- **Network Statistics:** Monitor network activity with detailed statistics for sent and received data.

## Usage

### Basic TCP Server

```csharp
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;

var options = new TcpServerOptions
{
    ReceiveBufferCapacity = 1024,
    SendBufferCapacity = 1024,
    KeepAlive = true
};

var server = new TcpServer("127.0.0.1", 9000, options);

server.Start();

Console.WriteLine("Server started on 127.0.0.1:9000");
Console.ReadLine();

server.Stop();
```

### WebSocket Server with SSL

```csharp
using XmobiTea.ProtonNetServer;
using XmobiTea.ProtonNetServer.Options;
using XmobiTea.ProtonNetServer.Ssl;

var sslContext = new SslContext("server.pfx", "password");
var options = new TcpServerOptions
{
    NoDelay = true,
    TcpKeepAliveTime = 60,
    TcpKeepAliveInterval = 30,
    TcpKeepAliveRetryCount = 5
};

var server = new WssServer("127.0.0.1", 9001, options, sslContext);

server.Start();

Console.WriteLine("Secure WebSocket server started on wss://127.0.0.1:9001");
Console.ReadLine();

server.Stop();
```

## Supported Data Types

- **TcpServerOptions:** Configuration options for TCP servers.
- **UdpServerOptions:** Configuration options for UDP servers.
- **WebSocket:** A class representing a WebSocket connection, supporting text and binary frames.
- **SslContext:** SSL/TLS context for secure connections.

## Extensibility

XmobiTea.ProtonNetServer is designed with extensibility in mind. You can extend its functionality by:

- **Custom Sessions:** Inherit from `TcpSession`, `UdpSession`, or `WsSession` to create custom session handling logic.
- **Custom Protocols:** Implement custom protocol handling by overriding the `OnReceived` method in your session classes.
- **Server Events:** Override server lifecycle methods like `OnStarting`, `OnStopping`, `OnError`, etc., to add custom behavior.

## Contributing

Contributions to XmobiTea.ProtonNetServer are welcome! Please follow these guidelines:

1. Fork the repository.
2. Create a new branch with a descriptive name.
3. Commit your changes with clear and concise messages.
4. Open a pull request to the `main` branch.

## License

XmobiTea.ProtonNetServer is licensed under the MIT License. See the `LICENSE` file for more details.

## Acknowledgments

Special thanks to the open-source community for their contributions and continuous support.

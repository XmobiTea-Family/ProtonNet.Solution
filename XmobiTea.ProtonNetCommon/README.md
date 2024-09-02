
# XmobiTea.ProtonNetCommon

## Project Name
XmobiTea.ProtonNetCommon

## Installation Instructions
To install XmobiTea.ProtonNetCommon, follow these steps:
1. Clone the repository: `git clone https://github.com/yourusername/XmobiTea.ProtonNetCommon.git`
2. Navigate into the project directory: `cd XmobiTea.ProtonNetCommon`
3. Restore the project dependencies: `dotnet restore`
4. Build the project: `dotnet build`

## Features
- Provides common utilities and helpers for networking in ProtonNet projects.
- Includes reusable components for handling HTTP requests and responses.
- Implements optimized memory management for network communication.

## Usage Examples
Here are some examples of how to use XmobiTea.ProtonNetCommon:

### Example 1: Creating an HTTP Response
```csharp
using XmobiTea.ProtonNetCommon;
using XmobiTea.ProtonNetCommon.Types;

var response = new HttpResponse(StatusCode.OK);
response.SetContentType(".html");
response.SetBody("<html><body>Hello, World!</body></html>");

Console.WriteLine(response.BodyAsString);
```

### Example 2: Using TcpServerOptions
```csharp
using XmobiTea.ProtonNetServer.Options;

var options = new TcpServerOptions
{
    AcceptorBacklog = 2048,
    KeepAlive = true,
    NoDelay = true
};

Console.WriteLine($"Backlog: {options.AcceptorBacklog}, KeepAlive: {options.KeepAlive}, NoDelay: {options.NoDelay}");
```

## Supported Data Types
- `string` for text data, headers, and protocol.
- `byte[]` for raw data buffers and network communication.

## Extensibility
- Extend `HttpResponse` to handle additional HTTP status codes or headers.
- Customize `TcpServerOptions` and `UdpServerOptions` for specific network configurations.
- Implement additional helper methods or utilities as needed.

## Contributing Guidelines
1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit your changes: `git commit -am 'Add new feature'`
4. Push to the branch: `git push origin feature/new-feature`
5. Open a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments
- Inspired by common networking patterns and open-source libraries.
- Thanks to the community for their contributions and feedback.


# XmobiTea.ProtonNet.Server.WebApi

## Installation Instructions

To install the `XmobiTea.ProtonNet.Server.WebApi` package, follow these steps:

1. Ensure you have .NET Core SDK installed on your machine.
2. Clone the repository or download the source code.
3. Open the solution in your preferred IDE (e.g., Visual Studio, Visual Studio Code).
4. Restore the NuGet packages using the command:
   ```
   dotnet restore
   ```
5. Build the project:
   ```
   dotnet build
   ```

## Features

- **Web API Server**: Supports HTTP and HTTPS protocols for serving web API requests.
- **Session Management**: Includes session handling, supporting multiple simultaneous connections.
- **Static File Serving**: Capable of serving static files and folders with automatic caching and file watching.
- **Middleware Support**: Provides a middleware pipeline for request processing.
- **Custom Controllers**: Easily create and bind custom controllers for handling specific routes.

## Usage Examples

```csharp
public class Startup
{
    public static void Main(string[] args)
    {
        var server = new WebApiServerEntry.Builder()
            .SetStartupSettings(StartupSettings.NewBuilder()
                .SetHttpServer(HttpServerSettings.NewBuilder()
                    .SetEnable(true)
                    .SetPort(8080)
                    .Build())
                .Build())
            .Build()
            .GetServer();

        server.Start();
    }
}
```

## Supported Data Types

- **HttpRequest**: Represents an incoming HTTP request.
- **HttpResponse**: Represents an outgoing HTTP response.
- **MiddlewareContext**: Holds context information during middleware execution.
- **SessionConfigSettings**: Configuration for session handling.
- **StaticCache**: Manages caching for static files.

## Extensibility

The `XmobiTea.ProtonNet.Server.WebApi` package is designed with extensibility in mind:

- **Custom Middleware**: You can create your own middleware by implementing the appropriate delegate.
- **Custom Controllers**: Derive from `WebApiController` to create your own controllers with custom routes.

## Contributing Guidelines

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit them (`git commit -am 'Add new feature'`).
4. Push the branch (`git push origin feature-branch`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Acknowledgments

- Thanks to the contributors of the `XmobiTea` project.
- Special mention to the .NET community for their continuous support.

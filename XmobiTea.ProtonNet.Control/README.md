
# XmobiTea.ProtonNet.Control

## Installation Instructions

To install the `XmobiTea.ProtonNet.Control` library, simply include it in your project using the following NuGet command:

```bash
dotnet add package XmobiTea.ProtonNet.Control
```

Alternatively, you can add the package reference directly in your `.csproj` file:

```xml
<PackageReference Include="XmobiTea.ProtonNet.Control" Version="1.0.0" />
```

## Features

- **Platform-Specific Command Handling**: Execute commands tailored to different operating systems (Windows, OSX, Linux).
- **Service Management**: Start, stop, install, and uninstall Proton services on supported platforms.
- **Logging Integration**: Integrated with `XmobiTea.Logging` for detailed logging capabilities.
- **Customizable Command Execution**: Abstract classes and interfaces to allow customization and extension of command execution.

## Usage Examples

### Example: Running a Debug Command

```csharp
var handler = new WindowsExecuteHandler("ProtonInstanceName");
handler.Execute(Command.Debug);
```

### Example: Installing a Service

```csharp
var handler = new LinuxExecuteHandler("ProtonInstanceName");
handler.Execute(Command.Install);
```

### Example: Checking Service Status

```csharp
var handler = new OSXExecuteHandler("ProtonInstanceName");
handler.Execute(Command.Status);
```

## Supported Data Types

- `PlatformOS`: Enum representing the supported operating systems.
- `Command`: Enum representing the various commands supported by the handlers.
- `ProtonInstance`: Model representing the configuration of a Proton service instance.

## Extensibility

The library is designed with extensibility in mind. You can create custom handlers by inheriting from `AbstractExecuteHandler` and implementing the required methods for your specific use case.

### Creating a Custom Handler

```csharp
class CustomExecuteHandler : AbstractExecuteHandler
{
    public CustomExecuteHandler(string name) : base(name) { }

    public override PlatformOS GetPlatformOS() => PlatformOS.Unknown;

    protected override void OnExecuteVersion() { /* Custom implementation */ }
}
```

## Contributing Guidelines

Contributions to `XmobiTea.ProtonNet.Control` are welcome. Please follow these guidelines:

1. Fork the repository and create a new branch for your feature or bug fix.
2. Write tests for any new features or changes.
3. Submit a pull request with a detailed description of your changes.

## License

`XmobiTea.ProtonNet.Control` is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments

Special thanks to the open-source community for their continuous support and contributions.

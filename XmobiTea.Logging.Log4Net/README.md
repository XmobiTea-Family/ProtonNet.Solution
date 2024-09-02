# XmobiTea.Logging.Log4Net

## Installation

To install `XmobiTea.Logging.Log4Net`, you can use the NuGet Package Manager:
Install-Package XmobiTea.Logging.Log4Net

Or use the .NET CLI:
dotnet add package XmobiTea.Logging.Log4Net

## Features

- **Integration with Log4Net:** Provides seamless integration with the popular Log4Net logging framework.
- **Advanced Logging Features:** Leverage Log4Net’s powerful logging capabilities within the XmobiTea.Logging ecosystem.
- **Customizable Configurations:** Allows detailed configuration of Log4Net through XML or code-based setup.

## Usage Examples

```csharp
using XmobiTea.Logging.Log4Net;
using XmobiTea.Logging.Config;

var logger = LoggerFactory.CreateLogger(config => { config.UseLog4Net("log4net.config"); });

logger.LogInfo("This is an information log using Log4Net."); logger.LogError("This is an error log using Log4Net.");
```

## Supported Data Types

- **LogLevel:** Defines the level of logging (Debug, Info, Warning, Error, etc.).
- **Log4Net Configuration:** Supports XML-based configuration for Log4Net.

## Extensibility

`XmobiTea.Logging.Log4Net` can be extended by customizing Log4Net configurations to fit specific logging needs.

## Contributing

If you wish to contribute to this project, please open a pull request on GitHub. All contributions are welcome!

## License

`XmobiTea.Logging.Log4Net` is released under the MIT License. See the LICENSE file for more details.

## Acknowledgments

Thanks to the Log4Net and .NET development communities for providing the tools and resources to build this integration.

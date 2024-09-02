# XmobiTea.Logging

## Installation

To install `XmobiTea.Logging`, you can use the NuGet Package Manager:
Install-Package XmobiTea.Logging

Or use the .NET CLI:
dotnet add package XmobiTea.Logging

## Features

- **Flexible Logging Configuration:** Easily configure logging levels, outputs, and formats.
- **Multiple Output Destinations:** Supports logging to console, files, or custom destinations.
- **Performance Optimized:** Lightweight and designed for minimal performance overhead.

## Usage Examples
using XmobiTea.Logging; using XmobiTea.Logging.Config;

var logger = LoggerFactory.CreateLogger(config => { config.SetLevel(LogLevel.Debug); config.AddConsoleOutput(); config.AddFileOutput("log.txt"); });

logger.LogInformation("This is an information log."); logger.LogError("This is an error log.");

## Supported Data Types

- **LogLevel:** Defines the level of logging (Debug, Info, Warning, Error, etc.).
- **LoggerConfiguration:** Used to configure various aspects of the logger.

## Extensibility

`XmobiTea.Logging` can be extended by adding custom logging outputs or formats through the `LoggerConfiguration` API.

## Contributing

If you wish to contribute to this project, please open a pull request on GitHub. All contributions are welcome!

## License

`XmobiTea.Logging` is released under the MIT License. See the LICENSE file for more details.

## Acknowledgments

Thanks to the .NET development community for providing the tools and resources to build this library.

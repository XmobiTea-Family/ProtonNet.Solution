
# XmobiTea.ProtonNet.Control.Agent

## Installation

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/XmobiTea.ProtonNet.Control.Agent.git
   ```
2. Navigate to the project directory:
   ```
   cd XmobiTea.ProtonNet.Control.Agent
   ```
3. Restore the dependencies:
   ```
   dotnet restore
   ```
4. Build the project:
   ```
   dotnet build
   ```

## Features

- Provides an implementation for starting and controlling various server types (Web API, Socket) using different agent types (Service, Plain, WorkerService).
- Supports SSL/TLS configuration and session management.
- Includes utility classes for argument parsing and server configuration.

## Usage

### Example: Starting the Agent

To start the agent with specific configurations, use the following command:
```
dotnet run -- -agentType Plain -name "MyAgent" -binPath "/path/to/bin" -protonBinPath "/path/to/proton" -logPath "/path/to/log" -assemblyName "MyAssembly" -startupSettingsFilePath "/path/to/settings.json" -log4netFilePath "/path/to/log4net.config" -serverType WebApi
```

### Supported Arguments
- `-agentType`: Specifies the type of agent (`Plain`, `Service`, or `WorkerService`).
- `-name`: The name of the agent.
- `-binPath`: The path to the binary files.
- `-protonBinPath`: The path to the Proton binary files.
- `-logPath`: The path to the log files.
- `-assemblyName`: The name of the assembly to load.
- `-startupSettingsFilePath`: The path to the startup settings file in JSON format.
- `-log4netFilePath`: The path to the Log4Net configuration file.
- `-serverType`: The type of server to run (`WebApi`, `Socket`).

## Supported Data Types

- **AgentType**: Enumeration of agent types (Service, Plain, WorkerService).
- **ServerType**: Enumeration of server types (WebApi, Socket).
- **StartupAgentInfo**: Contains the necessary information for starting an agent, including paths and server configuration.
- **SessionConfigSettings**: Configuration for managing sessions, including keep-alive settings and buffer sizes.
- **SslConfigSettings**: Configuration for SSL/TLS, including certificate path and password.

## Extensibility

- The project is designed to be extensible. New agent types can be added by implementing the `IStartupAgent` interface.
- Custom server configurations can be created by extending the existing configuration classes or creating new ones.

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit them (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Create a new Pull Request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Acknowledgments

- [Newtonsoft.Json](https://www.newtonsoft.com/json) for JSON parsing and serialization.
- [Log4Net](https://logging.apache.org/log4net/) for logging support.

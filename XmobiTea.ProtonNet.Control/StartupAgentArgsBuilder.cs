using System.Text;

namespace XmobiTea.ProtonNet.Control
{
    /// <summary>
    /// The StartupAgentArgsBuilder class is used to build startup arguments for the Proton Agent.
    /// </summary>
    class StartupAgentArgsBuilder
    {
        /// <summary>
        /// Gets or sets the name of the agent.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the binary path for the agent.
        /// </summary>
        public string BinPath { get; set; }

        /// <summary>
        /// Gets or sets the Proton binary path.
        /// </summary>
        public string ProtonBinPath { get; set; }

        /// <summary>
        /// Gets or sets the log file path.
        /// </summary>
        public string LogPath { get; set; }

        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the startup settings file path.
        /// </summary>
        public string StartupSettingsFilePath { get; set; }

        /// <summary>
        /// Gets or sets the Log4net configuration file path.
        /// </summary>
        public string Log4netFilePath { get; set; }

        /// <summary>
        /// Gets or sets the server type.
        /// </summary>
        public string ServerType { get; set; }

        /// <summary>
        /// Gets or sets the agent type.
        /// </summary>
        public string AgentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the agent runs as a background service.
        /// </summary>
        public bool IsBackgroundService { get; set; }

        /// <summary>
        /// Sets the name of the agent.
        /// </summary>
        /// <param name="name">The name of the agent.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Sets the binary path for the agent.
        /// </summary>
        /// <param name="binPath">The binary path.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetBinPath(string binPath)
        {
            this.BinPath = binPath;
            return this;
        }

        /// <summary>
        /// Sets the Proton binary path.
        /// </summary>
        /// <param name="protonBinPath">The Proton binary path.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetProtonBinPath(string protonBinPath)
        {
            this.ProtonBinPath = protonBinPath;
            return this;
        }

        /// <summary>
        /// Sets the log file path.
        /// </summary>
        /// <param name="logPath">The log file path.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetLogPath(string logPath)
        {
            this.LogPath = logPath;
            return this;
        }

        /// <summary>
        /// Sets the assembly name.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetAssemblyName(string assemblyName)
        {
            this.AssemblyName = assemblyName;
            return this;
        }

        /// <summary>
        /// Sets the startup settings file path.
        /// </summary>
        /// <param name="startupSettingsFilePath">The startup settings file path.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetStartupSettingsFilePath(string startupSettingsFilePath)
        {
            this.StartupSettingsFilePath = startupSettingsFilePath;
            return this;
        }

        /// <summary>
        /// Sets the Log4net configuration file path.
        /// </summary>
        /// <param name="log4netFilePath">The Log4net configuration file path.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetLog4netFilePath(string log4netFilePath)
        {
            this.Log4netFilePath = log4netFilePath;
            return this;
        }

        /// <summary>
        /// Sets the server type.
        /// </summary>
        /// <param name="serverType">The server type.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetServerType(string serverType)
        {
            this.ServerType = serverType;
            return this;
        }

        /// <summary>
        /// Sets the agent type.
        /// </summary>
        /// <param name="agentType">The agent type.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetAgentType(string agentType)
        {
            this.AgentType = agentType;
            return this;
        }

        /// <summary>
        /// Sets whether the agent runs as a background service.
        /// </summary>
        /// <param name="isBackgroundService">Indicates if the agent is a background service.</param>
        /// <returns>The current instance of StartupAgentArgsBuilder.</returns>
        public StartupAgentArgsBuilder SetIsBackgroundService(bool isBackgroundService)
        {
            this.IsBackgroundService = isBackgroundService;
            return this;
        }

        /// <summary>
        /// Private constructor to enforce the use of the builder pattern.
        /// </summary>
        private StartupAgentArgsBuilder() { }

        /// <summary>
        /// Creates a new instance of StartupAgentArgsBuilder.
        /// </summary>
        /// <returns>A new instance of StartupAgentArgsBuilder.</returns>
        public static StartupAgentArgsBuilder NewBuilder() => new StartupAgentArgsBuilder();

        /// <summary>
        /// Builds the startup arguments string.
        /// </summary>
        /// <returns>The startup arguments string.</returns>
        public string Build()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"-name {this.Name} ");
            stringBuilder.Append($"-binPath '{this.BinPath}' ");
            stringBuilder.Append($"-protonBinPath '{this.ProtonBinPath}' ");
            stringBuilder.Append($"-logPath '{this.LogPath}' ");
            stringBuilder.Append($"-assemblyName {this.AssemblyName} ");
            stringBuilder.Append($"-startupSettingsFilePath '{this.StartupSettingsFilePath}' ");
            stringBuilder.Append($"-log4netFilePath '{this.Log4netFilePath}' ");
            stringBuilder.Append($"-serverType {this.ServerType.ToString()} ");
            stringBuilder.Append($"-agentType {this.AgentType.ToString()} ");
            stringBuilder.Append($"-isBackgroundService {this.IsBackgroundService.ToString()}");

            return stringBuilder.ToString();
        }

    }

}

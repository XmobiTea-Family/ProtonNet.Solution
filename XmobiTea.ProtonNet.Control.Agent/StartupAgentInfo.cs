using XmobiTea.ProtonNet.Control.Agent.Types;

namespace XmobiTea.ProtonNet.Control.Agent
{
    /// <summary>
    /// Represents the startup agent information.
    /// </summary>
    class StartupAgentInfo
    {
        /// <summary>
        /// Gets the name of the agent.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the path to the binary file.
        /// </summary>
        public string BinPath { get; }

        /// <summary>
        /// Gets the path to the Proton binary file.
        /// </summary>
        public string ProtonBinPath { get; }

        /// <summary>
        /// Gets the path to the log file.
        /// </summary>
        public string LogPath { get; }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Gets the path to the startup settings file.
        /// </summary>
        public string StartupSettingsFilePath { get; }

        /// <summary>
        /// Gets the path to the Log4net configuration file.
        /// </summary>
        public string Log4netFilePath { get; }

        /// <summary>
        /// Gets the type of server.
        /// </summary>
        public ServerType ServerType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupAgentInfo"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to create the instance.</param>
        private StartupAgentInfo(Builder builder)
        {
            this.Name = builder.Name;
            this.BinPath = builder.BinPath;
            this.ProtonBinPath = builder.ProtonBinPath;
            this.LogPath = builder.LogPath;
            this.AssemblyName = builder.AssemblyName;
            this.StartupSettingsFilePath = builder.StartupSettingsFilePath;
            this.Log4netFilePath = builder.Log4netFilePath;
            this.ServerType = builder.ServerType;
        }

        /// <summary>
        /// Creates a new builder for the <see cref="StartupAgentInfo"/> class.
        /// </summary>
        /// <returns>A new instance of the builder.</returns>
        public static Builder NewBuider() => new Builder();

        /// <summary>
        /// Builder class for creating instances of <see cref="StartupAgentInfo"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the name of the agent.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the path to the binary file.
            /// </summary>
            public string BinPath { get; set; }

            /// <summary>
            /// Gets or sets the path to the Proton binary file.
            /// </summary>
            public string ProtonBinPath { get; set; }

            /// <summary>
            /// Gets or sets the path to the log file.
            /// </summary>
            public string LogPath { get; set; }

            /// <summary>
            /// Gets or sets the name of the assembly.
            /// </summary>
            public string AssemblyName { get; set; }

            /// <summary>
            /// Gets or sets the path to the startup settings file.
            /// </summary>
            public string StartupSettingsFilePath { get; set; }

            /// <summary>
            /// Gets or sets the path to the Log4net configuration file.
            /// </summary>
            public string Log4netFilePath { get; set; }

            /// <summary>
            /// Gets or sets the type of server.
            /// </summary>
            public ServerType ServerType { get; set; }

            /// <summary>
            /// Sets the name of the agent.
            /// </summary>
            /// <param name="name">The name of the agent.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetName(string name)
            {
                this.Name = name;
                return this;
            }

            /// <summary>
            /// Sets the path to the binary file.
            /// </summary>
            /// <param name="binPath">The path to the binary file.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetBinPath(string binPath)
            {
                if (!string.IsNullOrEmpty(binPath)) binPath = binPath.Replace("'", string.Empty);

                this.BinPath = binPath;
                return this;
            }

            /// <summary>
            /// Sets the path to the Proton binary file.
            /// </summary>
            /// <param name="protonBinPath">The path to the Proton binary file.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetProtonBinPath(string protonBinPath)
            {
                if (!string.IsNullOrEmpty(protonBinPath)) protonBinPath = protonBinPath.Replace("'", string.Empty);

                this.ProtonBinPath = protonBinPath;
                return this;
            }

            /// <summary>
            /// Sets the path to the log file.
            /// </summary>
            /// <param name="logPath">The path to the log file.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetLogPath(string logPath)
            {
                if (!string.IsNullOrEmpty(logPath)) logPath = logPath.Replace("'", string.Empty);

                this.LogPath = logPath;
                return this;
            }

            /// <summary>
            /// Sets the name of the assembly.
            /// </summary>
            /// <param name="assemblyName">The name of the assembly.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetAssemblyName(string assemblyName)
            {
                this.AssemblyName = assemblyName;
                return this;
            }

            /// <summary>
            /// Sets the path to the startup settings file.
            /// </summary>
            /// <param name="startupSettingsFilePath">The path to the startup settings file.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetStartupSettingsFilePath(string startupSettingsFilePath)
            {
                if (!string.IsNullOrEmpty(startupSettingsFilePath)) startupSettingsFilePath = startupSettingsFilePath.Replace("'", string.Empty);

                this.StartupSettingsFilePath = startupSettingsFilePath;
                return this;
            }

            /// <summary>
            /// Sets the path to the Log4net configuration file.
            /// </summary>
            /// <param name="log4netFilePath">The path to the Log4net configuration file.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetLog4netFilePath(string log4netFilePath)
            {
                if (!string.IsNullOrEmpty(log4netFilePath)) log4netFilePath = log4netFilePath.Replace("'", string.Empty);

                this.Log4netFilePath = log4netFilePath;
                return this;
            }

            /// <summary>
            /// Sets the type of server.
            /// </summary>
            /// <param name="serverType">The type of server.</param>
            /// <returns>The builder instance.</returns>
            public Builder SetServerType(ServerType serverType)
            {
                this.ServerType = serverType;
                return this;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Builds a new instance of the <see cref="StartupAgentInfo"/> class.
            /// </summary>
            /// <returns>A new instance of the <see cref="StartupAgentInfo"/> class.</returns>
            public StartupAgentInfo Build() => new StartupAgentInfo(this);

        }

    }

}

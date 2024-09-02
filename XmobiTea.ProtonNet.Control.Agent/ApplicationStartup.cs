using System;
using XmobiTea.Logging;
using XmobiTea.Logging.Log4Net;
using XmobiTea.ProtonNet.Control.Agent.Agents;
using XmobiTea.ProtonNet.Control.Agent.Helper;
using XmobiTea.ProtonNet.Control.Agent.Types;

namespace XmobiTea.ProtonNet.Control.Agent
{
    /// <summary>
    /// Handles the startup of the application, including configuration and agent creation.
    /// </summary>
    class ApplicationStartup
    {
        /// <summary>
        /// Gets the type of agent to be used.
        /// </summary>
        public AgentType AgentType { get; }

        /// <summary>
        /// Gets the startup agent information.
        /// </summary>
        public StartupAgentInfo StartupAgentInfo { get; }

        /// <summary>
        /// Gets a value indicating whether the agent is a background service.
        /// </summary>
        public bool IsBackgroundService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartup"/> class.
        /// </summary>
        /// <param name="builder">The builder used to initialize the application startup.</param>
        private ApplicationStartup(Builder builder)
        {
            this.AgentType = builder.AgentType;
            this.StartupAgentInfo = builder.StartupAgentInfo;
            this.IsBackgroundService = builder.IsBackgroundService;
        }

        /// <summary>
        /// Creates a new startup agent based on the configuration.
        /// </summary>
        /// <returns>The newly created startup agent.</returns>
        public IStartupAgent NewStartupAgent()
        {
            this.SetupLog(this.StartupAgentInfo);

            return this.CreateStartupAgent(this.AgentType, this.StartupAgentInfo);
        }

        /// <summary>
        /// Creates a startup agent based on the agent type.
        /// </summary>
        /// <param name="agentType">The type of agent to create.</param>
        /// <param name="startupAgentInfo">The startup agent information.</param>
        /// <returns>The newly created startup agent.</returns>
        private IStartupAgent CreateStartupAgent(AgentType agentType, StartupAgentInfo startupAgentInfo)
        {
            if (agentType == AgentType.Service)
            {
#if NETCOREAPP
                return WorkerServiceStartupAgent.NewBuilder().SetStartupAgentInfo(startupAgentInfo).Build();
#else
                return ServiceStartupAgent.NewBuilder().SetStartupAgentInfo(startupAgentInfo).Build();
#endif
            }
            else if (agentType == AgentType.Plain)
            {
                return PlainStartupAgent.NewBuilder().SetStartupAgentInformation(startupAgentInfo).Build();
            }
            else
            {
                throw new System.Exception("Invalid agent type " + agentType);
            }
        }

        /// <summary>
        /// Sets up the logging configuration based on the startup agent information.
        /// </summary>
        /// <param name="startupAgentInfo">The startup agent information.</param>
        private void SetupLog(StartupAgentInfo startupAgentInfo)
        {
            log4net.GlobalContext.Properties["LogPath"] = startupAgentInfo.LogPath;
            var configFileInfo = new System.IO.FileInfo(startupAgentInfo.Log4netFilePath);

            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(configFileInfo);

                System.Console.WriteLine("Server init log success.");
            }
            else
            {
                LogManager.SetDefaultLoggerFactory(DefaultLogType.Console);

                System.Console.WriteLine("Log not exists.");
            }
        }

        /// <summary>
        /// Creates a new builder for <see cref="ApplicationStartup"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for <see cref="ApplicationStartup"/> with fluent API.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the type of agent to be used.
            /// </summary>
            public AgentType AgentType { get; set; }

            /// <summary>
            /// Gets or sets the startup agent information.
            /// </summary>
            public StartupAgentInfo StartupAgentInfo { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the agent is a background service.
            /// </summary>
            public bool IsBackgroundService { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the type of agent to be used.
            /// </summary>
            /// <param name="agentType">The type of agent.</param>
            /// <returns>The current instance of the builder.</returns>
            public Builder SetAgentType(AgentType agentType)
            {
                this.AgentType = agentType;
                return this;
            }

            /// <summary>
            /// Sets the startup agent information.
            /// </summary>
            /// <param name="startupAgentInfo">The startup agent information.</param>
            /// <returns>The current instance of the builder.</returns>
            public Builder SetStartupAgentInfo(StartupAgentInfo startupAgentInfo)
            {
                this.StartupAgentInfo = startupAgentInfo;
                return this;
            }

            /// <summary>
            /// Parses command-line arguments to configure the builder.
            /// </summary>
            /// <param name="args">Command-line arguments.</param>
            /// <returns>The current instance of the builder.</returns>
            public Builder SetArgs(string[] args)
            {
                this.AgentType = this.GetAgentType(args);
                this.StartupAgentInfo = this.GetStartupAgentInfo(args);
                this.IsBackgroundService = this.GetIsBackgroundService(args);

                return this;
            }

            /// <summary>
            /// Retrieves the agent type from command-line arguments.
            /// </summary>
            /// <param name="args">Command-line arguments.</param>
            /// <returns>The type of agent.</returns>
            private AgentType GetAgentType(string[] args) => (AgentType)System.Enum.Parse(typeof(AgentType), ArgsUtils.GetArgValue(args, names: "-agentType"), true);

            /// <summary>
            /// Retrieves the startup agent information from command-line arguments.
            /// </summary>
            /// <param name="args">Command-line arguments.</param>
            /// <returns>The startup agent information.</returns>
            private StartupAgentInfo GetStartupAgentInfo(string[] args)
            {
                var answer = StartupAgentInfo.NewBuider()
                    .SetName(ArgsUtils.GetArgValue(args, names: "-name"))
                    .SetBinPath(ArgsUtils.GetArgValue(args, names: "-binPath"))
                    .SetProtonBinPath(ArgsUtils.GetArgValue(args, names: "-protonBinPath"))
                    .SetLogPath(ArgsUtils.GetArgValue(args, names: "-logPath"))
                    .SetAssemblyName(ArgsUtils.GetArgValue(args, names: "-assemblyName"))
                    .SetStartupSettingsFilePath(ArgsUtils.GetArgValue(args, names: "-startupSettingsFilePath"))
                    .SetLog4netFilePath(ArgsUtils.GetArgValue(args, names: "-log4netFilePath"))
                    .SetServerType((ServerType)System.Enum.Parse(typeof(ServerType), ArgsUtils.GetArgValue(args, names: "-serverType"), true))
                    .Build();

                return answer;
            }

            /// <summary>
            /// Retrieves whether the agent is a background service from command-line arguments.
            /// </summary>
            /// <param name="args">Command-line arguments.</param>
            /// <returns><c>true</c> if the agent is a background service; otherwise, <c>false</c>.</returns>
            private bool GetIsBackgroundService(string[] args) => Convert.ToBoolean(ArgsUtils.GetArgValue(args, names: "-isBackgroundService"));

            /// <summary>
            /// Builds an instance of <see cref="ApplicationStartup"/> using the current configuration.
            /// </summary>
            /// <returns>An instance of <see cref="ApplicationStartup"/>.</returns>
            public ApplicationStartup Build() => new ApplicationStartup(this);

        }

    }

}

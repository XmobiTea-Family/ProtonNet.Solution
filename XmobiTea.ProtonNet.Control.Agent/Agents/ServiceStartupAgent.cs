#if !NETCOREAPP
using System;
using System.IO;
using System.ServiceProcess;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Control.Agent.Types;

namespace XmobiTea.ProtonNet.Control.Agent.Agents
{
    /// <summary>
    /// Service base implementation for starting and stopping a service with a specific startup agent.
    /// </summary>
    class ServiceStartupAgent : ServiceBase, IStartupAgent
    {
        private ILogger logger { get; }
        private AppDomain appDomain { get; set; }

        private StartupAgentInfo startupAgentInfo { get; }

        private ServiceStartupAgent(Builder builder)
        {
            this.logger = LogManager.GetLogger(this);

            this.startupAgentInfo = builder.StartupAgentInfo;
        }

        /// <summary>
        /// Starts the service and executes the startup agent in a new application domain.
        /// </summary>
        public void Start()
        {
            var currentDomain = AppDomain.CurrentDomain;

            this.appDomain = AppDomain.CreateDomain(this.startupAgentInfo.Name, currentDomain.Evidence, new AppDomainSetup()
            {
                ApplicationName = this.startupAgentInfo.Name,
                ConfigurationFile = $"./XmobiTea.ProtonNet.Control.Agent.exe.config",
                ShadowCopyFiles = "true",
                CachePath = Path.Combine(this.startupAgentInfo.BinPath, "__cache"),
            });

            var startupAgentArgs = StartupAgentArgsBuilder.NewBuilder()
                .SetStartupAgentInfo(this.startupAgentInfo)
                .SetAgentType(AgentType.Plain)
                .SetIsBackgroundService(true)
                .Build();

            var ret = this.appDomain.ExecuteAssemblyByName("XmobiTea.ProtonNet.Control.Agent", args: startupAgentArgs);
            Environment.ExitCode = ret;
        }

        /// <summary>
        /// Handles the OnStart event when the service starts.
        /// </summary>
        /// <param name="args">Arguments for the start event.</param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.logger.Info("[ServiceStartupAgent] Service start...");

            this.Start();
        }

        /// <summary>
        /// Handles the OnStop event when the service stops.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            this.logger.Info("[ServiceStartupAgent] Service stopped.");

            AppDomain.Unload(this.appDomain);
        }

        /// <summary>
        /// Creates a new Builder instance.
        /// </summary>
        /// <returns>A new Builder instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for creating instances of ServiceStartupAgent.
        /// </summary>
        public class Builder
        {
            public StartupAgentInfo StartupAgentInfo { get; set; }

            /// <summary>
            /// Sets the startup agent information for the builder.
            /// </summary>
            /// <param name="startupAgentInfo">The startup agent information to set.</param>
            /// <returns>The updated Builder instance.</returns>
            public Builder SetStartupAgentInfo(StartupAgentInfo startupAgentInfo)
            {
                this.StartupAgentInfo = startupAgentInfo;
                return this;
            }

            internal Builder() { }

            /// <summary>
            /// Builds a new instance of ServiceStartupAgent using the builder configuration.
            /// </summary>
            /// <returns>A new instance of ServiceStartupAgent.</returns>
            public ServiceStartupAgent Build() => new ServiceStartupAgent(this);
        }
    }

}
#endif
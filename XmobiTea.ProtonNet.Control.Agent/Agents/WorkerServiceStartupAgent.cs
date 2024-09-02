#if NETCOREAPP
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XmobiTea.Logging;

namespace XmobiTea.ProtonNet.Control.Agent.Agents
{
    /// <summary>
    /// Background service implementation for starting and stopping a worker service with a specific startup agent.
    /// </summary>
    class WorkerServiceStartupAgent : BackgroundService, IStartupAgent
    {
        private ILogger logger { get; }

        private StartupAgentInfo startupAgentInfo { get; }

        private IStartupAgent plainStartupAgent { get; }

        private WorkerServiceStartupAgent(Builder builder)
        {
            this.logger = LogManager.GetLogger(this);

            this.startupAgentInfo = builder.StartupAgentInfo;

            this.plainStartupAgent = PlainStartupAgent.NewBuilder()
                .SetStartupAgentInformation(this.startupAgentInfo)
                .Build();
        }

        private WorkerServiceStartupAgent()
        {
            this.logger = LogManager.GetLogger(this);

            this.startupAgentInfo = StaticProperty.StartupAgentInfo;

            this.plainStartupAgent = PlainStartupAgent.NewBuilder()
                .SetStartupAgentInformation(this.startupAgentInfo)
                .Build();
        }

        /// <summary>
        /// Starts the worker service agent.
        /// </summary>
        public void Start()
        {
            //var currentDomain = AppDomain.CurrentDomain;

            //var startupAgentArgs = StartupAgentArgsBuilder.NewBuilder()
            //    .SetStartupAgentInfo(this.startupAgentInfo)
            //    .SetAgentType(AgentType.Plain)
            //    .SetIsBackgroundService(true)
            //    .Build();

            //currentDomain.ExecuteAssemblyByName("XmobiTea.ProtonNet.Control.Agent", args: startupAgentArgs);

            this.plainStartupAgent.Start();
        }

        /// <summary>
        /// Stops the worker service agent.
        /// </summary>
        public void Stop()
        {
            this.plainStartupAgent.Stop();
        }

        /// <summary>
        /// Asynchronously starts the background service.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously stops the background service.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.Stop();

            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Executes the background service logic asynchronously.
        /// </summary>
        /// <param name="stoppingToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Start();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates a new Builder instance for configuring WorkerServiceStartupAgent.
        /// </summary>
        /// <returns>A new Builder instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for creating instances of WorkerServiceStartupAgent.
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
            /// Builds a new instance of WorkerServiceStartupAgent using the builder configuration.
            /// </summary>
            /// <returns>A new instance of WorkerServiceStartupAgent.</returns>
            public WorkerServiceStartupAgent Build() => new WorkerServiceStartupAgent(this);
        }

    }

}
#endif
using XmobiTea.ProtonNet.Control.Agent.Types;

namespace XmobiTea.ProtonNet.Control.Agent
{
    /// <summary>
    /// Builds arguments for starting an agent.
    /// </summary>
    class StartupAgentArgsBuilder
    {
        /// <summary>
        /// Gets or sets the startup agent information.
        /// </summary>
        public StartupAgentInfo StartupAgentInfo { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent.
        /// </summary>
        public AgentType AgentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the agent is a background service.
        /// </summary>
        public bool IsBackgroundService { get; set; }

        /// <summary>
        /// Sets the startup agent information.
        /// </summary>
        /// <param name="startupAgentInfo">The startup agent information.</param>
        /// <returns>The current instance of the builder.</returns>
        public StartupAgentArgsBuilder SetStartupAgentInfo(StartupAgentInfo startupAgentInfo)
        {
            this.StartupAgentInfo = startupAgentInfo;
            return this;
        }

        /// <summary>
        /// Sets the type of the agent.
        /// </summary>
        /// <param name="agentType">The type of the agent.</param>
        /// <returns>The current instance of the builder.</returns>
        public StartupAgentArgsBuilder SetAgentType(AgentType agentType)
        {
            this.AgentType = agentType;
            return this;
        }

        /// <summary>
        /// Sets whether the agent is a background service.
        /// </summary>
        /// <param name="isBackgroundService">A value indicating whether the agent is a background service.</param>
        /// <returns>The current instance of the builder.</returns>
        public StartupAgentArgsBuilder SetIsBackgroundService(bool isBackgroundService)
        {
            this.IsBackgroundService = isBackgroundService;
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupAgentArgsBuilder"/> class.
        /// </summary>
        private StartupAgentArgsBuilder() { }

        /// <summary>
        /// Creates a new builder for <see cref="StartupAgentArgsBuilder"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="StartupAgentArgsBuilder"/> class.</returns>
        public static StartupAgentArgsBuilder NewBuilder() => new StartupAgentArgsBuilder();

        /// <summary>
        /// Builds the arguments for starting the agent.
        /// </summary>
        /// <returns>An array of arguments as strings.</returns>
        public string[] Build()
        {
            var answer = new string[20];
            answer[0] = "-name";
            answer[1] = this.StartupAgentInfo.Name;

            answer[2] = "-binPath";
            answer[3] = this.StartupAgentInfo.BinPath;

            answer[4] = "-protonBinPath";
            answer[5] = this.StartupAgentInfo.ProtonBinPath;

            answer[6] = "-logPath";
            answer[7] = this.StartupAgentInfo.LogPath;

            answer[8] = "-assemblyName";
            answer[9] = this.StartupAgentInfo.AssemblyName;

            answer[10] = "-startupSettingsFilePath";
            answer[11] = this.StartupAgentInfo.StartupSettingsFilePath;

            answer[12] = "-log4netFilePath";
            answer[13] = this.StartupAgentInfo.Log4netFilePath;

            answer[14] = "-serverType";
            answer[15] = this.StartupAgentInfo.ServerType.ToString();

            answer[16] = "-agentType";
            answer[17] = this.AgentType.ToString();

            answer[18] = "-isBackgroundService";
            answer[19] = this.IsBackgroundService.ToString();

            return answer;
        }

    }

}

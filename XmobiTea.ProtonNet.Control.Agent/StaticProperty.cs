namespace XmobiTea.ProtonNet.Control.Agent
{
    /// <summary>
    /// Provides static properties for the agent.
    /// </summary>
    static class StaticProperty
    {
#if NETCOREAPP
        /// <summary>
        /// Gets or sets the information related to the startup agent.
        /// </summary>
        public static StartupAgentInfo StartupAgentInfo { get; set; }
#endif

    }

}

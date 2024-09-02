namespace XmobiTea.ProtonNet.Control.Models
{
    /// <summary>
    /// Represents a configuration instance for a Proton server.
    /// </summary>
    class ProtonInstance
    {
        /// <summary>
        /// Gets or sets the name of the Proton instance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is enabled.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Gets or sets the type of server for this Proton instance.
        /// </summary>
        public string ServerType { get; set; }

        /// <summary>
        /// Gets or sets the path to the binary file for the instance.
        /// </summary>
        public string BinPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly for the instance.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the path to the startup settings file for the instance.
        /// </summary>
        public string StartupSettingsFilePath { get; set; }

        /// <summary>
        /// Gets or sets the path to the Log4Net configuration file for the instance.
        /// </summary>
        public string Log4NetFilePath { get; set; }

        /// <summary>
        /// Gets or sets the startup type for the instance.
        /// </summary>
        public string StartupType { get; set; }

    }

}

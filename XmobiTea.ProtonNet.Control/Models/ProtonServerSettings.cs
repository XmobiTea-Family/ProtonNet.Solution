using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Control.Models
{
    /// <summary>
    /// Represents the settings for a Proton server instance.
    /// </summary>
    class ProtonServerSettings
    {
        /// <summary>
        /// Gets or sets the target runtime environment for the Proton server.
        /// </summary>
        public string TargetRuntime { get; set; }

        /// <summary>
        /// Gets or sets the target framework for the Proton server.
        /// </summary>
        public string TargetFramework { get; set; }

        /// <summary>
        /// Gets or sets the list of Proton instances configured for the server.
        /// </summary>
        public List<ProtonInstance> Instances { get; set; }

    }

}

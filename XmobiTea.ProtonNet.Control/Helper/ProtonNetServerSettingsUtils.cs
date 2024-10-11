using Newtonsoft.Json;
using System;
using System.IO;
using XmobiTea.ProtonNet.Control.Models;

namespace XmobiTea.ProtonNet.Control.Helper
{
    /// <summary>
    /// Utility class for handling Proton server settings.
    /// </summary>
    class ProtonNetServerSettingsUtils
    {
        /// <summary>
        /// The path to the Proton server settings JSON file.
        /// </summary>
        private static readonly string ProtonNetServerSettingsFilePath = "ProtonNetServerSettings.json";

        /// <summary>
        /// Retrieves a Proton instance by name from the Proton server settings.
        /// </summary>
        /// <param name="name">The name of the Proton instance to retrieve.</param>
        /// <returns>The ProtonInstance matching the specified name.</returns>
        /// <exception cref="Exception">Thrown when the instance is not found in the settings file.</exception>
        public static ProtonNetInstance GetInstance(string name)
        {
            var protonNetServerSettings = GetProtonNetServerSettings();

            var answer = protonNetServerSettings.Instances.Find(x => x.Name == name);

            if (answer == null)
                throw new Exception($"Cannot find instance with name {name} in ProtonNetServerSettings.json file.");

            return answer;
        }

        /// <summary>
        /// Retrieves the Proton server settings from the settings JSON file.
        /// </summary>
        /// <returns>The ProtonNetServerSettings object parsed from the JSON file.</returns>
        /// <exception cref="Exception">Thrown when the settings file cannot be found or the Instances field is missing.</exception>
        public static ProtonNetServerSettings GetProtonNetServerSettings()
        {
            var finalProtonNetServerSettingsFilePath = LibraryUtils.CombineFromRootPath(ProtonNetServerSettingsFilePath);

            if (!File.Exists(finalProtonNetServerSettingsFilePath))
                throw new Exception("Cannot find ProtonNetServerSettings.json file.");

            var answer = JsonConvert.DeserializeObject<ProtonNetServerSettings>(File.ReadAllText(finalProtonNetServerSettingsFilePath));

            if (answer.Instances == null)
                throw new Exception("Missing Instances field in ProtonNetServerSettings.json file.");

            return answer;
        }

    }

}

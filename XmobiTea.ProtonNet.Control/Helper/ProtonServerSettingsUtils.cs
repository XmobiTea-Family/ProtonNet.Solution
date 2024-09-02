using Newtonsoft.Json;
using System;
using System.IO;
using XmobiTea.ProtonNet.Control.Models;

namespace XmobiTea.ProtonNet.Control.Helper
{
    /// <summary>
    /// Utility class for handling Proton server settings.
    /// </summary>
    class ProtonServerSettingsUtils
    {
        /// <summary>
        /// The path to the Proton server settings JSON file.
        /// </summary>
        private static readonly string ProtonServerSettingsFilePath = "ProtonServerSettings.json";

        /// <summary>
        /// Retrieves a Proton instance by name from the Proton server settings.
        /// </summary>
        /// <param name="name">The name of the Proton instance to retrieve.</param>
        /// <returns>The ProtonInstance matching the specified name.</returns>
        /// <exception cref="Exception">Thrown when the instance is not found in the settings file.</exception>
        public static ProtonInstance GetInstance(string name)
        {
            var protonServerSettings = GetProtonServerSettings();

            var answer = protonServerSettings.Instances.Find(x => x.Name == name);

            if (answer == null)
                throw new Exception($"Cannot find instance with name {name} in ProtonServerSettings.json file.");

            return answer;
        }

        /// <summary>
        /// Retrieves the Proton server settings from the settings JSON file.
        /// </summary>
        /// <returns>The ProtonServerSettings object parsed from the JSON file.</returns>
        /// <exception cref="Exception">Thrown when the settings file cannot be found or the Instances field is missing.</exception>
        public static ProtonServerSettings GetProtonServerSettings()
        {
            var finalProtonServerSettingsFilePath = LibraryUtils.CombineFromRootPath(ProtonServerSettingsFilePath);

            if (!File.Exists(finalProtonServerSettingsFilePath))
                throw new Exception("Cannot find ProtonServerSettings.json file.");

            var answer = JsonConvert.DeserializeObject<ProtonServerSettings>(File.ReadAllText(finalProtonServerSettingsFilePath));

            if (answer.Instances == null)
                throw new Exception("Missing Instances field in ProtonServerSettings.json file.");

            return answer;
        }

    }

}

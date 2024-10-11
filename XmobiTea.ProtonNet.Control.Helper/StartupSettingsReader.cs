using Newtonsoft.Json;
using System.IO;
using XmobiTea.ProtonNet.Control.Helper.Models;

namespace XmobiTea.ProtonNet.Control.Helper
{
    /// <summary>
    /// Provides implementations for reading Web API and Socket startup settings from JSON files.
    /// This class implements the <see cref="IStartupSettingsReader"/> interface to deserialize 
    /// JSON files into their corresponding startup settings objects.
    /// </summary>
    public class StartupSettingsReader : IStartupSettingsReader
    {
        /// <summary>
        /// Loads the Web API startup settings from the specified file path.
        /// It reads the content of the file and deserializes it into an instance of 
        /// <see cref="WebApiStartupSettings"/>.
        /// </summary>
        /// <param name="startupSettingsFilePath">The path to the JSON file containing the Web API startup settings.</param>
        /// <returns>An instance of <see cref="WebApiStartupSettings"/> deserialized from the JSON file.</returns>
        public WebApiStartupSettings LoadWebApiStartupSettings(string startupSettingsFilePath) =>
            JsonConvert.DeserializeObject<WebApiStartupSettings>(File.ReadAllText(startupSettingsFilePath));

        /// <summary>
        /// Loads the Socket startup settings from the specified file path.
        /// It reads the content of the file and deserializes it into an instance of 
        /// <see cref="SocketStartupSettings"/>.
        /// </summary>
        /// <param name="startupSettingsFilePath">The path to the JSON file containing the Socket startup settings.</param>
        /// <returns>An instance of <see cref="SocketStartupSettings"/> deserialized from the JSON file.</returns>
        public SocketStartupSettings LoadSocketStartupSettings(string startupSettingsFilePath) =>
            JsonConvert.DeserializeObject<SocketStartupSettings>(File.ReadAllText(startupSettingsFilePath));

    }

}

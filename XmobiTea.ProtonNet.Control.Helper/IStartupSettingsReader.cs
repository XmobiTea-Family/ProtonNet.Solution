using XmobiTea.ProtonNet.Control.Helper.Models;

namespace XmobiTea.ProtonNet.Control.Helper
{
    /// <summary>
    /// Defines methods for reading startup settings related to Web API and Socket configurations.
    /// This interface provides methods to load startup settings from JSON files for both Web API
    /// and Socket applications.
    /// </summary>
    public interface IStartupSettingsReader
    {
        /// <summary>
        /// Loads the Web API startup settings from the specified file path.
        /// It reads the content of the file and deserializes it into an instance of 
        /// <see cref="WebApiStartupSettings"/>.
        /// </summary>
        /// <param name="startupSettingsFilePath">The path to the JSON file containing the Web API startup settings.</param>
        /// <returns>An instance of <see cref="WebApiStartupSettings"/> deserialized from the JSON file.</returns>
        WebApiStartupSettings LoadWebApiStartupSettings(string startupSettingsFilePath);
        /// <summary>
        /// Loads the Socket startup settings from the specified file path.
        /// </summary>
        /// <param name="startupSettingsFilePath">The path to the JSON file containing the Socket startup settings.</param>
        /// <returns>An instance of <see cref="SocketStartupSettings"/> deserialized from the JSON file.</returns>
        SocketStartupSettings LoadSocketStartupSettings(string startupSettingsFilePath);

    }

}

namespace XmobiTea.ProtonNet.Control.Types
{
    /// <summary>
    /// The Command enum defines the various commands that can be issued to control the ProtonNet application.
    /// </summary>
    enum Command
    {
        /// <summary>
        /// Command to display the version information.
        /// </summary>
        Version,

        /// <summary>
        /// Command to display help information.
        /// </summary>
        Help,

        /// <summary>
        /// Command to run the application in debug mode.
        /// </summary>
        Debug,

        /// <summary>
        /// Command to start the application.
        /// </summary>
        Start,

        /// <summary>
        /// Command to stop the application.
        /// </summary>
        Stop,

        /// <summary>
        /// Command to restart the application.
        /// </summary>
        Restart,

        /// <summary>
        /// Command to install the application as a service.
        /// </summary>
        Install,

        /// <summary>
        /// Command to uninstall the application service.
        /// </summary>
        Uninstall,

        /// <summary>
        /// Command to check the status of the application.
        /// </summary>
        Status,

        /// <summary>
        /// Command to view application logs.
        /// </summary>
        Log,

    }

}

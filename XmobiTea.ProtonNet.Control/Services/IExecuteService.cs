using System.Collections.Generic;
#if NETCOREAPP
using System.Runtime.InteropServices;
#endif
using XmobiTea.ProtonNet.Control.Handlers;
using XmobiTea.ProtonNet.Control.Types;

namespace XmobiTea.ProtonNet.Control.Services
{
    /// <summary>
    /// Interface for executing commands based on the platform.
    /// </summary>
    interface IExecuteService
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        void Execute(Command command);
    }

    /// <summary>
    /// Implementation of IExecuteService that manages and executes commands using platform-specific handlers.
    /// </summary>
    class ExecuteService : IExecuteService
    {
        /// <summary>
        /// Gets a dictionary mapping platform OS to their corresponding execute handlers.
        /// </summary>
        private IDictionary<PlatformOS, IExecuteHandler> executeHandlerDict { get; }

        /// <summary>
        /// Initializes a new instance of the ExecuteService class.
        /// </summary>
        public ExecuteService() => this.executeHandlerDict = new Dictionary<PlatformOS, IExecuteHandler>();

        /// <summary>
        /// Adds an execute handler to the service.
        /// </summary>
        /// <param name="executeHandler">The execute handler to add.</param>
        public void AddExecuteHandler(IExecuteHandler executeHandler) => this.executeHandlerDict[executeHandler.GetPlatformOS()] = executeHandler;

        /// <summary>
        /// Executes the specified command using the appropriate handler for the current platform.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void Execute(Command command)
        {
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.executeHandlerDict[PlatformOS.Windows].Execute(command);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                this.executeHandlerDict[PlatformOS.OSX].Execute(command);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                this.executeHandlerDict[PlatformOS.Linux].Execute(command);
            else
                throw new System.Exception("Invalid OS platform");
#else
            this.executeHandlerDict[PlatformOS.Windows].Execute(command);
#endif
        }

    }

}

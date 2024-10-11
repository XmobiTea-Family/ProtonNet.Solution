using System;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Logging;
using XmobiTea.ProtonNet.Control.Helper;
using XmobiTea.ProtonNet.Control.Models;
using XmobiTea.ProtonNet.Control.Types;

namespace XmobiTea.ProtonNet.Control.Handlers
{
    /// <summary>
    /// Delegate for executing commands.
    /// </summary>
    delegate void CommandRunDelegate();

    /// <summary>
    /// Interface for command execution handlers.
    /// </summary>
    interface IExecuteHandler
    {
        /// <summary>
        /// Gets the platform OS for this handler.
        /// </summary>
        /// <returns>The platform OS.</returns>
        PlatformOS GetPlatformOS();

        /// <summary>
        /// Executes a specific command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        void Execute(Command command);
    }

    /// <summary>
    /// Abstract base class for command execution handlers.
    /// </summary>
    abstract class AbstractExecuteHandler : IExecuteHandler
    {
        private IDictionary<Command, CommandRunDelegate> commandExecuteDict { get; }

        protected ILogger logger { get; }
        protected string name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractExecuteHandler"/> class.
        /// </summary>
        /// <param name="name">The name of the handler.</param>
        public AbstractExecuteHandler(string name)
        {
            this.logger = LogManager.GetLogger(this);
            this.name = name;

            this.commandExecuteDict = new Dictionary<Command, CommandRunDelegate>();
            this.AddCommandExecutes();
        }

        /// <summary>
        /// Gets the platform OS for this handler.
        /// </summary>
        /// <returns>The platform OS.</returns>
        public abstract PlatformOS GetPlatformOS();

        /// <summary>
        /// Executes a specific command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void Execute(Command command) => this.commandExecuteDict[command].Invoke();

        /// <summary>
        /// Adds command implementations to the dictionary.
        /// </summary>
        private void AddCommandExecutes()
        {
            this.commandExecuteDict[Command.Version] = this.ExecuteVersion;
            this.commandExecuteDict[Command.Help] = this.ExecuteHelp;
            this.commandExecuteDict[Command.Debug] = this.ExecuteDebug;
            this.commandExecuteDict[Command.Start] = this.ExecuteStart;
            this.commandExecuteDict[Command.Stop] = this.ExecuteStop;
            this.commandExecuteDict[Command.Restart] = this.ExecuteRestart;
            this.commandExecuteDict[Command.Install] = this.ExecuteInstall;
            this.commandExecuteDict[Command.Uninstall] = this.ExecuteUninstall;
            this.commandExecuteDict[Command.Status] = this.ExecuteStatus;
            this.commandExecuteDict[Command.Log] = this.ExecuteLog;
        }

        /// <summary>
        /// Executes the version command.
        /// </summary>
        private void ExecuteVersion()
        {
            Console.WriteLine($"Current OS {this.GetPlatformOS()}.");
            Console.WriteLine($"Proton Agent Control version {StaticProperty.ProtonAgentControlVersion}.");

            this.OnExecuteVersion();
        }

        /// <summary>
        /// Executes the help command.
        /// </summary>
        private void ExecuteHelp()
        {
            Console.WriteLine("We support command debug, start, stop, restart, install, uninstall, status, log");
            Console.WriteLine("Ex to call start like:");
            Console.WriteLine("proton-control start {name}");

            this.OnExecuteHelp();
        }

        /// <summary>
        /// Executes the debug command.
        /// </summary>
        private void ExecuteDebug()
        {
            var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);

            if (!instance.Enable)
            {
                this.logger.Info($"instance with name {this.name} not enable in config, please enable this.");
                Console.WriteLine($"instance with name {this.name} not enable in config, please enable this.");
                return;
            }

            var platformPath = LibraryUtils.GetPlatformPath();

            var controlAgentFileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "XmobiTea.ProtonNet.Control.Agent.exe" : "XmobiTea.ProtonNet.Control.Agent";
            var controlAgentPath = LibraryUtils.CombineFromRootPath("libs", "agents", platformPath, controlAgentFileName);

            var args = StartupAgentArgsBuilder.NewBuilder()
                .SetName(this.name)
                .SetBinPath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath))
                .SetProtonBinPath(LibraryUtils.CombineFromRootPath("libs", "agents", platformPath))
                .SetLogPath(LibraryUtils.CombineFromRootPath("logs", this.name))
                .SetAssemblyName(instance.AssemblyName)
                .SetStartupSettingsFilePath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.StartupSettingsFilePath))
                .SetLog4netFilePath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.Log4NetFilePath))
                .SetServerType(instance.ServerType)
                .SetAgentType("Plain")
                .SetIsBackgroundService(false)
                .Build();

            this.OnExecuteDebug(instance, controlAgentPath, args);
        }

        /// <summary>
        /// Executes the start command.
        /// </summary>
        private void ExecuteStart()
        {
            var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);

            if (!instance.Enable)
            {
                this.logger.Info($"instance with name {this.name} not enable in config, please enable this.");
                Console.WriteLine($"instance with name {this.name} not enable in config, please enable this.");
                return;
            }

            var logPath = LibraryUtils.CombineFromRootPath("logs", this.name);
            if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);

            this.OnExecuteStart(instance, success =>
            {
                this.ExecuteStatus();
            });
        }

        /// <summary>
        /// Executes the stop command.
        /// </summary>
        private void ExecuteStop()
        {
            this.OnExecuteStop(null);
        }

        /// <summary>
        /// Executes the restart command.
        /// </summary>
        private void ExecuteRestart()
        {
            this.OnExecuteStop(success =>
            {
                if (success)
                {
                    this.ExecuteStart();
                }
            });
        }

        /// <summary>
        /// Executes the install command.
        /// </summary>
        private void ExecuteInstall()
        {
            var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);

            if (!instance.Enable)
            {
                this.logger.Info($"instance with name {this.name} not enable in config, please enable this.");
                Console.WriteLine($"instance with name {this.name} not enable in config, please enable this.");
                return;
            }

            var platformPath = LibraryUtils.GetPlatformPath();

            var controlAgentFileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "XmobiTea.ProtonNet.Control.Agent.exe" : "XmobiTea.ProtonNet.Control.Agent";
            var controlAgentPath = LibraryUtils.CombineFromRootPath("libs", "agents", platformPath, controlAgentFileName);

            var args = StartupAgentArgsBuilder.NewBuilder()
                .SetName(this.name)
                .SetBinPath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath))
                .SetProtonBinPath(LibraryUtils.CombineFromRootPath("libs", "agents", platformPath))
                .SetLogPath(LibraryUtils.CombineFromRootPath("logs", this.name))
                .SetAssemblyName(instance.AssemblyName)
                .SetStartupSettingsFilePath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.StartupSettingsFilePath))
                .SetLog4netFilePath(LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.Log4NetFilePath))
                .SetServerType(instance.ServerType)
                .SetAgentType("Service")
                .SetIsBackgroundService(false)
                .Build();

            this.OnExecuteInstall(instance, controlAgentPath, args);
        }

        /// <summary>
        /// Executes the uninstall command.
        /// </summary>
        private void ExecuteUninstall()
        {
            this.OnExecuteUninstall();
        }

        /// <summary>
        /// Executes the status command.
        /// </summary>
        private void ExecuteStatus()
        {
            this.OnExecuteStatus();
        }

        /// <summary>
        /// Executes the log command.
        /// </summary>
        private void ExecuteLog()
        {
            var logFilePaths = this.GetLogFilePaths();

            if (logFilePaths.Length == 0)
            {
                this.logger.Info("No log file found in log path.");
                return;
            }

            this.OnExecuteLog(logFilePaths);
        }

        /// <summary>
        /// Retrieves log file paths.
        /// </summary>
        /// <returns>An array of log file paths.</returns>
        private string[] GetLogFilePaths()
        {
            var answer = new List<string>();

            var logPath = LibraryUtils.CombineFromRootPath("logs");
            if (Directory.Exists(logPath))
                answer.AddRange(Directory.GetFiles(logPath, "*.log"));

            if (!string.IsNullOrEmpty(this.name))
            {
                logPath = LibraryUtils.CombineFromRootPath("logs", this.name);
                if (Directory.Exists(logPath))
                    answer.AddRange(Directory.GetFiles(logPath, "*.log"));
            }

            return answer.ToArray();
        }

        /// <summary>
        /// Method to handle the version command, to be overridden by derived classes.
        /// </summary>
        protected virtual void OnExecuteVersion() { }

        /// <summary>
        /// Method to handle the help command, to be overridden by derived classes.
        /// </summary>
        protected virtual void OnExecuteHelp() { }

        /// <summary>
        /// Method to handle the debug command, to be overridden by derived classes.
        /// </summary>
        /// <param name="instance">The proton instance to debug.</param>
        /// <param name="controlAgentPath">The path to the control agent.</param>
        /// <param name="args">The arguments for the control agent.</param>
        protected virtual void OnExecuteDebug(ProtonNetInstance instance, string controlAgentPath, string args) { }

        /// <summary>
        /// Method to handle the start command, to be overridden by derived classes.
        /// </summary>
        /// <param name="instance">The proton instance to start.</param>
        /// <param name="onDone">Callback to execute after starting.</param>
        protected virtual void OnExecuteStart(ProtonNetInstance instance, Action<bool> onDone) { }

        /// <summary>
        /// Method to handle the stop command, to be overridden by derived classes.
        /// </summary>
        /// <param name="onDone">Callback to execute after stopping.</param>
        protected virtual void OnExecuteStop(Action<bool> onDone) { }

        /// <summary>
        /// Method to handle the install command, to be overridden by derived classes.
        /// </summary>
        /// <param name="instance">The proton instance to install.</param>
        /// <param name="controlAgentPath">The path to the control agent.</param>
        /// <param name="args">The arguments for the control agent.</param>
        protected virtual void OnExecuteInstall(ProtonNetInstance instance, string controlAgentPath, string args) { }

        /// <summary>
        /// Method to handle the uninstall command, to be overridden by derived classes.
        /// </summary>
        protected virtual void OnExecuteUninstall() { }

        /// <summary>
        /// Method to handle the status command, to be overridden by derived classes.
        /// </summary>
        protected virtual void OnExecuteStatus() { }

        /// <summary>
        /// Method to handle the log command, to be overridden by derived classes.
        /// </summary>
        /// <param name="logFilePaths">The paths to the log files.</param>
        protected virtual void OnExecuteLog(string[] logFilePaths) { }

    }

}

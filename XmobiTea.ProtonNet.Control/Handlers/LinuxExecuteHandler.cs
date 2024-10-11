using System.Diagnostics;
using XmobiTea.ProtonNet.Control.Helper;
using XmobiTea.ProtonNet.Control.Models;
using XmobiTea.ProtonNet.Control.Types;

namespace XmobiTea.ProtonNet.Control.Handlers
{
    /// <summary>
    /// Handler for executing commands on Linux platform.
    /// </summary>
    class LinuxExecuteHandler : AbstractExecuteHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinuxExecuteHandler"/> class.
        /// </summary>
        /// <param name="name">The name of the handler.</param>
        public LinuxExecuteHandler(string name) : base(name)
        {
        }

        /// <summary>
        /// Gets the platform OS for this handler.
        /// </summary>
        /// <returns>The platform OS as <see cref="PlatformOS.Linux"/>.</returns>
        public override PlatformOS GetPlatformOS() => PlatformOS.Linux;

        /// <summary>
        /// Executes the version command (Linux-specific implementation).
        /// </summary>
        protected override void OnExecuteVersion()
        {
            // Implementation for version command
        }

        /// <summary>
        /// Executes the help command (Linux-specific implementation).
        /// </summary>
        protected override void OnExecuteHelp()
        {
            // Implementation for help command
        }

        /// <summary>
        /// Executes the debug command (Linux-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to debug.</param>
        /// <param name="controlAgentPath">The path to the control agent executable.</param>
        /// <param name="args">Arguments for the debug command.</param>
        protected override void OnExecuteDebug(ProtonNetInstance instance, string controlAgentPath, string args)
        {
            try
            {
                var process = Process.Start(controlAgentPath, $"{args}");
                process.WaitForExit();
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Please run as root user for Proton Agent Control", ex);
                return;
            }
        }

        /// <summary>
        /// Executes the start command (Linux-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to start.</param>
        /// <param name="onDone">Callback to be invoked when the operation is done.</param>
        protected override void OnExecuteStart(ProtonNetInstance instance, System.Action<bool> onDone)
        {
            var servicePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service");

            if (!System.IO.Directory.Exists(servicePath))
            {
                this.logger.Error("Service is not installed, please run install before start.");
                return;
            }

            var startServiceFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "start-service.sh");

            try
            {
                Process.Start(startServiceFilePath);
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Exception when running Proton Agent Control", ex);
                onDone?.Invoke(false);
                return;
            }

            onDone?.Invoke(true);
        }

        /// <summary>
        /// Executes the stop command (Linux-specific implementation).
        /// </summary>
        /// <param name="onDone">Callback to be invoked when the operation is done.</param>
        protected override void OnExecuteStop(System.Action<bool> onDone)
        {
            var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);
            var servicePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service");

            if (!System.IO.Directory.Exists(servicePath))
            {
                this.logger.Error("Service is not started.");
                return;
            }

            var stopServiceFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "stop-service.sh");

            try
            {
                Process.Start(stopServiceFilePath);
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Exception when running Proton Agent Control", ex);
                onDone?.Invoke(false);
                return;
            }

            onDone?.Invoke(true);
        }

        /// <summary>
        /// Executes the install command (Linux-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to install.</param>
        /// <param name="controlAgentPath">The path to the control agent executable.</param>
        /// <param name="args">Arguments for the install command.</param>
        protected override void OnExecuteInstall(ProtonNetInstance instance, string controlAgentPath, string args)
        {
            var servicePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service");
            if (System.IO.Directory.Exists(servicePath))
            {
                this.logger.Error("Service has already been installed.");
                return;
            }

            var platformPath = LibraryUtils.GetPlatformPath();
            var pidFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "instance.pid");

            if (!System.IO.Directory.Exists(servicePath)) System.IO.Directory.CreateDirectory(servicePath);

            Process.Start("mkdir", $"-p \"{servicePath}\"");

            {
                var startServiceFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "start-service.sh");

                var stringBuilder = new System.Text.StringBuilder();
                stringBuilder.AppendLine("#!/bin/sh");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"PID_FILE_PATH={pidFilePath}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("if [ -f \"${PID_FILE_PATH}\" ]; then");
                stringBuilder.AppendLine("\tPID=$(cat \"${PID_FILE_PATH}\");");
                stringBuilder.AppendLine("fi");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("if [ -z \"${PID}\" ]; then");
                stringBuilder.AppendLine($"\t{controlAgentPath} \\");
                stringBuilder.AppendLine($"\t\t-name {this.name} \\");
                stringBuilder.AppendLine($"\t\t-binPath '{LibraryUtils.CombineFromRootPath("applications", instance.BinPath)}' \\");
                stringBuilder.AppendLine($"\t\t-protonBinPath '{LibraryUtils.CombineFromRootPath("libs", "agents", platformPath)}' \\");
                stringBuilder.AppendLine($"\t\t-logPath '{LibraryUtils.CombineFromRootPath("logs", this.name)}' \\");
                stringBuilder.AppendLine($"\t\t-assemblyName {instance.AssemblyName} \\");
                stringBuilder.AppendLine($"\t\t-startupSettingsFilePath '{LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.StartupSettingsFilePath)}' \\");
                stringBuilder.AppendLine($"\t\t-log4netFilePath {LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.Log4NetFilePath)} \\");
                stringBuilder.AppendLine($"\t\t-serverType {instance.ServerType} \\");
                stringBuilder.AppendLine($"\t\t-agentType Plain \\");
                stringBuilder.AppendLine($"\t\t-isBackgroundService True \\");
                stringBuilder.AppendLine($"\t\t> /dev/null 2>&1 &");
                stringBuilder.AppendLine($"\techo $! > \"{pidFilePath}\"");
                stringBuilder.AppendLine("\techo Service started on PID $(cat \"${PID_FILE_PATH}\")");
                stringBuilder.AppendLine("else");
                stringBuilder.AppendLine("\techo \"Another instance process is already running at ${PID}. Please stop the instance before starting.\"");
                stringBuilder.AppendLine("fi");

                System.IO.File.WriteAllText($"{startServiceFilePath}", stringBuilder.ToString());

                this.logger.Info($"Created file start-service.sh at path {servicePath}");

                try
                {
                    Process.Start("chmod", $"+x {startServiceFilePath}");
                }
                catch (System.Exception ex)
                {
                    this.logger.Error("Cannot chmod +x for start-service.sh file", ex);
                }
            }

            {
                var dockerServiceFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "docker-service.sh");

                var stringBuilder = new System.Text.StringBuilder();
                stringBuilder.AppendLine("#!/bin/sh");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"{controlAgentPath} \\");
                stringBuilder.AppendLine($"\t-name {this.name} \\");
                stringBuilder.AppendLine($"\t-binPath '{LibraryUtils.CombineFromRootPath("applications", instance.BinPath)}' \\");
                stringBuilder.AppendLine($"\t-protonBinPath '{LibraryUtils.CombineFromRootPath("libs", "agents", platformPath)}' \\");
                stringBuilder.AppendLine($"\t-logPath '{LibraryUtils.CombineFromRootPath("logs", this.name)}' \\");
                stringBuilder.AppendLine($"\t-assemblyName {instance.AssemblyName} \\");
                stringBuilder.AppendLine($"\t-startupSettingsFilePath '{LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.StartupSettingsFilePath)}' \\");
                stringBuilder.AppendLine($"\t-log4netFilePath {LibraryUtils.CombineFromRootPath("applications", instance.BinPath, instance.Log4NetFilePath)} \\");
                stringBuilder.AppendLine($"\t-serverType {instance.ServerType} \\");
                stringBuilder.AppendLine($"\t-agentType Plain \\");
                stringBuilder.AppendLine($"\t-isBackgroundService True \\");

                System.IO.File.WriteAllText($"{dockerServiceFilePath}", stringBuilder.ToString());

                this.logger.Info($"Created file docker-service.sh at path {servicePath}");

                try
                {
                    Process.Start("chmod", $"+x {dockerServiceFilePath}");
                }
                catch (System.Exception ex)
                {
                    this.logger.Error("Cannot chmod +x for docker-service.sh file", ex);
                }
            }

            {
                var stopServiceFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "stop-service.sh");

                var stringBuilder = new System.Text.StringBuilder();
                stringBuilder.AppendLine("#!/bin/sh");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"PID_FILE_PATH={pidFilePath}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("if [ ! -f \"${PID_FILE_PATH}\" ]; then");
                stringBuilder.AppendLine("\techo \"No proton instance process is running.\"");
                stringBuilder.AppendLine("\texit 0");
                stringBuilder.AppendLine("fi");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("PID=$(cat \"${PID_FILE_PATH}\");");

                stringBuilder.AppendLine("if [ -z \"${PID}\" ]; then");
                stringBuilder.AppendLine("\techo \"No proton instance process is running.\"");
                stringBuilder.AppendLine("else");
                stringBuilder.AppendLine("\tkill -9 ${PID}");
                stringBuilder.AppendLine("\techo kill -9 PID ${PID}");
                stringBuilder.AppendLine("\techo service stopped.");
                stringBuilder.AppendLine("fi");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("rm \"${PID_FILE_PATH}\"");

                System.IO.File.WriteAllText($"{stopServiceFilePath}", stringBuilder.ToString());

                this.logger.Info($"Created file stop-service.sh at path {servicePath}");

                try
                {
                    Process.Start("chmod", $"+x {stopServiceFilePath}");
                }
                catch (System.Exception ex)
                {
                    this.logger.Error("Cannot chmod +x for stop-service.sh file", ex);
                }
            }
        }

        /// <summary>
        /// Executes the uninstall command (Linux-specific implementation).
        /// </summary>
        protected override void OnExecuteUninstall()
        {
            this.OnExecuteStop(success =>
            {
                var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);
                var servicePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service");

                if (!System.IO.Directory.Exists(servicePath))
                {
                    this.logger.Error("Service is not installed.");
                    return;
                }

                Process.Start("rm", $"-f -r \"{servicePath}\"");

                this.logger.Info("Remove service success.");
            });
        }

        /// <summary>
        /// Executes the status command (Linux-specific implementation).
        /// </summary>
        protected override void OnExecuteStatus()
        {
            var instance = ProtonNetServerSettingsUtils.GetInstance(this.name);
            var processId = this.GetProcessId(instance);

            if (string.IsNullOrEmpty(processId))
            {
                this.logger.Info("Service is not running");
                return;
            }

            this.logger.Info($"Service running on PID {processId}.");
        }

        /// <summary>
        /// Executes the log command (Linux-specific implementation).
        /// </summary>
        /// <param name="logFilePaths">The paths to the log files to open.</param>
        protected override void OnExecuteLog(string[] logFilePaths)
        {
            try
            {
                var logFilePath = logFilePaths.Length > 1 ? logFilePaths[1] : logFilePaths[0];

                var process = Process.Start("tail", $"-n 300 -f \"{logFilePath}\"");
                process.WaitForExit();
            }
            catch (System.Exception ex)
            {
                this.logger.Error("Exception when running Proton Agent Control", ex);
                return;
            }
        }

        /// <summary>
        /// Gets the process ID of the running service.
        /// </summary>
        /// <param name="instance">The proton instance to get the process ID for.</param>
        /// <returns>The process ID if it exists; otherwise, null.</returns>
        private string GetProcessId(ProtonNetInstance instance)
        {
            var pidFilePath = LibraryUtils.CombineFromRootPath("applications", instance.BinPath, "__service", "instance.pid");

            if (!System.IO.File.Exists(pidFilePath)) return null;

            return System.IO.File.ReadAllText(pidFilePath);
        }

    }

}

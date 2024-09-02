using System;
using System.Diagnostics;
using System.Text;
using XmobiTea.ProtonNet.Control.Helper;
using XmobiTea.ProtonNet.Control.Models;
using XmobiTea.ProtonNet.Control.Types;

namespace XmobiTea.ProtonNet.Control.Handlers
{
    /// <summary>
    /// Handler for executing commands on Windows platform.
    /// </summary>
    class WindowsExecuteHandler : AbstractExecuteHandler
    {
        private static readonly string ProtonAgentControl = "ProtonAgentControl";
        private static readonly string LogExpertName = "win/LogExpert-1.6.7/LogExpert.exe";

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsExecuteHandler"/> class.
        /// </summary>
        /// <param name="name">The name of the handler.</param>
        public WindowsExecuteHandler(string name) : base(name)
        {
        }

        /// <summary>
        /// Gets the platform OS for this handler.
        /// </summary>
        /// <returns>The platform OS as <see cref="PlatformOS.Windows"/>.</returns>
        public override PlatformOS GetPlatformOS() => PlatformOS.Windows;

        /// <summary>
        /// Executes the version command (Windows-specific implementation).
        /// </summary>
        protected override void OnExecuteVersion()
        {
            // Implementation for version command
        }

        /// <summary>
        /// Executes the help command (Windows-specific implementation).
        /// </summary>
        protected override void OnExecuteHelp()
        {
            // Implementation for help command
        }

        /// <summary>
        /// Executes the debug command (Windows-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to debug.</param>
        /// <param name="controlAgentPath">The path to the control agent executable.</param>
        /// <param name="args">Arguments for the debug command.</param>
        protected override void OnExecuteDebug(ProtonInstance instance, string controlAgentPath, string args)
        {
            try
            {
                var process = Process.Start(controlAgentPath, $"{args}");
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Executes the start command (Windows-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to start.</param>
        /// <param name="onDone">Callback to be invoked when the operation is done.</param>
        protected override void OnExecuteStart(ProtonInstance instance, Action<bool> onDone)
        {
            var startInfo = new ProcessStartInfo("sc", $"start \"{ProtonAgentControl} - {this.name}\"")
            {
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(startInfo);
                var stdoutx = process.StandardOutput.ReadToEnd();
                var stderrx = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                onDone?.Invoke(false);
                return;
            }

            onDone?.Invoke(true);
        }

        /// <summary>
        /// Executes the stop command (Windows-specific implementation).
        /// </summary>
        /// <param name="onDone">Callback to be invoked when the operation is done.</param>
        protected override void OnExecuteStop(Action<bool> onDone)
        {
            var startInfo = new ProcessStartInfo("sc", $"stop \"{ProtonAgentControl} - {this.name}\"")
            {
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(startInfo);
                var stdoutx = process.StandardOutput.ReadToEnd();
                var stderrx = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                onDone?.Invoke(false);
                return;
            }

            onDone?.Invoke(true);
        }

        /// <summary>
        /// Executes the install command (Windows-specific implementation).
        /// </summary>
        /// <param name="instance">The proton instance to install.</param>
        /// <param name="controlAgentPath">The path to the control agent executable.</param>
        /// <param name="args">Arguments for the install command.</param>
        protected override void OnExecuteInstall(ProtonInstance instance, string controlAgentPath, string args)
        {
            var startInfo = new ProcessStartInfo("sc", $"create \"{ProtonAgentControl} - {this.name}\" start={instance.StartupType} error=normal binpath=\"\\\"{controlAgentPath}\\\" {args}\"")
            {
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(startInfo);
                var stdoutx = process.StandardOutput.ReadToEnd();
                var stderrx = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                process.WaitForExit();

                if (string.IsNullOrEmpty(stderrx)) this.OnExecuteDescription();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Updates the description of the service (Windows-specific implementation).
        /// </summary>
        private void OnExecuteDescription()
        {
            var platform = LibraryUtils.GetPlatformPath();

            var startInfo = new ProcessStartInfo("sc", $"description \"{ProtonAgentControl} - {this.name}\" \"Create from ProtonAgentControl support for platform {platform}.\"")
            {
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(startInfo);
                var stdoutx = process.StandardOutput.ReadToEnd();
                var stderrx = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Executes the uninstall command (Windows-specific implementation).
        /// </summary>
        protected override void OnExecuteUninstall()
        {
            this.OnExecuteStop(success =>
            {
                if (success)
                {
                    var startInfo = new ProcessStartInfo("sc", $"delete \"{ProtonAgentControl} - {this.name}\"")
                    {
                        UseShellExecute = false,
                        Verb = "runas",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    try
                    {
                        var process = Process.Start(startInfo);
                        var stdoutx = process.StandardOutput.ReadToEnd();
                        var stderrx = process.StandardError.ReadToEnd();

                        if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                        if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                        if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                        if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                        process.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                        Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                        return;
                    }
                }
            });
        }

        /// <summary>
        /// Executes the status command (Windows-specific implementation).
        /// </summary>
        protected override void OnExecuteStatus()
        {
            var startInfo = new ProcessStartInfo("sc", $"query \"{ProtonAgentControl} - {this.name}\"")
            {
                UseShellExecute = false,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                var process = Process.Start(startInfo);
                var stdoutx = process.StandardOutput.ReadToEnd();
                var stderrx = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stdoutx)) this.logger.Info($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) this.logger.Error($"Error: {stderrx}");

                if (!string.IsNullOrEmpty(stdoutx)) Console.WriteLine($"Output: {stdoutx}");
                if (!string.IsNullOrEmpty(stderrx)) Console.WriteLine($"Error: {stderrx}");

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Executes the log command (Windows-specific implementation).
        /// </summary>
        /// <param name="logFilePaths">The paths to the log files to open.</param>
        protected override void OnExecuteLog(string[] logFilePaths)
        {
            var logArgs = this.GetArgsForLog(logFilePaths);

            try
            {
                Process.Start($"{LibraryUtils.CombineFromRootPath("tools", LogExpertName)}", $"{logArgs}");
            }
            catch (Exception ex)
            {
                this.logger.Error("Please run as administrator for Proton Agent Control", ex);
                Console.WriteLine($"Please run as administrator for Proton Agent Control {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Builds arguments for the log command.
        /// </summary>
        /// <param name="logFilePaths">The paths to the log files.</param>
        /// <returns>The arguments for the log command.</returns>
        private string GetArgsForLog(string[] logFilePaths)
        {
            var argsBuilder = new StringBuilder();

            foreach (var logFilePath in logFilePaths)
            {
                argsBuilder.Append($"\"{logFilePath.Replace("\\", "/")}\" ");
            }

            return argsBuilder.ToString();
        }

    }

}

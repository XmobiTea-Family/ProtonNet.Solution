using XmobiTea.Logging;
using XmobiTea.Logging.Log4Net;
using XmobiTea.ProtonNet.Control.Handlers;
using XmobiTea.ProtonNet.Control.Helper;
using XmobiTea.ProtonNet.Control.Services;
using XmobiTea.ProtonNet.Control.Types;

namespace XmobiTea.ProtonNet.Control
{
    /// <summary>
    /// The ApplicationStartup class is responsible for initializing and running the ProtonNet Control application.
    /// </summary>
    class ApplicationStartup
    {
        /// <summary>
        /// Gets the command that will be executed by the application.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the name associated with this application startup instance.
        /// </summary>
        public string Name { get; }

        private IExecuteService executeService { get; }

        /// <summary>
        /// Initializes a new instance of the ApplicationStartup class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the ApplicationStartup instance.</param>
        private ApplicationStartup(Builder builder)
        {
            this.SetupLog();

            this.Command = builder.Command;
            this.Name = builder.Name;

            this.executeService = this.NewExecuteService();
        }

        /// <summary>
        /// Runs the application by executing the configured command.
        /// </summary>
        public void Run() => this.executeService.Execute(this.Command);

        /// <summary>
        /// Creates a new instance of IExecuteService with appropriate handlers based on the platform.
        /// </summary>
        /// <returns>A configured instance of IExecuteService.</returns>
        private IExecuteService NewExecuteService()
        {
            var answer = new ExecuteService();

            answer.AddExecuteHandler(new LinuxExecuteHandler(this.Name));
            answer.AddExecuteHandler(new OSXExecuteHandler(this.Name));
            answer.AddExecuteHandler(new WindowsExecuteHandler(this.Name));

            return answer;
        }

        /// <summary>
        /// Sets up logging for the application, configuring Log4Net if available.
        /// </summary>
        private void SetupLog()
        {
            log4net.GlobalContext.Properties["LogPath"] = LibraryUtils.CombineFromRootPath("logs");
            var configFileInfo = new System.IO.FileInfo(LibraryUtils.CombineFromRootPath("control", "proton-control-log4net.config"));

            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(configFileInfo);

                System.Console.WriteLine("Server init log success.");
            }
            else
            {
                LogManager.SetDefaultLoggerFactory(DefaultLogType.Console);

                System.Console.WriteLine("Log not exists.");
            }
        }

        /// <summary>
        /// Creates a new builder instance for configuring and creating an ApplicationStartup instance.
        /// </summary>
        /// <returns>A new instance of the Builder class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// The Builder class is used to configure and create an instance of ApplicationStartup.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the command to be executed by the application.
            /// </summary>
            public Command Command { get; set; }

            /// <summary>
            /// Gets or sets the name associated with the application startup instance.
            /// </summary>
            public string Name { get; set; }

            internal Builder() { }

            /// <summary>
            /// Sets the command to be executed.
            /// </summary>
            /// <param name="command">The command to execute.</param>
            /// <returns>The current instance of the Builder.</returns>
            public Builder SetCommand(Command command)
            {
                this.Command = command;
                return this;
            }

            /// <summary>
            /// Sets the name for the application startup instance.
            /// </summary>
            /// <param name="name">The name to associate with the instance.</param>
            /// <returns>The current instance of the Builder.</returns>
            public Builder SetName(string name)
            {
                this.Name = name;
                return this;
            }

            /// <summary>
            /// Configures the builder using command-line arguments.
            /// </summary>
            /// <param name="args">The command-line arguments.</param>
            /// <returns>The current instance of the Builder.</returns>
            public Builder SetArgs(string[] args)
            {
                this.Command = (Command)System.Enum.Parse(typeof(Command), args[0], true);
                this.Name = args.Length > 1 ? args[1] : string.Empty;

                return this;
            }

            /// <summary>
            /// Builds and returns a configured instance of ApplicationStartup.
            /// </summary>
            /// <returns>A new instance of ApplicationStartup configured with the builder.</returns>
            public ApplicationStartup Build() => new ApplicationStartup(this);

        }

    }

}

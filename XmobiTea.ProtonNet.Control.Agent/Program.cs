#if NETCOREAPP
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XmobiTea.ProtonNet.Control.Agent.Agents;
#endif

using XmobiTea.ProtonNet.Control.Agent.Types;

namespace XmobiTea.ProtonNet.Control.Agent
{
    /// <summary>
    /// Entry point for the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry method for the application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        static void Main(string[] args)
        {
            var applicationStartup = ApplicationStartup.NewBuilder()
                    .SetArgs(args)
                    .Build();

            // Check if the application is running interactively or in Plain AgentType
            if (System.Environment.UserInteractive || applicationStartup.AgentType == AgentType.Plain)
            {
                var startupAgent = applicationStartup.NewStartupAgent();
                startupAgent.Start();

                // Wait for user input if not running as a background service
                if (!applicationStartup.IsBackgroundService)
                {
                    System.Console.WriteLine("Press Enter to stop server...");
                    System.Console.ReadLine();
                    startupAgent.Stop();
                }
            }
            else
            {
#if NETCOREAPP
                // In .NET Core, configure and run the hosted service
                StaticProperty.StartupAgentInfo = applicationStartup.StartupAgentInfo;

                var builder = Host.CreateDefaultBuilder(args)
                    .UseSystemd()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<WorkerServiceStartupAgent>();
                    });

                var host = builder.Build();
                host.RunAsync();

#else
                // In .NET Framework, run the startup agent as a Windows service
                using (var startupAgent = (System.ServiceProcess.ServiceBase)applicationStartup.NewStartupAgent())
                    System.ServiceProcess.ServiceBase.Run(startupAgent);

#endif

            }

        }

    }

}

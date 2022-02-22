using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System.IO;

namespace Store.ECommerce.Core
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    internal class Program
    {
        const string AppName = "Store.ECommerce";

        public static HealthCheckResult ServiceBusState { get; private set; } = HealthCheckResult.Healthy();

        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = configuration.CreateSerilogLogger(AppName);
            ConfigureNServiceBusLogging();

            var host = BuildWebHost(args, configuration)
                .Build();
            host.Run();
        }

        static IHostBuilder BuildWebHost(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration(Program.AppName);
                    endpointConfiguration.PurgeOnStartup(true);
                    endpointConfiguration.ApplyCommonConfiguration(transport =>
                    {
                        var routing = transport.Routing();
                        routing.RouteToEndpoint(typeof(Messages.Commands.SubmitOrder).Assembly, "Store.Messages.Commands", "Store.Sales");
                    }, error =>
                    {
                        ServiceBusState = HealthCheckResult.Unhealthy("Critical error on endpoint", error);
                    });

                    return endpointConfiguration;
                })
                .UseSerilog();

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        static void ConfigureNServiceBusLogging()
        {
            Microsoft.Extensions.Logging.ILoggerFactory extensionsLoggerFactory = new SerilogLoggerFactory();
            NServiceBus.Logging.ILoggerFactory nservicebusLoggerFactory = new ExtensionsLoggerFactory(extensionsLoggerFactory);
            NServiceBus.Logging.LogManager.UseFactory(nservicebusLoggerFactory);
        }
    }
}

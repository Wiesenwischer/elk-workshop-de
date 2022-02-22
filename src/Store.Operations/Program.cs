using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Store.Operations;
using System.IO;
using Microsoft.Extensions.Diagnostics.HealthChecks;

class Program
{
    const string AppName = "Store.Operations";

    public static HealthCheckResult ServiceBusState { get; private set; } = HealthCheckResult.Healthy();

    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        Log.Logger = configuration.CreateSerilogLogger(AppName);
        ConfigureNServiceBusLogging();

        var host = CreateHostBuilder(args, configuration)
            .Build();
        host.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseConsoleLifetime()
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration(Program.AppName);
                endpointConfiguration.ApplyCommonConfiguration(transport =>
                {
                }, error =>
                {
                    ServiceBusState = HealthCheckResult.Unhealthy("Critical error on endpoint", error);
                });

                return endpointConfiguration;
            })
            .UseSerilog();
    }

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

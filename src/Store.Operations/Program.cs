using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Store.Shared;
using System.IO;
using NServiceBus.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

class Program
{
    const string AppName = "Store.Operations";

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
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseConsoleLifetime()
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration(Program.AppName);
                endpointConfiguration.ApplyCommonConfiguration(transport =>
                {
                });

                return endpointConfiguration;
            })
            .ConfigureServices(sp => sp.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq")))
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

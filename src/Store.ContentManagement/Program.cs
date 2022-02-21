using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Store.Shared;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
                endpointConfiguration.ApplyCommonConfiguration(transport =>
                {
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(Store.Messages.RequestResponse.ProvisionDownloadRequest), "Store.Operations");
                });

                return endpointConfiguration;
            })
            .ConfigureServices(sp => sp.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq")));
    }
}

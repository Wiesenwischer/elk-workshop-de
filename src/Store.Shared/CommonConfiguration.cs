using NServiceBus;
using NServiceBus.Encryption.MessageProperty;
using NServiceBus.MessageMutator;
using Store.Shared;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration,
        Action<TransportExtensions<RabbitMQTransport>> messageEndpointMappings = null, Action<Exception> onCriticalError = null)
    {
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=rabbitmq");
        transport.UseConventionalRoutingTopology();

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();

        endpointConfiguration.DefineCriticalErrorAction(ctx=>
        {
            onCriticalError?.Invoke(ctx.Exception);
            return CriticalErrorActions.RestartContainer(ctx);
        });

        messageEndpointMappings?.Invoke(transport);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var defaultKey = "2015-10";
        var ascii = Encoding.ASCII;
        var encryptionService = new RijndaelEncryptionService(
            encryptionKeyIdentifier: defaultKey,
            key: ascii.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);
        endpointConfiguration.RegisterMessageMutator(new DebugFlagMutator());
    }

    public static ILogger CreateSerilogLogger(this IConfiguration configuration, string applicationContext)
    {
        string logstashUrl = configuration["Serilog:LogstashUrl"];
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("ApplicationContext", applicationContext)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
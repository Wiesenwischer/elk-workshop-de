using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Store.Shared;

namespace Store.ECommerce.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "liveness" })
                .AddCheck("servicebus", () => Program.ServiceBusState, new[] { "messaging", "nservicebus" })
                .AddSignalRHub($"http://localhost{OrdersHub.Url}", "orders-hub", tags: new[] { "messaging", "signalr" })
                .AddRabbitMQ(name: "rabbit-mq", rabbitConnectionString: "amqp://guest:guest@rabbitmq:5672", tags: new[] { "messaging", "rabbit-mq" });

            services.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq"));

            services.AddControllersWithViews();
            services.AddSignalR(c => c.EnableDetailedErrors = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<OrdersHub>(OrdersHub.Url);
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}

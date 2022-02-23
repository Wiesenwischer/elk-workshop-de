using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Sales
{
    public class PingHealthCheck :  IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var ping = new Ping();
                var reply = await ping.SendPingAsync("store-ecommerce");

                if (reply.Status != IPStatus.Success)
                {
                    return HealthCheckResult.Unhealthy();
                }

                return reply.RoundtripTime > 100 ? HealthCheckResult.Degraded() : HealthCheckResult.Healthy();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pay.Recorrencia.Gestao.Crosscutting.Health
{
    public class ExampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var HealthCheckResultHealthy = true;

            if (HealthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("UP"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("DOWN"));
        }
    }
}

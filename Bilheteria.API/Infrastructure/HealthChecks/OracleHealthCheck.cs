using Bilheteria.API.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bilheteria.API.Infrastructure.HealthChecks
{
    public class OracleHealthCheck : IHealthCheck
    {
        private readonly ApplicationContext _context;

        public OracleHealthCheck(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var conseguiuConectar = await _context.Database.CanConnectAsync(cancellationToken);

                return conseguiuConectar
                    ? HealthCheckResult.Healthy("Conexao com o banco Oracle estabelecida com sucesso.")
                    : HealthCheckResult.Unhealthy("Nao foi possivel conectar ao banco Oracle.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha ao verificar a conexao com o banco Oracle.", ex);
            }
        }
    }
}

using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Application.UseCases;
using Bilheteria.API.Domain.Interfaces;
using Bilheteria.API.Infrastructure.Data;
using Bilheteria.API.Infrastructure.Data.Repositories;
using Bilheteria.API.Infrastructure.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseOracle(configuration.GetConnectionString("Oracle"));
            });

            services.AddTransient<IFilmeRepository, FilmeRepository>();
            services.AddTransient<ISessaoRepository, SessaoRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();

            services.AddTransient<IFilmeUseCase, FilmeUseCase>();
            services.AddTransient<ISessaoUseCase, SessaoUseCase>();
            services.AddTransient<IProdutoUseCase, ProdutoUseCase>();
            services.AddTransient<IPedidoUseCase, PedidoUseCase>();

            services
                .AddHealthChecks()
                .AddCheck<OracleHealthCheck>("oracle_db");

            return services;
        }
    }
}

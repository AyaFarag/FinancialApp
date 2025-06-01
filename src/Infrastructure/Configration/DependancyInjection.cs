using FinancialApp.Infrastructure.Repositories;

namespace FinancialApp.Infrastructure.Configration
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
        }
    }
}



using FiscalManager.Application.Interfaces;
using FiscalManager.Infrastructure.Configurations;
using FiscalManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FiscalManager.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FiscalDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly("FiscalManager.Infrastructure") // Define onde as migrations serão salvas
                ));

            //Registro de serviços
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }

    }
}

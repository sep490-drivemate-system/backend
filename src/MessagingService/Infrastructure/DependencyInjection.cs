using Microsoft.EntityFrameworkCore;
using MessagingService.Infrastructure.Persistence.Context;
using MessagingService.Infrastructure.UoW;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Register database context
            services.AddDbContext<MessagingDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("MESSAGINGSERVICECONNECTION");
                options.UseNpgsql(connectionString);
            });

            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}


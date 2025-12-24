using MessagingService.Domain.Interfaces;
using MessagingService.Infrastructure.Persistence.Context;
using MessagingService.Infrastructure.Repositories;
using MessagingService.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
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
                // Add timezone to connection string if not already present
                if (!connectionString.Contains("TimeZone", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString += ";TimeZone=Asia/Ho_Chi_Minh";
                }
                options.UseNpgsql(connectionString);
            });

            // Register repositories and UnitOfWork
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}


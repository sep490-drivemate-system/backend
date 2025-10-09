using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceService.Infrastructure.Persistence.Context;

namespace ResourceService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<ResourceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("RESOURCESERVICECONNECTION") 
                    ?? configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            });

            // Add repositories and services here
            
            return services;
        }
    }
}

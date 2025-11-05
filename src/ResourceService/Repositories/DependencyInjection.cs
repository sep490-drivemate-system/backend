using Microsoft.EntityFrameworkCore;
using ResourceService.Repositories.Implementation;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;

namespace ResourceService.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<ResourceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BLOGSERVICECONNECTION"))); 

            // Đăng ký Repository & UnitOfWork
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

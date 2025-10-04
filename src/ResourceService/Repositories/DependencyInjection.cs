using Microsoft.EntityFrameworkCore;
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

            // Đăng ký Repository (nếu có)

            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

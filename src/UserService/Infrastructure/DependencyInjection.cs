using Microsoft.EntityFrameworkCore;
using SharedLibrary.CloudinaryStorage;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<UserServiceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("USERSERVICECONNECTION");
                options.UseNpgsql(connectionString);
            });

            // Đăng ký Repository (nếu có)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();
            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

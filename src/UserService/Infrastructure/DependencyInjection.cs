using Microsoft.EntityFrameworkCore;
using System;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("USERSERVICECONNECTION")));

            // Đăng ký Repository (nếu có)
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

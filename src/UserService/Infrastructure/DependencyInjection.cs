using Microsoft.EntityFrameworkCore;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using SharedLibrary.SharedKernel.Password;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Persistence.Context;
using UserService.Infrastructure.UoW;

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
                var connectionString = configuration.GetConnectionString("user_service_db_connection");
                options.UseNpgsql(connectionString);
            });

            // Đăng ký Repository (nếu có)
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

using System;

namespace UserService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký Repository (nếu có)
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Đăng ký service khác (cache, email, storage…)
            // services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

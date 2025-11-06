using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Application.Interfaces;
using BookingService.Infrastructure.UoW;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Jwt;


namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Register database context
            services.AddDbContext<BookingDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("BOOKINGSERVICECONNECTION");
                options.UseNpgsql(connectionString);
            });
            
            // Đăng ký dịch vụ hệ thống
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();

            // Đăng ký dịch vụ bên thứ ba
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();

            // Register RabbitMQ service (with fallback to mock if connection fails)            
            //services.AddSingleton<IRabbitMQService, RabbitMQService>();
            //services.AddHostedService<RabbitMQHostedService>();
            return services;
        }
    }
}

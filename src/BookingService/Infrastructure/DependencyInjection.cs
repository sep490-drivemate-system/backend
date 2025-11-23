using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Application.Interfaces;
using BookingService.Infrastructure.UoW;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Http.Interfaces;
using SharedLibrary.SharedKernel.Http.Implementation;


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

            // Đăng ký sử dụng HttpClient gọi đến các Microservice bằng phương thức http.
            services.AddHttpClient("UserServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Userservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for user service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_BookingService");
            });

            services.AddHttpClient("PaymentServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Paymentservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for payment service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_BookingService");
            });

            // Đăng ký dịch vụ bên thứ ba
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();
            services.AddScoped<ISystemConfigurationHttpService, SystemConfigurationHttpService>();

            // Register RabbitMQ service (with fallback to mock if connection fails)            
            //services.AddSingleton<IRabbitMQService, RabbitMQService>();
            //services.AddHostedService<RabbitMQHostedService>();
            return services;
        }
    }
}

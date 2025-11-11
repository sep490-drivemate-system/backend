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
                var connectionString = configuration.GetConnectionString("USERSERVICECONNECTION");
                options.UseNpgsql(connectionString);
            });
            
            // Đăng ký service khác (cache, email, storage…)
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IEmailService, EmailService>();
            
            // Đăng ký sử dụng HttpClient gọi đến các Microservice bằng phương thức http.
            services.AddHttpClient("BookingServiceClient", client =>
            {
                var base_address = configuration.GetConnectionString("Bookingservice_connection");

                if (string.IsNullOrEmpty(base_address))
                {
                    throw new ApplicationException("Can not find base address for booking service");
                }

                client.BaseAddress = new Uri(base_address);
                client.DefaultRequestHeaders.Add("User-Agent", "DriveMate_UserService");
            });

            // Đăng ký các service bên thứ 3
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();

            return services;
        }
    }
}

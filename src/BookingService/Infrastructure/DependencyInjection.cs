using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Repositories;
using BookingService.Application.Interfaces;
using BookingService.Infrastructure.Messaging.Settings;
using BookingService.Infrastructure.Messaging.Services;
using BookingService.Infrastructure.UoW;

namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            services.AddDbContext<BookingDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("BOOKINGSERVICECONNECTION") 
                    ?? configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            });

            // Đăng ký Repository
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IDrivingSessionRepository, DrivingSessionRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IDrivingSkillRepository, DrivingSkillRepository>();
            services.AddScoped<IRoadTypeRepository, RoadTypeRepository>();

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            // Configure RabbitMQ settings
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            
            // Register RabbitMQ service (with fallback to mock if connection fails)
            var useRealRabbitMQ = configuration.GetValue<bool>("RabbitMQ:UseRealConnection", true);
            

                services.AddSingleton<IRabbitMQService, RabbitMQService>();

            
            // Add payment messaging service
            services.AddScoped<IPaymentMessagingService, PaymentMessagingService>();

            return services;
        }
    }
}

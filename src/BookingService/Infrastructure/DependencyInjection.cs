using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Repositories;
using BookingService.Application.Interfaces;
using BookingService.Infrastructure.UoW;
using BookingService.Infrastructure.Messaging.Config;
using BookingService.Infrastructure.Messaging.Implementation;
using BookingService.Infrastructure.Messaging.Interface;


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
            
            // Register Unit Of Work.
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            // Đăng ký Repository
            //services.AddScoped<IBookingRepository, BookingRepository>();
            //services.AddScoped<IPackageRepository, PackageRepository>();
            //services.AddScoped<IDrivingSessionRepository, DrivingSessionRepository>();
            //services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            //services.AddScoped<IDrivingSkillRepository, DrivingSkillRepository>();
            //services.AddScoped<IRoadTypeRepository, RoadTypeRepository>();
            //services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register RabbitMQ service (with fallback to mock if connection fails)            
            //services.AddSingleton<IRabbitMQService, RabbitMQService>();
            //services.AddHostedService<RabbitMQHostedService>();

            Console.WriteLine("Completed");

            return services;
        }
    }
}

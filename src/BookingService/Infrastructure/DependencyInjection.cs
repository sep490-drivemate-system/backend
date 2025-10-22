using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Repositories;
using BookingService.Application.Interfaces;

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

            return services;
        }
    }
}

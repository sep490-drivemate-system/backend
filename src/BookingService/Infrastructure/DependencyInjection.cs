using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Persistence.Context;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Repositories;

namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext
            switch (configuration.GetValue<string>("DatabaseSystem")) {
                case "SQLServer":
                    services.AddDbContext<BookingDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("BOOKINGSERVICECONNECTION")));
                    break;
                case "PosgreSQL":
                    services.AddDbContext<BookingDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("BOOKINGSERVICECONNECTION")));
                    break;
                default:
                    throw new InvalidOperationException("The given database system is not supported by the application");

            }

            // Đăng ký Repository
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IDrivingSessionRepository, DrivingSessionRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            return services;
        }
    }
}

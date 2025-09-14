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
            services.AddDbContext<BookingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BOOKINGSERVICECONNECTION")));

            // Đăng ký Repository
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IDrivingSessionRepository, DrivingSessionRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            return services;
        }
    }
}

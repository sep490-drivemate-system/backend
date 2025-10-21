using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BookingService.Application.Interfaces;
using BookingService.Application.Services;

namespace BookingService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Add application services here
            // services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IDrivingSkillService, DrivingSkillService>();
            services.AddScoped<IRoadTypeService, RoadTypeService>();

            return services;
        }
    }
}

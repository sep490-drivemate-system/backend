using BookingService.Application.Interfaces;
using BookingService.Application.UseCase;
using BookingService.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IBookingUseCase, BookingUseCase>();
            services.AddScoped<IDrivingSessionUseCase, DrivingSessionUseCase>();
            services.AddScoped<IDrivingSkillService, DrivingSkillService>();
            services.AddScoped<IRoadTypeService, RoadTypeService>();
            services.AddScoped<IPackageService, PackageService>();

            return services;
        }
    }
}

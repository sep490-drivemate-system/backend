using BookingService.Application.Interfaces;
using BookingService.Domain.Interfaces;
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
             services.AddScoped<IBookingUseCase, BookingService>();

            return services;
        }
    }
}

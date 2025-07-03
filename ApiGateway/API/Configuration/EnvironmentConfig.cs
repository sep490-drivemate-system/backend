using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using SharedLibrary.Auth.Jwt;
using System.Text;

namespace API.Configuration
{
    public static class EnvironmentConfig
    {
        public static IServiceCollection AddEnvironmentConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            return services;
        }
    }
}

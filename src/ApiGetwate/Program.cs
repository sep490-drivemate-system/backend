
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using SharedLibrary.Jwt;
using System.Text;

namespace ApiGetwate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");

            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddControllers();
            
            // Load appropriate Ocelot configuration based on environment
            var ocelotFile = builder.Environment.IsProduction() ? "ocelot.Production.json" : 
                           builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.json";
            
            // Replace environment variables in Ocelot config for production
            if (builder.Environment.IsProduction())
            {
                var userServiceHost = Environment.GetEnvironmentVariable("USERSERVICE_HOST") ?? "localhost";
                var bookingServiceHost = Environment.GetEnvironmentVariable("BOOKINGSERVICE_HOST") ?? "localhost";
                var resourceServiceHost = Environment.GetEnvironmentVariable("RESOURCESERVICE_HOST") ?? "localhost";
                var apiGatewayHost = Environment.GetEnvironmentVariable("APIGATEWAY_HOST") ?? "localhost";
                
                var ocelotPath = Path.Combine(Directory.GetCurrentDirectory(), ocelotFile);
                var ocelotContent = File.ReadAllText(ocelotPath);
                ocelotContent = ocelotContent.Replace("${USERSERVICE_HOST}", userServiceHost);
                ocelotContent = ocelotContent.Replace("${BOOKINGSERVICE_HOST}", bookingServiceHost);
                ocelotContent = ocelotContent.Replace("${RESOURCESERVICE_HOST}", resourceServiceHost);
                ocelotContent = ocelotContent.Replace("${APIGATEWAY_HOST}", apiGatewayHost);
                
                // Write temporary config file
                var tempOcelotPath = Path.Combine(Directory.GetCurrentDirectory(), "ocelot.temp.json");
                File.WriteAllText(tempOcelotPath, ocelotContent);
                
                builder.Configuration.AddJsonFile("ocelot.temp.json", optional: false, reloadOnChange: true);
            }
            else
            {
                builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);
            }
            builder.Services.AddOcelot(config);
            builder.Services.AddSwaggerForOcelot(config, c =>
            {
                c.GenerateDocsForGatewayItSelf = false;
            });

            var jwtSettings = config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],

                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            // Remove HTTPS redirection for HTTP-only setup
            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}

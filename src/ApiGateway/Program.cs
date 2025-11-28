
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Requester;
using Ocelot.Values;
using SharedLibrary.Jwt;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ApiGetwate.Middleware;

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
            
            var ocelotFile = builder.Environment.IsProduction() ? "ocelot.Production.json" : 
                           builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.json";
            
            if (builder.Environment.IsProduction())
            {
                var userServiceHost = Environment.GetEnvironmentVariable("USERSERVICE_HOST") ?? "localhost";
                var bookingServiceHost = Environment.GetEnvironmentVariable("BOOKINGSERVICE_HOST") ?? "localhost";
                var resourceServiceHost = Environment.GetEnvironmentVariable("RESOURCESERVICE_HOST") ?? "localhost";
                var messagingServiceHost = Environment.GetEnvironmentVariable("MESSAGINGSERVICE_HOST") ?? "localhost";
                var apiGatewayHost = Environment.GetEnvironmentVariable("APIGATEWAY_HOST") ?? "localhost";
                
                var ocelotPath = Path.Combine(Directory.GetCurrentDirectory(), ocelotFile);
                var ocelotContent = File.ReadAllText(ocelotPath);
                ocelotContent = ocelotContent.Replace("${USERSERVICE_HOST}", userServiceHost);
                ocelotContent = ocelotContent.Replace("${BOOKINGSERVICE_HOST}", bookingServiceHost);
                ocelotContent = ocelotContent.Replace("${RESOURCESERVICE_HOST}", resourceServiceHost);
                ocelotContent = ocelotContent.Replace("${MESSAGINGSERVICE_HOST}", messagingServiceHost);
                ocelotContent = ocelotContent.Replace("${APIGATEWAY_HOST}", apiGatewayHost);
                
                var tempOcelotPath = Path.Combine(Directory.GetCurrentDirectory(), "ocelot.temp.json");
                File.WriteAllText(tempOcelotPath, ocelotContent);
                
                builder.Configuration.AddJsonFile("ocelot.temp.json", optional: false, reloadOnChange: true);
            }
            else
            {
                builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);
            }

            if (builder.Environment.IsProduction())
            {
                builder.Services.AddTransient<IgnoreSslDelegatingHandler>();
                builder.Services.AddOcelot(config)
                    .AddDelegatingHandler<IgnoreSslDelegatingHandler>();
            }
            else
            {
                builder.Services.AddOcelot(config);
            }
            
            builder.Services.AddSwaggerForOcelot(config, c =>
            {
                c.GenerateDocsForGatewayItSelf = false;
            });

            var jwtSettings = config.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var key = Encoding.UTF8.GetBytes(keyString);

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

            app.UseGlobalExceptionHandler();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            
            app.UseAuthenticationExceptionHandler();
            
            app.UseAuthorization();
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            }).UseOcelot().Wait();

            app.Run();
        }
    }

    public class IgnoreSslDelegatingHandler : DelegatingHandler
    {
        public IgnoreSslDelegatingHandler()
        {
            InnerHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        }
    }
}

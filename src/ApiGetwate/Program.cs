
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
            var ocelotFile = builder.Environment.IsProduction() ? "ocelot.Production.json" : "ocelot.json";
            builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);
            builder.Services.Configure<JwtSettings>(config.GetSection("Jwt"));
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}

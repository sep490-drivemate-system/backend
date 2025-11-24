using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.CloudinaryStorage;
using System.Text;
using UserService.Application;
using UserService.Application.Jobs;
using UserService.Infrastructure;
using UserService.Infrastructure.Persistence.Context;

namespace UserService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();
            builder.Services.AddInfrastructure(config);
            builder.Services.AddApplication(config);
            
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = config["Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "User Service API",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT Bearer token"
                });


                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHangfireDashboard();
            }
            
            // Enable Swagger in production for Railway
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Remove HTTPS redirection for HTTP-only setup
            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.MapControllers();

            RunningJobs.AddRunningJobs();

            app.Run();
        }
    }
}

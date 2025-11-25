using MessagingService.Application;
using MessagingService.Infrastructure;
using MessagingService.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Jwt;
using System.Text;

namespace MessagingService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR();

            builder.Services.AddInfrastructure(config);

            builder.Services.AddApplication(config);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrWhiteSpace(accessToken) &&
                                (path.StartsWithSegments("/chatHub") || path.StartsWithSegments("/notificationHub")))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddScoped<IJwtService, JwtService>();

            // Swagger configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Messaging Service API",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
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

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable Swagger in production
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}

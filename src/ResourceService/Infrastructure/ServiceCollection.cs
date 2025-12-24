using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.Mapping;
using ResourceService.Application.Interfaces;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Application.Services;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;
using ResourceService.Infrastructure.Repositories;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using Resend;
using SharedLibrary.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResourceService.Infrastructure
{
    public static class ServiceCollection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Security with Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role,
                    };
                });

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            // Configure swagger doc
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Resource Service API",
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

            // Configure CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Configure DbContext
            services.AddDbContext<ResourceDbContext>(options =>
            {
                var connectionString = config.GetConnectionString("BLOGSERVICECONNECTION");
                // Add timezone to connection string if not already present
                if (!connectionString.Contains("TimeZone", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString += ";TimeZone=Asia/Ho_Chi_Minh";
                }
                options.UseNpgsql(connectionString);
            });

            // Configure mapper
            services.AddScoped(provider => new MapperConfiguration(cfg => { 
                cfg.AddProfile<MainMappingProfile>();
            }).CreateMapper());

            // HttpClient
            services.AddHttpClient();

            // Shared library services
            services.AddScoped<IJwtService, JwtService>();
            
            // Register Resend for email service
            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = Environment.GetEnvironmentVariable("RESEND_APITOKEN") ?? Environment.GetEnvironmentVariable("RESEND_API_KEY") ?? string.Empty;
            });
            
            services.AddScoped<IEmailService, EmailService>();

            // Third party
            services.AddScoped<ICloudinaryServiceProvider, CloudinaryServiceProvider>();

            // Services/ Use Cases / Features / ...
            services.AddScoped<IResourcesService, ResourcesService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IQAService, QAService>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IQARepository, QARepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Application Service Provider
            services.AddScoped<IApplicationServiceProvider, ApplicationServiceProvider>();

            return services;
        }
    }
}

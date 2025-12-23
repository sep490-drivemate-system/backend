using ResourceService.Infrastructure;
namespace ResourceService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Configuration.AddEnvironmentVariables();

            // Bind to PORT provided by hosting platform (e.g., Railway)
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

            // Add services to the container.
            builder.Services.ConfigureInfrastructure(config);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Enable Swagger in all environments so it works on Railway (Production).
            app.UseSwagger();
            app.UseSwaggerUI();
            
            // In container environments behind a reverse proxy/HTTPS terminator (like Railway),
            // it's usually not necessary to force HTTPS redirection here.
            // app.UseHttpsRedirection();
            // CORS should run before auth to allow preflight without auth headers
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();
        }
    }
}

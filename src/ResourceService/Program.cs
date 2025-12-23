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

            // Enable Swagger in all environments (including Production on Railway)
            app.UseSwagger();
            app.UseSwaggerUI();

            // Do not force HTTPS redirection inside container; Railway terminates HTTPS at the edge
            // and forwards HTTP into the container. Forcing HTTPS here can cause redirect issues.
            // app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            // Simple health endpoint for testing from Railway
            app.MapGet("/health", () => Results.Ok("OK from ResourceService"));

            app.MapControllers();
            
            app.Run();
        }
    }
}

using System.Net;
using System.Text.Json;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.SharedKernel.Messages;

namespace ApiGetwate.Middleware
{
    public class AuthenticationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationExceptionMiddleware> _logger;

        public AuthenticationExceptionMiddleware(
            RequestDelegate next, 
            ILogger<AuthenticationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Xử lý response 401 và 403 sau khi Ocelot routing hoàn thành
            if (context.Response.HasStarted)
            {
                return; // Response đã được gửi, không thể modify
            }

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorizedAsync(context);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await HandleForbiddenAsync(context);
            }
        }

        private async Task HandleUnauthorizedAsync(HttpContext context)
        {
            var path = context.Request.Path;
            _logger.LogWarning(LogMessages.UnauthorizedAccessAttempt, 
                path, context.Connection.RemoteIpAddress);

            // Clear response - chỉ clear nếu chưa start
            context.Response.Clear();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = ErrorMessages.Unauthorized,
                ErrorCode = ServiceError.Unauthorized,
                Value = null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleForbiddenAsync(HttpContext context)
        {
            var path = context.Request.Path;
            var user = context.User?.Identity?.Name ?? "Unknown";
            
            _logger.LogWarning(LogMessages.ForbiddenAccessAttempt, user, path);

            // Clear response - chỉ clear nếu chưa start
            context.Response.Clear();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = ErrorMessages.Forbidden,
                ErrorCode = ServiceError.Forbidden,
                Value = null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    public static class AuthenticationExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationExceptionMiddleware>();
        }
    }
}


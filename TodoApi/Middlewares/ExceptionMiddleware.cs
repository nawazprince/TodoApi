using System.Text.Json;
using TodoApi.DTOs;

namespace TodoApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiResponse<string>() { StatusCode = context.Response.StatusCode, ErrorMessage = ex.Message, Data = ex.StackTrace?.ToString() }
                    : new ApiResponse<string>() { StatusCode = StatusCodes.Status500InternalServerError, ErrorMessage = "Internal Server Error." };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}

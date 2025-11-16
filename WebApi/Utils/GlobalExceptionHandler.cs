using System.Net;
using Application.Utils;

namespace WebApi.Utils;

public class GlobalExceptionMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;


    public GlobalExceptionMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
        _logger = loggerFactory.CreateLogger<GlobalExceptionMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something wrong happened, details: {ex.Message}");
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorMessage = _env.IsProduction() 
                ? "Internal server error" 
                : ex.ToString();
            
            var result = ApiResponse.Error(errorMessage, HttpStatusCode.InternalServerError);
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApi.Filters;


[AttributeUsage(AttributeTargets.Method)]
internal sealed class IdempotentAttribute(int cacheTimeInMinutes = IdempotentAttribute.DefaultCacheTimeInMinutes)
    : Attribute, IAsyncActionFilter
{
    private const int DefaultCacheTimeInMinutes = 60;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(cacheTimeInMinutes);

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Idempotence-Key", out var idempotenceKeyValue) ||
            !Guid.TryParse(idempotenceKeyValue, out Guid idempotenceKey))
        {
            context.Result = new BadRequestObjectResult("Invalid or missing Idempotence-Key header");
            return;
        }

        var cache = context.HttpContext
            .RequestServices.GetRequiredService<IDistributedCache>();

        var cacheKey = $"Idempotent_{idempotenceKey}";
        var cachedResult = await cache.GetStringAsync(cacheKey);
        if (cachedResult is not null)
        {
            var response = JsonSerializer.Deserialize<IdempotentResponse>(cachedResult)!;

            var result = new ObjectResult(response.Value) { StatusCode = response.StatusCode };
            context.Result = result;

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult { StatusCode: >= 200 and < 300 } objectResult)
        {
            int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            IdempotentResponse response = new(statusCode, objectResult.Value);

            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheDuration }
            );
        }
    }
}

internal sealed class IdempotentResponse(int statusCode, object? value)
{
    public int StatusCode { get; } = statusCode;
    public object? Value { get; } = value;
}
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Utils;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetAsync<T>(
        this IDistributedCache cache, 
        string key, 
        CancellationToken token = default)
    {
        var bytes = await cache.GetAsync(key, token);
        
        if (bytes == null)
            return default;
        
        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static async Task SetAsync<T>(
        this IDistributedCache cache, 
        string key, 
        T value, 
        DistributedCacheEntryOptions? options = null,
        CancellationToken token = default)
    {
        var json = JsonSerializer.Serialize(value);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        await cache.SetAsync(key, bytes, options ?? new DistributedCacheEntryOptions(), token);
    }
}
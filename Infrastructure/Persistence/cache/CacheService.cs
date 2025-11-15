using System.Text.Json;
using Application.Contracts;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Persistence.cache;

public class CacheService(IDatabase redis, ILogger<CacheService> logger) : ICacheService
{
    private static readonly string FailureLogTemplate = "[CacheServiceFailure]- {}"; 
    public async Task<long> PushAsync<T>(T item, string key)
    {
        var serialized = JsonSerializer.Serialize(item);
        return await redis.ListRightPushAsync(key, serialized);
    }
    
    public async Task<T?> PopAsync<T>(string key)
    {
        try
        {
            var value = await redis.ListLeftPopAsync(key);
            if (!value.HasValue)
                return default;
        
            return JsonSerializer.Deserialize<T>(value.ToString());
        }
        catch (Exception e)
        {
            logger.LogError(FailureLogTemplate, e.Message);
            return default;
        }
    }
    
    public async Task<long> BulkPushAsync<T>(string key, List<T> items)
    {
        var serializedItems = items
            .Select(item => (RedisValue)JsonSerializer.Serialize(item))
            .ToArray();
        
        return await redis.ListRightPushAsync(key, serializedItems);
    }
    
    public async Task<bool> RemoveAsync(string key)
    {
        return await redis.KeyDeleteAsync(key);
    }
    
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var serialized = JsonSerializer.Serialize(value);
        return await redis.StringSetAsync(key, serialized, expiration);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await redis.StringGetAsync(key);
        
        if (!value.HasValue)
            return default;
        
        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task<TimeSpan?> PingAsync()
    {
        try
        {
            return await redis.PingAsync();
        }
        catch (Exception e)
        {
            logger.LogError(FailureLogTemplate, e.Message);
            return null;
        }
    }

    public async Task<bool> KeyExistsAsync(string key)
     => await redis.KeyExistsAsync(key);
}
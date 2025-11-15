namespace Application.Contracts;

public interface ICacheService
{
    Task<long> PushAsync<T>(T item, string key);
    Task<T?> PopAsync<T>(string key);

    Task<long> BulkPushAsync<T>(string key, List<T> items);
    Task<bool> RemoveAsync(string key);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task<T?> GetAsync<T>(string key);
    Task<TimeSpan?> PingAsync();
    Task<bool> KeyExistsAsync(string key);
}
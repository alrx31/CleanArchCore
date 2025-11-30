using System.Text.Json;
using Core.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Core.Services;

public class DistributedCacheService<TKey, TValue>(IDistributedCache database) : ICacheService<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private readonly DistributedCacheEntryOptions cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
    };

    public async Task<TValue?> GetAsync(TKey key)
    {
        var data = await database.GetStringAsync(JsonSerializer.Serialize(key));
        return data == null ? default : JsonSerializer.Deserialize<TValue>(data);
    }

    public async Task SetAsync(TKey key, TValue value)
    {
        await database.SetStringAsync(JsonSerializer.Serialize(key), JsonSerializer.Serialize(value), cacheOptions);
    }

    public async Task RemoveAsync(TKey key)
    {
        await database.RemoveAsync(JsonSerializer.Serialize(key));
    }
}

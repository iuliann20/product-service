using Microsoft.Extensions.Caching.Distributed;
using ProductService.Domain.Caching;
using ProductService.Domain.Contracts.Responses;
using System.Text.Json;

namespace ProductService.Infrastructure.Caching
{
    public sealed class CatalogCache : ICatalogCache
    {
        private readonly IDistributedCache _cache;
        public CatalogCache(IDistributedCache cache) => _cache = cache;

        private static string PKey(Guid id) => $"product:{id}";
        private static DistributedCacheEntryOptions Ttl(TimeSpan ttl) => new() { AbsoluteExpirationRelativeToNow = ttl };

        public async Task<ProductDetailDto?> GetProductAsync(Guid productId, CancellationToken ct = default)
        {
            var bytes = await _cache.GetAsync(PKey(productId), ct);
            if (bytes is null) return null;
            return JsonSerializer.Deserialize<ProductDetailDto>(bytes);
        }

        public Task SetProductAsync(ProductDetailDto dto, TimeSpan ttl, CancellationToken ct = default)
            => _cache.SetAsync(PKey(dto.Id), JsonSerializer.SerializeToUtf8Bytes(dto), Ttl(ttl), ct);

        public Task InvalidateProductAsync(Guid productId, CancellationToken ct = default)
            => _cache.RemoveAsync(PKey(productId), ct);

        public async Task<T?> GetSearchAsync<T>(string key, CancellationToken ct = default)
        {
            var bytes = await _cache.GetAsync($"search:{key}", ct);
            if (bytes is null) return default;
            return JsonSerializer.Deserialize<T>(bytes);
        }

        public Task SetSearchAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default)
            => _cache.SetAsync($"search:{key}", JsonSerializer.SerializeToUtf8Bytes(value), Ttl(ttl), ct);
    }
}

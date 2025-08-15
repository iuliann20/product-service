using ProductService.Domain.Contracts.Responses;

namespace ProductService.Domain.Caching
{
    public interface ICatalogCache
    {
        Task<ProductDetailDto?> GetProductAsync(Guid productId, CancellationToken ct = default);
        Task SetProductAsync(ProductDetailDto dto, TimeSpan ttl, CancellationToken ct = default);
        Task InvalidateProductAsync(Guid productId, CancellationToken ct = default);

        Task<T?> GetSearchAsync<T>(string key, CancellationToken ct = default);
        Task SetSearchAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default);
    }
}

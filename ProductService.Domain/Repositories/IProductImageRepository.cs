using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories
{
    public interface IProductImageRepository
    {
        Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(ProductImage image, CancellationToken ct = default);
        void Remove(ProductImage image);
        Task ClearMainAsync(Guid productId, CancellationToken ct = default);
        Task<Dictionary<Guid, string>> GetMainUrlsMapAsync(IReadOnlyList<Guid> productIds, CancellationToken ct = default);
    }
}

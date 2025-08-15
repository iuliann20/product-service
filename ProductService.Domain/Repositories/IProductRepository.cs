using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Product?> GetWithImagesAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Product product, CancellationToken ct = default);

        Task<(IReadOnlyList<Product> items, int total)> SearchAsync(
            Guid? categoryId, string? text, decimal? priceMin, decimal? priceMax, bool onlyActive,
            string? sortBy, bool sortDesc, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}

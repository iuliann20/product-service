using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories
{
    public interface IProductReviewRepository
    {
        Task AddAsync(ProductReview review, CancellationToken ct = default);
        Task<ProductReview?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<ProductReview>> ListApprovedForProductAsync(Guid productId, CancellationToken ct = default);
    }
}

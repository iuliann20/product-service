using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.Repositories;

namespace ProductService.Persistence.Repositories
{
    public sealed class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ProductServiceDbContext _db;
        public ProductReviewRepository(ProductServiceDbContext db) => _db = db;

        public async Task AddAsync(ProductReview review, CancellationToken ct = default) =>
            await _db.Set<ProductReview>().AddAsync(review, ct);

        public Task<ProductReview?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Set<ProductReview>().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<IReadOnlyList<ProductReview>> ListApprovedForProductAsync(Guid productId, CancellationToken ct = default) =>
            _db.Set<ProductReview>()
               .Where(r => r.ProductId == productId && r.Status == ReviewStatus.Approved)
               .OrderByDescending(r => r.CreatedAtUtc)
               .ToListAsync(ct)
               .ContinueWith(t => (IReadOnlyList<ProductReview>)t.Result, ct);
    }
}

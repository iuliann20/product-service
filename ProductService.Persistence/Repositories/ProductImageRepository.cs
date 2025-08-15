using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Persistence.Repositories
{
    public sealed class ProductImageRepository : IProductImageRepository
    {
        private readonly ProductServiceDbContext _db;
        public ProductImageRepository(ProductServiceDbContext db) => _db = db;

        public Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Set<ProductImage>().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(ProductImage image, CancellationToken ct = default) =>
            await _db.Set<ProductImage>().AddAsync(image, ct);

        public void Remove(ProductImage image) => _db.Set<ProductImage>().Remove(image);

        public Task ClearMainAsync(Guid productId, CancellationToken ct = default) =>
            _db.Set<ProductImage>().Where(i => i.ProductId == productId && i.IsMain)
               .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsMain, false), ct);

        public async Task<Dictionary<Guid, string>> GetMainUrlsMapAsync(IReadOnlyList<Guid> productIds, CancellationToken ct = default)
        {
            var q = await _db.Set<ProductImage>()
                .Where(i => productIds.Contains(i.ProductId) && i.IsMain)
                .Select(i => new { i.ProductId, i.ImageUrl })
                .ToListAsync(ct);

            return q.GroupBy(x => x.ProductId)
                    .ToDictionary(g => g.Key, g => g.First().ImageUrl);
        }
    }
}

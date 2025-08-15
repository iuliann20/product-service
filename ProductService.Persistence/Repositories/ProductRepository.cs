using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ProductServiceDbContext _db;
        public ProductRepository(ProductServiceDbContext db) => _db = db;

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Set<Product>().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<Product?> GetWithImagesAsync(Guid id, CancellationToken ct = default) =>
            _db.Set<Product>().Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task AddAsync(Product product, CancellationToken ct = default) =>
            await _db.Set<Product>().AddAsync(product, ct);

        public async Task<(IReadOnlyList<Product> items, int total)> SearchAsync(
            Guid? categoryId, string? text, decimal? priceMin, decimal? priceMax, bool onlyActive,
            string? sortBy, bool sortDesc, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var q = _db.Set<Product>().AsQueryable();

            if (onlyActive) q = q.Where(p => p.IsActive);
            if (categoryId.HasValue) q = q.Where(p => p.CategoryId == categoryId.Value);
            if (!string.IsNullOrWhiteSpace(text))
                q = q.Where(p => p.Name.Contains(text) || (p.Description ?? "").Contains(text));
            if (priceMin.HasValue) q = q.Where(p => p.Price >= priceMin.Value);
            if (priceMax.HasValue) q = q.Where(p => p.Price <= priceMax.Value);

            (string sb, bool desc) = Normalize(sortBy, sortDesc);
            q = (sb, desc) switch
            {
                ("price", false) => q.OrderBy(p => p.Price),
                ("price", true) => q.OrderByDescending(p => p.Price),
                ("name", false) => q.OrderBy(p => p.Name),
                ("name", true) => q.OrderByDescending(p => p.Name),
                ("createdat", true) => q.OrderByDescending(p => p.CreatedAt),
                _ => q.OrderBy(p => p.CreatedAt)
            };

            pageSize = Math.Clamp(pageSize, 1, 100);
            pageNumber = Math.Max(pageNumber, 1);

            var total = await q.CountAsync(ct);
            var items = await q.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return (items, total);

            static (string, bool) Normalize(string? s, bool desc) => ((s ?? "").Trim().ToLowerInvariant(), desc);
        }
    }
}

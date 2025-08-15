using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Persistence.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ProductServiceDbContext _db;
        public CategoryRepository(ProductServiceDbContext db) => _db = db;

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _db.Set<Category>().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<IReadOnlyList<Category>> ListActiveAsync(CancellationToken ct = default) =>
            _db.Set<Category>().Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync(ct)
                          .ContinueWith(t => (IReadOnlyList<Category>)t.Result, ct);

        public async Task AddAsync(Category category, CancellationToken ct = default) =>
            await _db.Set<Category>().AddAsync(category, ct);

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var c = await GetByIdAsync(id, ct);
            if (c is null) return;
            _db.Set<Category>().Remove(c);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductService.Domain.Entities;
using ProductService.Domain.Shared;
using ProductService.Persistence;
using System.Linq.Expressions;

namespace ProductService.Persistence.Repositories
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected readonly ProductServiceDbContext dbContext;
        protected readonly DbSet<T> table;

        public BaseRepository(ProductServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<T>();
        }

        public Result<IQueryable<T>> Get(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return Result.Create(table.Where(predicate));

            return Result.Create((IQueryable<T>)table);
        }

        public async Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                return Result.Create(await table.Where(predicate).ToListAsync(cancellationToken));

            return Result.Create(await table.ToListAsync(cancellationToken));
        }

        public async Task<int> CountAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await table
                    .Where(predicate)
                    .CountAsync();

            return await table.CountAsync();
        }

        public async Task<T> CreateAsync(T entity, bool commit = true, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                throw new ArgumentNullException("Entity cannot be null.");

            EntityEntry<T> entry = await table.AddAsync(entity);
            entity = entry.Entity;
            if (commit)
                await CommitAsync(cancellationToken);

            return entity;
        }

        public void Update(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException("Entity cannot be null.");
            table.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity cannot be null.");

            table.Remove(entity);
        }

        public async Task<List<TResult>> QueryAsync<TResult>(
             Expression<Func<T, bool>> predicate = null,
             Expression<Func<T, TResult>> selector = null,
             CancellationToken cancellationToken = default,
             params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (selector == null)
            {
                return (List<TResult>)(object)await query.ToListAsync(cancellationToken);
            }

            return await query.Select(selector).ToListAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

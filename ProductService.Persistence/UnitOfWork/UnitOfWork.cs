using ProductService.Domain.Repositories;
using ProductService.Persistence.Repositories;

namespace ProductService.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductServiceDbContext _dbContext;

        private readonly bool _useTransactions;
        private ITestRepository _testRepository;

        public UnitOfWork(ProductServiceDbContext dbContext, bool useTransactions = true)
        {
            _dbContext = dbContext;
            _useTransactions = useTransactions;
        }

        public ITestRepository TestRepository => _testRepository ??= new TestRepository(_dbContext);


        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_useTransactions)
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    int affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return affectedRows;
                }
                catch
                {
                    _dbContext.Database.RollbackTransaction();
                    throw;
                }
            }
            else
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

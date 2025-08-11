namespace ProductService.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITestRepository TestRepository { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}

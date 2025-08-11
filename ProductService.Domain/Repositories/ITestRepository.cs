namespace ProductService.Domain.Repositories
{
    public interface ITestRepository
    {
        Task<List<int>> GetTestMethod(int id);
        Task TestMehod();
        Task<int> GetId(int id);
    }
}

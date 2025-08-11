using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Persistence;

namespace ProductService.Persistence.Repositories
{
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {
        public TestRepository(ProductServiceDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<int>> GetTestMethod(int id)
        {
            return new List<int> { 1, 2, 3 };
        }

        public async Task TestMehod()
        {
            await CreateAsync(new Test());
        }

        public async Task<int> GetId(int id)
        {
            var test = await GetAsync(x => x.Id == id);

            return test.Value.FirstOrDefault().Id;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Entities;
using ProductService.IntegrationTests.Configure;
using ProductService.Persistence;

namespace ProductService.IntegrationTests.MockData
{
    internal static class TestMockData
    {
        public static async Task<int> InsertDataWithDbContext(CustomWebApplicationFactory factory)
        {
            using (var scope = factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductServiceDbContext>();

                var entityToAdd = new Test();
                //Pe Set<T>() se pune entitatea care reprezinta tabela din DB. 
                await dbContext.Set<Test>().AddAsync(entityToAdd);

                var id = await dbContext.SaveChangesAsync();
                return id;
            }
        }
    }
}

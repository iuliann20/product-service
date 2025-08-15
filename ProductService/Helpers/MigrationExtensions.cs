using Microsoft.EntityFrameworkCore;
using ProductService.Persistence;

namespace ProductService.Helpers
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ProductServiceDbContext dbContext = scope.ServiceProvider.GetService<ProductServiceDbContext>();
            dbContext.Database.Migrate();
        }
    }
}

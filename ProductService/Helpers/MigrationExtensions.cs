namespace ProductService.Helpers
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            //using ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            //dbContext.Database.Migrate();
        }
    }
}

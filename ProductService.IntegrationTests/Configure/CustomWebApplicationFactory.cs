using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared.Settings;
using ProductService.Persistence;
using ProductService.Persistence.UnitOfWork;

namespace ProductService.IntegrationTests.Configure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public IConfiguration Configuration { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = config.Build();
            });
            builder.ConfigureServices(services =>
            {
                var tokenManagement = Configuration.GetSection("TokenManagement").Get<TokenManagement>();
                services.AddSingleton(tokenManagement);

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProductServiceDbContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ProductServiceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                services.AddScoped<IUnitOfWork>(provider =>
                {
                    var dbContext = provider.GetRequiredService<ProductServiceDbContext>();
                    return new UnitOfWork(dbContext);
                });

                services.AddScoped<DbContext, ProductServiceDbContext>();
            });
        }
    }
}

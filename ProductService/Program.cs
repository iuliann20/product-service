using Asp.Versioning.ApiExplorer;
using ProductService.Configuration;
using ProductService.DependencyInjection;
using ProductService.Helpers;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Startig up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddAplication();

    builder.Services.ConfigureAuthentication(builder.Configuration);

    builder.Services.RegisterServices(builder.Configuration);

    builder.Services.AddPersistence(builder.Configuration);

    builder.Services.AddJwtAuthentication(builder.Configuration);

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<DeprecatedRouteFilter>();
    });

    builder.Services.ConfigureVersioning();

    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();
    app.Services.ConfigureLogging(builder.Configuration);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                        $"API {description.GroupName.ToUpperInvariant()}");
            }
        });
    }
    app.ApplyMigrations();

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseAuthentication();  
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

public partial class Program { }
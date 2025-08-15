using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Application.Behaviors;
using ProductService.Domain.Repositories;
using ProductService.Domain.Services;
using ProductService.Domain.Shared;
using ProductService.Domain.Shared.Settings;
using ProductService.Infrastructure.Messaging.MassTransitConfig;
using ProductService.Infrastructure.Services;
using ProductService.Persistence;
using ProductService.Persistence.Repositories;
using ProductService.Persistence.UnitOfWork;

namespace ProductService.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Database");

            Ensure.NotNullOrWhiteSpace(connectionString);
            var encryption = new AESEncryptionService();

            services.AddDbContext<ProductServiceDbContext>((sp, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(encryption.Decrypt(connectionString));
            });

            services.AddMemoryCache();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();

            return services;
        }

        public static IServiceProvider ConfigureLogging(this IServiceProvider services, IConfiguration configuration)
        {

            //using (var scope = services.CreateScope())
            //{
            //    var encryptionService = scope.ServiceProvider.GetService<IAESEncryptionService>();
            //    var metricsSettings = scope.ServiceProvider.GetService<IOptions<MetricsSettings>>();
            //    var username = encryptionService.Decrypt(metricsSettings.Value.Username);
            //    var password = encryptionService.Decrypt(metricsSettings.Value.Password);

            //    var options = new ElasticsearchSinkOptions(new Uri(configuration["SerilogElasticSearchSettings:ElasticSearchUrl"]))
            //    {
            //        IndexFormat = configuration["SerilogElasticSearchSettings:IndexFormat"],
            //        ModifyConnectionSettings = (c) => c.ServerCertificateValidationCallback(
            //                (o, certificate, arg3, arg4) => { return true; })
            //                                            .BasicAuthentication(username, password),
            //        ConnectionTimeout = TimeSpan.FromSeconds(metricsSettings.Value.RequestTimeoutSeconds)
            //    };

            //    Log.Logger = new LoggerConfiguration()
            //        .ReadFrom.Configuration(configuration)
            //        .WriteTo.Elasticsearch(options)
            //        .Enrich.WithMachineName()
            //        .Enrich.FromLogContext()
            //        .CreateLogger();
            //}

            return services;
        }

        public static IServiceProvider DatabaseInitializer(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductServiceDbContext>();

                dbContext.Database.Migrate();
            }
            return serviceProvider;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAESEncryptionService, AESEncryptionService>();
            services.Configure<TokenManagement>(configuration.GetSection("TokenManagement"));
            services.AddHttpContextAccessor();
            services.AddMessaging(configuration);
            return services;
        }

        public static IServiceCollection ConfigureVersioning(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"ProductService Module API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "API folosit pentru aplicatia de ProductService.",
                        Contact = new OpenApiContact
                        {
                            Name = "Silitra Iulian",
                            Email = "silitra.iulian1997@gmail.com"
                        }
                    });
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                }
                });

                options.UseInlineDefinitionsForEnums();
            });
            services.AddApiVersioning(option =>
            {
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = ApiVersion.Default;
                option.ReportApiVersions = true;
            }).AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VV";
                option.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}

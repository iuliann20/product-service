using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Infrastructure.Messaging.MassTransitConfig
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
        {
            var section = cfg.GetSection("RabbitMq");
            var host = section["Host"] ?? "localhost";
            var vhost = section["VirtualHost"] ?? "/";
            var user = section["Username"] ?? "guest";
            var pass = section["Password"] ?? "guest";

            services.AddMassTransit(x =>
            {
                x.AddConsumers(AssemblyReference.Assembly);

                x.AddDelayedMessageScheduler();
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(host, vhost, h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });
                    
                    bus.UseDelayedMessageScheduler();
                    bus.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    
                    bus.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}

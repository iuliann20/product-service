using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductService.Persistence;
using ProductService.Persistence.Outbox;

namespace ProductService.Infrastructure.Outbox
{
    public sealed class OutboxDispatcher : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<OutboxDispatcher> _logger;

        public OutboxDispatcher(IServiceProvider sp, ILogger<OutboxDispatcher> logger)
        {
            _sp = sp; _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int batchSize = 50;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ProductServiceDbContext>();
                    var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var messages = await db.Set<OutboxMessage>()
                        .Where(m => m.ProcessedOnUtc == null)
                        .OrderBy(m => m.OccurredOnUtc)
                        .Take(batchSize)
                        .ToListAsync(stoppingToken);

                    foreach (var msg in messages)
                    {
                        try
                        {
                            var type = Type.GetType(msg.Type, throwOnError: true)!;
                            var obj = JsonConvert.DeserializeObject(msg.Content, type, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            })!;

                            await bus.Publish(obj, type, stoppingToken);
                            msg.ProcessedOnUtc = DateTime.UtcNow;
                            msg.Error = null;
                        }
                        catch (Exception ex)
                        {
                            msg.Error = ex.Message;
                            _logger.LogError(ex, "Failed to dispatch outbox message {Id}", msg.Id);
                        }
                    }

                    if (messages.Count > 0)
                        await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "OutboxDispatcher loop error");
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}

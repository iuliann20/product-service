using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.Domain.Primitives;
using ProductService.Persistence.Outbox;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Persistence.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
      : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var domainEvents = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregateRoot =>
                {
                    IReadOnlyCollection<IDomainEvent> domainEvents = aggregateRoot.GetDomainEvents().ToList();
                    aggregateRoot.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            if (!domainEvents.Any())
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var saveResult = await base.SavingChangesAsync(eventData, result, cancellationToken);

            var outboxMessages = new List<OutboxMessage>();

            foreach (var domainEvent in domainEvents)
            {
                foreach (var integrationEvent in DomainEventMapper.Map(domainEvent))
                {
                    var type = integrationEvent.GetType();
                    outboxMessages.Add(new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        OccurredOnUtc = DateTime.UtcNow,
                        Type = type.AssemblyQualifiedName!,
                        Content = JsonConvert.SerializeObject(integrationEvent, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                    });
                }
            }

            if (outboxMessages.Any())
            {
                dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return saveResult;
        }
    }
}

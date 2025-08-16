using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.Domain.Primitives;
using ProductService.Domain.Repositories;
using ProductService.Persistence.Outbox;

namespace ProductService.Persistence.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ProductServiceDbContext _dbContext;

        public UnitOfWork(ProductServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDomainEventsToOutboxMessages();
            UpdateAuditableEntities();

            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDomainEventsToOutboxMessages()
        {
            var domainEvents = _dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .SelectMany(ar =>
                {
                    var events = ar.GetDomainEvents().ToList();
                    ar.ClearDomainEvents();
                    return events;
                })
                .ToList();

            if (!domainEvents.Any()) return;

            var outboxMessages = new List<OutboxMessage>();

            foreach (var de in domainEvents)
            {
                foreach (var ie in Outbox.DomainEventMapper.Map(de))
                {
                    var type = ie.GetType(); // integration event CLR type
                    outboxMessages.Add(new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        OccurredOnUtc = DateTime.UtcNow,
                        Type = type.AssemblyQualifiedName!, // for deserialization
                        Content = JsonConvert.SerializeObject(ie, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                    });
                }
            }

            _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        }

        private void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry<IAuditableEntity>> entries =
                _dbContext
                    .ChangeTracker
                    .Entries<IAuditableEntity>();

            foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(a => a.CreatedOnUtc)
                        .CurrentValue = DateTime.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(a => a.ModifiedOnUtc)
                        .CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}

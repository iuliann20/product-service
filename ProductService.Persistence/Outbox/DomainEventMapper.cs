using ProductService.Domain.Events;
using ProductService.Domain.Events.IntegrationEvents;
using ProductService.Domain.Primitives;

namespace ProductService.Persistence.Outbox
{
    internal static class DomainEventMapper
    {
        public static IEnumerable<object> Map(IDomainEvent @event)
        {
            var now = DateTime.UtcNow;

            return @event switch
            {
                ProductCreatedDomainEvent e => new object[] { new ProductCreatedIntegrationEvent(e.ProductId, e.Name, e.CategoryId, e.Price, e.StockQuantity, e.IsActive, now) },
                ProductUpdatedDomainEvent e => new object[] { new ProductUpdatedIntegrationEvent(e.ProductId, e.Name, e.CategoryId, e.Price, e.IsActive, now) },
                StockAdjustedDomainEvent e => new object[] { new StockAdjustedIntegrationEvent(e.ProductId, e.NewStockQuantity, e.Delta, now) },
                ProductDeactivatedDomainEvent e => new object[] { new ProductDeactivatedIntegrationEvent(e.ProductId, now) },
                _ => Array.Empty<object>()
            };
        }
    }
}

using MassTransit;
using ProductService.Domain.Primitives;

namespace ProductService.Domain.Events.Product.Events
{
    public sealed record ProductCreatedDomainEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, int StockQuantity, bool IsActive) : IDomainEvent;
    public sealed record ProductUpdatedDomainEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, bool IsActive) : IDomainEvent;
    public sealed record StockAdjustedDomainEvent(Guid ProductId, int NewStockQuantity, int Delta) : IDomainEvent;
    public sealed record ProductDeactivatedDomainEvent(Guid ProductId) : IDomainEvent;
}

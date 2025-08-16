using MassTransit;

namespace ProductService.Domain.Events.Product.IntegrationEvents
{
    [EntityName("product-created")]
    public sealed record ProductCreatedIntegrationEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, int StockQuantity, bool IsActive, DateTime OccurredAtUtc);
    [EntityName("product-updated")]
    public sealed record ProductUpdatedIntegrationEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, bool IsActive, DateTime OccurredAtUtc);
    [EntityName("stock-adjusted")]
    public sealed record StockAdjustedIntegrationEvent(Guid ProductId, int NewStockQuantity, int Delta, DateTime OccurredAtUtc);
    [EntityName("product-deactivated")]
    public sealed record ProductDeactivatedIntegrationEvent(Guid ProductId, DateTime OccurredAtUtc);
}

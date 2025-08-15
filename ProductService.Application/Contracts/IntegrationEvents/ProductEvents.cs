namespace ProductService.Application.Contracts.IntegrationEvents
{
    public sealed record ProductCreatedIntegrationEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, int StockQuantity, bool IsActive, DateTime OccurredAtUtc);
    public sealed record ProductUpdatedIntegrationEvent(Guid ProductId, string Name, Guid CategoryId, decimal Price, bool IsActive, DateTime OccurredAtUtc);
    public sealed record StockAdjustedIntegrationEvent(Guid ProductId, int NewStockQuantity, int Delta, DateTime OccurredAtUtc);
    public sealed record ProductDeactivatedIntegrationEvent(Guid ProductId, DateTime OccurredAtUtc);
}

using ProductService.Domain.Contracts.Responses;

namespace ProductService.Domain.Events.Order
{
    public sealed record OrderPlacedIntegrationEvent(Guid OrderId, Guid UserId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
    public sealed record OrderCancelledIntegrationEvent(Guid OrderId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
    public sealed record OrderPaidIntegrationEvent(Guid OrderId, Guid UserId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
}

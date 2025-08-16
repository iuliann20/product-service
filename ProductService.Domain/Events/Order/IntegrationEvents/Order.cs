using MassTransit;
using ProductService.Domain.Contracts.Responses;

namespace ProductService.Domain.Events.Order.IntegrationEvents
{
    [EntityName("order-placed")]
    public sealed record OrderPlacedIntegrationEvent(Guid OrderId, Guid UserId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
    [EntityName("order-cancelled")]
    public sealed record OrderCancelledIntegrationEvent(Guid OrderId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
    [EntityName("order-paid")]
    public sealed record OrderPaidIntegrationEvent(Guid OrderId, Guid UserId, IReadOnlyList<OrderItemDto> Items, DateTime OccurredAtUtc);
}

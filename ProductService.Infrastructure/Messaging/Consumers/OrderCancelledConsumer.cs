using MassTransit;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Events.Order;
using ProductService.Domain.Repositories;

namespace ProductService.Infrastructure.Messaging.Consumers
{
    public sealed class OrderCancelledConsumer : IConsumer<OrderCancelledIntegrationEvent>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<OrderCancelledConsumer> _logger;

        public OrderCancelledConsumer(IProductRepository products, IUnitOfWork uow, ILogger<OrderCancelledConsumer> logger)
        {
            _products = products; _uow = uow; _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var product = await _products.GetByIdAsync(item.ProductId, context.CancellationToken)
                        ?? throw new InvalidOperationException($"Product {item.ProductId} not found.");
                product.AdjustStock(+item.Quantity);
            }

            await _uow.SaveChangesAsync(context.CancellationToken);
            _logger.LogInformation("OrderCancelled processed: {OrderId}", context.Message.OrderId);
        }
    }
}

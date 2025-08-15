using MassTransit;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Events.Order;
using ProductService.Domain.Repositories;

namespace ProductService.Infrastructure.Messaging.Consumers
{
    public sealed class OrderPlacedConsumer : IConsumer<OrderPlacedIntegrationEvent>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(IProductRepository products, IUnitOfWork uow, ILogger<OrderPlacedConsumer> logger)
        {
            _products = products; _uow = uow; _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var product = await _products.GetByIdAsync(item.ProductId, context.CancellationToken)
                        ?? throw new InvalidOperationException($"Product {item.ProductId} not found.");

                product.AdjustStock(-item.Quantity); 
            }

            await _uow.SaveChangesAsync(context.CancellationToken);
            _logger.LogInformation("OrderPlaced processed: {OrderId}", context.Message.OrderId);
        }
    }
}

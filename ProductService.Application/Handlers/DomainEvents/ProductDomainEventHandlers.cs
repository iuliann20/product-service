using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Events.Product.Events;

namespace ProductService.Application.Handlers.DomainEvents
{
    public sealed class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly ILogger<ProductCreatedDomainEventHandler> _logger;

        public ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Product created: {ProductId} - {Name}", notification.ProductId, notification.Name);
            return Task.CompletedTask;
        }
    }

    public sealed class ProductUpdatedDomainEventHandler : INotificationHandler<ProductUpdatedDomainEvent>
    {
        private readonly ILogger<ProductUpdatedDomainEventHandler> _logger;

        public ProductUpdatedDomainEventHandler(ILogger<ProductUpdatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Product updated: {ProductId} - {Name}", notification.ProductId, notification.Name);
            return Task.CompletedTask;
        }
    }

    public sealed class StockAdjustedDomainEventHandler : INotificationHandler<StockAdjustedDomainEvent>
    {
        private readonly ILogger<StockAdjustedDomainEventHandler> _logger;

        public StockAdjustedDomainEventHandler(ILogger<StockAdjustedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(StockAdjustedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stock adjusted for product: {ProductId}, New quantity: {NewStock}, Delta: {Delta}", 
                notification.ProductId, notification.NewStockQuantity, notification.Delta);
            return Task.CompletedTask;
        }
    }

    public sealed class ProductDeactivatedDomainEventHandler : INotificationHandler<ProductDeactivatedDomainEvent>
    {
        private readonly ILogger<ProductDeactivatedDomainEventHandler> _logger;

        public ProductDeactivatedDomainEventHandler(ILogger<ProductDeactivatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductDeactivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Product deactivated: {ProductId}", notification.ProductId);
            return Task.CompletedTask;
        }
    }
}

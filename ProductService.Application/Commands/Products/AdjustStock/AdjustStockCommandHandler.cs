using MassTransit;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Application.Contracts.IntegrationEvents;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.AdjustStock
{
    public sealed class AdjustStockCommandHandler : ICommandHandler<AdjustStockCommand, int>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly IPublishEndpoint _publisher;

        public AdjustStockCommandHandler(IProductRepository products, IUnitOfWork uow, IPublishEndpoint publisher)
        {
            _products = products;
            _uow = uow;
            _publisher = publisher;
        }

        public async Task<Result<int>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetByIdAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");
            product.AdjustStock(request.Delta);
            await _uow.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new StockAdjustedIntegrationEvent(product.Id, product.StockQuantity, request.Delta, DateTime.UtcNow), cancellationToken);

            return Result.Create(product.StockQuantity);
        }
    }
}

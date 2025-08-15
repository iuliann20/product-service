using MassTransit;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.AdjustStock
{
    public sealed class AdjustStockCommandHandler : ICommandHandler<AdjustStockCommand, int>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly ICatalogCache _cache;

        public AdjustStockCommandHandler(IProductRepository products, IUnitOfWork uow, ICatalogCache cache)
        {
            _products = products; _uow = uow; _cache = cache;
        }

        public async Task<Result<int>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetByIdAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");

            product.AdjustStock(request.Delta);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cache.InvalidateProductAsync(product.Id, cancellationToken);

            return Result.Create(product.StockQuantity);
        }
    }
}

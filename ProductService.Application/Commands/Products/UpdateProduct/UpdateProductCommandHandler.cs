using MassTransit;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly ICatalogCache _cache;

        public UpdateProductCommandHandler(IProductRepository products, IUnitOfWork uow, ICatalogCache cache)
        {
            _products = products;
            _uow = uow; _cache = cache;
        }


        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetByIdAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");

            product.Update(request.Name, request.Description, request.Price, request.CategoryId, request.IsActive);

            await _uow.SaveChangesAsync(cancellationToken);
            await _cache.InvalidateProductAsync(product.Id, cancellationToken);

            return Result.Success();
        }
    }
}

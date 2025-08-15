using MassTransit;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Application.Contracts.IntegrationEvents;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.UpdateProduct
{
    public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly IPublishEndpoint _publisher;

        public UpdateProductCommandHandler(IProductRepository products, IUnitOfWork uow, IPublishEndpoint publisher)
        {
            _products = products; _uow = uow; _publisher = publisher;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetByIdAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");
            product.Name = request.Name;
            product.Description = request.Description;
            product.SetPrice(request.Price);
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;

            await _uow.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ProductUpdatedIntegrationEvent(
                product.Id, product.Name, product.CategoryId, product.Price, product.IsActive, DateTime.UtcNow), cancellationToken);

            return Result.Success();
        }
    }
}

using MassTransit;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Application.Contracts.IntegrationEvents;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;
        private readonly IPublishEndpoint _publisher;

        public CreateProductCommandHandler(IProductRepository products, IUnitOfWork uow, IPublishEndpoint publisher)
        {
            _products = products; _uow = uow; _publisher = publisher;
        }
        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                StockQuantity = request.StockQuantity,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _products.AddAsync(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ProductCreatedIntegrationEvent(
                product.Id, product.Name, product.CategoryId, product.Price, product.StockQuantity, product.IsActive, DateTime.UtcNow), cancellationToken);

            return product.Id;
        }
    }
}

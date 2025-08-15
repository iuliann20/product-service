using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _uow;

        public CreateProductCommandHandler(IProductRepository products, IUnitOfWork uow)
        {
            _products = products;
            _uow = uow;
        }
        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name, request.Description, request.Price, request.CategoryId, request.StockQuantity, request.IsActive);

            await _products.AddAsync(product, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

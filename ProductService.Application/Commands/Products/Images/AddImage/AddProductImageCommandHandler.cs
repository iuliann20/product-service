using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.Images.AddImage
{
    public sealed class AddProductImageCommandHandler : ICommandHandler<AddProductImageCommand, Guid>
    {
        private readonly IProductRepository _products;
        private readonly IProductImageRepository _images;
        private readonly IUnitOfWork _uow;
        private readonly ICatalogCache _cache;

        public AddProductImageCommandHandler(IProductRepository products, IProductImageRepository images, IUnitOfWork uow, ICatalogCache cache)
        {
            _products = products;
            _images = images;
            _uow = uow;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new InvalidOperationException("Product not found.");

            if (request.IsMain)
                await _images.ClearMainAsync(request.ProductId, cancellationToken);

            var img = new ProductImage { ProductId = request.ProductId, ImageUrl = request.ImageUrl, IsMain = request.IsMain };

            await _images.AddAsync(img, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cache.InvalidateProductAsync(product.Id, cancellationToken);

            return Result.Create(img.Id);
        }
    }
}

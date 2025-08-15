using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Queries.Products.GetProductById
{
    public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDetailDto>
    {
        private readonly IProductRepository _products;
        public GetProductByIdQueryHandler(IProductRepository products) => _products = products;

        public async Task<Result<ProductDetailDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _products.GetWithImagesAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");

            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                StockQuantity = product.StockQuantity,
                CreatedAt = product.CreatedAt,
                Images = product.Images.OrderByDescending(i => i.IsMain).Select(i => i.ImageUrl).ToList()
            };
        }
    }
}

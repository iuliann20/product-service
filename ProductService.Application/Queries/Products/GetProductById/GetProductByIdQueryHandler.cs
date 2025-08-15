using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Queries.Products.GetProductById
{
    public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDetailDto>
    {
        private readonly IProductRepository _products;
        private readonly ICatalogCache _cache;

        public GetProductByIdQueryHandler(IProductRepository products, ICatalogCache cache)
        {
            _products = products; _cache = cache;
        }

        public async Task<Result<ProductDetailDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetProductAsync(request.Id, cancellationToken);
            if (cached is not null) return cached;

            var product = await _products.GetWithImagesAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Product not found.");
            var dto = new ProductDetailDto
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

            await _cache.SetProductAsync(dto, TimeSpan.FromMinutes(5), cancellationToken);
            return Result.Create(dto);
        }
    }
}

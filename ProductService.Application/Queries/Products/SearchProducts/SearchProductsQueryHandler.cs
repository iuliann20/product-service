using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Queries.Products.SearchProducts
{
    public sealed class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, PagedResult<ProductListItemDto>>
    {
        private readonly IProductRepository _products;
        private readonly IProductImageRepository _images;

        public SearchProductsQueryHandler(IProductRepository products, IProductImageRepository images)
        {
            _products = products; _images = images;
        }

        public async Task<Result<PagedResult<ProductListItemDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            var (items, total) = await _products.SearchAsync(
                r.CategoryId, r.Text, r.PriceMin, r.PriceMax, r.OnlyActive,
                r.SortBy, string.Equals(r.SortOrder, "desc", StringComparison.OrdinalIgnoreCase),
                r.PageNumber, r.PageSize, cancellationToken);

            // Extract main images in one shot
            var ids = items.Select(p => p.Id).ToList();
            var mainMap = await _images.GetMainUrlsMapAsync(ids, cancellationToken);

            var mapped = items.Select(p => new ProductListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                MainImageUrl = mainMap.TryGetValue(p.Id, out var url) ? url : null
            }).ToList();

            return new PagedResult<ProductListItemDto>
            {
                Items = mapped,
                PageNumber = r.PageNumber,
                PageSize = r.PageSize,
                TotalCount = total
            };
        }
    }
}

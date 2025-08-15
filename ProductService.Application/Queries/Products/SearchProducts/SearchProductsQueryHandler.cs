using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Contracts.Requests;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;
using System.Security.Cryptography;
using System.Text;

namespace ProductService.Application.Queries.Products.SearchProducts
{
    public sealed class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, PagedResult<ProductListItemDto>>
    {
        private readonly IProductRepository _products;
        private readonly IProductImageRepository _images;
        private readonly ICatalogCache _cache;

        public SearchProductsQueryHandler(IProductRepository products, IProductImageRepository images, ICatalogCache cache)
        {
            _products = products;
            _images = images;
            _cache = cache;
        }

        public async Task<Result<PagedResult<ProductListItemDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            var key = CacheKey(r);
            var cached = await _cache.GetSearchAsync<PagedResult<ProductListItemDto>>(key, cancellationToken);
            if (cached is not null) return cached;

            var (items, total) = await _products.SearchAsync(
                r.CategoryId, r.Text, r.PriceMin, r.PriceMax, r.OnlyActive,
                r.SortBy, string.Equals(r.SortOrder, "desc", StringComparison.OrdinalIgnoreCase),
                r.PageNumber, r.PageSize, cancellationToken);

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

            var result = new PagedResult<ProductListItemDto>
            {
                Items = mapped,
                PageNumber = r.PageNumber,
                PageSize = r.PageSize,
                TotalCount = total
            };

            await _cache.SetSearchAsync(key, result, TimeSpan.FromSeconds(30), cancellationToken);

            return result;
        }

        private static string CacheKey(SearchProductsRequest r)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(r);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(bytes);
        }
    }
}

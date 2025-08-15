using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Requests;
using ProductService.Domain.Contracts.Responses;

namespace ProductService.Application.Queries.Products.SearchProducts
{
    public sealed record SearchProductsQuery(SearchProductsRequest Request) : IQuery<PagedResult<ProductListItemDto>>;
}

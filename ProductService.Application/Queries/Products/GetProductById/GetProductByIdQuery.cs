using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;

namespace ProductService.Application.Queries.Products.GetProductById
{
    public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDetailDto>;
}

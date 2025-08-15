using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;

namespace ProductService.Application.Queries.Categories.GetCategories
{
    public sealed record GetCategoriesQuery : IQuery<IReadOnlyList<CategoryDto>>;
}

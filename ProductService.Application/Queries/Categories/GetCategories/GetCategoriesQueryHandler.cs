using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Queries.Categories.GetCategories
{
    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
    {
        private readonly ICategoryRepository _cats;
        public GetCategoriesQueryHandler(ICategoryRepository cats) => _cats = cats;
        public async Task<Domain.Shared.Result<IReadOnlyList<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var items = await _cats.ListActiveAsync(cancellationToken);
            return items.Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description }).ToList();

        }
    }
}

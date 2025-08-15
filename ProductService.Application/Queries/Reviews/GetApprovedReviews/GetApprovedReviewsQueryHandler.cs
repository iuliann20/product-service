using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Queries.Reviews.GetApprovedReviews
{
    public sealed class GetApprovedReviewsQueryHandler : IQueryHandler<GetApprovedReviewsQuery, IReadOnlyList<ProductReviewDto>>
    {
        private readonly IProductReviewRepository _reviews;
        public GetApprovedReviewsQueryHandler(IProductReviewRepository reviews) => _reviews = reviews;

        public async Task<Result<IReadOnlyList<ProductReviewDto>>> Handle(GetApprovedReviewsQuery request, CancellationToken cancellationToken)
        {
            var items = await _reviews.ListApprovedForProductAsync(request.ProductId, cancellationToken);

            return items.Select(x => new ProductReviewDto
            {
                Id = x.Id,
                Rating = x.Rating,
                Comment = x.Comment,
                CreatedAtUtc = x.CreatedAtUtc
            }).ToList();
        }
    }
}

using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Contracts.Responses;

namespace ProductService.Application.Queries.Reviews.GetApprovedReviews
{
    public sealed record GetApprovedReviewsQuery(Guid ProductId) : IQuery<IReadOnlyList<ProductReviewDto>>;
}

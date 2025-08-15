using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Reviews.CreateReview
{
    public sealed record CreateReviewCommand(Guid UserId, Guid ProductId, int Rating, string? Comment) : ICommand<Guid>;
}

using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Reviews.ApproveReview
{
    public sealed record ApproveReviewCommand(Guid ReviewId) : ICommand;
}

using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Reviews.RejectReview
{
    public sealed record RejectReviewCommand(Guid ReviewId) : ICommand;
}

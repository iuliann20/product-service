using MediatR;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Enums;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Reviews.ApproveReview
{
    public sealed class ApproveReviewCommandHandler : ICommandHandler<ApproveReviewCommand>
    {
        private readonly IProductReviewRepository _reviews;
        private readonly IUnitOfWork _uow;

        public ApproveReviewCommandHandler(IProductReviewRepository reviews, IUnitOfWork uow)
        {
            _reviews = reviews;
            _uow = uow;
        }
        public async Task<Result> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviews.GetByIdAsync(request.ReviewId, cancellationToken) ?? throw new InvalidOperationException("Review not found.");

            review.Status = ReviewStatus.Approved;

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

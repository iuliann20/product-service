using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Reviews.CreateReview
{
    public sealed class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, Guid>
    {
        private readonly IProductRepository _products;
        private readonly IProductReviewRepository _reviews;
        private readonly IUnitOfWork _uow;

        public CreateReviewCommandHandler(IProductRepository products, IProductReviewRepository reviews, IUnitOfWork uow)
        {
            _products = products; _reviews = reviews; _uow = uow;
        }

        public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _ = await _products.GetByIdAsync(request.ProductId, cancellationToken) ?? throw new InvalidOperationException("Product not found.");

            var review = new ProductReview
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment,
                Status = ReviewStatus.Pending
            };

            await _reviews.AddAsync(review, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success(review.Id);
        }
    }
}

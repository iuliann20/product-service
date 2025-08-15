using FluentValidation;

namespace ProductService.Application.Commands.Reviews.CreateReview
{
    public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).MaximumLength(2000);
        }
    }
}

using MediatR;
using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _cats;
        private readonly IUnitOfWork _uow;

        public UpdateCategoryCommandHandler(ICategoryRepository cats, IUnitOfWork uow)
        {
            _cats = cats; _uow = uow;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _cats.GetByIdAsync(request.Id, cancellationToken) ?? throw new InvalidOperationException("Category not found.");

            category.Name = request.Name;

            category.Description = request.Description;

            category.IsActive = request.IsActive;

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

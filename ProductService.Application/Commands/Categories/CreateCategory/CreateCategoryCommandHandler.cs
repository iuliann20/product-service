using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _cats;
        private readonly IUnitOfWork _uow;
        public CreateCategoryCommandHandler(ICategoryRepository cats, IUnitOfWork uow)
        {
            _cats = cats; _uow = uow;
        }

        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { Name = request.Name, Description = request.Description, IsActive = true };

            await _cats.AddAsync(category, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}

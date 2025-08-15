using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _cats;
        private readonly IUnitOfWork _uow;

        public DeleteCategoryCommandHandler(ICategoryRepository cats, IUnitOfWork uow)
        {
            _cats = cats; _uow = uow;
        }
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _cats.DeleteAsync(request.Id, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

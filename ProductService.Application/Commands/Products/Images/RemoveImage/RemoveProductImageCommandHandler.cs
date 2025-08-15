using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.Images.RemoveImage
{
    public sealed class RemoveProductImageCommandHandler : ICommandHandler<RemoveProductImageCommand>
    {
        private readonly IProductImageRepository _images;
        private readonly IUnitOfWork _uow;

        public RemoveProductImageCommandHandler(IProductImageRepository images, IUnitOfWork uow)
        {
            _images = images;
            _uow = uow;
        }
        public async Task<Result> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            var img = await _images.GetByIdAsync(request.ImageId, cancellationToken) ?? throw new InvalidOperationException("Image not found.");

            if (img.ProductId != request.ProductId) throw new UnauthorizedAccessException();

            _images.Remove(img);

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

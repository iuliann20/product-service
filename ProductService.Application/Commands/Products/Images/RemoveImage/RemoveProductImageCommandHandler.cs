using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Caching;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Products.Images.RemoveImage
{
    public sealed class RemoveProductImageCommandHandler : ICommandHandler<RemoveProductImageCommand>
    {
        private readonly IProductImageRepository _images;
        private readonly IUnitOfWork _uow;
        private readonly ICatalogCache _cache;

        public RemoveProductImageCommandHandler(IProductImageRepository images, IUnitOfWork uow, ICatalogCache cache)
        {
            _images = images;
            _uow = uow;
            _cache = cache;
        }
        public async Task<Result> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            var img = await _images.GetByIdAsync(request.ImageId, cancellationToken) ?? throw new InvalidOperationException("Image not found.");

            if (img.ProductId != request.ProductId) throw new UnauthorizedAccessException();

            _images.Remove(img);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cache.InvalidateProductAsync(img.ProductId, cancellationToken);

            return Result.Success();
        }
    }
}

using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Products.Images.RemoveImage
{
    public sealed record RemoveProductImageCommand(Guid ProductId, Guid ImageId) : ICommand;
}

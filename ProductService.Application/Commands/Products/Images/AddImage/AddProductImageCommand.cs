using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Products.Images.AddImage
{
    public sealed record AddProductImageCommand(Guid ProductId, string ImageUrl, bool IsMain) : ICommand<Guid>;
}

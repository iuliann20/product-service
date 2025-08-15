using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Products.UpdateProduct
{
    public sealed record UpdateProductCommand(Guid Id, string Name, string? Description, decimal Price, Guid CategoryId, bool IsActive) : ICommand;
}

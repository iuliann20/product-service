using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Products.CreateProduct
{
    public sealed record CreateProductCommand(string Name, string? Description, decimal Price, Guid CategoryId, int StockQuantity, bool IsActive) : ICommand<Guid>;
}

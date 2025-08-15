using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Products.AdjustStock
{
    public sealed record AdjustStockCommand(Guid Id, int Delta) : ICommand<int>;
}

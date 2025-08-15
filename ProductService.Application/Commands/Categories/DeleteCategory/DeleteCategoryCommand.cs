using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Categories.DeleteCategory
{
    public sealed record DeleteCategoryCommand(Guid Id) : ICommand;
}

using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Categories.CreateCategory
{
    public sealed record CreateCategoryCommand(string Name, string? Description) : ICommand<Guid>;
}

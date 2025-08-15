using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Categories.UpdateCategory
{
    public sealed record UpdateCategoryCommand(Guid Id, string Name, string? Description, bool IsActive) : ICommand;
}

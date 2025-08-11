using MediatR;

namespace ProductService.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}

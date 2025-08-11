using MediatR;
using ProductService.Domain.Shared;

namespace ProductService.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

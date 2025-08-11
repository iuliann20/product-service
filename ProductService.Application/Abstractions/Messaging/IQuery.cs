using MediatR;
using ProductService.Domain.Shared;

namespace ProductService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
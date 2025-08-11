using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Errors;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Commands.Test
{
    internal sealed class TestCommandHandler : ICommandHandler<TestCommand>
    {
        private readonly IUnitOfWork _sqluow;

        public TestCommandHandler(IUnitOfWork sqluow)
        {
            _sqluow = sqluow;
        }

        public async Task<Result> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            await _sqluow.TestRepository.TestMehod();

            if (request.Id == 1)
            {
                return Result.Failure(TestErrors.TestError);
            }
            return Result.Success();
        }
    }
}

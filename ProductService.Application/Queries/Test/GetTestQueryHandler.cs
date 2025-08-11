using ProductService.Application.Abstractions.Messaging;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;

namespace ProductService.Application.Queries.Test
{
    internal sealed class GetTestQueryHandler : IQueryHandler<GetTestQuery, List<int>>
    {
        private readonly IUnitOfWork _sqluow;

        public GetTestQueryHandler(IUnitOfWork sqluow)
        {
            _sqluow = sqluow;
        }

        public async Task<Result<List<int>>> Handle(GetTestQuery request, CancellationToken cancellationToken)
        {
            var response = await _sqluow.TestRepository.GetTestMethod(request.Id);

            return Result.Create(response);
        }
    }
}

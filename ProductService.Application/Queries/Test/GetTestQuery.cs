using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Queries.Test
{
    public class GetTestQuery : IQuery<List<int>>
    {
        public int Id { get; set; }
    }
}

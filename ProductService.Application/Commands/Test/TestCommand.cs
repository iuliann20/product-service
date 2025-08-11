using ProductService.Application.Abstractions.Messaging;

namespace ProductService.Application.Commands.Test
{
    public class TestCommand : ICommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

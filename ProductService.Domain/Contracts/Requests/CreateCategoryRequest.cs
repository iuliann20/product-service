namespace ProductService.Domain.Contracts.Requests
{
    public sealed class CreateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}

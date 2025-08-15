namespace ProductService.Domain.Contracts.Responses
{
    public sealed class ProductListItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
        public bool IsActive { get; init; }
        public Guid CategoryId { get; init; }
        public string? MainImageUrl { get; init; }
    }
}

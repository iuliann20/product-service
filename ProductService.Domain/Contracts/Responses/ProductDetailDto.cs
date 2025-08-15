namespace ProductService.Domain.Contracts.Responses
{
    public sealed class ProductDetailDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public Guid CategoryId { get; init; }
        public bool IsActive { get; init; }
        public int StockQuantity { get; init; }
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<string> Images { get; init; } = Array.Empty<string>();
    }
}

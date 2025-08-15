namespace ProductService.Domain.Contracts.Requests
{
    public sealed class UpdateProductRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

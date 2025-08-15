namespace ProductService.Domain.Contracts.Responses
{
    public sealed class ProductReviewDto
    {
        public Guid Id { get; init; }
        public int Rating { get; init; }
        public string? Comment { get; init; }
        public DateTime CreatedAtUtc { get; init; }
    }
}

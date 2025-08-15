namespace ProductService.Domain.Contracts.Requests
{
    public sealed class CreateReviewRequest
    {
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

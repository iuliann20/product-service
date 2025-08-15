namespace ProductService.Domain.Contracts.Requests
{
    public sealed class AddImageRequest
    {
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; } = false;
    }
}

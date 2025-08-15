namespace ProductService.Domain.Contracts.Requests
{
    public sealed class UpdateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

namespace ProductService.Domain.Contracts.Requests
{
    public sealed class SearchProductsRequest
    {
        public Guid? CategoryId { get; set; }
        public string? Text { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public bool OnlyActive { get; set; } = true;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } // price|createdAt|name
        public string? SortOrder { get; set; } // asc|desc
    }
}

namespace ProductService.Domain.Contracts.Requests
{
    public sealed class AdjustStockRequest
    {
        public int Delta { get; set; }
    }
}

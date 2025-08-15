namespace ProductService.Domain.Contracts.Responses
{
    public sealed record OrderItemDto(Guid ProductId, int Quantity);
}

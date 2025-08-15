using ProductService.Domain.Primitives;

namespace ProductService.Domain.Entities
{
    public sealed class ProductImage : Entity
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsMain { get; set; } = false;

        public Product Product { get; set; } = default!;
    }
}

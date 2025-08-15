using ProductService.Domain.Primitives;

namespace ProductService.Domain.Entities
{
    public sealed class Category : Entity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

using ProductService.Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities
{
    public sealed class Product : AggregateRoot
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; } = default!;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();

        public void AdjustStock(int delta)
        {
            var newValue = StockQuantity + delta;
            if (newValue < 0) throw new InvalidOperationException("Stock cannot go negative.");
            StockQuantity = newValue;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0) throw new InvalidOperationException("Price cannot be negative.");
            Price = price;
        }
    }
}

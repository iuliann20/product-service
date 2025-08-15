using ProductService.Domain.Enums;
using ProductService.Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities
{
    public sealed class ProductReview : Entity
    {
        public ProductReview() : base(Guid.NewGuid()) { }

        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }        // 1..5
        public string? Comment { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        public Product Product { get; set; } = default!;
    }
}

using ProductService.Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities
{
    public class ProductImage : Entity
    {
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}

using ProductService.Domain.Events;
using ProductService.Domain.Primitives;

namespace ProductService.Domain.Entities
{
    public sealed class Product : AggregateRoot
    {
        public Product() : base(Guid.NewGuid()) { }

        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public Guid CategoryId { get; private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Category Category { get; private set; } = default!;
        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
        public ICollection<ProductReview> Reviews { get; private set; } = new List<ProductReview>();

        public static Product Create(string name, string? description, decimal price, Guid categoryId, int stock, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("Name required.");
            if (price < 0) throw new InvalidOperationException("Price cannot be negative.");
            if (stock < 0) throw new InvalidOperationException("Stock cannot be negative.");

            var p = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
                StockQuantity = stock,
                IsActive = isActive,
                CreatedAt = DateTime.UtcNow
            };

            p.RaiseDomainEvent(new ProductCreatedDomainEvent(p.Id, p.Name, p.CategoryId, p.Price, p.StockQuantity, p.IsActive));
            return p;
        }

        public void Update(string name, string? description, decimal price, Guid categoryId, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("Name required.");
            if (price < 0) throw new InvalidOperationException("Price cannot be negative.");

            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            IsActive = isActive;

            RaiseDomainEvent(new ProductUpdatedDomainEvent(Id, Name, CategoryId, Price, IsActive));
        }

        public void AdjustStock(int delta)
        {
            var newValue = StockQuantity + delta;
            if (newValue < 0) throw new InvalidOperationException("Stock cannot go negative.");

            StockQuantity = newValue;
            RaiseDomainEvent(new StockAdjustedDomainEvent(Id, StockQuantity, delta));
        }

        public void Deactivate()
        {
            IsActive = false;
            RaiseDomainEvent(new ProductDeactivatedDomainEvent(Id));
        }

        public void SetPrice(decimal price)
        {
            if (price < 0) throw new InvalidOperationException("Price cannot be negative.");
            Price = price;
            RaiseDomainEvent(new ProductUpdatedDomainEvent(Id, Name, CategoryId, Price, IsActive));
        }
    }
}

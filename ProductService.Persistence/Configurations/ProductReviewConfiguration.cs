using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Persistence.Constants;

namespace ProductService.Persistence.Configurations
{
    internal sealed class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable("ProductReviews");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(2000);
            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}

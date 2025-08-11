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
            builder.ToTable(TableNames.ProductReviews);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comment).IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Persistence.Constants;

namespace ProductService.Persistence.Configurations
{
    internal sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable(TableNames.ProductImages);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl).IsRequired();
        }
    }
}

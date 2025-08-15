using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Persistence.Outbox;
using ProductService.Persistence.Constants;

namespace ProductService.Persistence.Configurations
{
    internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable(TableNames.OutboxMessages);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.OccurredOnUtc).IsRequired();
        }
    }
}

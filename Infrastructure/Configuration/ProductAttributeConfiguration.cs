using Ecommerce.Common.Models.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("product_attributes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(256).IsRequired(false);

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

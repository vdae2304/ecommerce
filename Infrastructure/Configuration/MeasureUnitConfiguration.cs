using Ecommerce.Common.Models.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class MeasureUnitConfiguration : IEntityTypeConfiguration<MeasureUnit>
    {
        public void Configure(EntityTypeBuilder<MeasureUnit> builder)
        {
            builder.ToTable("measure_units");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Symbol).IsUnique();

            builder.Property(x => x.Symbol).HasMaxLength(8).IsRequired();
            builder.Property(x => x.Type).IsRequired();

            builder.HasMany<Product>()
                .WithOne(x => x.DimensionUnits)
                .HasForeignKey(x => x.DimensionUnitsId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany<Product>()
                .WithOne(x => x.WeightUnits)
                .HasForeignKey(x => x.WeightUnitsId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany<Product>()
                .WithOne(x => x.VolumeUnits)
                .HasForeignKey(x => x.VolumeUnitsId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

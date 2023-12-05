using Ecommerce.Common.Models.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Schema
{
    public class MediaImageConfiguration : IEntityTypeConfiguration<MediaImage>
    {
        public void Configure(EntityTypeBuilder<MediaImage> builder)
        {
            builder.ToTable("MediaImages");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.FileId).IsUnique();

            builder.Property(x => x.FileId).IsRequired();
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.Width).IsRequired();
            builder.Property(x => x.Height).IsRequired();

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

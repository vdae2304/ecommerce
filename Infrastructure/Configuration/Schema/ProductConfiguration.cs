using Ecommerce.Common.Models.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Schema
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Sku).IsUnique();

            builder.Property(x => x.Sku).HasMaxLength(12).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasPrecision(10, 4).IsRequired();
            builder.Property(x => x.CrossedOutPrice).HasPrecision(10, 4).IsRequired(false);
            builder.Property(x => x.MinPurchaseQuantity).IsRequired();
            builder.Property(x => x.MaxPurchaseQuantity).IsRequired();
            builder.Property(x => x.InStock).IsRequired();
            builder.Property(x => x.Unlimited).IsRequired();
            builder.Property(x => x.Enabled).IsRequired();

            builder.HasOne(x => x.Thumbnail)
                .WithOne()
                .HasForeignKey<Product>(x => x.ThumbnailId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.GalleryImages)
                .WithMany()
                .UsingEntity<ProductImages>(
                    image => image.HasOne<MediaImage>().WithMany().HasForeignKey(x => x.ImageId),
                    product => product.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId));

            builder.HasMany(x => x.Categories)
                .WithMany(x => x.Products)
                .UsingEntity<ProductCategories>(
                    category => category.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId),
                    product => product.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId));

            builder.HasMany(x => x.Attributes)
                .WithOne()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

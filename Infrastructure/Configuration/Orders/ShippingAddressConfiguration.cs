using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Orders
{
    public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.ToTable("ShippingAddresses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Phone).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Street).HasMaxLength(256).IsRequired();
            builder.Property(x => x.City).HasMaxLength(256).IsRequired();
            builder.Property(x => x.State).HasMaxLength(256).IsRequired();
            builder.Property(x => x.PostalCode).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Comments).IsRequired(false);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Orders
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Recipient).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Phone).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Street).HasMaxLength(256).IsRequired();
            builder.Property(x => x.StreetNumber).HasMaxLength(256).IsRequired();
            builder.Property(x => x.AptNumber).HasMaxLength(256).IsRequired(false);
            builder.Property(x => x.Neighbourhood).HasMaxLength(256).IsRequired();
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

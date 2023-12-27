using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Orders
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CardOwner).HasMaxLength(256).IsRequired();
            builder.Property(x => x.CardNumber).HasMaxLength(256).IsRequired();
            builder.Property(x => x.CVV).HasMaxLength(256).IsRequired();
            builder.Property(x => x.ExpiryMonth).HasMaxLength(256).IsRequired();
            builder.Property(x => x.ExpiryYear).HasMaxLength(256).IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.BillingAddress)
                .WithMany()
                .HasForeignKey(x => x.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CreatedAt).HasPrecision(0).IsRequired();
            builder.Property(x => x.UpdatedAt).HasPrecision(0).IsRequired();
        }
    }
}

using Ecommerce.Common.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration.Orders
{
    public class OrderProductsConfiguration : IEntityTypeConfiguration<OrderProducts>
    {
        public void Configure(EntityTypeBuilder<OrderProducts> builder)
        {
            builder.ToTable("OrderProducts");
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.Property(x => x.Price).HasPrecision(10, 4).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

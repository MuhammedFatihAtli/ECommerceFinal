using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // OrderConfig sınıfı, Order entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.ShippingAddress)
                .HasMaxLength(500);

            builder.Property(o => o.TrackingNumber)
                .HasMaxLength(100);

            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
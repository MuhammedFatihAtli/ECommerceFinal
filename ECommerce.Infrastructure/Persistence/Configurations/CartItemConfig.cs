using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // CartItemConfig sınıfı, CartItem entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            //Entity sınıflarının EF Core ile nasıl eşleneceğini (HasKey, HasOne, Property, ToTable )...... tanımlandığı kısım.

            //CartItem tablosunun nasıl oluşturulacağı ve kolonlarının hangi kurallara uyacağıyla ilgili ayarlar
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Quantity)
                   .IsRequired();

            builder.Property(ci => ci.GuestId)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(ci => ci.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(ci => ci.UpdatedDate)
                   .IsRequired(false);

            builder.Property(ci => ci.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(ci => ci.Status)
                   .HasDefaultValue(true);
        }
    }
}

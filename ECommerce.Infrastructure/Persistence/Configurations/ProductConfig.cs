using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    // ProductConfig sınıfı, Product entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Stock)
                   .IsRequired();

            builder.Property(p => p.ImagePath)
                   .HasMaxLength(300);

            builder.Property(p => p.CreatedDate)
                   .IsRequired();

            builder.Property(p => p.UpdatedDate)
                   .IsRequired(false);

            builder.Property(p => p.IsDeleted)
                   .HasDefaultValue(false);         

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict); // veya Cascade değilse sorun çözülür

            builder.HasOne(p => p.Promotion)
                   .WithMany(promo => promo.Products)
                   .HasForeignKey(p => p.PromotionId)
                   .HasConstraintName("FK_Products_Promotions_PromotionId")
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Seller)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}


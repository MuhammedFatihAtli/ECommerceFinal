using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Entities;

public class PromotionConfig : IEntityTypeConfiguration<Promotion>
{
    // PromotionConfig sınıfı, Promotion entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.DiscountRate)
               .IsRequired()
               .HasColumnType("decimal(5,2)"); // örn: 15.50 %

        builder.Property(p => p.StartDate)
               .IsRequired();

        builder.Property(p => p.EndDate)
               .IsRequired();
       
    }
}

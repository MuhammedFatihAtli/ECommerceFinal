using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // SellerConfig sınıfı, Seller entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class SellerConfig : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.Property(s => s.CompanyName)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(s => s.LogoUrl)
                   .HasMaxLength(300)
                   .IsRequired(false);

            builder.Property(s => s.Address)
                   .HasMaxLength(300)
                   .IsRequired(false);

            builder.Property(s => s.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(s => s.UpdatedDate)
                   .IsRequired(false);

            builder.Property(s => s.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}

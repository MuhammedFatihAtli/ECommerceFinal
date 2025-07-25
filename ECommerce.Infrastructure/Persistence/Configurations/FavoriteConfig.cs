using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // FavoriteConfig sınıfı, Favorite entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class FavoriteConfig : IEntityTypeConfiguration<Favorite>
    {
        
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}


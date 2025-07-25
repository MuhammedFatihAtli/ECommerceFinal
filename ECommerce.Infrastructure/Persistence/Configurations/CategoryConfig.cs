using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    // CategoryConfig sınıfı, Category entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            builder.Property(c => c.CreatedDate)
                   .IsRequired();

            builder.Property(c => c.UpdatedDate)
                   .IsRequired(false);

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);          

            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);//kategori silindiğinde ona bağlı ürünler otomatik silinmez.son kısım

            builder.HasQueryFilter(c => !c.IsDeleted);//IsDeleted alanı false olan kategoriler filtrelenir.
        }
    }
}



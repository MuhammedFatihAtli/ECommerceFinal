using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // CommentConfig sınıfı, Category entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Text)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(c => c.Rating)
                   .IsRequired();

            builder.Property(c => c.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.UpdatedDate)
                   .IsRequired(false);

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}


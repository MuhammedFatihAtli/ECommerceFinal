using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfig : IEntityTypeConfiguration<User>
{
    // UserConfig sınıfı, User entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasMany(u => u.Enrollments)
               .WithOne(e => e.User)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.IsDeleted)
               .HasDefaultValue(false);

        builder.Property(u => u.Status)
               .HasDefaultValue(true);

        builder.Property(u => u.CreatedDate)
               .HasDefaultValueSql("GETDATE()");
    }
}


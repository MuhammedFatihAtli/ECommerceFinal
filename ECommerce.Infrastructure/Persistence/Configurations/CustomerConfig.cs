using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    // CustomerConfig sınıfı, Customer entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Address)
               .HasMaxLength(250)
               .IsRequired(false);

        builder.Property(c => c.PhoneNumber)
               .HasMaxLength(20)
               .IsRequired(false);

        builder.Property(c => c.ProfileImageUrl)
               .HasMaxLength(300)
               .IsRequired(false);
        builder.Property(c => c.CreatedDate)
                  .HasDefaultValueSql("GETDATE()");

        builder.Property(c => c.UpdatedDate)
               .IsRequired(false);

        builder.Property(c => c.IsDeleted)
               .HasDefaultValue(false);
    }
}

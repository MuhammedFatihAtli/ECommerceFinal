using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    // EnrollmentConfig sınıfı, Enrollment entity'sinin EF Core ile nasıl eşleneceğini tanımlar.
    public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
          

            builder
                .HasOne(e => e.User)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

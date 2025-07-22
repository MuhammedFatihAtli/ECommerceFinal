using ECommerce.Domain.Commons;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<User> Users { get; set; }       
        public DbSet<Order> Orders { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.ProductId });

            modelBuilder.Entity<CartItem>()
                .HasIndex(c => new { c.GuestId, c.ProductId });

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired(false);

            // Tüm entity tiplerini döngü ile geziyoruz
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                // Eğer entity tipi BaseEntity sınıfından türemişse
                if (typeof(BaseEntity).IsAssignableFrom(item.ClrType))
                {
                    // Lambda ifadesinde kullanılacak parametreyi tanımlıyoruz (örnek: "e" => e.IsDeleted == false)
                    var parameter = Expression.Parameter(item.ClrType, "e");

                    // IsDeleted property'si false olanları filtreleyecek bir ifade oluşturuyoruz
                    var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                            Expression.Constant(false) // IsDeleted == false
                        ), parameter);

                    // Bu filtreyi entity'e uyguluyoruz – Soft Delete mantığı (silinmemiş olanlar)
                    modelBuilder.Entity(item.ClrType).HasQueryFilter(filter);
                }
            }

            // Assembly içindeki tüm IEntityTypeConfiguration yapılandırmalarını otomatik olarak uygula
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // Bu sadece design-time için kullanılır
        //        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ECommerce2;Integrated Security=True;");
        //    }
        //}
    }
}

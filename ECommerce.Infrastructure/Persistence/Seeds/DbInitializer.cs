using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Domain.Commons;

namespace ECommerce.Infrastructure.Persistence.Seeds
{
    public static class DbInitializer
    {
        // Veritabanında eksik migration varsa uygular
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        // Rolleri oluşturur
        public static async Task EnsureRolesExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roles = { RoleNames.Admin, RoleNames.Customer, RoleNames.Seller };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }

        // Admin kullanıcıyı oluşturur
        public static async Task EnsureAdminUserExistsAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string adminEmail = "admin@site.com";
            string adminPassword = "Admin123!";
            string adminFullName = "admin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail) ??
                            await userManager.FindByNameAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User(adminFullName, adminEmail)
                {
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Admin kullanıcısı oluşturulamadı: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, RoleNames.Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
                }
            }
        }

        public static async Task EnsureSellerUserExistsAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string sellerEmail = "seller@site.com";
            string sellerPassword = "Seller123!";

            // Kullanıcıyı mail ya da kullanıcı adına göre ara
            var sellerUser = await userManager.FindByEmailAsync(sellerEmail) ??
                             await userManager.FindByNameAsync(sellerEmail);

            if (sellerUser == null)
            {
                sellerUser = new Seller
                {
                    UserName = sellerEmail,
                    Email = sellerEmail,
                    EmailConfirmed = true,

                    // Zorunlu alanlar
                    FirstName = "Demo",
                    LastName = "Satıcı",
                    FullName = "Demo Satıcı",
                    CompanyName = "Demo Şirketi",
                    Address = "İstanbul, Türkiye",
                    LogoUrl = "/images/default-logo.png"
                };

                var result = await userManager.CreateAsync(sellerUser, sellerPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Seller kullanıcısı oluşturulamadı: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(sellerUser, RoleNames.Seller);
            }
            else
            {
                // Zaten varsa, rol atanmış mı kontrol et
                if (!await userManager.IsInRoleAsync(sellerUser, RoleNames.Seller))
                {
                    await userManager.AddToRoleAsync(sellerUser, RoleNames.Seller);
                }
            }
        }


        //public static async Task EnsureAdminSellerExistsAsync(this IServiceProvider serviceProvider)
        //{
        //    using var scope = serviceProvider.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        //    string adminEmail = "admin@site.com";
        //    var adminUser = await userManager.FindByEmailAsync(adminEmail);

        //    if (adminUser == null)
        //        return;

        //    // DbContext'ten aynı Id'ye sahip Seller var mı kontrol et
        //    var existingSeller = await context.Sellers
        //        .FirstOrDefaultAsync(s => s.Id == adminUser.Id);

        //    if (existingSeller == null)
        //    {
        //        // Yeni Seller oluştur ve ekle
        //        var seller = new Seller
        //        {
        //            Id = adminUser.Id,
        //            UserName = adminUser.UserName,
        //            Email = adminUser.Email,
        //            CompanyName = "Admin Firma",
        //            LogoUrl = "/images/admin-logo.png",
        //            Address = "Merkez/Ankara",
        //            CreatedDate = DateTime.Now,
        //            FirstName = "Admin",
        //            LastName = "User",
        //            FullName = "Admin User",
        //            EmailConfirmed = true,
        //            Status = true,
        //        };

        //        context.Sellers.Add(seller);
        //    }
        //    else
        //    {
        //        // Mevcut Seller güncelle
        //        existingSeller.UserName = adminUser.UserName;
        //        existingSeller.Email = adminUser.Email;
        //        existingSeller.CompanyName = "Admin Firma";
        //        existingSeller.LogoUrl = "/images/admin-logo.png";
        //        existingSeller.Address = "Merkez/Ankara";
        //        existingSeller.FirstName = "Admin";
        //        existingSeller.LastName = "User";
        //        existingSeller.FullName = "Admin User";
        //        existingSeller.EmailConfirmed = true;
        //        existingSeller.Status = true;

        //        // Zaten takip edilen entity olduğu için Update çağrısına gerek yok
        //    }

        //    await context.SaveChangesAsync();
        //}


        public static async Task EnsureCustomerUsersExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var customers = new List<Customer>
    {
        new Customer
        {
            UserName = "customer1@site.com",
            Email = "customer1@site.com",
            EmailConfirmed = true,
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            FullName = "Ahmet Yılmaz",
            Address = "Ankara, Türkiye",
            PhoneNumber = "05001112233",
            ProfileImageUrl = "/images/customer1.png"
        },
        new Customer
        {
            UserName = "customer2@site.com",
            Email = "customer2@site.com",
            EmailConfirmed = true,
            FirstName = "Zeynep",
            LastName = "Demir",
            FullName = "Zeynep Demir",
            Address = "İzmir, Türkiye",
            PhoneNumber = "05002223344",
            ProfileImageUrl = "/images/customer2.png"
        },
        new Customer
        {
            UserName = "customer3@site.com",
            Email = "customer3@site.com",
            EmailConfirmed = true,
            FirstName = "Mehmet",
            LastName = "Kara",
            FullName = "Mehmet Kara",
            Address = "İstanbul, Türkiye",
            PhoneNumber = "05003334455",
            ProfileImageUrl = "/images/customer3.png"
        }
    };

            string defaultPassword = "Customer123!";

            foreach (var customer in customers)
            {
                var existingUser = await userManager.FindByEmailAsync(customer.Email);
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(customer, defaultPassword);

                    if (!result.Succeeded)
                    {
                        throw new Exception("Customer kullanıcısı oluşturulamadı: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }

                    await userManager.AddToRoleAsync(customer, RoleNames.Customer);
                }
            }
        }


    }
}
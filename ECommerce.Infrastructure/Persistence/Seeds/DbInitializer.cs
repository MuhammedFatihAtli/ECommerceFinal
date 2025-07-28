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
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)//EF Core veritabanında eksik bir migration var mı diye kontrol
        {
            using var scope = app.ApplicationServices.CreateScope();//servislerin kullanılabilineceği bir alan oluşturma
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();//veritabanıyla iletişim

            if (context.Database.GetPendingMigrations().Any())//Uygulanmamış migration (değişiklik) var mı diye kontrol eder.
            {
                context.Database.Migrate();//varsa olan migrationları uygular
            }
        }

        // Rolleri oluşturur
        public static async Task EnsureRolesExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();//ASP.NET Identity’nin rol yöneticisini alır.

            string[] roles = { RoleNames.Admin, RoleNames.Customer, RoleNames.Seller };

            foreach (var role in roles)
            {
                //Rol yoksa oluştur 
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


        public static async Task EnsureCustomerUsersExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Müşteri kullanıcılarını tanımla
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

        public static async Task EnsureCategoriesExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
        {
            new Category("Elektronik", "Telefon, bilgisayar, televizyon gibi ürünler"),
            new Category("Moda", "Giyim, ayakkabı, aksesuar ürünleri"),
            new Category("Ev ve Yaşam", "Mobilya, ev dekorasyonu, mutfak ürünleri"),
            new Category("Kitap", "Roman, ders kitapları, dergiler"),
            new Category("Spor ve Outdoor", "Spor giyim, kamp ve doğa ürünleri"),
            new Category("Movie", "Film ve dizi ürünleri"),
            new Category("Game", "Video oyunları, konsollar ve aksesuarlar"),
        };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }
        public static async Task EnsureProductsExistAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Products.Any())
            {
                // Kategori ve Satıcı ID’lerini al
                var category = await context.Categories.FirstOrDefaultAsync();
                var seller = await context.Users.OfType<Seller>().FirstOrDefaultAsync();

                if (category == null || seller == null)
                {
                    throw new Exception("Ürün eklenemedi çünkü kategori veya satıcı bulunamadı.");
                }

                var products = new List<Product>
        {
            new Product
            {
                Name = "iPhone 14",
                Description = "Apple'ın en yeni akıllı telefonu",
                Price = 39999,
                Stock = 50,
                ImagePath = "/images/products/ASSET_MMS_97292542.png",
                CategoryId = category.Id,
                CategoryName = category.Name,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Gaming Laptop",
                Description = "Yüksek performanslı oyun laptopu",
                Price = 29999,
                Stock = 30,
                ImagePath = "/images/products/header-new-laptop.png",
                CategoryId = category.Id,
                CategoryName = category.Name,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Bluetooth Kulaklık",
                Description = "Kablosuz bluetooth kulaklık",
                Price = 799,
                Stock = 100,
                ImagePath = "/images/Products/images.jpg",
                CategoryId = category.Id,
                CategoryName = category.Name,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Toprak Ana Biblo",
                Description = "Ev dekorasyonu eşyası",
                Price = 1299,
                Stock = 10,
                ImagePath = "/images/products/Demeter.jpg",
                CategoryId = 3,
                CategoryName = "Ev ve Yaşam",
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Game of War",
                Description = "Movie",
                Price = 299,
                Stock = 120,
                ImagePath = "/images/products/space-vertical-wallpaper-preview.jpg",
                CategoryId = 6,
                CategoryName = "Movie , Film ve dizi ürünleri",
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Erlik Han 1",
                Description = "Movie",
                Price = 199,
                Stock = 200,
                ImagePath = "/images/products/Erlik_Han_card_design (1).png",
                CategoryId = 6,
                CategoryName = "Movie , Film ve dizi ürünleri",
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Erlik Han 2",
                Description = "Movie",
                Price = 199,
                Stock = 100,
                ImagePath = "/images/products/Erlik_Han_card_design (1).png",
                CategoryId = 6,
                CategoryName = "Movie , Film ve dizi ürünleri",
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Toprak Ana Biblo - 2",
                Description = "Defolu ev dekorasyonu eşyası",
                Price = 599,
                Stock = 10,
                ImagePath = "/images/products/Demeter.jpg",
                CategoryId = 3,
                CategoryName = "Ev ve Yaşam",
                SellerId = seller.Id
            },new Product
            {
                Name = "Kadın Deri Ceket",
                Description = "Şık ve modern deri ceket",
                Price = 899,
                Stock = 20,
                 ImagePath = "/images/products/kadın_ceket.png",
                CategoryId =2,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Suç ve Ceza",
                Description = "Fyodor Dostoyevski'nin klasik eseri",
                Price = 49,
                Stock = 100,
                ImagePath = "/images/products/Suç_ve_Ceza.png",
                CategoryId = 4,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "Topuklu Ayakkabı",
                Description = "Hafif ve zarif ayakkabı",
                Price = 5999,
                Stock = 30,
                ImagePath = "/images/products/png-clipart-women-shoes-women-shoes-thumbnail.png",
                CategoryId = 5,
                SellerId = seller.Id
            },
            new Product
            {
                Name = "PlayStation 5",
                Description = "Sony PlayStation 5 Konsol - 1TB",
                Price = 21999,
                Stock = 7,
                ImagePath = "/images/products/playstation-5.png",
                CategoryId = 7,
                SellerId = seller.Id 
            }
        };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

    }
}
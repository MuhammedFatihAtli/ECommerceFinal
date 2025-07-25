using ECommerce.Infrastructure.IOCs;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.MVC.Filters;
using ECommerce.Application.Mappers;

namespace ECommerce.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        { // Uygulama yapılandırıcısı (builder) başlatılıyor
            var builder = WebApplication.CreateBuilder(args);
            // MVC yapılandırması + global exception filter ekleniyor
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();// Her kontrolcüye otomatik hata filtresi
            })
            .AddApplicationPart(typeof(ECommerce.Presentation.Class1).Assembly);// Presentation katmanındaki controller'ları da tanır
             // Veritabanı bağlantısı ve EF Core yapılandırması
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Identity (kullanıcı yönetimi) yapılandırması
            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            { // Şifre kuralları oldukça esnek bırakılmış
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<AppDbContext>()// Identity için EF kullanımı
            .AddDefaultTokenProviders();// Şifre sıfırlama vb. işlemler için token sağlayıcılar
             // Session (oturum) yapılandırması
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // IOC (bağımlılıkları) servis olarak ekler
            builder.Services.AddIOCRegister();
            // Exception filter servisi scoped olarak ekleniyor
            builder.Services.AddScoped<GlobalExceptionFilter>();
            // AutoMapper yapılandırması (tüm domain'deki eşlemeleri alır)
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Web uygulaması inşa ediliyor
            var app = builder.Build();
            // Ortama göre hata yönetimi ayarlanıyor
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Ortama göre hata yönetimi ayarlanıyor
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Production'da hata sayfası
                app.UseHsts(); // HTTPS zorlaması için güvenlik başlığı
            }
            // HTTPS yönlendirme ve statik dosya servisi aktif ediliyor
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Rotalama aktif ediliyor
            app.UseRouting();
            // Session middleware'i aktif ediliyor
            app.UseSession();
            // Kimlik doğrulama ve yetkilendirme aktif ediliyor
            app.UseAuthentication();
            app.UseAuthorization();

            // Area'lı route tanımı (örnek: /Admin/Home/Index)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            // Varsayılan route tanımı (örnek: /Home/Index)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Veritabanı migration ve seed işlemleri burada çağrılıyor

            app.ConfigureAndCheckMigration();


            // Rol ve kullanıcı verilerinin başlangıçta oluşturulması
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await services.EnsureRolesExistAsync();
                await services.EnsureAdminUserExistsAsync();
                await services.EnsureSellerUserExistsAsync();
                //await services.EnsureAdminSellerExistsAsync();
                await services.EnsureCustomerUsersExistAsync();
            }

             // Uygulamanın çalışacağı portlar belirleniyor (manuel olarak)
            app.Urls.Clear();
            app.Urls.Add("http://localhost:5000");
            app.Urls.Add("https://localhost:5001");
            // Uygulama çalıştırılıyor
            app.Run();
        }
    }
}

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
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            })
            .AddApplicationPart(typeof(ECommerce.Presentation.Class1).Assembly);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddIOCRegister();

            builder.Services.AddScoped<GlobalExceptionFilter>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ? Geliþtirici dostu
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            // ? Area desteði
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ? DB migration ve seed iþlemleri
            app.ConfigureAndCheckMigration();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await services.EnsureRolesExistAsync();
                await services.EnsureAdminUserExistsAsync();
                await services.EnsureSellerUserExistsAsync();
                //await services.EnsureAdminSellerExistsAsync();
                await services.EnsureCustomerUsersExistAsync();
            }

            // Geliþtirme portlarý
            app.Urls.Clear();
            app.Urls.Add("http://localhost:5000");
            app.Urls.Add("https://localhost:5001");

            app.Run();
        }
    }
}


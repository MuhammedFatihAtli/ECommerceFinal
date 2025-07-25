using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Security;
using ECommerce.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerce.Infrastructure.IOCs
{
    public static class Records
    {
        public static void AddIOCRegister(this IServiceCollection services)
        {
            //veritabanı işlemlerini yapar.Domain katmanındaki I...Repository arayüzlerinin implementasyonları.
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>(); //**
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>(); //**

            services.AddScoped<IUnitOfWork, UnitOfWork>();//SaveChanges() gibi ortak işlemleri yönetir.

            //Uygulama katmanındaki I...Service arayüzlerinin implementasyonları.

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IServiceUnit, ServiceUnit>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IJwtService, JwtService>();//JWT token üretimi ve kontrolü için.
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICommentService, CommentService>();



            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();//ASP.NET Core’un PasswordHasher<T> sınıfı ile kullanıcı şifreleri hash’lenir.
        }
    }
}

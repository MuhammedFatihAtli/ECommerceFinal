using AutoMapper;
using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IServiceUnit _serviceUnit;

        public CartController(
            ICartService cartService,
            IProductService productService,
            IMapper mapper,
            IAuthService authService,
            IServiceUnit serviceUnit)
        {
            _cartService = cartService;
            _productService = productService;
            _mapper = mapper;
            _authService = authService;
            _serviceUnit = serviceUnit;
        }

        private async Task<int?> GetCurrentUserIdAsync()// Kullanıcının ID'sini döndüren yardımcı metot
        {
            return await _authService.GetCurrentUserIdAsync(User);
        }

        private async Task<string> GetSessionIdAsync()// Kullanıcının session (oturum) ID'sini döndüren yardımcı metot
        {
            if (!HttpContext.Session.IsAvailable)
                await HttpContext.Session.LoadAsync();// Oturum yüklü değilse yükle

            var sessionId = HttpContext.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId)) // Oturumdan sessionId'yi al
            {
                sessionId = Guid.NewGuid().ToString(); // Eğer yoksa yeni bir GUID oluştur
                HttpContext.Session.SetString("SessionId", sessionId);// Session'a kaydet
            }

            return sessionId;
        }

        // Sepet sayfasını gösteren metot
        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserIdAsync(); // Giriş yapan kullanıcının ID'si
            var sessionId = await GetSessionIdAsync();// Giriş yapmayan kullanıcılar için session ID

            var cartItems = userId.HasValue
                ? await _cartService.GetCartItemsAsync(userId.Value)// Kullanıcıya ait sepet
                : await _cartService.GetCartItemsAsync(sessionId);// Session'a ait sepet
            // Sepet DTO’sunu doldur
            var cartDto = new CartDTO
            {
                Items = cartItems.Select(item => new CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Ürün Yok",
                    ImageUrl = item.Product?.ImagePath ?? "",
                    UnitPrice = item.Product?.Price ?? 0,
                    Stock = item.Quantity
                }).ToList()
            };


            return View(cartDto);
        }
        // Sepete ürün ekleyen metot
        public async Task<IActionResult> Add(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.AddToCartAsync(userId.Value, id);// Kullanıcıya ait sepete ekle
            else
                await _cartService.AddToCartAsync(sessionId, id);// Session'a ait sepete ekle

            return RedirectToAction("Index");
        }


        // Sepetten ürün silen metot
        public async Task<IActionResult> Remove(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.RemoveFromCartAsync(userId.Value, id);// Kullanıcıya ait sepetten sil
            else
                await _cartService.RemoveFromCartAsync(sessionId, id); // Session sepetinden sil

            return RedirectToAction("Index");
        }
        // Sepeti tamamen temizleyen metot
        public async Task<IActionResult> Clear()
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.ClearCartAsync(userId.Value);// Kullanıcı sepetini temizle
            else
                await _cartService.ClearCartAsync(sessionId); // Misafir sepetini temizle

            return RedirectToAction("Index");
        }
        // Checkout işlemine yönlendiren (şimdilik sadece ödeme ekranına gider)
        [HttpGet]
        public IActionResult Checkout()
        {
            return RedirectToAction("Payment");// Ödeme sayfasına yönlendir
        }
        // Ödeme ekranını görüntüleyen metot
        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }
        // Ödeme formu gönderildikten sonra çalışan metot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentPost()
        {
            var userId = await GetCurrentUserIdAsync();// Kullanıcı ID'si (varsa)
            var sessionId = await GetSessionIdAsync(); // Session ID
              // Kullanıcıya veya oturuma ait sepeti al
            var cartItems = userId.HasValue
                ? await _cartService.GetCartItemsAsync(userId.Value)
                : await _cartService.GetCartItemsAsync(sessionId);

            if (cartItems.Any())// Sepet boş değilse
            {
                var user = userId.HasValue ? await _authService.GetCurrentUserAsync(User) : null;
                // Sipariş detaylarını oluştur
                var orderItems = cartItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.Product?.Name ?? "",
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0,
                    TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList();
                // Sipariş DTO'su oluştur
                var orderDto = new OrderDTO
                {
                    UserId = userId ?? 0,
                    CustomerId = userId ?? 0,
                    UserFullName = user?.FullName ?? "Misafir",
                    UserEmail = user?.Email ?? "guest@ecommerce.com",
                    TotalAmount = orderItems.Sum(x => x.TotalPrice),
                    Status = OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    ShippingAddress = "-",
                    OrderItems = orderItems
                };
                // Siparişi veritabanına kaydet
                await _serviceUnit.OrderService.CreateOrderAsync(orderDto);

                // Sepeti temizle
                if (userId.HasValue)
                    await _cartService.ClearCartAsync(userId.Value);
                else
                    await _cartService.ClearCartAsync(sessionId);

                TempData["Success"] = "Sipariş başarıyla oluşturuldu!";
            }

            return RedirectToAction("Index", "Product");
        }
    }
}



using AutoMapper;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.Application.VMs.Cart;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    // Controller "Customer" alanında yer alır
    [Area("Customer")]
    // Sadece "Customer" rolüne sahip kullanıcılar bu controller'a erişebilir
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        // Servis bağımlılıkları tanımlanır
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        // Bağımlılıkların constructor üzerinden enjekte edilmesi
        public CartController(
            ICartService cartService,
            IProductService productService,
            IMapper mapper,
            IAuthService authService)
        {
            _cartService = cartService;
            _productService = productService;
            _mapper = mapper;
            _authService = authService;
        }
        // Sepet sayfası
        public async Task<IActionResult> Index()
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            // Kullanıcının sepet öğeleri alınır
            var cartItems = await _cartService.GetCartItemsAsync(userId.Value);
            // Sepet DTO'su oluşturulur ve doldurulur
            var cartDto = new ECommerce.Application.DTOs.BasketDTOs.CartDTO
            {
                Items = cartItems.Select(item => new ECommerce.Application.DTOs.BasketDTOs.CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "",
                    ImageUrl = item.Product?.ImagePath ?? "",
                    UnitPrice = item.Product?.Price ?? 0,
                    Stock = item.Quantity
                }).ToList()
            };
            return View(cartDto);
        }

        // Sepete ürün ekleme
        public async Task<IActionResult> Add(int id)
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            // Ürün sepete eklenir
            await _cartService.AddToCartAsync(userId.Value, id);
            return RedirectToAction("Index");
        }

        // Sepetten ürün çıkarma
        public async Task<IActionResult> Remove(int id)
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cartService.RemoveFromCartAsync(userId.Value, id);
            return RedirectToAction("Index");
        }


        // Sepeti tamamen temizleme
        public async Task<IActionResult> Clear()
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var items = await _cartService.GetCartItemsAsync(userId.Value);
            foreach (var item in items)
            {
                await _cartService.RemoveFromCartAsync(userId.Value, item.ProductId);
            }
            return RedirectToAction("Index");
        }


        // Ödeme yönlendirmesi (checkout)
        public IActionResult Checkout()
        {
            // Burada gerçek ödeme sayfasına yönlendirme yapılabilir
            
            return RedirectToAction("Payment", "Cart"); // Ödeme sayfası yoksa bir View dönebilir
        }

        // Ödeme formu (GET)
        [HttpGet]
        public IActionResult Payment()
        {
            // Boş form gönderilir
            return View(new PaymentVM()); 
        }
        // Ödeme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentVM model)
        {
            // Form valid değilse yeniden göster
            if (!ModelState.IsValid)
            {
                
                return View("Payment", model);
            }
   
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _cartService.GetCartItemsAsync(userId.Value);
            if (cartItems.Any())
            {
                var user = await _authService.GetCurrentUserAsync(User);
                int customerId = userId.Value;

                // Siparişe ait ürünlerin listesi hazırlanır
                var orderItems = cartItems.Select(item => new ECommerce.Application.DTOs.OrderDTOs.OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.Product?.Name ?? "",
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0,
                    TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList();
                // Sipariş DTO'su oluşturulur
                var orderDto = new ECommerce.Application.DTOs.OrderDTOs.OrderDTO
                {
                    UserId = userId.Value,
                    CustomerId = customerId,
                    UserFullName = user.FullName,
                    UserEmail = user.Email,
                    TotalAmount = orderItems.Sum(x => x.TotalPrice),
                    Status = ECommerce.Domain.Enums.OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    ShippingAddress = model.Address,
                    OrderItems = orderItems
                };
                // OrderService servisinin alınması (Service Locator pattern kullanılmış)
                var serviceUnit = HttpContext.RequestServices.GetService(typeof(ECommerce.Application.UnitOfWorks.IServiceUnit)) as ECommerce.Application.UnitOfWorks.IServiceUnit;
                // Sipariş veritabanına kaydedilir
                await serviceUnit.OrderService.CreateOrderAsync(orderDto);
                // Siparişten sonra sepet temizlenir
                foreach (var item in cartItems)
                {
                    await _cartService.RemoveFromCartAsync(userId.Value, item.ProductId);
                }
            }

            TempData["Success"] = "Ödeme başarılı, siparişiniz oluşturuldu.";
            return RedirectToAction("Index", "Product");
        }


    }
}


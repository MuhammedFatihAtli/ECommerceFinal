using AutoMapper;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

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

        public async Task<IActionResult> Index()
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _cartService.GetCartItemsAsync(userId.Value);
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

        public async Task<IActionResult> Add(int id)
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cartService.AddToCartAsync(userId.Value, id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            await _cartService.RemoveFromCartAsync(userId.Value, id);
            return RedirectToAction("Index");
        }

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

        public IActionResult Checkout()
        {
            // Burada gerçek ödeme sayfasına yönlendirme yapılabilir
            return RedirectToAction("Payment", "Cart"); // Ödeme sayfası yoksa bir View dönebilir
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentPost()
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var cartItems = await _cartService.GetCartItemsAsync(userId.Value);
            if (cartItems.Any())
            {
                var user = await _authService.GetCurrentUserAsync(User);
                int customerId = userId.Value;
                var orderItems = cartItems.Select(item => new ECommerce.Application.DTOs.OrderDTOs.OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.Product?.Name ?? "",
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0,
                    TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList();
                var orderDto = new ECommerce.Application.DTOs.OrderDTOs.OrderDTO
                {
                    UserId = userId.Value,
                    CustomerId = customerId,
                    UserFullName = user.FullName,
                    UserEmail = user.Email,
                    TotalAmount = orderItems.Sum(x => x.TotalPrice),
                    Status = ECommerce.Domain.Enums.OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    ShippingAddress = "-",
                    OrderItems = orderItems
                };
                var serviceUnit = HttpContext.RequestServices.GetService(typeof(ECommerce.Application.UnitOfWorks.IServiceUnit)) as ECommerce.Application.UnitOfWorks.IServiceUnit;
                await serviceUnit.OrderService.CreateOrderAsync(orderDto);
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


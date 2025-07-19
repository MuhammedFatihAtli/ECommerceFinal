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

        private async Task<int?> GetCurrentUserIdAsync()
        {
            return await _authService.GetCurrentUserIdAsync(User);
        }

        private async Task<string> GetSessionIdAsync()
        {
            if (!HttpContext.Session.IsAvailable)
                await HttpContext.Session.LoadAsync();

            var sessionId = HttpContext.Session.GetString("SessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SessionId", sessionId);
            }

            return sessionId;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            var cartItems = userId.HasValue
                ? await _cartService.GetCartItemsAsync(userId.Value)
                : await _cartService.GetCartItemsAsync(sessionId);

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

        public async Task<IActionResult> Add(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.AddToCartAsync(userId.Value, id);
            else
                await _cartService.AddToCartAsync(sessionId, id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.RemoveFromCartAsync(userId.Value, id);
            else
                await _cartService.RemoveFromCartAsync(sessionId, id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            if (userId.HasValue)
                await _cartService.ClearCartAsync(userId.Value);
            else
                await _cartService.ClearCartAsync(sessionId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return RedirectToAction("Payment");
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
            var userId = await GetCurrentUserIdAsync();
            var sessionId = await GetSessionIdAsync();

            var cartItems = userId.HasValue
                ? await _cartService.GetCartItemsAsync(userId.Value)
                : await _cartService.GetCartItemsAsync(sessionId);

            if (cartItems.Any())
            {
                var user = userId.HasValue ? await _authService.GetCurrentUserAsync(User) : null;

                var orderItems = cartItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.Product?.Name ?? "",
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0,
                    TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList();

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

                await _serviceUnit.OrderService.CreateOrderAsync(orderDto);

                // Clear cart
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



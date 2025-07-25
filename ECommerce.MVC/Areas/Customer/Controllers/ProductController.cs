using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Domain.Entities;
using System.Threading.Tasks;
using System.Security.Claims;
using ECommerce.Application.DTOs;
using System;
using System.Linq;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class ProductController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<User> _userManager;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        // Constructor üzerinden servisler enjekte edilir
        public ProductController(IServiceUnit service, IProductService productService, ICartService cartService, IMapper mapper, IFavoriteService favoriteService, UserManager<User> userManager)

        {
            _service = service;
            _favoriteService = favoriteService;
            _userManager = userManager;
            _productService = productService;
            _cartService = cartService;
            _mapper = mapper;
        }

     
        // 🔹 Kullanıcının daha önce sipariş verdiği ürünleri listeler
        public async Task<IActionResult> Index(int? categoryId)
        {
            var user = User.Identity?.Name;
            // Giriş yapan kullanıcının Id'si alınıyor
            var orders = await _service.OrderService.GetOrdersByUserAsync((await _service.UserService.GetAllAsync()).FirstOrDefault(u => u.UserName == user)?.Id ?? 0);
            
            // Kullanıcının daha önce sipariş verdiği ürün Id'leri
            var orderedProductIds = orders.SelectMany(o => o.OrderItems).Select(oi => oi.ProductId).Distinct().ToList();
            // Ürünler filtreleniyor
            var products = (await _service.ProductService.GetAllProductsAsync(false, false)).Where(p => orderedProductIds.Contains(p.Id)).ToList();
            // Kategoriler view'e taşınıyor
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            return View(products);
        }

        // 🔹 Ürün detayları ve yorumlar
        public async Task<IActionResult> Details(int id)
        {
            var product = await _service.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Ürün bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            // Ürün yorumları alınıyor
            var comments = await _service.CommentService.GetCommentsByProductIdAsync(id);
            ViewBag.Comments = comments;
            return View(product); // View: ProductDTO
        }

        // 🔹 Ürünü favorilere ekler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _favoriteService.AddFavoriteAsync(user.Id, productId);
            TempData["Success"] = "Ürün favorilere eklendi.";
            return RedirectToAction("Index", "Dashboard");
        }

        // 🔹 Kullanıcının favori ürünlerini listeler
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _favoriteService.GetFavoritesByCustomerAsync(user.Id);
            return View(favorites);
        }

        // 🔹 Ürünü favorilerden çıkarır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _favoriteService.RemoveFavoriteAsync(user.Id, productId);
            TempData["Success"] = "Ürün favorilerden çıkarıldı.";
            return RedirectToAction("Favorites");
        }

        // 🔹 Sepete ürün ekler
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // Kullanıcı kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account", new { area = "" });

            await _cartService.AddToCartAsync(int.Parse(userId), productId);
            return RedirectToAction("Index", "Cart");
        }
        // 🔹 Ürüne yorum yapar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int ProductId, string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                TempData["CommentError"] = "Yorum metni boş bırakılamaz.";
                return RedirectToAction("Details", new { id = ProductId });
            }
            var commentDto = new CommentDTO
            {
                ProductId = ProductId,
                Text = Text,
                CreatedAt = DateTime.Now
            };
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                commentDto.UserName = User.Identity.Name;
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                    userId = int.Parse(userIdClaim.Value);
            }
            else
            {
                commentDto.UserName = "Misafir";
            }
            await _service.CommentService.AddCommentAsync(commentDto, userId);
            return RedirectToAction("Details", new { id = ProductId });
        }
        // 🔹 Kullanıcının tüm siparişlerini siler (Geliştirici amaçlı)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _service.OrderService.GetOrdersByUserAsync(user.Id);
            foreach (var order in orders)
            {
                await _service.OrderService.DeleteOrderAsync(order.Id);
            }
            TempData["Success"] = "Tüm siparişleriniz silindi.";
            return RedirectToAction("Index");
        }
    }
}


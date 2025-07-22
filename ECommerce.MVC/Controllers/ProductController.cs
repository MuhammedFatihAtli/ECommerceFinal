using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.MVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<User> _userManager;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
                    
        public ProductController(IServiceUnit service, IProductService productService, ICartService cartService, IMapper mapper, IFavoriteService favoriteService, UserManager<User> userManager)

        {
            _service = service;
            _favoriteService = favoriteService;
            _userManager = userManager;
            _productService = productService;
            _cartService = cartService;
            _mapper = mapper;
        }

        // 🔹 Ürünleri kategoriye göre listele
        public async Task<IActionResult> Index(int? categoryId)
        {
            var user = User.Identity?.Name;
            var orders = await _service.OrderService.GetOrdersByUserAsync((await _service.UserService.GetAllAsync()).FirstOrDefault(u => u.UserName == user)?.Id ?? 0);
            var orderedProductIds = orders.SelectMany(o => o.OrderItems).Select(oi => oi.ProductId).Distinct().ToList();
            var products = (await _service.ProductService.GetAllProductsAsync(false, false)).Where(p => orderedProductIds.Contains(p.Id)).ToList();
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            ProductDTO product = null;
            try
            {
                product = await _service.ProductService.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            if (product == null)
            {
                TempData["Error"] = "Ürün bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var comments = await _service.CommentService.GetCommentsByProductIdAsync(id) ?? new List<CommentDTO>();

            var viewModel = new ProductDetailVM
            {
                Product = product,
                Comments = comments,
                NewComment = new CommentDTO { ProductId = id }
            };

            return View(viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Favorilere eklemek için lütfen giriş yapınız.";
                TempData["ShowFavoriteMessage"] = true;
                return RedirectToAction("Login", "Account"); // Giriş sayfasına yönlendir
            }

            await _favoriteService.AddFavoriteAsync(user.Id, productId);
            TempData["Success"] = "Ürün favorilere eklendi.";
            return RedirectToAction("Index", "Dashboard");
        }


        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Kullanıcı giriş yapmamış, örneğin:
                // return RedirectToAction("Login", "Account");
                // Ya da:
                TempData["Error"] = "Favorileri görüntülemek için giriş yapmalısınız.";
                return RedirectToAction("Index", "Home");
            }

            var favorites = await _favoriteService.GetFavoritesByCustomerAsync(user.Id);
            return View(favorites);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _favoriteService.RemoveFavoriteAsync(user.Id, productId);
            TempData["Success"] = "Ürün favorilerden çıkarıldı.";
            return RedirectToAction("Favorites");
        }

        // 🔹 Sepete ürün ekle
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // Kullanıcı kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Sepete eklemek için lütfen giriş yapınız.";
                TempData["ShowCartMessage"] = true;
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            await _cartService.AddToCartAsync(int.Parse(userId), productId);
            return RedirectToAction("Index", "Cart");
        }

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


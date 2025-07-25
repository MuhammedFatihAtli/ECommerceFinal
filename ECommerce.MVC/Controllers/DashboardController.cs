using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.UnitOfWorks;
using System.Linq;

namespace ECommerce.MVC.Controllers
{
    // Ana sayfa (Dashboard) işlemlerini yöneten controller
    public class DashboardController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;

        public DashboardController(IServiceUnit service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        // Dashboard / ürün listeleme ekranı
        public async Task<IActionResult> Index(int? categoryId, string search, decimal? minPrice, decimal? maxPrice, bool? inStock)
        {// Kategoriler ViewBag’e atanarak filtre dropdown'u oluşturulur
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;

            IEnumerable<ECommerce.Application.DTOs.ProductDTOs.ProductDTO> products;
            if (categoryId.HasValue) // Kategoriye göre ürün filtreleme yapılır
            {
                products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);
            }
            else // Tüm ürünler alınır (silinmiş ve onaysız ürünler hariç)
            {
                products = await _service.ProductService.GetAllProductsAsync(false, false);
            }

            if (!string.IsNullOrEmpty(search)) // Arama filtresi uygulanır (ürün adı başlangıcı eşleşenler)
            {
                products = products.Where(p => p.Name != null && p.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase));
            }
            if (minPrice.HasValue) // Minimum fiyat filtresi
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue) // Maksimum fiyat filtresi
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            if (inStock.HasValue && inStock.Value)  // Sadece stokta ürün varsa filtrelenir
            {
                products = products.Where(p => p.Stock > 0);
            }

            var rnd = new Random(); // Ürünler rastgele sıralanır (vitrin gibi)
            var randomProducts = products.OrderBy(x => rnd.Next()).ToList();
            // Sonuçlar view'e gönderilir
            return View(randomProducts);
        }
    }
}

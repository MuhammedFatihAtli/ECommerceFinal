using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.UnitOfWorks;
using System.Linq;

namespace ECommerce.MVC.Areas.Customer.Controllers
{

    // Bu controller Customer alanında bulunur
    [Area("Customer")]
    // Giriş yapmış kullanıcılar erişebilir (herhangi bir role özel değil)
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;

        // Bağımlılıkların constructor ile alınması
        public DashboardController(IServiceUnit service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }



        // Ana sayfa (Dashboard) – ürün listeleme ve filtreleme
        public async Task<IActionResult> Index(int? categoryId, string search, decimal? minPrice, decimal? maxPrice, bool? inStock)
        {
            // Tüm kategorileri çek ve ViewBag'e aktar – filtre menüsünde kullanılır
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = categories;
            // Filtre değerlerini ViewBag ile taşı – formda değerleri korumak için
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;

            // Ürünleri çek (kategoriye göre ya da tümü)
            IEnumerable<ECommerce.Application.DTOs.ProductDTOs.ProductDTO> products;
            if (categoryId.HasValue)
            {
                products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);
            }
            else
            {
                // Varsayılan olarak tüm aktif ve silinmemiş ürünleri getir
                products = await _service.ProductService.GetAllProductsAsync(false, false);
            }

            // Arama filtresi (ürün adının başına göre)

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase));
            }
            // Minimum fiyat filtresi
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            // Maksimum fiyat filtresi
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            // Sadece stokta olanlar seçilmişse
            if (inStock.HasValue && inStock.Value)
            {
                products = products.Where(p => p.Stock > 0);
            }
            // Ürünleri rastgele sırala – kullanıcıya her seferinde farklı sıralama sunulabilir
            var rnd = new Random();
            var randomProducts = products.OrderBy(x => rnd.Next()).ToList();
            // View'e gönder
            return View(randomProducts);
        }
    }
}

using ECommerce.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;

namespace ECommerce.MVC.Controllers
{
    // Uygulamanýn ana sayfasý ve temel sayfalarý yöneten controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;// Loglama servisi
        private readonly IServiceUnit _service; // Servisleri tek noktadan yönetmek için Unit of Work

        public HomeController(ILogger<HomeController> logger, IServiceUnit service)
        {
            _logger = logger;
            _service = service;
        }
        // Ana sayfa - Ürün listeleme, filtreleme, arama
        public async Task<IActionResult> Index(int? categoryId, string search, decimal? minPrice, decimal? maxPrice, bool? inStock)
        { // Kategoriler alýnýr ve ViewBag ile view'a gönderilir
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;
           
            IEnumerable<ECommerce.Application.DTOs.ProductDTOs.ProductDTO> products;
            // Eðer kategori seçildiyse, o kategoriye ait ürünleri getir
            if (categoryId.HasValue)
            { // Tüm aktif ve silinmemiþ ürünleri getir
                products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);
            }
            else
            {
                products = await _service.ProductService.GetAllProductsAsync(false, false);
            }
            // Arama filtresi uygula
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase));
            }
            // Fiyat aralýðý filtreleri
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            if (inStock.HasValue && inStock.Value)  // Stokta olan ürünleri getir
            {
                products = products.Where(p => p.Stock > 0);
            }
            // Ürünleri rastgele sýrala

            var rnd = new Random();
            var randomProducts = products.OrderBy(x => rnd.Next()).ToList();
            ViewBag.Products = randomProducts;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // Hata sayfasý - varsayýlan hata yönlendirmesi
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

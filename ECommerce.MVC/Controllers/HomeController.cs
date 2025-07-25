using ECommerce.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;

namespace ECommerce.MVC.Controllers
{
    // Uygulaman�n ana sayfas� ve temel sayfalar� y�neten controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;// Loglama servisi
        private readonly IServiceUnit _service; // Servisleri tek noktadan y�netmek i�in Unit of Work

        public HomeController(ILogger<HomeController> logger, IServiceUnit service)
        {
            _logger = logger;
            _service = service;
        }
        // Ana sayfa - �r�n listeleme, filtreleme, arama
        public async Task<IActionResult> Index(int? categoryId, string search, decimal? minPrice, decimal? maxPrice, bool? inStock)
        { // Kategoriler al�n�r ve ViewBag ile view'a g�nderilir
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;
           
            IEnumerable<ECommerce.Application.DTOs.ProductDTOs.ProductDTO> products;
            // E�er kategori se�ildiyse, o kategoriye ait �r�nleri getir
            if (categoryId.HasValue)
            { // T�m aktif ve silinmemi� �r�nleri getir
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
            // Fiyat aral��� filtreleri
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            if (inStock.HasValue && inStock.Value)  // Stokta olan �r�nleri getir
            {
                products = products.Where(p => p.Stock > 0);
            }
            // �r�nleri rastgele s�rala

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
        // Hata sayfas� - varsay�lan hata y�nlendirmesi
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

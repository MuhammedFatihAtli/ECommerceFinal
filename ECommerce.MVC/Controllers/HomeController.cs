using ECommerce.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;

namespace ECommerce.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceUnit _service;

        public HomeController(ILogger<HomeController> logger, IServiceUnit service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index(int? categoryId, string search, decimal? minPrice, decimal? maxPrice, bool? inStock)
        {
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.InStock = inStock;

            IEnumerable<ECommerce.Application.DTOs.ProductDTOs.ProductDTO> products;
            if (categoryId.HasValue)
            {
                products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);
            }
            else
            {
                products = await _service.ProductService.GetAllProductsAsync(false, false);
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.StartsWith(search, StringComparison.OrdinalIgnoreCase));
            }
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            if (inStock.HasValue && inStock.Value)
            {
                products = products.Where(p => p.Stock > 0);
            }

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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

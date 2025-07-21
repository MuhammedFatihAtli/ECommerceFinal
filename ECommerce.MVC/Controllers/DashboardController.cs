using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.UnitOfWorks;
using System.Linq;

namespace ECommerce.MVC.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;

        public DashboardController(IServiceUnit service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
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
            return View(randomProducts);
        }
    }
}

using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.UnitOfWorks;
using System.Linq;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;

        public DashboardController(IServiceUnit service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        //public IActionResult Index()
        //{
        //    var userCount = _userManager.Users.CountAsync();

        //    ViewBag.UserCount = userCount;
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            var products = (await _service.ProductService.GetAllProductsAsync(false, false)).ToList();
            var rnd = new Random();
            var randomProducts = products.OrderBy(x => rnd.Next()).ToList();
            return View(randomProducts);
        }
    }
}

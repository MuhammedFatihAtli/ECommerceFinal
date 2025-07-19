using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            // Giriş yapan satıcının bilgilerini ViewBag ile gönder
            ViewBag.CustomerName = User.Identity?.Name;
            ViewBag.CustomerEmail = User.FindFirstValue(ClaimTypes.Email);
            return View();
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            // Giriş yapan satıcının bilgilerini ViewBag ile gönder
            ViewBag.SellerName = User.Identity?.Name;
            ViewBag.SellerEmail = User.FindFirstValue(ClaimTypes.Email);
            return View();
        }
    }
}


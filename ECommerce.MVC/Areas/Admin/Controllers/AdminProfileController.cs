using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            // Giriş yapan adminin bilgilerini ViewBag ile gönder
            ViewBag.AdminName = User.Identity?.Name;
            ViewBag.AdminEmail = User.FindFirstValue(ClaimTypes.Email);
            return View();
        }
    }
} 
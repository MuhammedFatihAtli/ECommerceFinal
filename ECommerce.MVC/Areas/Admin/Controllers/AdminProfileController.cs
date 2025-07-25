using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    // Bu Controller'ın "Admin" alanına (Area) ait olduğunu belirtir
    [Area("Admin")]
    // Bu Controller'a yalnızca "Admin" rolündeki kullanıcıların erişebileceğini belirtir
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        // Admin profili için ana sayfa (Index) aksiyonu
        public IActionResult Index()
        {
            // Giriş yapan adminin bilgilerini ViewBag ile gönder
            ViewBag.AdminName = User.Identity?.Name;
            // Giriş yapan kullanıcının e-posta adresini Claim üzerinden al ve ViewBag ile gönder
            ViewBag.AdminEmail = User.FindFirstValue(ClaimTypes.Email);
            // Index View'ını döndür
            return View();
        }
    }
} 
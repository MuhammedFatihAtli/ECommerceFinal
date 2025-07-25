using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize]
    public class DashboardController : Controller
    {
        // Satıcı ana sayfasını (dashboard) döndürür
        public IActionResult Index()
        {
            return View();
        }
    }
}

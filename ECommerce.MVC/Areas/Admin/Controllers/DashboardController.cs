using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        // ASP.NET Core Identity'den gelen UserManager, kullanıcı işlemleri için kullanılır
        private readonly UserManager<User> _userManager;


        // Constructor - UserManager dependency injection ile alınır
        public DashboardController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        // Admin paneli ana sayfası (dashboard) için action
        public async Task<IActionResult> Index()
        {
            // Sistemdeki toplam kullanıcı sayısını asenkron olarak al
            var userCount =await _userManager.Users.CountAsync();
            // Kullanıcı sayısını ViewBag ile View tarafına aktar
            ViewBag.UserCount = userCount;
            // Index görünümünü döndür
            return View();
        }
    }
}

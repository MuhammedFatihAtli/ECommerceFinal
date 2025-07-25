using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class ProfileController : Controller
    {
        private readonly ICommentService _commentService;

        // Yorum servisi dependency injection ile alınır
        public ProfileController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // 🔹 Kullanıcının profil sayfası
        public IActionResult Index()
        {
            // Giriş yapan satıcının bilgilerini ViewBag ile gönder
            ViewBag.CustomerName = User.Identity?.Name;
            ViewBag.CustomerEmail = User.FindFirstValue(ClaimTypes.Email);
            return View();
        }
        // 🔹 Kullanıcının profil sayfasından yorum eklemesini sağlayan POST metodu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentDTO commentDto)
        {
            // ModelState geçerli değilse tekrar profile yönlendir
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            // Yorum oluşturulma tarihi atanır
            commentDto.CreatedAt = DateTime.Now;
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                commentDto.UserName = User.Identity.Name;
                // Kullanıcının Id değeri alınır (Identity üzerinden)
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                    userId = int.Parse(userIdClaim.Value);
            }
            else
            {    // Giriş yapılmadıysa "Misafir" olarak isim atanır
                commentDto.UserName = "Misafir";
            }
            // Yorum servis aracılığıyla veritabanına kaydedilir
            await _commentService.AddCommentAsync(commentDto, userId);
            // Profil sayfasına geri dönülür
            return RedirectToAction("Index");
        }
    }
}


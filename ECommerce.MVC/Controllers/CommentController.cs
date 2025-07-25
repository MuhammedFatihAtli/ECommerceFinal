using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Controllers
{
    // Ürün yorumlarını yöneten controller
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;


        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        // Yorum ekleme işlemi - sadece POST isteklerinde çalışır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentDTO commentDto)
        {
            // Model doğrulaması başarısızsa ürün detay sayfasına geri yönlendir
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Product", new { id = commentDto.ProductId });
            }
            // Yorumun oluşturulma zamanı belirlenir
            commentDto.CreatedAt = DateTime.Now;

            int? userId = null;// Varsayılan olarak null (misafir kullanıcılar için)
            if (User.Identity.IsAuthenticated)// Kullanıcı giriş yaptıysa
            {
                commentDto.UserName = User.Identity.Name;
                // Kullanıcının ID'si Claim'den alınır
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                    userId = int.Parse(userIdClaim.Value); // string → int dönüşümü yapılır
            }
            else
            {
                commentDto.UserName = "Misafir";// Giriş yapmamışsa anonim kullanıcı adı atanır
            }
            // Yorumu servis üzerinden veritabanına ekle
            await _commentService.AddCommentAsync(commentDto, userId);
            // İşlemden sonra tekrar ürün detay sayfasına yönlendir
            return RedirectToAction("Details", "Product", new { id = commentDto.ProductId });
        }



    }
}

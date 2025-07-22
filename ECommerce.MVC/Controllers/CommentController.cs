using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;


        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentDTO commentDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Product", new { id = commentDto.ProductId });
            }

            commentDto.CreatedAt = DateTime.Now;

            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                commentDto.UserName = User.Identity.Name;

                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                    userId = int.Parse(userIdClaim.Value);
            }
            else
            {
                commentDto.UserName = "Misafir";
            }

            await _commentService.AddCommentAsync(commentDto, userId);

            return RedirectToAction("Details", "Product", new { id = commentDto.ProductId });
        }



    }
}

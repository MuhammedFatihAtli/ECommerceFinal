using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces
{
    // ICommentService.cs, yorum ekleme ve ürün yorumlarını alma işlemlerini tanımlar.
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO dto, int? userId);
        Task<List<CommentDTO>> GetCommentsByProductIdAsync(int productId);
    }

}

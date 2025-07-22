using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO dto, int? userId);
        Task<List<CommentDTO>> GetCommentsByProductIdAsync(int productId);
    }

}

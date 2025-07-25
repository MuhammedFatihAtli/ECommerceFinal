using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // CommentRepository sınıfı, Comment entity'si için veritabanı işlemlerini gerçekleştirir.
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
             
        public async Task<List<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await _context.Comments
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}

using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;
        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            return await _context.Favorites.Include(f => f.Product).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Favorite>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Favorites
                .Include(f => f.Product)
                .ThenInclude(p => p.Category)
                .Where(f => f.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Favorite> GetByCustomerAndProductAsync(int customerId, int productId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.CustomerId == customerId && f.ProductId == productId);
        }
    }
} 
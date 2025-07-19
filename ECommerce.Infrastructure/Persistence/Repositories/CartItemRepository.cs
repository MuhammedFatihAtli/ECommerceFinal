using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;
        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId && !x.IsDeleted);
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            return await _context.CartItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId /*&& !x.IsDeleted*/)
                .ToListAsync();
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> GetBySessionAndProductAsync(string guestId, int productId)
        {
            return await _context.CartItems
         .Include(x => x.Product)
         .FirstOrDefaultAsync(x => x.GuestId == guestId && x.ProductId == productId && x.Status);
        }

        public async Task<List<CartItem>> GetBySessionIdAsync(string sessionId)
        {
            return await _context.CartItems
                .Include(c => c.Product) // <-- BU ÇOK KRÝTÝK
                .Where(c => c.GuestId == sessionId)
                .ToListAsync();
        }

    }
} 
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // CartItemRepository s�n�f�, sepet ��eleri ile ilgili veritaban� i�lemlerini ger�ekle�tirir.
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;//veritaban�na eri�im.
        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);// EF Core bu kayd� belle�e ekler.
            await _context.SaveChangesAsync();//Veritaban�na yazar.
        }

        public async Task RemoveAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems
                .Include(x => x.Product)//Product tablosuyla ili�ki kurulmu�sa, o veriyi de getirir (Eager Loading).
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId && !x.IsDeleted);//ilk bulunan kayd� getirir -soft delete yap�lanlar gelmez.
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)//Kullan�c�n�n t�m sepetini getirir
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

        public async Task<CartItem?> GetBySessionAndProductAsync(string guestId, int productId)//Misafir kullan�c� ayn� �r�n� eklemi� mi kontrol.
        {
            return await _context.CartItems
         .Include(x => x.Product)
         .FirstOrDefaultAsync(x => x.GuestId == guestId && x.ProductId == productId && x.Status);
        }

        public async Task<List<CartItem>> GetBySessionIdAsync(string sessionId)//urun detaylar� ile birlikte misafirin sepetini getirir.
        {
            return await _context.CartItems
                .Include(c => c.Product) 
                .Where(c => c.GuestId == sessionId)
                .ToListAsync();
        }

    }
} 
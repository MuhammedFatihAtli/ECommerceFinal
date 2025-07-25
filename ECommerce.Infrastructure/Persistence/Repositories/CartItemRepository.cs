using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // CartItemRepository sýnýfý, sepet öðeleri ile ilgili veritabaný iþlemlerini gerçekleþtirir.
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;//veritabanýna eriþim.
        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);// EF Core bu kaydý belleðe ekler.
            await _context.SaveChangesAsync();//Veritabanýna yazar.
        }

        public async Task RemoveAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems
                .Include(x => x.Product)//Product tablosuyla iliþki kurulmuþsa, o veriyi de getirir (Eager Loading).
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId && !x.IsDeleted);//ilk bulunan kaydý getirir -soft delete yapýlanlar gelmez.
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)//Kullanýcýnýn tüm sepetini getirir
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

        public async Task<CartItem?> GetBySessionAndProductAsync(string guestId, int productId)//Misafir kullanýcý ayný ürünü eklemiþ mi kontrol.
        {
            return await _context.CartItems
         .Include(x => x.Product)
         .FirstOrDefaultAsync(x => x.GuestId == guestId && x.ProductId == productId && x.Status);
        }

        public async Task<List<CartItem>> GetBySessionIdAsync(string sessionId)//urun detaylarý ile birlikte misafirin sepetini getirir.
        {
            return await _context.CartItems
                .Include(c => c.Product) 
                .Where(c => c.GuestId == sessionId)
                .ToListAsync();
        }

    }
} 
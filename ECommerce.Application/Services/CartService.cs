using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ECommerce.Application.Services
{
    /// <summary>
    /// Hem giriş yapmış kullanıcılar hem de misafir kullanıcılar için alışveriş sepeti işlemlerini yönetmeye yönelik işlevsellik sağlar.
    /// </summary>
    /// <remarks>
    /// <see cref="CartService"/> sınıfı, alışveriş sepetine ürün ekleme, çıkarma, listeleme ve temizleme işlemlerini sağlar.
    /// Bu işlemler hem kullanıcı kimliği ile tanımlanan giriş yapmış kullanıcılar hem de oturum kimliği ile tanımlanan
    /// misafir kullanıcılar için desteklenmektedir.
    /// </remarks>

    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        // Giriş yapmış kullanıcı için sepete ürün ekleme
        public async Task AddToCartAsync(int userId, int productId)
        {
            var existing = await _cartItemRepository.GetByUserAndProductAsync(userId, productId);
            if (existing != null)
            {
                existing.Quantity += 1;
                await _cartItemRepository.UpdateAsync(existing);
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedDate = DateTime.UtcNow,
                    Status = true
                };
                await _cartItemRepository.AddAsync(cartItem);
            }
        }

        // Misafir kullanıcı için sepete ürün ekleme
        public async Task AddToCartAsync(string guestId, int productId)
        {
            var existing = await _cartItemRepository.GetBySessionAndProductAsync(guestId, productId);
            if (existing != null)
            {
                existing.Quantity += 1;
                await _cartItemRepository.UpdateAsync(existing);
            }
            else
            {
                var cartItem = new CartItem
                {
                    GuestId = guestId,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedDate = DateTime.UtcNow,
                    Status = true
                };
                await _cartItemRepository.AddAsync(cartItem);
            }
        }

        // Giriş yapmış kullanıcı için sepetten ürün çıkarma
        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var existing = await _cartItemRepository.GetByUserAndProductAsync(userId, productId);
            if (existing != null)
            {
                await _cartItemRepository.RemoveAsync(existing);
            }
        }

        // Misafir kullanıcı için sepetten ürün çıkarma
        public async Task RemoveFromCartAsync(string guestId, int productId)
        {
            var existing = await _cartItemRepository.GetBySessionAndProductAsync(guestId, productId);
            if (existing != null)
            {
                await _cartItemRepository.RemoveAsync(existing);
            }
        }

        // Giriş yapmış kullanıcı için sepeti getirme
        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            var items = await _cartItemRepository.GetByUserIdAsync(userId);
            return items.ToList();
        }

        // Misafir kullanıcı için sepeti getirme
        public async Task<List<CartItem>> GetCartItemsAsync(string guestId)
        {
            var items = await _cartItemRepository.GetBySessionIdAsync(guestId);
            return items.ToList();
        }

        // Giriş yapmış kullanıcı için sepeti temizleme
        public async Task ClearCartAsync(int userId)
        {
            var items = await _cartItemRepository.GetByUserIdAsync(userId);
            foreach (var item in items)
            {
                await _cartItemRepository.RemoveAsync(item);
            }
        }

        // Misafir kullanıcı için sepeti temizleme
        public async Task ClearCartAsync(string guestId)
        {
            var items = await _cartItemRepository.GetBySessionIdAsync(guestId);
            foreach (var item in items)
            {
                await _cartItemRepository.RemoveAsync(item);
            }
        }
    }
}

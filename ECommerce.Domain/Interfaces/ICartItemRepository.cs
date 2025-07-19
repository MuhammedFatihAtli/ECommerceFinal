using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddAsync(CartItem cartItem);
        Task RemoveAsync(CartItem cartItem);
        Task<CartItem> GetByUserAndProductAsync(int userId, int productId);
        Task<List<CartItem>> GetByUserIdAsync(int userId);
        Task UpdateAsync(CartItem cartItem);
        Task<CartItem?> GetBySessionAndProductAsync(string sessionId, int productId);
        Task<List<CartItem>> GetBySessionIdAsync(string sessionId);

    }
} 
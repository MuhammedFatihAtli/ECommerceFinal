using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, int productId);
        Task AddToCartAsync(string sessionId, int productId);

        Task RemoveFromCartAsync(int userId, int productId);
        Task RemoveFromCartAsync(string sessionId, int productId);

        Task<List<CartItem>> GetCartItemsAsync(int userId);
        Task<List<CartItem>> GetCartItemsAsync(string sessionId);

        Task ClearCartAsync(int userId);
        Task ClearCartAsync(string sessionId);
    }
}

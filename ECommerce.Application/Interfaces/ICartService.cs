using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    // ICartService.cs", sepet işlemleri için gerekli metotları tanımlar.
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

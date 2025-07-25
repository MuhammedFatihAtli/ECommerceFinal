using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    // IFavoriteService.cs", favori ürün iþlemleri için gerekli metotlarý tanýmlar.
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(int customerId, int productId);
        Task RemoveFavoriteAsync(int customerId, int productId);
        Task<IEnumerable<Favorite>> GetFavoritesByCustomerAsync(int customerId);
        Task<bool> IsFavoriteAsync(int customerId, int productId);
    }
} 
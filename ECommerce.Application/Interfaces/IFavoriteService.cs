using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(int customerId, int productId);
        Task RemoveFavoriteAsync(int customerId, int productId);
        Task<IEnumerable<Favorite>> GetFavoritesByCustomerAsync(int customerId);
        Task<bool> IsFavoriteAsync(int customerId, int productId);
    }
} 
using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddAsync(Favorite favorite);
        Task RemoveAsync(Favorite favorite);
        Task<Favorite> GetByIdAsync(int id);
        Task<IEnumerable<Favorite>> GetByCustomerIdAsync(int customerId);
        Task<Favorite> GetByCustomerAndProductAsync(int customerId, int productId);
    }
} 
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task AddFavoriteAsync(int customerId, int productId)
        {
            var existing = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (existing == null)
            {
                await _favoriteRepository.AddAsync(new Favorite { CustomerId = customerId, ProductId = productId });
            }
        }

        public async Task RemoveFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (favorite != null)
            {
                await _favoriteRepository.RemoveAsync(favorite);
            }
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesByCustomerAsync(int customerId)
        {
            return await _favoriteRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<bool> IsFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            return favorite != null;
        }
    }
} 
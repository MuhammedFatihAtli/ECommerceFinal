using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    // FavoriteService, favori �r�nleri y�netmek i�in gerekli i�lemleri sa�lar.
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        // Bu s�n�f, favori �r�nleri y�netmek i�in gerekli i�lemleri sa�lar.
        public async Task AddFavoriteAsync(int customerId, int productId)
        {
            var existing = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (existing == null)
            {
                await _favoriteRepository.AddAsync(new Favorite { CustomerId = customerId, ProductId = productId });
            }
        }

        // RemoveFavoriteAsync metodu, verilen m��teri ve �r�n kimli�ine sahip favori �r�n� siler.
        public async Task RemoveFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (favorite != null)
            {
                await _favoriteRepository.RemoveAsync(favorite);
            }
        }

        // GetFavoritesByCustomerAsync metodu, verilen m��teri kimli�ine sahip t�m favori �r�nleri getirir.
        public async Task<IEnumerable<Favorite>> GetFavoritesByCustomerAsync(int customerId)
        {
            return await _favoriteRepository.GetByCustomerIdAsync(customerId);
        }

        // IsFavoriteAsync metodu, verilen m��teri ve �r�n kimli�ine sahip favori �r�n�n varl���n� kontrol eder.
        public async Task<bool> IsFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            return favorite != null;
        }
    }
} 
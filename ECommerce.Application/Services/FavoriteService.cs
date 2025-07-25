using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    // FavoriteService, favori ürünleri yönetmek için gerekli iþlemleri saðlar.
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        // Bu sýnýf, favori ürünleri yönetmek için gerekli iþlemleri saðlar.
        public async Task AddFavoriteAsync(int customerId, int productId)
        {
            var existing = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (existing == null)
            {
                await _favoriteRepository.AddAsync(new Favorite { CustomerId = customerId, ProductId = productId });
            }
        }

        // RemoveFavoriteAsync metodu, verilen müþteri ve ürün kimliðine sahip favori ürünü siler.
        public async Task RemoveFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            if (favorite != null)
            {
                await _favoriteRepository.RemoveAsync(favorite);
            }
        }

        // GetFavoritesByCustomerAsync metodu, verilen müþteri kimliðine sahip tüm favori ürünleri getirir.
        public async Task<IEnumerable<Favorite>> GetFavoritesByCustomerAsync(int customerId)
        {
            return await _favoriteRepository.GetByCustomerIdAsync(customerId);
        }

        // IsFavoriteAsync metodu, verilen müþteri ve ürün kimliðine sahip favori ürünün varlýðýný kontrol eder.
        public async Task<bool> IsFavoriteAsync(int customerId, int productId)
        {
            var favorite = await _favoriteRepository.GetByCustomerAndProductAsync(customerId, productId);
            return favorite != null;
        }
    }
} 
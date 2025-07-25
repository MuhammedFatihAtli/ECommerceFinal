using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Favori ürünler için repository arayüzü.
    /// </summary>
    public interface IFavoriteRepository
    {
        /// <summary>
        /// Yeni favori ürünü ekler.
        /// </summary>
        /// <param name="favorite">Eklenecek favori ürünü.</param>
        Task AddAsync(Favorite favorite);

        /// <summary>
        /// Favori ürünü siler.
        /// </summary>
        /// <param name="favorite">Silinecek favori ürünü.</param>
        Task RemoveAsync(Favorite favorite);

        /// <summary>
        /// Favori ürünü ID ile getirir.
        /// </summary>
        /// <param name="id">Favori ID'si.</param>
        /// <returns>Favori ürün.</returns>
        Task<Favorite> GetByIdAsync(int id);

        /// <summary>
        /// Müþteriye ait tüm favori ürünleri getirir.
        /// </summary>
        /// <param name="customerId">Müþteri ID'si.</param>
        /// <returns>Favori ürün listesi.</returns>
        Task<IEnumerable<Favorite>> GetByCustomerIdAsync(int customerId);

        /// <summary>
        /// Müþteri ve ürün bazýnda favori ürünü getirir.
        /// </summary>
        /// <param name="customerId">Müþteri ID'si.</param>
        /// <param name="productId">Ürün ID'si.</param>
        /// <returns>Favori ürün (varsa).</returns>
        Task<Favorite> GetByCustomerAndProductAsync(int customerId, int productId);
    }
}

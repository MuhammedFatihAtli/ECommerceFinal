using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Favori �r�nler i�in repository aray�z�.
    /// </summary>
    public interface IFavoriteRepository
    {
        /// <summary>
        /// Yeni favori �r�n� ekler.
        /// </summary>
        /// <param name="favorite">Eklenecek favori �r�n�.</param>
        Task AddAsync(Favorite favorite);

        /// <summary>
        /// Favori �r�n� siler.
        /// </summary>
        /// <param name="favorite">Silinecek favori �r�n�.</param>
        Task RemoveAsync(Favorite favorite);

        /// <summary>
        /// Favori �r�n� ID ile getirir.
        /// </summary>
        /// <param name="id">Favori ID'si.</param>
        /// <returns>Favori �r�n.</returns>
        Task<Favorite> GetByIdAsync(int id);

        /// <summary>
        /// M��teriye ait t�m favori �r�nleri getirir.
        /// </summary>
        /// <param name="customerId">M��teri ID'si.</param>
        /// <returns>Favori �r�n listesi.</returns>
        Task<IEnumerable<Favorite>> GetByCustomerIdAsync(int customerId);

        /// <summary>
        /// M��teri ve �r�n baz�nda favori �r�n� getirir.
        /// </summary>
        /// <param name="customerId">M��teri ID'si.</param>
        /// <param name="productId">�r�n ID'si.</param>
        /// <returns>Favori �r�n (varsa).</returns>
        Task<Favorite> GetByCustomerAndProductAsync(int customerId, int productId);
    }
}

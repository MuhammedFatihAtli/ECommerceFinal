using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Sepet kalemleri i�in repository aray�z�.
    /// </summary>
    public interface ICartItemRepository
    {
        /// <summary>
        /// Yeni sepet kalemi ekler.
        /// </summary>
        /// <param name="cartItem">Eklenecek sepet kalemi.</param>
        Task AddAsync(CartItem cartItem);

        /// <summary>
        /// Sepet kalemini siler.
        /// </summary>
        /// <param name="cartItem">Silinecek sepet kalemi.</param>
        Task RemoveAsync(CartItem cartItem);

        /// <summary>
        /// Kullan�c� ve �r�n baz�nda sepet kalemini getirir.
        /// </summary>
        /// <param name="userId">Kullan�c� ID'si.</param>
        /// <param name="productId">�r�n ID'si.</param>
        /// <returns>Sepet kalemi.</returns>
        Task<CartItem> GetByUserAndProductAsync(int userId, int productId);

        /// <summary>
        /// Kullan�c�n�n t�m sepet kalemlerini listeler.
        /// </summary>
        /// <param name="userId">Kullan�c� ID'si.</param>
        /// <returns>Sepet kalemleri listesi.</returns>
        Task<List<CartItem>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Sepet kalemini g�nceller.
        /// </summary>
        /// <param name="cartItem">G�ncellenecek sepet kalemi.</param>
        Task UpdateAsync(CartItem cartItem);

        /// <summary>
        /// Misafir kullan�c�n�n session ID ve �r�n baz�nda sepet kalemini getirir.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="productId">�r�n ID'si.</param>
        /// <returns>Sepet kalemi (varsa).</returns>
        Task<CartItem?> GetBySessionAndProductAsync(string sessionId, int productId);

        /// <summary>
        /// Misafir kullan�c�n�n session ID baz�nda t�m sepet kalemlerini getirir.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <returns>Sepet kalemleri listesi.</returns>
        Task<List<CartItem>> GetBySessionIdAsync(string sessionId);
    }
}

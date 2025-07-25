using ECommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Sepet kalemleri için repository arayüzü.
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
        /// Kullanýcý ve ürün bazýnda sepet kalemini getirir.
        /// </summary>
        /// <param name="userId">Kullanýcý ID'si.</param>
        /// <param name="productId">Ürün ID'si.</param>
        /// <returns>Sepet kalemi.</returns>
        Task<CartItem> GetByUserAndProductAsync(int userId, int productId);

        /// <summary>
        /// Kullanýcýnýn tüm sepet kalemlerini listeler.
        /// </summary>
        /// <param name="userId">Kullanýcý ID'si.</param>
        /// <returns>Sepet kalemleri listesi.</returns>
        Task<List<CartItem>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Sepet kalemini günceller.
        /// </summary>
        /// <param name="cartItem">Güncellenecek sepet kalemi.</param>
        Task UpdateAsync(CartItem cartItem);

        /// <summary>
        /// Misafir kullanýcýnýn session ID ve ürün bazýnda sepet kalemini getirir.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="productId">Ürün ID'si.</param>
        /// <returns>Sepet kalemi (varsa).</returns>
        Task<CartItem?> GetBySessionAndProductAsync(string sessionId, int productId);

        /// <summary>
        /// Misafir kullanýcýnýn session ID bazýnda tüm sepet kalemlerini getirir.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <returns>Sepet kalemleri listesi.</returns>
        Task<List<CartItem>> GetBySessionIdAsync(string sessionId);
    }
}

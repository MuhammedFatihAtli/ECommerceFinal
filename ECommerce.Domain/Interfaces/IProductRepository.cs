using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Ürünler için repository arayüzü.
    /// IGenericRepository'den türetilmiştir.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Ürünleri kategorileriyle birlikte getirir.
        /// </summary>
        /// <returns>Kategorileri yüklü ürün listesi.</returns>
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();

        /// <summary>
        /// Belirtilen kategoriye ait ürünleri getirir.
        /// </summary>
        /// <param name="categoryId">Kategori ID'si.</param>
        /// <returns>Ürün listesi.</returns>
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);

        /// <summary>
        /// Belirtilen satıcıya ait ürünleri getirir.
        /// </summary>
        /// <param name="sellerId">Satıcı ID'si.</param>
        /// <returns>Ürün listesi.</returns>
        Task<List<Product>> GetProductsBySellerIdAsync(int sellerId);

        /// <summary>
        /// Belirtilen misafir ID'sine ait ürünleri getirir.
        /// </summary>
        /// <param name="guestId">Misafir ID'si.</param>
        /// <returns>Ürün listesi.</returns>
        Task<List<Product>> GetProductsByGuestIdAsync(int? guestId);

        /// <summary>
        /// ID'ye göre ürün getirir, opsiyonel olarak kategori bilgisini dahil eder.
        /// </summary>
        /// <param name="id">Ürün ID'si.</param>
        /// <param name="includeCategory">Kategori dahil edilsin mi?</param>
        /// <returns>Ürün.</returns>
        Task<Product> GetByIdAsync(int id, bool includeCategory = false);
    }
}

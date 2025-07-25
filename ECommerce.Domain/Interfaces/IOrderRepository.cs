using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Siparişlere özel repository arayüzü.
    /// IGenericRepository'den türetilmiştir.
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>
        /// Siparişleri, ilişkili detaylar (OrderItems, User vb.) ile birlikte getirir.
        /// </summary>
        /// <returns>Sipariş listesi.</returns>
        Task<IEnumerable<Order>> GetOrdersWithDetailsAsync();

        /// <summary>
        /// Belirtilen ID'ye sahip siparişi, detaylarıyla birlikte getirir.
        /// </summary>
        /// <param name="orderId">Sipariş ID'si.</param>
        /// <returns>Sipariş.</returns>
        Task<Order> GetOrderWithDetailsAsync(int orderId);

        /// <summary>
        /// Belirtilen kullanıcıya ait siparişleri getirir.
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si.</param>
        /// <returns>Sipariş listesi.</returns>
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);

        /// <summary>
        /// Belirtilen sipariş durumuna (OrderStatus) göre siparişleri getirir.
        /// </summary>
        /// <param name="status">Sipariş durumu.</param>
        /// <returns>Sipariş listesi.</returns>
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        /// <summary>
        /// Belirtilen tarih aralığındaki siparişleri getirir.
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi.</param>
        /// <param name="endDate">Bitiş tarihi.</param>
        /// <returns>Sipariş listesi.</returns>
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Toplam sipariş sayısını döner.
        /// </summary>
        /// <returns>Sipariş sayısı.</returns>
        Task<int> GetTotalOrdersCountAsync();

        /// <summary>
        /// Toplam gelir tutarını döner.
        /// </summary>
        /// <returns>Toplam gelir (decimal).</returns>
        Task<decimal> GetTotalRevenueAsync();

        /// <summary>
        /// Belirtilen tarih aralığında elde edilen gelir tutarını döner.
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi.</param>
        /// <param name="endDate">Bitiş tarihi.</param>
        /// <returns>Gelir tutarı.</returns>
        Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

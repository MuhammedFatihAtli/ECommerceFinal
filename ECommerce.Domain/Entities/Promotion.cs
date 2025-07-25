using System;
using System.Collections.Generic;
using ECommerce.Domain.Commons;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Ürünlere uygulanabilen promosyon veya indirim kampanyasını temsil eder.
    /// </summary>
    public class Promotion : BaseEntity
    {
        /// <summary>
        /// Promosyonun adı (örneğin, "Yaz İndirimi").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// İndirim oranı (yüzdelik değer, örn: 0.10 = %10 indirim).
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Promosyonun geçerlilik başlangıç tarihi.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Promosyonun geçerlilik bitiş tarihi.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Bu promosyondan faydalanan ürünlerin listesi.
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}


using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Müþterinin favorilerine eklediði ürünleri temsil eder.
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Favori kaydýnýn benzersiz kimliði.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Favoriyi ekleyen müþterinin ID'si.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Favoriyi ekleyen müþteri nesnesi.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Favoriye eklenen ürünün ID'si.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Favoriye eklenen ürün nesnesi.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Favori kaydýnýn oluþturulma tarihi.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

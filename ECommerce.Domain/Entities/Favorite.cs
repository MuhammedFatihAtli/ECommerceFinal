using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// M��terinin favorilerine ekledi�i �r�nleri temsil eder.
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Favori kayd�n�n benzersiz kimli�i.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Favoriyi ekleyen m��terinin ID'si.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Favoriyi ekleyen m��teri nesnesi.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Favoriye eklenen �r�n�n ID'si.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Favoriye eklenen �r�n nesnesi.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Favori kayd�n�n olu�turulma tarihi.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

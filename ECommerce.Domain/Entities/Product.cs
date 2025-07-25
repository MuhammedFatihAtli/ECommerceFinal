using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Satışa sunulan ürün bilgisini temsil eder.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Ürünün adı.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ürün açıklaması.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ürün fiyatı.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Ürün stok miktarı.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Ürün görselinin dosya yolu veya URL'si.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Kategori adı (kategori objesi yüklenmediğinde gösterim için).
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Ürünün ait olduğu kategori ID'si.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Ürünün ait olduğu kategori nesnesi.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Ürünü satan satıcı ID'si.
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// Ürünü satan satıcı nesnesi.
        /// </summary>
        public virtual Seller Seller { get; set; }

        /// <summary>
        /// Eğer ürün misafir kullanıcıya aitse misafir ID'si.
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// Ürüne ait misafir kullanıcı nesnesi (varsa).
        /// </summary>
        public virtual Guest? Guest { get; set; }

        /// <summary>
        /// Ürüne uygulanmış promosyon ID'si (varsa).
        /// </summary>
        public int? PromotionId { get; set; }

        /// <summary>
        /// Ürüne ait promosyon nesnesi (varsa).
        /// </summary>
        public virtual Promotion? Promotion { get; set; }

        /// <summary>
        /// Ürünle ilgili yorumların koleksiyonu.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

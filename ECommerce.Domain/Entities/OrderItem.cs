using ECommerce.Domain.Commons;
using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Bir siparişe ait ürün kalemini temsil eder.
    /// Her kalem bir ürünün belirli miktar ve fiyat bilgilerini içerir.
    /// </summary>
    public class OrderItem : BaseEntity
    {
        /// <summary>
        /// Belirtilen parametrelerle yeni bir sipariş kalemi oluşturur.
        /// </summary>
        /// <param name="orderId">Siparişin ID'si.</param>
        /// <param name="productId">Ürünün ID'si.</param>
        /// <param name="quantity">Ürün adedi.</param>
        /// <param name="unitPrice">Ürün birim fiyatı.</param>
        public OrderItem(int orderId, int productId, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        /// <summary>
        /// Parametresiz kurucu (ORM araçları için).
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Bu kalemin ait olduğu siparişin ID'si.
        /// </summary>
        public int OrderId { get; private set; }

        /// <summary>
        /// Bu kalemin ait olduğu sipariş nesnesi.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Sipariş kaleminde bulunan ürünün ID'si.
        /// </summary>
        public int ProductId { get; private set; }

        /// <summary>
        /// Sipariş kalemindeki ürün nesnesi.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Ürün adedi.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Ürünün birim fiyatı.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Toplam fiyat (adet * birim fiyat).
        /// </summary>
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}

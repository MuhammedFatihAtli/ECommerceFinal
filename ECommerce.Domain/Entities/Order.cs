using ECommerce.Domain.Commons;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Müşterilerin veya misafirlerin verdiği siparişi temsil eder.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Sipariş oluşturmak için kullanılan kurucu.
        /// Siparişi veren kullanıcı ID'si ve toplam tutar gerektirir.
        /// </summary>
        /// <param name="userId">Siparişi veren kullanıcının ID'si.</param>
        /// <param name="totalAmount">Siparişin toplam tutarı.</param>
        public Order(int userId, decimal totalAmount)
        {
            UserId = userId;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            OrderDate = DateTime.Now;
        }

        /// <summary>
        /// Parametresiz kurucu (EF Core ve benzeri ORM'ler için).
        /// </summary>
        public Order()
        {
        }

        /// <summary>
        /// Siparişi veren kullanıcının ID'si.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Siparişi veren kullanıcı nesnesi.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Siparişi veren müşteri ID'si.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Siparişi veren müşteri nesnesi.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Misafir kullanıcı ID'si (varsa).
        /// </summary>
        public int? GuestId { get; set; }

        /// <summary>
        /// Misafir kullanıcı nesnesi (varsa).
        /// </summary>
        public virtual Guest? Guest { get; set; }

        /// <summary>
        /// Siparişin toplam tutarı.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Siparişin mevcut durumu.
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// Siparişin verildiği tarih.
        /// </summary>
        public DateTime OrderDate { get; private set; }

        /// <summary>
        /// Siparişin gönderildiği tarih (varsa).
        /// </summary>
        public DateTime? ShippedDate { get; private set; }

        /// <summary>
        /// Siparişin teslim edildiği tarih (varsa).
        /// </summary>
        public DateTime? DeliveredDate { get; private set; }

        /// <summary>
        /// Siparişin teslimat adresi (opsiyonel).
        /// </summary>
        public string? ShippingAddress { get; set; }

        /// <summary>
        /// Siparişin takip numarası (varsa).
        /// </summary>
        public string? TrackingNumber { get; set; }

        /// <summary>
        /// Siparişle ilgili ek notlar (opsiyonel).
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Siparişe ait ürün kalemleri.
        /// </summary>
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Sipariş durumunu günceller.
        /// Gönderildi ve teslim edildi durumlarında ilgili tarihleri de günceller.
        /// </summary>
        /// <param name="newStatus">Yeni sipariş durumu.</param>
        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            UpdatedDate = DateTime.Now;

            switch (newStatus)
            {
                case OrderStatus.InTransit:
                    ShippedDate = DateTime.Now;
                    break;
                case OrderStatus.Delivered:
                    DeliveredDate = DateTime.Now;
                    break;
            }
        }

        /// <summary>
        /// Siparişe yeni bir ürün kalemi ekler.
        /// </summary>
        /// <param name="productId">Ürün ID'si.</param>
        /// <param name="quantity">Adet sayısı.</param>
        /// <param name="unitPrice">Birim fiyatı.</param>
        public void AddOrderItem(int productId, int quantity, decimal unitPrice)
        {
            OrderItems.Add(new OrderItem(Id, productId, quantity, unitPrice));
        }
    }
}

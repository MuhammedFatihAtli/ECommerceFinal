using ECommerce.Domain.Commons;
using System;

namespace ECommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public OrderItem(int orderId, int productId, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public OrderItem()
        {
        }

        public int OrderId { get; private set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; private set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
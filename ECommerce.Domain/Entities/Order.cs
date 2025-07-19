using ECommerce.Domain.Commons;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
 
    public class Order : BaseEntity
    {
        public Order(int userId, decimal totalAmount)
        {
            UserId = userId;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            OrderDate = DateTime.Now;
        }

        public Order()
        {
        }

        public int UserId { get; private set; }
        public virtual User User { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? GuestId { get; set; }
        public virtual Guest? Guest { get; set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? ShippedDate { get; private set; }
        public DateTime? DeliveredDate { get; private set; }
        public string? ShippingAddress { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

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

        public void AddOrderItem(int productId, int quantity, decimal unitPrice)
        {
            OrderItems.Add(new OrderItem(Id, productId, quantity, unitPrice));
        }
    }
}

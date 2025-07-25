using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public record OrderDTO : BaseDTO
    {
        // OrderDTO.cs", sipariş bilgilerini tutar.
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}

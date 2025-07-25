using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Interfaces
{
    // IOrderService.cs", sipariş işlemleri için gerekli metodları tanımlar.
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDTO>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<OrderDTO> UpdateOrderStatusAsync(OrderUpdateDTO updateDTO);
        Task DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderDTO>> GetOrdersByUserAsync(int userId);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
    }
}
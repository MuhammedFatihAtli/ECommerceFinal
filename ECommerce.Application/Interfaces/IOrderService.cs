using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
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
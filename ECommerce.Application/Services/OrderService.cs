using AutoMapper;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserAsync(userId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            // Order entity'sini constructor ile oluştur
            var order = new Order(orderDto.UserId, orderDto.TotalAmount)
            {
                CustomerId = orderDto.CustomerId,
                User = null, // opsiyonel, gerekirse set edilebilir
                ShippingAddress = orderDto.ShippingAddress,
                Notes = orderDto.Notes,
                TrackingNumber = orderDto.TrackingNumber,
                // Status ve OrderDate constructor'da atanıyor
            };
            // OrderItems'ı ekle
            foreach (var item in orderDto.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.Quantity, item.UnitPrice);
            }
            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(OrderUpdateDTO updateDTO)
        {
            var order = await _orderRepository.GetByIdAsync(updateDTO.Id);
            if (order == null)
                throw new NotFoundException($"Order with ID {updateDTO.Id} not found.");

            order.UpdateStatus(updateDTO.Status);

            if (!string.IsNullOrEmpty(updateDTO.TrackingNumber))
                order.TrackingNumber = updateDTO.TrackingNumber;

            if (!string.IsNullOrEmpty(updateDTO.Notes))
                order.Notes = updateDTO.Notes;

            _orderRepository.Update(order);
            return await GetOrderByIdAsync(order.Id);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException($"Order with ID {orderId} not found.");

            _orderRepository.Delete(order);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
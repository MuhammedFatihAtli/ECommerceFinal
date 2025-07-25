using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    // OrderUpdateDTO.cs", sipariş güncelleme işlemleri için gerekli bilgileri tutar.
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }
}
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class OrderController : Controller
    {
        private readonly IServiceUnit _service;

        public OrderController(IServiceUnit service)
        {
            _service = service;
        }

        // Tüm siparişleri listeler
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Index()
        {
            var orders = await _service.OrderService.GetAllOrdersAsync();
            return View(orders);
        }

        // Belirli bir siparişin detaylarını gösterir
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // Siparişin durumunu güncelleme formunu getirir
        [HttpGet]
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var updateDTO = new OrderUpdateDTO
            {
                Id = order.Id,
                Status = order.Status,
                TrackingNumber = order.TrackingNumber,
                Notes = order.Notes
            };

            return View(updateDTO);
        }

        // Siparişin durumunu günceller
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> UpdateStatus(OrderUpdateDTO updateDTO)
        {
            if (ModelState.IsValid)
            {
                await _service.OrderService.UpdateOrderStatusAsync(updateDTO);
                TempData["Success"] = "Sipariş durumu başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(updateDTO);
        }

        // Belirli bir duruma sahip siparişleri listeler
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> OrdersByStatus(OrderStatus status)
        {
            var orders = await _service.OrderService.GetOrdersByStatusAsync(status);
            ViewBag.Status = status;
            return View("Index", orders);
        }


        // Siparişi siler
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.OrderService.DeleteOrderAsync(id);
            TempData["Success"] = "Sipariş başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}

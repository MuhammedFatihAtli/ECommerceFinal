using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    // Admin alanında (Area) yer alan bir controller olduğunu belirtir
    [Area("Admin")]
    // Bu controller'a sadece "Admin" rolündeki kullanıcılar erişebilir
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // Servis katmanına erişim için kullanılan IServiceUnit arabirimi
        private readonly IServiceUnit _service;
        // Constructor ile servis bağımlılığı enjekte edilir (Dependency Injection)
        public OrderController(IServiceUnit service)
        {
            _service = service;
        }
        // Tüm siparişleri listeler
        // LogActionFilter: Her action çalıştığında loglama işlemi yapılır (özelleştirilmiş bir filtre)
        
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Index()
        {
            // Tüm siparişleri veritabanından asenkron olarak al
            var orders = await _service.OrderService.GetAllOrdersAsync();
            // View'a sipariş listesini gönder
            return View(orders);
        }

        // Belirli bir siparişin detaylarını gösterir
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Details(int id)
        {
            // İlgili siparişi ID'ye göre getir
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            // Sipariş bulunamazsa 404 döndür
            if (order == null)
            {
                return NotFound();
            }
            // Sipariş detaylarını View'a gönder
            return View(order);
        }

        // Siparişin durumunu güncelleme formunu getirir
        [HttpGet]
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            // İlgili siparişi getir
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            // Sipariş bilgilerini DTO'ya aktar ve View'a gönder
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
            // Model doğrulaması başarılıysa
            if (ModelState.IsValid)
            {
                // Sipariş durumu güncellenir
                await _service.OrderService.UpdateOrderStatusAsync(updateDTO);
                // Başarı mesajı eklenir
                TempData["Success"] = "Sipariş durumu başarıyla güncellendi.";
                // Sipariş listesine yönlendirilir
                return RedirectToAction(nameof(Index));
            }
            // Model doğrulaması geçersizse, form geri gönderilir
            return View(updateDTO);
        }

        // Belirli bir duruma göre filtrelenmiş siparişleri listeler
        
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> OrdersByStatus(OrderStatus status)
        {
            // Verilen durumdaki siparişleri getir
            var orders = await _service.OrderService.GetOrdersByStatusAsync(status);
            // ViewBag ile durumu View'a gönder
            ViewBag.Status = status;
            // Aynı Index görünümünü kullanarak sonuçları göster
            return View("Index", orders);
        }


        // Siparişi siler
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            // Siparişi sil
            await _service.OrderService.DeleteOrderAsync(id);
            // Başarı mesajı göster
            TempData["Success"] = "Sipariş başarıyla silindi.";
            // Sipariş listesine yönlendirilir
            return RedirectToAction(nameof(Index));
        }
    }
}

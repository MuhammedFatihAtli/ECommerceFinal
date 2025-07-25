using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;


        // Constructor üzerinden bağımlılıklar alınır
        public OrderController(IServiceUnit service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        // Siparişleri listele (en yeni en üstte)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Kullanıcı oturum açmamışsa login sayfasına yönlendir
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            // Siparişler çekilir
            var orders = await _service.OrderService.GetOrdersByUserAsync(user.Id);
            return View(orders);
        }

        // Sipariş detay
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            // Sipariş bulunamazsa veya kullanıcıya ait değilse hata döner
            if (order == null || order.UserId != user.Id)
            {
                TempData["Error"] = "Sipariş bulunamadı veya yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // Sipariş iptal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _service.OrderService.GetOrderByIdAsync(id);
            // Sipariş yoksa veya kullanıcıya ait değilse yetkisiz erişim
            if (order == null || order.UserId != user.Id)
            {
                TempData["Error"] = "Sipariş bulunamadı veya yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }
            // Sipariş zaten iptal edilmişse ya da teslim edildiyse iptal edilemez
            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
            {
                TempData["Error"] = "Bu sipariş iptal edilemez.";
                return RedirectToAction(nameof(Index));
            }
            // Sipariş durumu güncellenir
            var updateDto = new OrderUpdateDTO
            {
                Id = id,
                Status = OrderStatus.Cancelled
            };
            await _service.OrderService.UpdateOrderStatusAsync(updateDto);
            TempData["Success"] = "Sipariş başarıyla iptal edildi.";
            return RedirectToAction(nameof(Index));
        }
        // Tek ürünle hızlı sipariş oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "" });
            // Ürün kontrolü yapılır
            var product = await _service.ProductService.GetProductByIdAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Ürün bulunamadı!";
                return RedirectToAction("Index", "Dashboard");
            }
            // Kullanıcıdan CustomerId'yi bul
            var customer = await _service.CustomerService.GetByUserNameAsync(user.UserName);
            // Her sipariş 1 adet
            var orderItem = new ECommerce.Application.DTOs.OrderDTOs.OrderItemDTO
            {
                ProductId = product.Id,
                ProductTitle = product.Name,
                Quantity = 1,
                UnitPrice = product.Price,
                TotalPrice = product.Price
            };
            // Sipariş DTO'su oluşturulur
            var orderDto = new ECommerce.Application.DTOs.OrderDTOs.OrderDTO
            {
                UserId = user.Id,
                CustomerId = customer.Id,
                UserFullName = user.FullName,
                UserEmail = user.Email,
                TotalAmount = product.Price,
                Status = ECommerce.Domain.Enums.OrderStatus.Pending,
                OrderDate = DateTime.Now,
                ShippingAddress = "-",
                OrderItems = new List<ECommerce.Application.DTOs.OrderDTOs.OrderItemDTO> { orderItem }
            };
            // Sipariş veritabanına kaydedilir
            await _service.OrderService.CreateOrderAsync(orderDto);
            TempData["Success"] = "Siparişiniz başarıyla oluşturuldu.";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}

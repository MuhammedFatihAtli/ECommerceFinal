using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        // Servis katmanına ve AutoMapper'a erişim için gerekli nesneler
        private readonly IServiceUnit _service;
        private readonly IMapper _mapper;

        // Constructor: servis ve mapper dependency injection ile alınır
        public ProductController(IServiceUnit service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        // Ürünleri listeleyen sayfa
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Eğer kategori ID'si verilmişse o kategoriye ait ürünler listelenir
            var products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);

            // Kategorileri dropdown'da göstermek için SelectList olarak ViewBag'e aktar
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            return View(products);
        }
        // Ürün oluşturma formunu getiren GET action
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Kategori ve satıcı listeleri View'a gönderilir
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var sellers = await _service.SellerService.GetAllAsync();
            ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");

            return View();
        }

        // Ürün oluşturma işlemini yapan POST action

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO model)
        {
            // Model doğrulama başarısızsa View tekrar gösterilir
            if (!ModelState.IsValid)
            {
                var categories = await _service.CategoryService.GetAllAsync();
                var sellers = await _service.SellerService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                return View(model);
            }

            try
            {
                // Eğer SellerId belirtilmemişse, admin kullanıcının seller'ı atanır
              
                if (model.SellerId == 0)
                {
                    var adminUserName = User.Identity?.Name;

                    // Kullanıcı adı alınamadıysa hata gösterilir
                    if (string.IsNullOrEmpty(adminUserName))
                    {
                        TempData["Error"] = "Kullanıcı kimliği bulunamadı!";
                        var categories = await _service.CategoryService.GetAllAsync();
                        var sellers = await _service.SellerService.GetAllAsync();
                        ViewBag.Categories = new SelectList(categories, "Id", "Name");
                        ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                        return View(model);
                    }
                    // Admin kullanıcısına ait seller bilgisi alınır
                   
                    var adminSeller = await _service.SellerService.GetByUserNameAsync(adminUserName);
                    // Seller bulunamazsa uyarı verilir
                    if (adminSeller == null)
                    {
                        TempData["Error"] = "Admin'e ait seller kaydı bulunamadı. Lütfen önce oluşturun.";
                        var categories = await _service.CategoryService.GetAllAsync();
                        var sellers = await _service.SellerService.GetAllAsync();
                        ViewBag.Categories = new SelectList(categories, "Id", "Name");
                        ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                        return View(model);
                    }
                    // Admin seller atanır
                    model.SellerId = adminSeller.Id;
                }
                // Ürün oluşturulur
                await _service.ProductService.CreateProductAsync(model);

                TempData["Success"] = "Ürün başarıyla oluşturuldu!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcı bilgilendirilir
                TempData["Error"] = $"Ürün oluşturulurken bir hata oluştu: {ex.Message}";
                var categories = await _service.CategoryService.GetAllAsync();
                var sellers = await _service.SellerService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                return View(model);
            }
        }

        // Ürün düzenleme sayfasını getiren GET action

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _service.ProductService.GetProductByIdAsync(id);
            if (productDto == null) return NotFound();

            // DTO, düzenleme formuna uygun DTO'ya çevrilir
            // ProductDTO → ProductEditDTO dönüşümü (AutoMapper veya manuel)
            var editDto = _mapper.Map<ProductEditDTO>(productDto);

            // Kategoriler dropdown için hazırlanır
            // Kategorileri getir, SelectList olarak ViewBag'e koy
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", editDto.CategoryId);

            return View(editDto);
        }
        // Ürün düzenleme işlemini yapan POST action
        // POST: Ürün düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditDTO dto)
        {
            // URL ile gelen id ile DTO içindeki id uyuşmuyorsa hata döndür
            if (id != dto.Id)
                return BadRequest();

            // Model geçerli değilse form geri döndürülür
            if (!ModelState.IsValid)
            {
                // Model doğrulama hatalıysa, kategorileri tekrar yükle
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }

            try
            {
                // Ürün güncellenir
                await _service.ProductService.UpdateProductAsync(id, dto); // DTO tipi net olmalı
                TempData["Success"] = "Ürün başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir
                ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu: {ex.Message}");
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }
        }

        // Ürünü silmeden önce onay sayfasını gösterir
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Ürün bulunamadı!";
                return RedirectToAction("Index");
            }

            return View(product);
        }
        // Ürünü silen POST action
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.ProductService.DeleteProductAsync(id);
                TempData["Success"] = "Ürün başarıyla silindi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün silinirken bir hata oluştu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
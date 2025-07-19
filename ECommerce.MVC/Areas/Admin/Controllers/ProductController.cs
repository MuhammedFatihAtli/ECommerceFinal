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
        private readonly IServiceUnit _service;
        private readonly IMapper _mapper;

        public ProductController(IServiceUnit service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = await _service.ProductService.GetProductsByCategoryIdAsync(categoryId);
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var sellers = await _service.SellerService.GetAllAsync();
            ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");

            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    var categories = await _service.CategoryService.GetAllAsync();
        //    ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO model)
        {
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
                // Eğer seller seçilmemişse, admin seller kullanılacak
                if (model.SellerId == 0)
                {
                    var adminUserName = User.Identity?.Name;

                    if (string.IsNullOrEmpty(adminUserName))
                    {
                        TempData["Error"] = "Kullanıcı kimliği bulunamadı!";
                        var categories = await _service.CategoryService.GetAllAsync();
                        var sellers = await _service.SellerService.GetAllAsync();
                        ViewBag.Categories = new SelectList(categories, "Id", "Name");
                        ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                        return View(model);
                    }
                    // Admin'e ait seller kaydını getir
                    var adminSeller = await _service.SellerService.GetByUserNameAsync(adminUserName);

                    if (adminSeller == null)
                    {
                        TempData["Error"] = "Admin'e ait seller kaydı bulunamadı. Lütfen önce oluşturun.";
                        var categories = await _service.CategoryService.GetAllAsync();
                        var sellers = await _service.SellerService.GetAllAsync();
                        ViewBag.Categories = new SelectList(categories, "Id", "Name");
                        ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                        return View(model);
                    }

                    model.SellerId = adminSeller.Id;
                }

                await _service.ProductService.CreateProductAsync(model);

                TempData["Success"] = "Ürün başarıyla oluşturuldu!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün oluşturulurken bir hata oluştu: {ex.Message}";
                var categories = await _service.CategoryService.GetAllAsync();
                var sellers = await _service.SellerService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Sellers = new SelectList(sellers, "Id", "CompanyName");
                return View(model);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ProductCreateDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var categories = await _service.CategoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        return View(model);
        //    }

        //    try
        //    {
        //        var adminUserName = User.Identity?.Name;

        //        if (string.IsNullOrEmpty(adminUserName))
        //        {
        //            TempData["Error"] = "Kullanıcı kimliği bulunamadı!";
        //            var categories = await _service.CategoryService.GetAllAsync();
        //            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //            return View(model);
        //        }

        //        // Admin'e ait seller kaydını getir
        //        var adminSeller = await _service.SellerService.GetByUserNameAsync(adminUserName);

        //        if (adminSeller == null)
        //        {
        //            TempData["Error"] = "Admin kullanıcıya ait bir satıcı kaydı bulunamadı. Lütfen önce bir seller kaydı oluşturun.";
        //            var categories = await _service.CategoryService.GetAllAsync();
        //            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //            return View(model);
        //        }

        //        model.SellerId = adminSeller.Id;

        //        await _service.ProductService.CreateProductAsync(model);

        //        TempData["Success"] = "Ürün başarıyla oluşturuldu!";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Ürün oluşturulurken bir hata oluştu: {ex.Message}";
        //        var categories = await _service.CategoryService.GetAllAsync();
        //        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //        return View(model);
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var product = await _service.ProductService.GetProductByIdAsync(id);
        //    if (product == null)
        //    {
        //        TempData["Error"] = "Ürün bulunamadı!";
        //        return RedirectToAction("Index");
        //    }

        //    var categories = await _service.CategoryService.GetAllAsync();
        //    ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, ProductDTO model)
        //{
        //    if (id != model.Id)
        //        return BadRequest();

        //    if (ModelState.IsValid)
        //    {
        //        await _service.ProductService.UpdateProductAsync(id, model);
        //        TempData["Success"] = "Ürün başarıyla güncellendi!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var categories = await _service.CategoryService.GetAllAsync();
        //    ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
        //    return View(model);
        //}
        // GET: Ürün düzenleme sayfası
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _service.ProductService.GetProductByIdAsync(id);
            if (productDto == null) return NotFound();

            // ProductDTO → ProductEditDTO dönüşümü (AutoMapper veya manuel)
            var editDto = _mapper.Map<ProductEditDTO>(productDto);

            // Kategorileri getir, SelectList olarak ViewBag'e koy
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", editDto.CategoryId);

            return View(editDto);
        }

        // POST: Ürün düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                // Model doğrulama hatalıysa, kategorileri tekrar yükle
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }

            try
            {
                await _service.ProductService.UpdateProductAsync(id, dto); // DTO tipi net olmalı
                TempData["Success"] = "Ürün başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu: {ex.Message}");
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }
        }


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
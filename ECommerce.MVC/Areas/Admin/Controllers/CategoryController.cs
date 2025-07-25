using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // Servis katmanına erişim için bağımlılık bağımlılığı (Dependency Injection)
        private readonly IServiceUnit _service;

        // Constructor - IServiceUnit servisi enjekte edilir
        public CategoryController(IServiceUnit service)
        {
            _service = service;
        }
        // Kategori listesini getiren action
        public async Task<IActionResult> Index()
        {
            // Tüm kategorileri veritabanından asenkron olarak al
            var categories = await _service.CategoryService.GetAllAsync();
            // Kategorileri View'a gönder
            return View(categories);
        }
        // Yeni kategori oluşturma sayfasını döner
        public IActionResult Create()
        {
            return View();
        }
        // Yeni kategori oluşturma işlemini gerçekleştirir (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO dto)
        {  // Model geçerliyse
            if (ModelState.IsValid)
            {
                try
                {
                    // Yeni kategori ekle
                    await _service.CategoryService.AddAsync(dto);
                    // Başarı mesajını TempData ile gönder
                    TempData["Success"] = "Kategori başarıyla oluşturuldu!";
                    // Liste sayfasına yönlendir
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Hata durumunda hata mesajını TempData ile gönder
                    TempData["Error"] = ex.Message;
                    return View(dto);
                }
            }
            // Model geçerli değilse formu geri döndür
            return View(dto);
        }
        // Kategori düzenleme sayfasını getirir
        public async Task<IActionResult> Edit(int id)
        {
            // İlgili kategoriyi veritabanından al
            var category = await _service.CategoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            // Veriyi düzenleme DTO'suna dönüştür
            var editDto = new CategoryEditDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(editDto);
        }
        // Kategori düzenleme işlemini gerçekleştirir (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kategoriyi güncelle
                    await _service.CategoryService.UpdateAsync(dto);
                    TempData["Success"] = "Kategori başarıyla güncellendi!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return View(dto);
                }
            }
            return View(dto);
        }
        // Silme onayı için kategori detaylarını gösterir
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.CategoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }
        // Silme işlemini onaylayan action (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Kategoriyi sil
                await _service.CategoryService.DeleteAsync(id);
                TempData["Success"] = "Kategori başarıyla silindi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}

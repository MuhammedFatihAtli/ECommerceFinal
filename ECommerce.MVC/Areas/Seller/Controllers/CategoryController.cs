using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.MVC.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class CategoryController : Controller
    {
        private readonly IServiceUnit _service; // Servisleri yöneten birim

        // Constructor: ServiceUnit bağımlılığı enjekte ediliyor
        public CategoryController(IServiceUnit service)
        {
            _service = service;
        }

        // Kategori listesini gösterir
        public async Task<IActionResult> Index()
        {
            var categories = await _service.CategoryService.GetAllAsync();// Tüm kategorileri al
            return View(categories); // Listeyi View’a gönder
        }

        // Yeni kategori oluşturma formunu gösterir
        public IActionResult Create()
        {
            return View(); // Boş form göster
        }

        // Yeni kategori oluşturma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO dto)
        {
            if (ModelState.IsValid)// Form doğrulaması başarılıysa
            {
                try
                {
                    await _service.CategoryService.AddAsync(dto);// Yeni kategoriyi ekle
                    TempData["Success"] = "Kategori başarıyla oluşturuldu!";
                    return RedirectToAction("Index"); // Liste sayfasına dön
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return View(dto);
                }
            }
            return View(dto);
        }

        // Kategori düzenleme sayfası (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.CategoryService.GetByIdAsync(id);// ID’ye göre kategori getir
            if (category == null) return NotFound();

            // View'a DTO olarak gönderilecek nesne oluştur
            var editDto = new CategoryEditDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(editDto);
        } 
        // Kategori düzenleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditDTO dto)
        {
            if (ModelState.IsValid)// Doğrulama başarılıysa
            {
                try
                {
                    await _service.CategoryService.UpdateAsync(dto);// Güncelleme işlemi
                    TempData["Success"] = "Kategori başarıyla güncellendi!";// Başarı mesajı
                    return RedirectToAction("Index"); // Listeye dön
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return View(dto);
                }
            }
            return View(dto); // Geçersizse geri göster
        }

        // Kategori silme onay sayfası (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.CategoryService.GetByIdAsync(id);// ID’ye göre kategori getir
            if (category == null) return NotFound();

            return View(category);// Silme onay sayfasını göster
        }

        // Kategori silme işlemi (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.CategoryService.DeleteAsync(id); // Kategoriyi sil
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

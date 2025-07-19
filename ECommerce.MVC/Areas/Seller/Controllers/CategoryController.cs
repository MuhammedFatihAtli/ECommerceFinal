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
        private readonly IServiceUnit _service;

        public CategoryController(IServiceUnit service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _service.CategoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CategoryService.AddAsync(dto);
                    TempData["Success"] = "Kategori başarıyla oluşturuldu!";
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

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.CategoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            var editDto = new CategoryEditDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(editDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.CategoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
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
